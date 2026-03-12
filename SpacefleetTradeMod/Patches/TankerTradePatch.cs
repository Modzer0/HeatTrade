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
    /// ModifyResourceInCargo is patched (in ProfitOptimizerPatch.cs) to route fuel
    /// add/remove through the virtual tank for tankers, and block fuel for non-tankers.
    /// </summary>
    public static class VirtualFuelTank
    {
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
                existing.SetStorageMax(capacity);
                return existing;
            }

            var go = new GameObject($"VirtualFuelTank_{trader.GetInstanceID()}");
            go.transform.SetParent(trader.transform);
            go.hideFlags = HideFlags.HideAndDontSave;

            var inv = go.AddComponent<ResourceInventory>();
            inv.allResources = GlobalMarket.current.allResources;
            inv.SetStorageMax(capacity);
            inv.InitResources();

            tanks[trader] = inv;
            Plugin.Log.LogInfo($"Created virtual fuel tank for {trader.name} capacity={capacity}");
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
    /// After SetCargoStorages collects CARGO modules, inject the virtual fuel tank.
    /// Tank capacity = sum of all FUEL module storage across the fleet (configurable multiplier).
    /// </summary>
    [HarmonyPatch(typeof(Trader), nameof(Trader.SetCargoStorages))]
    public static class TankerSetCargoPatch
    {
        static void Prefix(Trader __instance, out bool __state)
        {
            __state = false;
            var fleet = __instance.GetComponent<FleetManager>();
            if (fleet == null) return;

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

            if (!__instance.cargoStorages.Contains(virtualTank))
            {
                __instance.cargoStorages.Add(virtualTank);
                __instance.totalStorageSpace += virtualTank.storageMax;
            }
        }
    }

    /// <summary>
    /// Suppress "No cargo ships in fleet!" notification for tanker fleets.
    /// </summary>
    [HarmonyPatch(typeof(Trader), "Notify")]
    public static class TankerSuppressNoCargoWarning
    {
        static bool Prefix(Trader __instance, string message)
        {
            if (message != null && message.Contains("No cargo") && VirtualFuelTank.Get(__instance) != null)
                return false;
            return true;
        }
    }

    /// <summary>
    /// Completely replaces TradeCycle for tankers that have a virtual fuel tank.
    /// Only evaluates DT_FUEL and DH_FUEL. Targets 15% profit margin when possible.
    /// Sets buyPrice/bestSellPrice so the UI displays correct values.
    /// </summary>
    [HarmonyPatch(typeof(Trader), "TradeCycle")]
    public static class TankerTradeCycleOverride
    {
        private const float MinProfitMarginPct = 0.15f;

        static bool Prefix(Trader __instance, ref bool __result)
        {
            var tank = VirtualFuelTank.Get(__instance);
            if (tank == null) return true; // Not a tanker

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

            // Check what fuel is in cargo (virtual tank + any cargo modules)
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
                __instance.profitMargin = __instance.buyPrice > 0
                    ? 1f - (float)__instance.buyPrice / (float)__instance.bestSellPrice
                    : 0f;

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
                // BUYING — only consider DT_FUEL and DH_FUEL with 15% margin preference
                __instance.isBuying = true;
                __instance.targetResource = null;
                __instance.targetQuantity = __instance.totalStorageSpace;

                var dtFuel = gm.allResources.GetResource(ResourceType.DT_FUEL);
                var dhFuel = gm.allResources.GetResource(ResourceType.DH_FUEL);

                Market bestMarket = null;
                ResourceDefinition bestResource = null;
                float bestScore = float.MinValue;
                int bestBuyPrice = 0;
                int bestSellPriceVal = 0;

                // Fallback for trades below 15% margin
                Market fallbackMarket = null;
                ResourceDefinition fallbackResource = null;
                float fallbackScore = float.MinValue;
                int fallbackBuyPrice = 0;
                int fallbackSellPrice = 0;

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

                    float distToSeller = Vector3.Distance(__instance.transform.position, seller.transform.position);
                    float distSellerToBuyer = Vector3.Distance(seller.transform.position, buyer.transform.position);
                    var fleet = __instance.GetComponent<FleetManager>();
                    float accG = (fleet != null && fleet.maxAccG > 0f) ? fleet.maxAccG : 1f;
                    float travelDays = ProfitOptimizerPatch.EstimateTravelTime(distToSeller + distSellerToBuyer, accG);
                    float timeTiebreaker = 1f / (1f + travelDays);

                    float score = totalProfit + timeTiebreaker;
                    bool meetsMargin = sellPriceAt >= buyPriceAt * (1f + MinProfitMarginPct);

                    if (meetsMargin)
                    {
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMarket = seller;
                            bestResource = fuel;
                            bestBuyPrice = buyPriceAt;
                            bestSellPriceVal = sellPriceAt;
                        }
                    }
                    else
                    {
                        if (score > fallbackScore)
                        {
                            fallbackScore = score;
                            fallbackMarket = seller;
                            fallbackResource = fuel;
                            fallbackBuyPrice = buyPriceAt;
                            fallbackSellPrice = sellPriceAt;
                        }
                    }
                }

                // Prefer 15%+ margin trades; fall back if none available
                if (bestMarket != null && bestResource != null)
                {
                    __instance.targetMarket = bestMarket;
                    __instance.targetResource = bestResource;
                    __instance.buyPrice = bestBuyPrice;
                    __instance.bestSellPrice = bestSellPriceVal;
                    __instance.profitMargin = 1f - (float)bestBuyPrice / (float)bestSellPriceVal;
                    Plugin.Log.LogDebug($"[TankerTrade] Buy {bestResource.resourceName} at {bestBuyPrice}, sell at {bestSellPriceVal}, margin={__instance.profitMargin:P1}");
                }
                else if (fallbackMarket != null && fallbackResource != null)
                {
                    __instance.targetMarket = fallbackMarket;
                    __instance.targetResource = fallbackResource;
                    __instance.buyPrice = fallbackBuyPrice;
                    __instance.bestSellPrice = fallbackSellPrice;
                    __instance.profitMargin = fallbackBuyPrice > 0
                        ? 1f - (float)fallbackBuyPrice / (float)fallbackSellPrice
                        : 0f;
                    Plugin.Log.LogDebug($"[TankerTrade] Fallback: {fallbackResource.resourceName} buy={fallbackBuyPrice} sell={fallbackSellPrice}");
                }
            }

            __result = __instance.targetMarket != null
                    && __instance.targetResource != null
                    && __instance.targetQuantity != 0f;
            return false;
        }
    }

    /// <summary>
    /// Recalculate fleet DV after any trade completes.
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
