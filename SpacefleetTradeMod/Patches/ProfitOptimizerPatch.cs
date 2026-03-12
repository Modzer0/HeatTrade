using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpacefleetTradeMod.Patches
{
    /// <summary>
    /// Optimizes the trade cycle for maximum profit:
    /// 
    /// 1. When buying, instead of picking a random resource with arbitrage potential,
    ///    score all resources by (bestSellPrice - bestBuyPrice) * available quantity
    ///    and pick the one with the highest total profit potential.
    /// 
    /// 2. When selling, the vanilla code already picks the best buyer. No change needed.
    /// 
    /// This replaces the randomized resource selection in GetBestSellerAnyExceptHostile
    /// with a profit-maximizing selection.
    /// </summary>
    [HarmonyPatch(typeof(GlobalMarket), nameof(GlobalMarket.GetBestSellerAnyExceptHostile))]
    public static class ProfitOptimizerPatch
    {
        static bool Prefix(
            GlobalMarket __instance,
            ref ResourceDefinition refResource,
            Market currentMarket,
            int mainFactionID,
            ref Market __result)
        {
            var fm = Traverse.Create(__instance).Field("fm").GetValue<FactionsManager>();
            if (fm == null) return true; // fallback to original

            var hostiles = fm.GetFactionFromID(mainFactionID).GetHostiles();
            var allMarkets = Traverse.Create(__instance).Field("allMarkets").GetValue<List<Market>>();
            if (allMarkets == null) return true;

            var friendlyMarkets = new List<Market>();
            foreach (var m in allMarkets)
            {
                if (m == null) continue;
                if (!hostiles.Contains(m.thisFaction.factionID))
                    friendlyMarkets.Add(m);
            }

            if (friendlyMarkets.Count == 0)
            {
                __result = null;
                return false;
            }

            // Score each resource by profit potential
            var allResources = __instance.allResources.resources;
            Market bestMarket = null;
            ResourceDefinition bestResource = null;
            float bestScore = 0f;

            foreach (var resource in allResources)
            {
                int avgBuy = __instance.GetAverageBuyPriceOf(resource);
                int avgSell = __instance.GetAverageSellPriceOf(resource);

                // Only consider resources where selling is profitable
                if (avgBuy >= avgSell) continue;

                // Find cheapest seller for this resource
                Market seller = __instance.GetBestSellerFromList(resource, friendlyMarkets);
                if (seller == null) continue;

                int buyPrice = seller.GetCurrentPrice(resource, true);

                // Find best buyer for this resource
                int sellPrice = 0;
                Market buyer = __instance.GetBestBuyerFromList(resource, friendlyMarkets, ref sellPrice);
                if (buyer == null || sellPrice <= buyPrice) continue;

                // Score = profit per unit * available quantity at seller
                float available = seller.GetQuantityAvailable(resource);
                if (available <= 0f) continue;

                float profitPerUnit = sellPrice - buyPrice;
                float score = profitPerUnit * Mathf.Min(available, 100f); // cap to avoid huge-volume low-margin dominating

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
                Plugin.Log.LogDebug($"[ProfitOptimizer] Best trade: buy {bestResource.resourceName} (score: {bestScore:F0})");
            }
            else
            {
                __result = null;
            }

            return false; // skip original
        }
    }
}
