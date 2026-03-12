using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace SpacefleetTradeMod.Patches
{
    /// <summary>
    /// Replaces vanilla random resource selection with profit-optimized selection.
    ///
    /// Scoring: totalProfit first, with travel time as tiebreaker.
    /// Trades must meet a 15% profit margin threshold when possible.
    /// Non-tanker cargo ships are hard-blocked from fuel resources at every level.
    /// </summary>
    [HarmonyPatch(typeof(GlobalMarket), nameof(GlobalMarket.GetBestSellerAnyExceptHostile))]
    public static class ProfitOptimizerPatch
    {
        internal static Trader CallingTrader;
        private const float gAccDay = 73234.4f;
        private const float MinProfitMarginPct = 0.15f;

        // Stored so TradeCycle postfix can set prices on the Trader for UI display
        internal static int LastBuyPrice;
        internal static int LastSellPrice;

        [HarmonyPriority(Priority.First)]
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
            var allMarkets = Traverse.Create(__instance)
                .Field("allMarkets").GetValue<List<Market>>();
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

            // Two-pass: first try trades meeting 15% margin, then fallback
            Market bestSeller = null;
            ResourceDefinition bestResource = null;
            float bestScore = float.MinValue;
            int bestBuy = 0, bestSell = 0;

            Market fallbackSeller = null;
            ResourceDefinition fallbackResource = null;
            float fallbackScore = float.MinValue;
            int fallbackBuy = 0, fallbackSell = 0;

            foreach (var resource in allResources)
            {
                // Hard block: non-tankers NEVER trade fuel
                if (!isTanker && VirtualFuelTank.IsFuelResource(resource))
                    continue;

                int avgBuy = __instance.GetAverageBuyPriceOf(resource);
                int avgSell = __instance.GetAverageSellPriceOf(resource);
                if (avgBuy >= avgSell) continue;

                Market seller = __instance.GetBestSellerFromList(resource, friendly);
                if (seller == null) continue;

                int buyPrice = seller.GetCurrentPrice(resource, true);

                int sellPrice = 0;
                Market buyer = __instance.GetBestBuyerFromList(
                    resource, friendly, ref sellPrice);
                if (buyer == null || sellPrice <= buyPrice) continue;

                float available = seller.GetQuantityAvailable(resource);
                if (available <= 0f) continue;

                float volume = Mathf.Min(available, cargoCapacity);
                float totalProfit = (sellPrice - buyPrice) * volume;

                float distToSeller = Vector3.Distance(
                    traderPos, seller.transform.position);
                float distSellerToBuyer = Vector3.Distance(
                    seller.transform.position, buyer.transform.position);
                float travelDays = EstimateTravelTime(
                    distToSeller + distSellerToBuyer, fleetAccG);
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
                else if (score > fallbackScore)
                {
                    fallbackScore = score;
                    fallbackSeller = seller;
                    fallbackResource = resource;
                    fallbackBuy = buyPrice;
                    fallbackSell = sellPrice;
                }
            }

            if (bestSeller != null && bestResource != null)
            {
                refResource = bestResource;
                __result = bestSeller;
                LastBuyPrice = bestBuy;
                LastSellPrice = bestSell;
                Plugin.Log.LogInfo($"[Optimizer] {bestResource.resourceName} buy={bestBuy} sell={bestSell} margin={(float)(bestSell-bestBuy)/bestBuy*100f:F1}%");
            }
            else if (fallbackSeller != null && fallbackResource != null)
            {
                refResource = fallbackResource;
                __result = fallbackSeller;
                LastBuyPrice = fallbackBuy;
                LastSellPrice = fallbackSell;
                Plugin.Log.LogInfo($"[Optimizer] Fallback: {fallbackResource.resourceName} buy={fallbackBuy} sell={fallbackSell}");
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
    /// Prefix: stash calling Trader so optimizer knows position/capacity.
    /// Postfix: set buyPrice/bestSellPrice for UI display, hard-block fuel.
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

                // Safety net: if fuel somehow got selected for a non-tanker, kill the trade
                if (!isTanker && VirtualFuelTank.IsFuelResource(__instance.targetResource))
                {
                    Plugin.Log.LogWarning($"[TraderContext] BLOCKED fuel trade for non-tanker {__instance.name}: {__instance.targetResource.resourceName}");
                    __instance.targetResource = null;
                    __instance.targetMarket = null;
                    __result = false;
                    return;
                }

                // Set buy/sell prices from optimizer so UI displays them
                if (__instance.isBuying && ProfitOptimizerPatch.LastBuyPrice > 0)
                {
                    __instance.buyPrice = ProfitOptimizerPatch.LastBuyPrice;
                    __instance.bestSellPrice = ProfitOptimizerPatch.LastSellPrice;
                    if (__instance.bestSellPrice > 0)
                        __instance.profitMargin = 1f - (float)__instance.buyPrice / (float)__instance.bestSellPrice;
                }
            }
        }
    }

    /// <summary>
    /// NUCLEAR OPTION: Patch Trade() itself to block fuel purchases for non-tankers
    /// BEFORE ExecuteTrade can spend credits. This is the last line of defense.
    /// Also handles selling: if a non-tanker somehow has fuel in cargo, allow the sell
    /// to go through so it can clear the stuck cargo.
    /// </summary>
    [HarmonyPatch(typeof(Trader), "Trade")]
    public static class TradeExecutionGuard
    {
        [HarmonyPriority(Priority.First)]
        static bool Prefix(Trader __instance, ref int credits, ref float realQuantity)
        {
            if (__instance.targetResource == null) return true;
            if (!VirtualFuelTank.IsFuelResource(__instance.targetResource)) return true;

            bool isTanker = VirtualFuelTank.Get(__instance) != null;
            if (isTanker) return true; // Tankers can trade fuel

            // Non-tanker with fuel as target
            if (__instance.isBuying)
            {
                // BLOCK: prevent ExecuteTrade from spending credits on fuel
                Plugin.Log.LogWarning($"[TradeGuard] BLOCKED fuel PURCHASE for {__instance.name}: {__instance.targetResource.resourceName} qty={realQuantity}");
                realQuantity = 0f;
                return false; // Skip Trade entirely — triggers StopTrade via targetQuantity check
            }

            // ALLOW selling: let the non-tanker dump fuel it shouldn't have
            Plugin.Log.LogInfo($"[TradeGuard] Allowing fuel SELL for {__instance.name} to clear stuck cargo");
            return true;
        }
    }

    /// <summary>
    /// Consolidated fuel cargo block for ModifyResourceInCargo.
    /// Non-tankers cannot add fuel to cargo. Tankers route through virtual tank.
    /// </summary>
    [HarmonyPatch(typeof(Trader), "ModifyResourceInCargo")]
    public static class ModifyResourceInCargoPatch
    {
        [HarmonyPriority(Priority.First)]
        static bool Prefix(Trader __instance, ResourceDefinition resource, float quantity)
        {
            if (!VirtualFuelTank.IsFuelResource(resource))
                return true; // Not fuel

            var tank = VirtualFuelTank.Get(__instance);

            if (tank != null)
            {
                // Tanker: route through virtual tank
                if (quantity >= 0f)
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
                return false;
            }

            // Non-tanker: block adding fuel, allow removing (to clear stuck cargo)
            if (quantity > 0f)
            {
                Plugin.Log.LogWarning($"[FuelBlock] Blocked {resource.resourceName} x{quantity:F1} from cargo on {__instance.name}");
                return false;
            }
            return true; // Allow removal
        }

        private static void FallbackToCargoModules(
            Trader trader, ResourceDefinition resource, float quantity)
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
