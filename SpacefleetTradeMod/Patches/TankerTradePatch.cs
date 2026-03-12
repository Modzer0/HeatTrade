using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace SpacefleetTradeMod.Patches
{
    /// <summary>
    /// Manages a virtual fuel cargo tank per Trader so tanker fleets can trade
    /// fuel (DT, DH, Volatiles, Antimatter) without touching their propulsion supply.
    ///
    /// A hidden GameObject with a real ResourceInventory component is created per Trader.
    /// This virtual tank is injected into cargoStorages so TradeCycle discovers fuel cargo.
    /// ModifyResourceInCargo is patched to route fuel add/remove through the virtual tank
    /// instead of the ship's actual CARGO modules (which don't hold fuel) or FUEL modules
    /// (which are the propulsion supply).
    /// </summary>
    public static class VirtualFuelTank
    {
        // Map each Trader instance to its virtual fuel tank inventory
        private static readonly Dictionary<Trader, ResourceInventory> tanks = new Dictionary<Trader, ResourceInventory>();

        private static readonly HashSet<ResourceType> fuelTypes = new HashSet<ResourceType>
        {
            ResourceType.DT_FUEL,
            ResourceType.DH_FUEL,
            ResourceType.VOLATILES,
            ResourceType.ANTIMATTER
        };

        public static bool IsFuelResource(ResourceDefinition res)
        {
            return res != null && fuelTypes.Contains(res.type);
        }

        public static ResourceInventory GetOrCreate(Trader trader, float capacity)
        {
            if (tanks.TryGetValue(trader, out var existing) && existing != null)
            {
                // Update capacity in case fleet changed
                existing.SetStorageMax(capacity);
                return existing;
            }

            // Create a hidden GameObject that lives as a child of the Trader
            var go = new GameObject($"VirtualFuelTank_{trader.GetInstanceID()}");
            go.transform.SetParent(trader.transform);
            go.hideFlags = HideFlags.HideAndDontSave;

            var inv = go.AddComponent<ResourceInventory>();
            inv.allResources = GlobalMarket.current.allResources;
            inv.SetStorageMax(capacity);
            inv.InitResources();

            tanks[trader] = inv;
            Plugin.Log.LogInfo($"Created virtual fuel tank for {trader.name} with capacity {capacity}");
            return inv;
        }

        public static ResourceInventory Get(Trader trader)
        {
            tanks.TryGetValue(trader, out var inv);
            return inv;
        }

        public static void Remove(Trader trader)
        {
            if (tanks.TryGetValue(trader, out var inv))
            {
                if (inv != null && inv.gameObject != null)
                    Object.Destroy(inv.gameObject);
                tanks.Remove(trader);
            }
        }
    }

    /// <summary>
    /// After SetCargoStorages collects CARGO modules, we inject the virtual fuel tank.
    /// Tank capacity = sum of all FUEL module storage across the fleet (configurable multiplier).
    /// </summary>
    [HarmonyPatch(typeof(Trader), nameof(Trader.SetCargoStorages))]
    public static class TankerSetCargoPatch
    {
        static void Postfix(Trader __instance)
        {
            var fleet = __instance.GetComponent<FleetManager>();
            if (fleet == null) return;

            // Calculate total fuel module capacity across the fleet
            float totalFuelCapacity = 0f;
            foreach (var ship in fleet.ships)
            {
                var fuelModules = ship.GetModulesOfType(PartType.FUEL);
                foreach (var fuelModule in fuelModules)
                {
                    if (fuelModule.inv != null)
                        totalFuelCapacity += fuelModule.inv.storageMax;
                }
            }

            if (totalFuelCapacity <= 0f) return;

            float tankCapacity = totalFuelCapacity * Plugin.VirtualTankCapacityMultiplier.Value;
            var virtualTank = VirtualFuelTank.GetOrCreate(__instance, tankCapacity);

            // Add to cargoStorages if not already present
            if (!__instance.cargoStorages.Contains(virtualTank))
            {
                __instance.cargoStorages.Add(virtualTank);
                __instance.totalStorageSpace += virtualTank.storageMax;
            }
        }
    }

    /// <summary>
    /// Patch ModifyResourceInCargo to route fuel resources through the virtual tank
    /// instead of the ship's CARGO modules (which normally don't hold fuel).
    /// 
    /// The original method iterates fleet.ships → GetModulesOfType(CARGO) → AddCargo/RemoveCargo.
    /// We intercept: if the resource is a fuel type and a virtual tank exists, we handle it
    /// ourselves and skip the original method.
    /// </summary>
    [HarmonyPatch(typeof(Trader), "ModifyResourceInCargo")]
    public static class TankerModifyResourcePatch
    {
        static bool Prefix(Trader __instance, ResourceDefinition resource, float quantity)
        {
            if (!VirtualFuelTank.IsFuelResource(resource))
                return true; // Not fuel — let original handle it

            var tank = VirtualFuelTank.Get(__instance);
            if (tank == null)
                return true; // No virtual tank — let original handle it

            bool adding = quantity >= 0f;
            if (adding)
            {
                float added = tank.AddResource(resource, quantity);
                quantity -= added;

                // If virtual tank is full, overflow into regular cargo modules
                if (quantity > 0f)
                    FallbackToCargoModules(__instance, resource, quantity);
            }
            else
            {
                float absQty = Mathf.Abs(quantity);
                float removed = tank.RemoveResource(resource, absQty);
                absQty -= removed;

                // If virtual tank didn't have enough, pull from regular cargo too
                if (absQty > 0f)
                    FallbackToCargoModules(__instance, resource, -absQty);
            }

            return false; // Skip original
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

    /// <summary>
    /// Recalculate fleet DV after any trade completes, since fuel trading
    /// could affect fleet mass calculations indirectly.
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

    /// <summary>
    /// Clean up virtual tank when Trader is destroyed.
    /// </summary>
    [HarmonyPatch(typeof(Trader), "OnDestroy")]
    public static class TankerCleanupPatch
    {
        static void Postfix(Trader __instance)
        {
            VirtualFuelTank.Remove(__instance);
        }
    }
}
