# Spacefleet Trade Optimizer Mod

BepInEx plugin for **Spacefleet: Heat Death** that overhauls the auto-trading economy.

## Features

### Profit-Maximized Trade Routes
Replaces the vanilla random resource selection with a scoring system that evaluates every resource by `(sellPrice - buyPrice) * availableQuantity` and picks the most profitable trade.

### Fuel Avoidance (Low Fuel Safety)
When a fleet's delta-v drops below 20% of max, markets with zero fuel stock are filtered out. Prevents traders from stranding at dry stations.

### Designated Fuel Production Hubs
- **Sao Felix** — DT fuel production at 100x normal output
- **New Anchorage** — DH fuel production at 100x normal output

### Tanker Fuel Trading
Tanker fleets (ships with FUEL modules) can now trade DT and DH fuel using a virtual fuel cargo tank that is separate from the ship's propulsion fuel supply. Tankers will only buy/sell DT_FUEL and DH_FUEL.

### Scarcity-Based Fuel Pricing
Fuel prices scale linearly with stock level:
- **Empty station** → basePrice × 1.20 (+20%)
- **Full station** → basePrice (no markup)

## Installation

### Prerequisites
- [BepInEx 5.4.x](https://github.com/BepInEx/BepInEx/releases) (Unity Mono, x64)

### Steps
1. Download and extract **BepInEx 5.x (Unity Mono x64)** into your game directory:
   ```
   <Steam>/steamapps/common/Spacefleet - Heat Death/
   ```
   You should see `winhttp.dll` and a `BepInEx/` folder in the game root.

2. Run the game once so BepInEx creates its folder structure, then close it.

3. Download `SpacefleetTradeMod.dll` from the [Releases](../../releases) page.

4. Copy `SpacefleetTradeMod.dll` into:
   ```
   <game directory>/BepInEx/plugins/SpacefleetTradeMod.dll
   ```

5. Launch the game. The mod loads automatically.

6. (Optional) After first run, tweak settings in:
   ```
   <game directory>/BepInEx/config/com.modder.spacefleet.trademod.cfg
   ```

## Configuration

| Section | Key | Default | Description |
|---------|-----|---------|-------------|
| Trading | FuelAvoidanceThreshold | 0.20 | Fleet fuel ratio below which dry stations are avoided |
| Fuel Pricing | FuelPriceEmptyMultiplier | 1.20 | Price multiplier at empty stock |
| Fuel Production | FuelProductionMultiplier | 100 | Output multiplier for designated fuel stations |
| Fuel Production | DTFuelStationName | Sao Felix | Station with boosted DT fuel production |
| Fuel Production | DHFuelStationName | New Anchorage | Station with boosted DH fuel production |
| Tanker | VirtualTankCapacityMultiplier | 1.0 | Virtual fuel tank size relative to fleet fuel module capacity |

## Building from Source

Set `GameDir` in `SpacefleetTradeMod.csproj` to your Spacefleet install path, then:
```
dotnet build SpacefleetTradeMod/SpacefleetTradeMod.csproj -c Release
```
Output: `SpacefleetTradeMod/bin/Release/net472/SpacefleetTradeMod.dll`
