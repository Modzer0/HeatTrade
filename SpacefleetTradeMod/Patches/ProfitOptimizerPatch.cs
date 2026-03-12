using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace SpacefleetTradeMod.Patches
{
    /// <summary>
    /// Replaces the vanilla random resource selection in GetBestSellerAnyExceptHostile
    /// with a profit-per-distance scoring system that accounts for travel cost.
    ///
    /// Score = (sellPrice - buyPrice) * min(available, cargoCapacity) / (1 + distance)
    ///
    /// This favors nearby high-margin trades over distant ones that would burn fuel
    /// and waste time. Distance is the straight-line distance from the trader to the
    /// seller market (in world units).
    ///
    /// Also prevents non-tanker cargo ships from selecting fuel resources — only
    /// traders with a virtual fuel tank (tankers) can trade fuel.
    /// </summary>
    [HarmonyPatch(typeof(GlobalMarket), nameof(GlobalMarket.GetBestSellerAnyExceptHostile))]
    public static class ProfitOptimizerPatch
    {
        // Stash the calling Trader so we can read position and cargo capacity
        internal static Trader CallingTrader;

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

            // Get trader position and cargo capacity for distance/volume scoring
            Vector3 traderPos = Vector3.zero;
            float cargoCapacity = 100f;
            bool isTanker = false;

            if (CallingTrader != null)
            {
                traderPos = CallingTrader.transform.position;
                cargoCapacity = CallingTrader.totalStorageSpace > 0f
                    ? CallingTrader.totalStorageSpace
                    : 100f;
                isTanker = VirtualFuelTank.Get(CallingTrader) != null;
            }

            var allResources = __instance.allResources.resources;
            Market bestMarket = null;
            ResourceDefinition bestResource = null;
            float bestScore = 0f;

            foreach (var resource in allResources)
            {
                // Non-tanker cargo ships must NOT trade fuel resources
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

                // Distance penalty: divide by (1 + distance) so nearby trades score higher.
                // Distance is in world units (kkm in-game). A minimum of 1 prevents div-by-zero
                // and means trades at the current station get full score.
                float distToSeller = Vector3.Distance(traderPos, seller.transform.position);
                float score = totalProfit / (1f + distToSeller);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMarket = seller;
                    bestResource = resource;
                }
            }

            if (bestMarket != null && bestResource != null)
            {
                refResource = bestResource;
                __result = bestMarket;
                Plugin.Log.LogDebug($"[ProfitOptimizer] Best trade: {bestResource.resourceName} (score: {bestScore:F0})");
            }
            else
            {
                __result = null;
            }

            return false;
        }
    }

    /// <summary>
    /// Stashes the calling Trader instance before TradeCycle runs so the
    /// ProfitOptimizerPatch can read position and cargo capacity.
    /// For tankers, the TankerTradeCycleOverride prefix runs first and
    /// skips the original, so this only matters for regular cargo traders.
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

            // If a non-tanker cargo ship somehow selected a fuel resource, reject it
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
