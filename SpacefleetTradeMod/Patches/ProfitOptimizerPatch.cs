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
    /// </summary>
    [HarmonyPatch(typeof(GlobalMarket), nameof(GlobalMarket.GetBestSellerAnyExceptHostile))]
    public static class ProfitOptimizerPatch
    {
        internal static Trader CallingTrader;
        private const float gAccDay = 73234.4f;

        static bool Prefix(
            GlobalMarket __instance,
            ref ResourceDefinition refResource,
            Market currentMarket,
            int mainFactionID,
            ref Market __result)
        {
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
            Market bestSeller = null;
            ResourceDefinition bestResource = null;
            float bestScore = float.MinValue;

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

                // Primary: total profit. Tiebreaker: faster trips win among equal profit.
                float score = totalProfit + timeTiebreaker;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestSeller = seller;
                    bestResource = resource;
                }
            }

            if (bestSeller != null && bestResource != null)
            {
                refResource = bestResource;
                __result = bestSeller;
                Plugin.Log.LogDebug($"[ProfitOptimizer] Best: {bestResource.resourceName} profit={bestScore:F0}");
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
    /// Postfix hard-blocks fuel trades for non-tankers as a safety net.
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
                if (!isTanker && VirtualFuelTank.IsFuelResource(__instance.targetResource))
                {
                    __result = false;
                }
            }
        }
    }

    /// <summary>
    /// Hard block: prevent non-tanker cargo ships from ever adding fuel to their cargo.
    /// Even if some code path bypasses the optimizer filter, this ensures fuel never
    /// enters a regular cargo ship's hold.
    /// </summary>
    [HarmonyPatch(typeof(Trader), "ModifyResourceInCargo")]
    public static class BlockCargoFuelPatch
    {
        [HarmonyPriority(Priority.VeryHigh)]
        static bool Prefix(Trader __instance, ResourceDefinition resource, float quantity)
        {
            // Only block ADDING fuel to non-tankers
            if (quantity <= 0f) return true; // selling/removing is fine
            if (!VirtualFuelTank.IsFuelResource(resource)) return true; // not fuel
            if (VirtualFuelTank.Get(__instance) != null) return true; // tanker, let TankerModifyResourcePatch handle it

            // Non-tanker trying to add fuel to cargo — block it
            Plugin.Log.LogWarning($"[FuelBlock] Blocked {resource.resourceName} from entering cargo on {__instance.name}");
            return false;
        }
    }
}
