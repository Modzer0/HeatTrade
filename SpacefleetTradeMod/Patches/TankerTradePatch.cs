using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace SpacefleetTradeMod.Patches
{
    /// <summary>
    /// Allows tanker fleets (ships with FUEL modules) to trade fuel resources.
    /// 
    /// The vanilla Trader.SetCargoStorages only collects CARGO-type modules.
    /// This patch adds FUEL-type module inventories to the cargo list so that
    /// fuel stored in tanker ships can be bought/sold at markets.
    /// 
    /// The Trader.TradeCycle will then naturally discover fuel in the cargo list
    /// and attempt to sell it, or buy fuel to fill the tanker's capacity.
    /// </summary>
    [HarmonyPatch(typeof(Trader), nameof(Trader.SetCargoStorages))]
    public static class TankerTradePatch
    {
        static void Postfix(Trader __instance)
        {
            var fleet = __instance.GetComponent<FleetManager>();
            if (fleet == null) return;

            foreach (var ship in fleet.ships)
            {
                var fuelModules = ship.GetModulesOfType(PartType.FUEL);
                foreach (var fuelModule in fuelModules)
                {
                    if (fuelModule.inv == null) continue;

                    // Avoid adding duplicates if the same inventory is already listed
                    if (__instance.cargoStorages.Contains(fuelModule.inv)) continue;

                    __instance.cargoStorages.Add(fuelModule.inv);
                    __instance.totalStorageSpace += fuelModule.inv.storageMax;
                }
            }
        }
    }

    /// <summary>
    /// When a tanker fleet sells fuel, we need to make sure the fuel is removed
    /// from the fuel module inventory (not just cargo). The vanilla ModifyResourceInCargo
    /// already iterates cargoStorages which now includes fuel modules (from the patch above),
    /// so this works automatically.
    /// 
    /// However, we also need to ensure the fleet's DV is recalculated after fuel is
    /// traded away, since selling fuel from the tank reduces delta-v.
    /// </summary>
    [HarmonyPatch(typeof(Trader), "Trade")]
    public static class TankerDvUpdatePatch
    {
        static void Postfix(Trader __instance)
        {
            var fleet = __instance.GetComponent<FleetManager>();
            if (fleet != null)
            {
                fleet.UpdateDv();
                fleet.UpdateFleet();
            }
        }
    }
}
