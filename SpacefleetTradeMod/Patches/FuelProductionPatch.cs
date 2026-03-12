using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace SpacefleetTradeMod.Patches
{
    /// <summary>
    /// Boosts fuel production at designated stations:
    /// - Sao Felix: DT fuel recipes run at 100x output
    /// - New Anchorage: DH fuel recipes run at 100x output
    /// 
    /// Works by patching Factory.ExecuteBatchProduction to multiply output quantities
    /// for fuel recipes at matching stations.
    /// </summary>
    [HarmonyPatch(typeof(Factory), "ExecuteBatchProduction")]
    public static class FuelProductionPatch
    {
        static void Prefix(Factory __instance, ProductionRecipe recipe)
        {
            // We don't modify the recipe itself (it's a ScriptableObject shared by all factories).
            // Instead we store the multiplier so the Postfix can add the extra output.
            int multiplier = GetFuelMultiplier(__instance, recipe);
            FuelProductionState.PendingMultiplier = multiplier;
            FuelProductionState.PendingFactory = __instance;
        }

        static void Postfix(Factory __instance, ProductionRecipe recipe)
        {
            int multiplier = FuelProductionState.PendingMultiplier;
            if (multiplier <= 1) return;

            // The original already ran once (1x). Add the remaining (multiplier - 1)x.
            var inventory = Traverse.Create(__instance).Field("inventory").GetValue<ResourceInventory>();
            if (inventory == null) return;

            int extraRuns = multiplier - 1;
            foreach (var output in recipe.Outputs)
            {
                inventory.AddResource(output.resource, output.quantity * extraRuns);
            }
            // Also consume extra inputs
            foreach (var input in recipe.Inputs)
            {
                inventory.RemoveResource(input.resource, input.quantity * extraRuns);
            }

            FuelProductionState.PendingMultiplier = 1;
            FuelProductionState.PendingFactory = null;
        }

        private static int GetFuelMultiplier(Factory factory, ProductionRecipe recipe)
        {
            var track = factory.GetComponent<Track>();
            if (track == null) return 1;

            string stationName = track.publicName ?? "";
            bool producesDT = false;
            bool producesDH = false;

            foreach (var output in recipe.Outputs)
            {
                if (output.resource == null) continue;
                if (output.resource.type == ResourceType.DT_FUEL) producesDT = true;
                if (output.resource.type == ResourceType.DH_FUEL) producesDH = true;
            }

            string dtStation = Plugin.DTFuelStationName.Value;
            string dhStation = Plugin.DHFuelStationName.Value;
            int mult = Plugin.FuelProductionMultiplier.Value;

            if (producesDT && stationName.Contains(dtStation))
                return mult;
            if (producesDH && stationName.Contains(dhStation))
                return mult;

            return 1;
        }
    }

    internal static class FuelProductionState
    {
        public static int PendingMultiplier = 1;
        public static Factory PendingFactory = null;
    }
}
