using HarmonyLib;
using UnityEngine;

namespace SpacefleetTradeMod.Patches
{
    /// <summary>
    /// Overrides fuel pricing so that:
    /// - When a station has zero fuel stock, price = basePrice * 1.20 (+20%)
    /// - When a station is full of fuel, price = basePrice (no markup)
    /// - Linear interpolation between those two extremes based on stock ratio
    /// 
    /// Only applies to DT_FUEL, DH_FUEL, and VOLATILES resource types.
    /// Non-fuel resources use the original pricing formula unchanged.
    /// </summary>
    [HarmonyPatch(typeof(Market), nameof(Market.GetCurrentPrice))]
    public static class FuelPricingPatch
    {
        static void Postfix(Market __instance, ref int __result, ResourceDefinition resource, bool isBuying)
        {
            if (resource == null) return;
            if (resource.type != ResourceType.DT_FUEL &&
                resource.type != ResourceType.DH_FUEL &&
                resource.type != ResourceType.VOLATILES)
                return;

            // Calculate stock ratio: 0 = empty, 1 = full
            float stockQty = __instance.inventory.GetQuantityOf(resource);
            float maxQty = __instance.inventory.storageMax;
            if (maxQty <= 0f) return;

            float stockRatio = Mathf.Clamp01(stockQty / maxQty);

            // Lerp: empty (ratio=0) -> basePrice * multiplier, full (ratio=1) -> basePrice
            float emptyMult = Plugin.FuelPriceEmptyMultiplier.Value; // default 1.20
            float priceMult = Mathf.Lerp(emptyMult, 1.0f, stockRatio);

            __result = Mathf.RoundToInt(resource.basePrice * priceMult);
        }
    }
}
