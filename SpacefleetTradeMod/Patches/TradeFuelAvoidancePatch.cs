using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace SpacefleetTradeMod.Patches
{
    /// <summary>
    /// When a fleet is below the fuel threshold (default 20%), filter out markets
    /// that have no fuel available for the fleet's fuel type. This prevents traders
    /// from stranding themselves at stations where they can't refuel.
    /// 
    /// Patches GlobalMarket.GetBestBuyerFromList and GetBestSellerFromList to remove
    /// markets that can't supply fuel when the trading fleet is low on fuel.
    /// </summary>
    [HarmonyPatch(typeof(GlobalMarket), nameof(GlobalMarket.GetBestBuyerOfExceptHostile))]
    public static class BuyerFuelAvoidancePatch
    {
        static void Prefix(GlobalMarket __instance, int mainFactionID)
        {
            // Store the faction ID so the list-level patch can use it
            FuelAvoidanceState.CurrentFactionID = mainFactionID;
        }
    }

    [HarmonyPatch(typeof(GlobalMarket), nameof(GlobalMarket.GetBestSellerAnyExceptHostile))]
    public static class SellerFuelAvoidancePatch
    {
        static void Prefix(GlobalMarket __instance, int mainFactionID)
        {
            FuelAvoidanceState.CurrentFactionID = mainFactionID;
        }
    }

    [HarmonyPatch(typeof(GlobalMarket), nameof(GlobalMarket.GetBestBuyerFromList))]
    public static class BuyerListFuelFilterPatch
    {
        static void Prefix(ref List<Market> marketsToCheck)
        {
            marketsToCheck = FuelAvoidanceHelper.FilterMarkets(marketsToCheck);
        }
    }

    [HarmonyPatch(typeof(GlobalMarket), nameof(GlobalMarket.GetBestSellerFromList))]
    public static class SellerListFuelFilterPatch
    {
        static void Prefix(ref List<Market> marketsToCheck)
        {
            marketsToCheck = FuelAvoidanceHelper.FilterMarkets(marketsToCheck);
        }
    }

    internal static class FuelAvoidanceState
    {
        public static int CurrentFactionID = -1;
    }

    internal static class FuelAvoidanceHelper
    {
        public static List<Market> FilterMarkets(List<Market> markets)
        {
            // Find the Trader component that's currently running a trade cycle.
            // We look for active traders belonging to the faction that initiated the search.
            var allTraders = Object.FindObjectsOfType<Trader>();
            Trader activeTrader = null;

            foreach (var t in allTraders)
            {
                if (!t.isTrading) continue;
                var track = t.GetComponent<Track>();
                if (track != null && track.factionID == FuelAvoidanceState.CurrentFactionID)
                {
                    activeTrader = t;
                    break;
                }
            }

            if (activeTrader == null) return markets;

            var fleet = activeTrader.GetComponent<FleetManager>();
            if (fleet == null || fleet.fleetDvMax <= 0f) return markets;

            float fuelRatio = fleet.fleetDv / fleet.fleetDvMax;
            if (fuelRatio >= Plugin.FuelAvoidanceThreshold.Value) return markets;

            // Fleet is low on fuel — determine what fuel type it needs
            ResourceDefinition fleetFuelType = null;
            foreach (var ship in fleet.ships)
            {
                fleetFuelType = ship.GetFuelResource();
                if (fleetFuelType != null) break;
            }

            if (fleetFuelType == null) return markets;

            // Filter out markets that have zero of the fleet's fuel type
            var filtered = new List<Market>();
            foreach (var market in markets)
            {
                float fuelAvailable = market.GetQuantityOf(fleetFuelType);
                if (fuelAvailable > 0f)
                {
                    filtered.Add(market);
                }
                else
                {
                    var track = market.GetComponent<Track>();
                    string name = track != null ? track.publicName : "Unknown";
                    Plugin.Log.LogDebug($"[FuelAvoidance] Skipping {name} — no {fleetFuelType.resourceName} (fleet at {fuelRatio:P0})");
                }
            }

            // If filtering removed ALL options, fall back to unfiltered to avoid deadlock
            return filtered.Count > 0 ? filtered : markets;
        }
    }
}
