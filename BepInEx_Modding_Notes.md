# BepInEx Modding Notes — Spacefleet: Heat Death

## Game Technical Profile

- Engine: Unity (Mono backend)
- Assembly: `Assembly-CSharp.dll`
- Location: `Spacefleet - Heat Death_Data\Managed\`
- .NET Target: Standard (netstandard reference in csproj, no explicit TFM — likely .NET Standard 2.0 / .NET Framework 4.x era)
- Rendering: Universal Render Pipeline (URP)
- UI: Unity UI + TextMeshPro
- All game classes are in the global namespace (no namespaces used)
- All gameplay classes inherit from `MonoBehaviour` — they're Unity components on GameObjects

## BepInEx Setup

### Installation

1. Download [BepInEx 5.x stable](https://github.com/BepInEx/BepInEx/releases) (the Unity Mono x64 build)
2. Extract into the game root folder (where the `.exe` lives)
3. Run the game once to let BepInEx generate its folder structure:
   - `BepInEx/plugins/` — drop compiled plugin DLLs here
   - `BepInEx/config/` — auto-generated config files
   - `BepInEx/patchers/` — preloader patchers (advanced)
4. Enable the console for debugging: edit `BepInEx/config/BepInEx.cfg`, set `[Logging.Console] Enabled = true`

### Plugin Project Setup

Create a C# Class Library project targeting `net472` (or `netstandard2.0`). Reference these DLLs from the game's `Managed` folder:

- `BepInEx.dll` (from BepInEx install)
- `0Harmony.dll` (from BepInEx install — this is HarmonyX)
- `Assembly-CSharp.dll` (the game)
- `UnityEngine.CoreModule.dll`
- Any other `UnityEngine.*.dll` modules you need

Minimal `.csproj` references:
```xml
<ItemGroup>
  <Reference Include="BepInEx" HintPath="$(GameDir)\BepInEx\core\BepInEx.dll" Private="false" />
  <Reference Include="0Harmony" HintPath="$(GameDir)\BepInEx\core\0Harmony.dll" Private="false" />
  <Reference Include="Assembly-CSharp" HintPath="$(GameDir)\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll" Private="false" />
  <Reference Include="UnityEngine.CoreModule" HintPath="$(GameDir)\Spacefleet - Heat Death_Data\Managed\UnityEngine.CoreModule.dll" Private="false" />
</ItemGroup>
```

## Plugin Skeleton

```csharp
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

[BepInPlugin("com.yourname.pluginid", "Plugin Name", "1.0.0")]
public class MyPlugin : BaseUnityPlugin
{
    internal static ManualLogSource Log;

    private void Awake()
    {
        Log = Logger;
        Logger.LogInfo("Plugin loaded");

        var harmony = new Harmony("com.yourname.pluginid");
        harmony.PatchAll(); // auto-discovers [HarmonyPatch] classes in this assembly
    }
}
```

## Harmony Patching Primer

BepInEx 5 ships with HarmonyX (a fork of Harmony 2). Three main patch types:

### Prefix — runs before the original method
- Can read/modify arguments (use `ref` keyword)
- Can skip the original by returning `false`
- Can set `__result` to override the return value
- Use sparingly — skipping originals breaks other mods patching the same method

```csharp
[HarmonyPatch(typeof(TargetClass), nameof(TargetClass.TargetMethod))]
class MyPatch
{
    static bool Prefix(ref float someArg, ref bool __result)
    {
        someArg *= 2f; // modify argument
        // return true to let original run, false to skip it
        return true;
    }
}
```

### Postfix — runs after the original method
- Can read/modify the return value via `ref __result`
- Safer than Prefix for most use cases — doesn't block other patches
- Can access the instance via `__instance`

```csharp
[HarmonyPatch(typeof(TargetClass), nameof(TargetClass.TargetMethod))]
class MyPatch
{
    static void Postfix(TargetClass __instance, ref int __result)
    {
        __result += 10; // modify return value
    }
}
```

### Transpiler — modifies IL instructions directly
- Most powerful but most complex
- Operates on `IEnumerable<CodeInstruction>`
- Use when Prefix/Postfix can't achieve what you need (e.g., changing logic mid-method)

### Special Parameters
- `__instance` — the object the method is called on (for instance methods)
- `__result` — the return value (use `ref` to modify)
- `__state` — pass state from Prefix to Postfix (same class only)
- Named parameters matching the original method's parameter names — injected automatically

## Key Classes to Patch (from Decompiled Source)

All classes are public and in the global namespace, making them straightforward to patch.

### Trading System

| Class | Key Methods / Fields | Mod Potential |
|---|---|---|
| `Trader` | `TradeCycle()`, `Trade()`, `StartTradeCycle()`, `OnArrival()`, `NewHour()` | Change trade logic, profit margins, resource selection |
| `Trader` | `minProfitMargin` (0.15), `currentMinProfitMargin`, `isBuying`, `targetResource`, `targetQuantity` | Configurable via Postfix on `Start()` or direct field access |
| `Market` | `GetCurrentPrice()`, `ExecuteTrade()`, `CanBuy()` | Modify pricing, transaction limits, spread |
| `Market` | `transactionSpread` (0.1), `idealStockRatio` (0.5), `resupplyRatePerHour` | Tweak market economics |
| `GlobalMarket` | `GetBestBuyerOfExceptHostile()`, `GetBestSellerAnyExceptHostile()`, `GetBestSellerFromList()` | Change how trade routes are selected |
| `Factory` | `ProductionCycle()`, `ExecuteBatchProduction()` | Modify production rates, recipes |

### Fleet & Navigation

| Class | Key Methods / Fields | Mod Potential |
|---|---|---|
| `FleetManager` | `SetStateTo()`, `Refuel()`, `Repair()`, `Resupply()` | Change fleet behavior, state transitions |
| `FleetManager` | `fleetDv`, `fleetDvMax`, `maxAccG` | Modify fleet performance |
| `Navigation` | `StartFlight()`, `Accelerate()`, `Decelerate()`, `accG`, `accRatio`, `decRatio` | Change flight mechanics |

### Economy & Factions

| Class | Key Methods / Fields | Mod Potential |
|---|---|---|
| `FactionsManager` | `ModPlayerCredits()`, `GetFactionFromID()` | Credit manipulation, faction relations |
| `Faction` | `credits`, `GetHostiles()` | Modify faction wealth, diplomacy |
| `ResourceDefinition` | `basePrice`, `resourceName`, `type` | Change base resource economics |

### Save/Load

| Class | Key Methods / Fields | Mod Potential |
|---|---|---|
| `GameSaver` | Serializes all fleet/trader/market state | Add custom save data |
| `GameLoader` | `LoadFleetTraderData()`, `InitTrader()` | Inject state on load |

## Practical Mod Examples

### Example 1: Configurable Profit Margin

```csharp
[BepInPlugin("com.example.tradetweak", "Trade Tweaker", "1.0.0")]
public class TradeTweakPlugin : BaseUnityPlugin
{
    public static ConfigEntry<float> ProfitMargin;

    private void Awake()
    {
        ProfitMargin = Config.Bind("Trading", "MinProfitMargin", 0.15f,
            "Minimum profit margin for auto-trading (0.15 = 15%)");
        new Harmony("com.example.tradetweak").PatchAll();
    }
}

[HarmonyPatch(typeof(Trader), "Start")]
class TraderStartPatch
{
    static void Postfix(Trader __instance)
    {
        __instance.minProfitMargin = TradeTweakPlugin.ProfitMargin.Value;
        __instance.currentMinProfitMargin = TradeTweakPlugin.ProfitMargin.Value;
    }
}
```

### Example 2: Override Market Pricing

```csharp
[HarmonyPatch(typeof(Market), nameof(Market.GetCurrentPrice))]
class PricePatch
{
    static void Postfix(ref int __result, ResourceDefinition resource, bool isBuying)
    {
        // Example: 20% discount on all buy prices
        if (isBuying)
            __result = Mathf.RoundToInt(__result * 0.8f);
    }
}
```

### Example 3: Log All Trades

```csharp
[HarmonyPatch(typeof(Market), nameof(Market.ExecuteTrade))]
class TradeLoggerPatch
{
    static void Prefix(ResourceDefinition resource, float quantity)
    {
        MyPlugin.Log.LogInfo($"Trade: {(quantity > 0 ? "BUY" : "SELL")} " +
            $"{Mathf.Abs(quantity):F1}t of {resource.resourceName}");
    }
}
```

### Example 4: Remove Hostile Faction Trade Restriction

```csharp
[HarmonyPatch(typeof(GlobalMarket), nameof(GlobalMarket.GetBestBuyerOfExceptHostile))]
class TradeWithAnyonePatch
{
    static bool Prefix(GlobalMarket __instance, ResourceDefinition resource,
        ref int bestPrice, ref Market __result)
    {
        // Bypass hostility check — use the unrestricted version instead
        __result = __instance.GetBestBuyerOf(resource, ref bestPrice);
        return false; // skip original
    }
}
```

## Accessibility Notes for Modders

### Private Fields
Many important fields are `private`. Access them via Harmony's `AccessTools`:
```csharp
var traderField = AccessTools.Field(typeof(FleetManager), "trader");
Trader trader = (Trader)traderField.GetValue(fleetManagerInstance);
```

Or use Harmony's `Traverse`:
```csharp
var trader = Traverse.Create(fleetManagerInstance).Field("trader").GetValue<Trader>();
```

### Coroutines
`Trader.StartTradeCycle()` is a coroutine (`IEnumerator`). Patching coroutines with Harmony is tricky because the compiler generates a state machine class. Options:
- Patch the methods the coroutine calls (e.g., `TradeCycle`, `StartFlight`) instead
- Use a Prefix on the coroutine method to prevent it from starting and run your own
- Use a Transpiler on the generated `MoveNext()` method (advanced)

### MonoBehaviour Lifecycle
All game classes are `MonoBehaviour` components. They're created by Unity, not by `new`. To find instances at runtime:
```csharp
var allTraders = UnityEngine.Object.FindObjectsOfType<Trader>();
var globalMarket = GlobalMarket.current; // singleton pattern
```

### Events / Delegates
The game uses C# events for timing hooks:
- `TimeManager.NewHour` — fires every game hour
- `TimeManager.NewDay` — fires every game day
- `Navigation.onArrival` — fires when a fleet arrives

You can subscribe to these from your plugin for non-invasive hooks.

## Build & Deploy Workflow

1. Build your plugin DLL
2. Copy it to `<GameRoot>/BepInEx/plugins/`
3. Launch the game
4. Check `BepInEx/LogOutput.log` for your plugin's log messages
5. Config files auto-generate in `BepInEx/config/` on first run

## Tips

- Use `BepInEx.Configuration` (`Config.Bind`) to make values user-configurable without recompiling
- The game uses `netstandard` — you have access to modern C# features (LINQ, async, etc.)
- All game classes are non-sealed and public — subclassing is possible but rarely needed with Harmony
- The `GlobalMarket.current` singleton gives you access to all registered markets at runtime
- `TimeManager.current` gives you the game clock for scheduling
- Test patches on `Postfix` first — they're safer and compose better with other mods
- For UI mods, the game uses Unity UI (Canvas-based) with TextMeshPro — reference `UnityEngine.UI.dll` and `Unity.TextMeshPro.dll`
