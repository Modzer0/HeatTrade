using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace SpacefleetTradeMod.Patches
{
    /// <summary>
    /// Replaces vanilla random resource selection with profit-optimized selection.
    ///
    /// Scoring: totalProfit first, with travel time as tiebreaker.
    ///   primaryScore = totalProfit = (sellPrice - buyPrice) * min(available, cargo)
    ///   tiebreaker   = 1 / (1 + travelDays)  (range 0-1, higher = faster)
    ///   finalScore   = totalProfit + tiebreaker
    ///
    /// Multi-day trips are fine if they yield more total profit. Time only
    /// breaks ties between trades with similar profit.
    ///
    /// Non-tanker cargo ships are hard-blocked from fuel resources at every level.
    /// Trades must meet a 15% profit margin threshold when possible.
    /// </summary>
    [HarmonyPatch(typeof(GlobalMarket), nameof(GlobalMarket.GetBestSellerAnyExceptHostile))]
    public static class ProfitOptimizerPatch
    {
        internal static Trader CallingTrader;
        private const float gAccDay = 73234.4f;
        private const float MinProfitMarginPct = 0.15f;

        // Stored by the patch so TradeCycle postfix can set prices on the Trader
        internal static int LastBuyPrice;
        internal static int LastSellPrice;

        static bool Prefix(
            GlobalMarket __instance,
            ref ResourceDefinition refResource,
            Market currentMarket,
            int mainFactionID,
            ref Market __result)
        {
            LastBuyPrice = 0;
            LastSellPrice = 0;

            var fm = Traverse.Create(__instance).Field("fm").GetValue<FactionsManager>();
            if (fm == null) return true;

            var hostiles = fm.GetFactionFromID(mainFactionID).GetHostiles();
            var allMarkets = Traverse.Create(__instance).Field("allMarkets").GetValue<List<Market>>();
            if (allMarkets == null) return true;

            var friendly = new List<Market>();
            foreach (var m in allMarkets)
            {
                if (m != null && !hostiles.Contains(m.thisFaction.factionID))
                    friendly.Add(m);
            }

            if (friendly.Count == 0)
            {
                __result = null;
                return false;
            }

            Vector3 traderPos = Vector3.zero;
            float cargoCapacity = 100f;
            float fleetAccG = 1f;
            bool isTanker = false;

            if (CallingTrader != null)
            {
                traderPos = CallingTrader.transform.position;
                cargoCapacity = CallingTrader.totalStorageSpace > 0f
                    ? CallingTrader.totalStorageSpace : 100f;
                isTanker = VirtualFuelTank.Get(CallingTrader) != null;

                var fleet = CallingTrader.GetComponent<FleetManager>();
                if (fleet != null && fleet.maxAccG > 0f)
                    fleetAccG = fleet.maxAccG;
            }

            var allResources = __instance.allResources.resources;

            // Two-pass: first try trades meeting 15% margin, then fallback to any profitable
            Market bestSeller = null;
            ResourceDefinition bestResource = null;
            float bestScore = float.MinValue;
            int bestBuy = 0;
            int bestSell = 0;

            // Fallback candidates (profitable but below 15%)
            Market fallbackSeller = null;
            ResourceDefinition fallbackResource = null;
            float fallbackScore = float.MinValue;
            int fallbackBuy = 0;
            int fallbackSell = 0;

            foreach (var resource in allResources)
            {
                // Hard block: non-tankers never trade fuel
                if (!isTanker && VirtualFuelTank.IsFuelResource(resource))
                    continue;

                int avgBuy = __instance.GetAverageBuyPriceOf(resource);
                int avgSell = __instance.GetAverageSellPriceOf(resource);
                if (avgBuy >= avgSell) continue;

                Market seller = __instance.GetBestSellerFromList(resource, friendly);
                if (seller == null) continue;

                int buyPrice = seller.GetCurrentPrice(resource, true);

                int sellPrice = 0;
                Market buyer = __instance.GetBestBuyerFromList(resource, friendly, ref sellPrice);
                if (buyer == null || sellPrice <= buyPrice) continue;

                float available = seller.GetQuantityAvailable(resource);
                if (available <= 0f) continue;

                float profitPerUnit = sellPrice - buyPrice;
                float volume = Mathf.Min(available, cargoCapacity);
                float totalProfit = profitPerUnit * volume;

                // Time as tiebreaker only (0 to 1 range, won't outweigh profit)
                float distToSeller = Vector3.Distance(traderPos, seller.transform.position);
                float distSellerToBuyer = Vector3.Distance(seller.transform.position, buyer.transform.position);
                float travelDays = EstimateTravelTime(distToSeller + distSellerToBuyer, fleetAccG);
                float timeTiebreaker = 1f / (1f + travelDays);

                float score = totalProfit + timeTiebreaker;

                bool meetsMargin = sellPrice >= buyPrice * (1f + MinProfitMarginPct);

                if (meetsMargin)
                {
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestSeller = seller;
                        bestResource = resource;
                        bestBuy = buyPrice;
                        bestSell = sellPrice;
                    }
                }
                else
                {
                    // Track best fallback in case nothing meets 15%
                    if (score > fallbackScore)
                    {
                        fallbackScore = score;
                        fallbackSeller = seller;
                        fallbackResource = resource;
                        fallbackBuy = buyPrice;
                        fallbackSell = sellPrice;
                    }
                }
            }

            // Prefer 15%+ trades; fall back to best available if none qualify
            if (bestSeller != null && bestResource != null)
            {
                refResource = bestResource;
                __result = bestSeller;
                LastBuyPrice = bestBuy;
                LastSellPrice = bestSell;
                float margin = (float)(bestSell - bestBuy) / bestBuy * 100f;
                Plugin.Log.LogDebug($"[ProfitOptimizer] Best: {bestResource.resourceName} buy={bestBuy} sell={bestSell} margin={margin:F1}% profit={bestScore:F0}");
            }
            else if (fallbackSeller != null && fallbackResource != null)
            {
                refResource = fallbackResource;
                __result = fallbackSeller;
                LastBuyPrice = fallbackBuy;
                LastSellPrice = fallbackSell;
                float margin = (float)(fallbackSell - fallbackBuy) / Mathf.Max(1, fallbackBuy) * 100f;
                Plugin.Log.LogDebug($"[ProfitOptimizer] Fallback: {fallbackResource.resourceName} buy={fallbackBuy} sell={fallbackSell} margin={margin:F1}%");
            }
            else
            {
                __result = null;
            }

            return false;
        }

        internal static float EstimateTravelTime(float distance, float accG)
        {
            if (distance <= 0f) return 0f;
            float accKkm = accG * gAccDay;
            if (accKkm <= 0f) return float.MaxValue;
            return 2f * Mathf.Sqrt(distance / accKkm);
        }
    }

    /// <summary>
    /// Stashes the calling Trader before TradeCycle runs.
    /// Postfix sets buyPrice/bestSellPrice on the Trader after the buying branch,
    /// and hard-blocks fuel trades for non-tankers as a safety net.
    /// </summary>
    [HarmonyPatch(typeof(Trader), "TradeCycle")]
    public static class TraderContextPatch
    {
        [HarmonyPriority(Priority.First)]
        static void Prefix(Trader __instance)
        {
            ProfitOptimizerPatch.CallingTrader = __instance;
        }

        static void Postfix(Trader __instance, ref bool __result)
        {
            ProfitOptimizerPatch.CallingTrader = null;

            if (__result && __instance.targetResource != null)
            {
                bool isTanker = VirtualFuelTank.Get(__instance) != null;

                // Hard block: non-tankers must never trade fuel
                if (!isTanker && VirtualFuelTank.IsFuelResource(__instance.targetResource))
                {
                    Plugin.Log.LogWarning($"[TraderContext] Blocked fuel trade for non-tanker {__instance.name}: {__instance.targetResource.resourceName}");
                    __result = false;
                    return;
                }

                // Set buy/sell prices from the optimizer so the UI displays them
                if (__instance.isBuying && ProfitOptimizerPatch.LastBuyPrice > 0)
                {
                    __instance.buyPrice = ProfitOptimizerPatch.LastBuyPrice;
                    __instance.bestSellPrice = ProfitOptimizerPatch.LastSellPrice;
                    if (__instance.bestSellPrice > 0 && __instance.buyPrice > 0)
                    {
                        __instance.profitMargin = 1f - (float)__instance.buyPrice / (float)__instance.bestSellPrice;
                    }
                    Plugin.Log.LogDebug($"[TraderContext] Set prices for {__instance.name}: buy={__instance.buyPrice} sell={__instance.bestSellPrice} margin={__instance.profitMargin:P1}");
                }
            }
        }
    }

    /// <summary>
    /// Consolidated fuel cargo block for ModifyResourceInCargo.
    /// Non-tankers cannot add fuel to cargo. Tankers route fuel through virtual tank.
    /// Single patch avoids Harmony prefix chaining issues.
    /// </summary>
    [HarmonyPatch(typeof(Trader), "ModifyResourceInCargo")]
    public static class ModifyResourceInCargoPatch
    {
        [HarmonyPriority(Priority.First)]
        static bool Prefix(Trader __instance, ResourceDefinition resource, float quantity)
        {
            if (!VirtualFuelTank.IsFuelResource(resource))
                return true; // Not fuel — let original handle it

            var tank = VirtualFuelTank.Get(__instance);

            if (tank != null)
            {
                // Tanker: route through virtual tank
                bool adding = quantity >= 0f;
                if (adding)
                {
                    float added = tank.AddResource(resource, quantity);
                    quantity -= added;
                    if (quantity > 0f)
                        FallbackToCargoModules(__instance, resource, quantity);
                }
                else
                {
                    float absQty = Mathf.Abs(quantity);
                    float removed = tank.RemoveResource(resource, absQty);
                    absQty -= removed;
                    if (absQty > 0f)
                        FallbackToCargoModules(__instance, resource, -absQty);
                }
                return false; // Skip original
            }

            // Non-tanker trying to add fuel — block it
            if (quantity > 0f)
            {
                Plugin.Log.LogWarning($"[FuelBlock] Blocked {resource.resourceName} x{quantity:F1} from entering cargo on {__instance.name}");
                return false;
            }

            // Removing/selling fuel from non-tanker is fine (shouldn't happen but safe)
            return true;
        }

        private static void FallbackToCargoModules(Trader trader, ResourceDefinition resource, float quantity)
        {
            var fleet = trader.GetComponent<FleetManager>();
            if (fleet == null) return;

            bool adding = quantity >= 0f;
            foreach (var ship in fleet.ships)
            {
                foreach (var module in ship.GetModulesOfType(PartType.CARGO))
                {
                    if (adding)
                    {
                        float added = module.AddCargo(resource, quantity);
                        quantity -= added;
                        if (quantity <= 0f) return;
                    }
                    else
                    {
                        float removed = module.RemoveCargo(resource, Mathf.Abs(quantity));
                        quantity += removed;
                        if (quantity >= 0f) return;
                    }
                }
            }
        }
    }
}
