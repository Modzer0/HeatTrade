using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace SpacefleetTradeMod.Patches
{
    /// <summary>
    /// Manages a virtual fuel cargo tank per Trader so tanker fleets can trade
    /// DT and DH fuel without touching their propulsion supply.
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
            ResourceType.DH_FUEL
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
    /// Also suppresses the "No cargo ships in fleet!" warning for tankers since
    /// the virtual tank counts as cargo.
    /// </summary>
    [HarmonyPatch(typeof(Trader), nameof(Trader.SetCargoStorages))]
    public static class TankerSetCargoPatch
    {
        // Prefix: temporarily add a dummy to prevent the "No cargo" notification
        // from firing inside the original method for tanker fleets.
        static void Prefix(Trader __instance, out bool __state)
        {
            __state = false;
            var fleet = __instance.GetComponent<FleetManager>();
            if (fleet == null) return;

            // Check if this fleet has fuel modules (i.e. is a tanker)
            foreach (var ship in fleet.ships)
            {
                if (ship.GetModulesOfType(PartType.FUEL).Count > 0)
                {
                    __state = true;
                    return;
                }
            }
        }

        static void Postfix(Trader __instance, bool __state)
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
    /// Suppress the "No cargo ships in fleet!" notification for tanker fleets
    /// that have a virtual fuel tank. The original Notify fires inside SetCargoStorages
    /// before our postfix can add the virtual tank.
    /// </summary>
    [HarmonyPatch(typeof(Trader), "Notify")]
    public static class TankerSuppressNoCargoWarning
    {
        static bool Prefix(Trader __instance, string message)
        {
            // Only suppress the specific "no cargo" warning for tankers
            if (message != null && message.Contains("No cargo") && VirtualFuelTank.Get(__instance) != null)
                return false; // Skip notification
            return true;
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
    /// Completely replaces TradeCycle for tankers that have a virtual fuel tank.
    /// Only evaluates DT_FUEL and DH_FUEL — no other resources are considered.
    /// This prevents the profit optimizer from picking non-fuel trades.
    /// </summary>
    [HarmonyPatch(typeof(Trader), "TradeCycle")]
    public static class TankerTradeCycleOverride
    {
        static bool Prefix(Trader __instance, ref bool __result)
        {
            var tank = VirtualFuelTank.Get(__instance);
            if (tank == null) return true; // Not a tanker — let original + profit optimizer run

            if (!__instance.isTrading)
            {
                __result = false;
                return false;
            }

            __instance.targetMarket = null;
            __instance.bestSellPrice = 0;
            __instance.SetCargoStorages();

            __instance.targetResource = null;
            __instance.targetQuantity = 0f;
            __instance.totalStorageSpace = 0f;

            // Check what's in our cargo (virtual tank + any cargo modules)
            bool hasCargo = false;
            foreach (var cargoStorage in __instance.cargoStorages)
            {
                if (cargoStorage.GetTotalQuantity() > 0f)
                {
                    var res = cargoStorage.GetResource();
                    if (res != null && VirtualFuelTank.IsFuelResource(res))
                    {
                        if (__instance.targetResource == null)
                        {
                            hasCargo = true;
                            __instance.targetResource = res;
                            __instance.targetQuantity += cargoStorage.GetQuantityOf(res);
                        }
                        else if (res == __instance.targetResource)
                        {
                            __instance.targetQuantity += cargoStorage.GetQuantityOf(res);
                        }
                    }
                }
                __instance.totalStorageSpace += cargoStorage.storageMax;
            }

            if (__instance.totalStorageSpace == 0f)
            {
                __result = false;
                return false;
            }

            var gm = GlobalMarket.current;
            var factionID = __instance.GetComponent<Track>().GetFaction().factionID;
            var fm = Traverse.Create(gm).Field("fm").GetValue<FactionsManager>();
            var hostiles = fm.GetFactionFromID(factionID).GetHostiles();
            var allMarkets = Traverse.Create(gm).Field("allMarkets").GetValue<List<Market>>();

            // Build friendly market list
            var friendly = new List<Market>();
            foreach (var m in allMarkets)
            {
                if (m != null && !hostiles.Contains(m.thisFaction.factionID))
                    friendly.Add(m);
            }

            if (hasCargo && __instance.targetResource != null && __instance.targetQuantity > 0f)
            {
                // SELLING — find best buyer for the fuel we're carrying
                __instance.isBuying = false;
                int bestPrice = 0;
                __instance.targetMarket = gm.GetBestBuyerFromList(__instance.targetResource, friendly, ref bestPrice);
                __instance.bestSellPrice = bestPrice > 0 ? bestPrice : 1;
                __instance.profitMargin = 1f - (float)__instance.buyPrice / (float)__instance.bestSellPrice;

                if (__instance.bestSellPrice < __instance.buyPrice * (1f + __instance.currentMinProfitMargin))
                {
                    __instance.currentMinProfitMargin /= 2f;
                    if (__instance.currentMinProfitMargin < 0.001f)
                        __instance.currentMinProfitMargin = -100000f;
                    __result = false;
                    return false;
                }
                __instance.currentMinProfitMargin = __instance.minProfitMargin;
            }
            else
            {
                // BUYING — only consider DT_FUEL and DH_FUEL
                __instance.isBuying = true;
                __instance.targetResource = null;
                __instance.targetQuantity = __instance.totalStorageSpace;

                var dtFuel = gm.allResources.GetResource(ResourceType.DT_FUEL);
                var dhFuel = gm.allResources.GetResource(ResourceType.DH_FUEL);

                Market bestMarket = null;
                ResourceDefinition bestResource = null;
                float bestScore = 0f;
                int bestBuyPrice = 0;
                int bestSellPrice = 0;

                foreach (var fuel in new[] { dtFuel, dhFuel })
                {
                    if (fuel == null) continue;

                    Market seller = gm.GetBestSellerFromList(fuel, friendly);
                    if (seller == null) continue;

                    int buyPriceAt = seller.GetCurrentPrice(fuel, true);

                    int sellPriceAt = 0;
                    Market buyer = gm.GetBestBuyerFromList(fuel, friendly, ref sellPriceAt);
                    if (buyer == null || sellPriceAt <= buyPriceAt) continue;

                    float available = seller.GetQuantityAvailable(fuel);
                    if (available <= 0f) continue;

                    float profitPerUnit = sellPriceAt - buyPriceAt;
                    float volume = Mathf.Min(available, __instance.totalStorageSpace);
                    float totalProfit = profitPerUnit * volume;

                    // Round-trip time: trader→seller + seller→buyer
                    float distToSeller = Vector3.Distance(__instance.transform.position, seller.transform.position);
                    float distSellerToBuyer = Vector3.Distance(seller.transform.position, buyer.transform.position);
                    float totalDist = distToSeller + distSellerToBuyer;

                    var fleet = __instance.GetComponent<FleetManager>();
                    float accG = (fleet != null && fleet.maxAccG > 0f) ? fleet.maxAccG : 1f;
                    float accKkm = accG * 73234.4f;
                    float travelTime = totalDist > 0f ? 2f * Mathf.Sqrt(totalDist / accKkm) : 0f;
                    float totalTime = travelTime + 0.5f;

                    float score = totalProfit / totalTime;
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMarket = seller;
                        bestResource = fuel;
                        bestBuyPrice = buyPriceAt;
                        bestSellPrice = sellPriceAt;
                    }
                }

                if (bestMarket != null && bestResource != null)
                {
                    __instance.targetMarket = bestMarket;
                    __instance.targetResource = bestResource;
                    __instance.buyPrice = bestBuyPrice;
                    __instance.bestSellPrice = bestSellPrice;
                    __instance.profitMargin = 1f - (float)bestBuyPrice / (float)bestSellPrice;
                }
            }

            __result = __instance.targetMarket != null
                    && __instance.targetResource != null
                    && __instance.targetQuantity != 0f;
            return false; // Skip original TradeCycle entirely
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
