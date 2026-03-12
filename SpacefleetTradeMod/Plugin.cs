using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace SpacefleetTradeMod
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGUID = "com.modder.spacefleet.trademod";
        public const string PluginName = "Spacefleet Trade Optimizer";
        public const string PluginVersion = "1.0.0";

        internal static ManualLogSource Log;

        // --- Config entries ---
        public static ConfigEntry<float> FuelAvoidanceThreshold;
        public static ConfigEntry<float> FuelPriceEmptyMultiplier;
        public static ConfigEntry<int> FuelProductionMultiplier;
        public static ConfigEntry<string> DTFuelStationName;
        public static ConfigEntry<string> DHFuelStationName;

        private void Awake()
        {
            Log = Logger;

            FuelAvoidanceThreshold = Config.Bind("Trading", "FuelAvoidanceThreshold", 0.20f,
                "Fleet fuel ratio below which stations without fuel for sale are avoided (0.20 = 20%)");

            FuelPriceEmptyMultiplier = Config.Bind("Fuel Pricing", "FuelPriceEmptyMultiplier", 1.20f,
                "Price multiplier when station fuel stock is empty (1.20 = +20%). At full stock, price = base price.");

            FuelProductionMultiplier = Config.Bind("Fuel Production", "FuelProductionMultiplier", 100,
                "Production multiplier for designated fuel stations (Sao Felix for DT, New Anchorage for DH)");

            DTFuelStationName = Config.Bind("Fuel Production", "DTFuelStationName", "Sao Felix",
                "Station publicName that gets boosted DT fuel production");

            DHFuelStationName = Config.Bind("Fuel Production", "DHFuelStationName", "New Anchorage",
                "Station publicName that gets boosted DH fuel production");

            var harmony = new Harmony(PluginGUID);
            harmony.PatchAll();

            Logger.LogInfo($"{PluginName} v{PluginVersion} loaded.");
        }
    }
}
