# Spacefleet Trade Optimizer Mod

BepInEx plugin for Spacefleet: Heat Death that overhauls the auto-trading economy.

## Features

### 1. Profit-Maximized Trade Routes
Replaces the vanilla random resource selection with a scoring system that evaluates every resource by `(sellPrice - buyPrice) * availableQuantity` and picks the most profitable trade. Sellers are still filtered by hostility and stock availability.

### 2. Fuel Avoidance (Low Fuel Safety)
When a fleet's delta-v drops below 20% of max, the mod filters out any market that has zero stock of the fleet's fuel type. Prevents traders from stranding themselves at dry stations. Falls back to unfiltered if all markets are dry (avoids deadlock).

### 3. Designated Fuel Production Hubs
- **Sao Felix** — DT fuel recipes produce at 100x normal output
- **New Anchorage** — DH fuel recipes produce at 100x normal output

The multiplier applies to `Factory.ExecuteBatchProduction` at matching stations. Extra output is added after the normal production run, and extra inputs are consumed to match.

### 4. Tanker Fuel Trading
Ships with FUEL-type modules can now trade fuel at markets. The mod adds fuel module inventories to the Trader's cargo list, so tanker fleets will buy fuel cheap and sell it where prices are high. Fleet delta-v is recalculated after each fuel trade.

### 5. Scarcity-Based Fuel Pricing
Fuel prices (DT_FUEL, DH_FUEL, VOLATILES) now scale linearly with stock level:
- **Empty station** → basePrice × 1.20 (+20%)
- **Full station** → basePrice (no markup)
- Intermediate stock levels interpolate between these values

## Configuration

After first run, edit `BepInEx/config/com.modder.spacefleet.trademod.cfg`:

```ini
[Trading]
# Fleet fuel ratio below which stations without fuel are avoided (0.20 = 20%)
FuelAvoidanceThreshold = 0.20

[Fuel Pricing]
# Price multiplier when station fuel stock is empty (1.20 = +20%)
FuelPriceEmptyMultiplier = 1.20

[Fuel Production]
# Production multiplier for designated fuel stations
FuelProductionMultiplier = 100

# Station names for fuel production hubs
DTFuelStationName = Sao Felix
DHFuelStationName = New Anchorage
```

## Installation

1. Install BepInEx 5.x (Unity Mono x64) into the game root
2. Build this project or grab the release DLL
3. Drop `SpacefleetTradeMod.dll` into `BepInEx/plugins/`
4. Launch the game

## Build

Set `GameDir` in the `.csproj` to your Spacefleet install path, then:
```
dotnet build -c Release
```
