using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace SpacefleetTradeMod.Patches
{
    /// <summary>
    /// Replaces vanilla random resource selection with profit-per-time optimization.
    ///
    /// For each resource, evaluates every seller→buyer pair and scores:
    ///   score = totalProfit / estimatedRoundTripTime
    ///
    /// Round trip = trader→seller + seller→buyer (the sell leg).
    /// Travel time is estimated using brachistochrone physics:
    ///   time ≈ 2 * sqrt(distance / (accG * gAccDay))
    /// where gAccDay converts G to game distance units per day².
    ///
    /// This ensures cargo ships pick the trade that makes the most credits
    /// per unit of game time, naturally preferring nearby high-margin trades.
    ///
    /// Non-tanker cargo ships are blocked from trading fuel resources.
    /// </summary>
    [HarmonyPatch(typeof(GlobalMarket), nameof(GlobalMarket.GetBestSellerAnyExceptHostile))]
    public static class ProfitOptimizerPatch
    {
        internal static Trader CallingTrader;

        // gAccDay: 1G in game distance units per day².
        // From Navigation.cs: accKkm = accG * gAccDay, where gAccDay = 73234.4f
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

            // Trader context
            Vector3 traderPos = Vector3.zero;
            float cargoCapacity = 100f;
            float fleetAccG = 1f;
            bool isTanker = false;

            if (CallingTrader != null)
            {
                traderPos = CallingTrader.transform.position;
                cargoCapacity = CallingTrader.totalStorageSpace > 0f
                    ? CallingTrader.totalStorageSpace
                    : 100f;
                isTanker = VirtualFuelTank.Get(CallingTrader) != null;

                var fleet = CallingTrader.GetComponent<FleetManager>();
                if (fleet != null && fleet.maxAccG > 0f)
                    fleetAccG = fleet.maxAccG;
            }

            var allResources = __instance.allResources.resources;
            Market bestSeller = null;
            ResourceDefinition bestResource = null;
            float bestScore = 0f;

            foreach (var resource in allResources)
            {
                // Non-tanker cargo ships must NOT trade fuel
                if (!isTanker && VirtualFuelTank.IsFuelResource(resource))
                    continue;

                // Quick filter: no arbitrage opportunity globally
                int avgBuy = __instance.GetAverageBuyPriceOf(resource);
                int avgSell = __instance.GetAverageSellPriceOf(resource);
                if (avgBuy >= avgSell) continue;

                // Find cheapest seller
                Market seller = __instance.GetBestSellerFromList(resource, friendly);
                if (seller == null) continue;

                int buyPrice = seller.GetCurrentPrice(resource, true);

                // Find best buyer and their price
                int sellPrice = 0;
                Market buyer = __instance.GetBestBuyerFromList(resource, friendly, ref sellPrice);
                if (buyer == null || sellPrice <= buyPrice) continue;

                float available = seller.GetQuantityAvailable(resource);
                if (available <= 0f) continue;

                float profitPerUnit = sellPrice - buyPrice;
                float volume = Mathf.Min(available, cargoCapacity);
                float totalProfit = profitPerUnit * volume;

                // Estimate round-trip time: trader→seller + seller→buyer
                float distToSeller = Vector3.Distance(traderPos, seller.transform.position);
                float distSellerToBuyer = Vector3.Distance(seller.transform.position, buyer.transform.position);
                float totalDist = distToSeller + distSellerToBuyer;

                float travelTime = EstimateTravelTime(totalDist, fleetAccG);

                // Add a base time for docking/trading (prevents infinite score at zero distance)
                float totalTime = travelTime + 0.5f;

                float score = totalProfit / totalTime;

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
                Plugin.Log.LogDebug($"[ProfitOptimizer] Best: {bestResource.resourceName} (cr/day: {bestScore:F0})");
            }
            else
            {
                __result = null;
            }

            return false;
        }

        /// <summary>
        /// Estimates travel time for a brachistochrone trajectory (accelerate half, decelerate half).
        /// time = 2 * sqrt(distance / (accG * gAccDay))
        /// Returns time in game days.
        /// </summary>
        private static float EstimateTravelTime(float distance, float accG)
        {
            if (distance <= 0f) return 0f;
            float accKkm = accG * gAccDay;
            if (accKkm <= 0f) return float.MaxValue;
            // Half-distance accel + half-distance decel = 2 * sqrt(halfDist / (0.5 * accKkm))
            // Simplifies to: 2 * sqrt(distance / accKkm)
            return 2f * Mathf.Sqrt(distance / accKkm);
        }
    }

    /// <summary>
    /// Stashes the calling Trader before TradeCycle so ProfitOptimizerPatch
    /// can access position, cargo capacity, and fleet acceleration.
    /// Also blocks non-tanker cargo ships from selling fuel resources.
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
}
