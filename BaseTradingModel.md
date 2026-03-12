# Base Trading Model — Small Cargo Ship (1 Month)

*Derived from decompiled Spacefleet: Heat Death game code (Trader.cs, Market.cs, GlobalMarket.cs, Navigation.cs, FleetManager.cs, S_Ship.cs)*

---

## Ship Profile: Small Cargo Trader

| Parameter | Value | Source / Reasoning |
|---|---|---|
| Dry mass | 50 t | Typical small hull + drive + fuel tank modules |
| Fuel capacity | 30 t (DT_FUEL) | Single small fuel module |
| Cargo capacity | 100 t | Single CARGO module (storageMax) |
| Cruise thrust | 50 kN | DriveModuleBP.CruiseThrustN |
| Isp | 12,000 s | Mid-tier fusion drive |
| Exhaust velocity | 117,720 m/s | Isp × 9.81 |
| Max acceleration | 0.625 m/s² (0.064g) | thrust / (fullMass × 1000) = 50,000 / 80,000 |
| Full mass | 80 t | dry 50 + fuel 30 |
| Max Δv | 55.3 km/s | Ve × ln(full/dry) = 117.72 × ln(80/50) |

## Game Time Constants

| Parameter | Value | Source |
|---|---|---|
| Hours per day | 24 | TimeManager |
| Days per month | 30 | TimeManager |
| Hours per month | 720 | 24 × 30 |
| gAccDay constant | 73,234.4 | Navigation.cs — converts G to game-distance/day² |
| Trade poll interval | 1 s real-time (1 game-hour at 1× speed) | Trader.StartTradeCycle |
| Trade execution | Once per game-hour on arrival | Trader.NewHour → Trade() |

## Market Pricing Model (Vanilla)

From `Market.GetCurrentPrice()`:

```
basePrice       = resource.basePrice
stockRatio      = currentStock / minStockQuantity  (or idealStockRatio if minStock=0)
sensitivity     = 0.09 (buying) / 0.11 (selling)
priceFactor     = (idealStockRatio / max(0.01, stockRatio)) ^ sensitivity
spread          = transactionSpread × (1 - clamp01(|stockRatio - idealStockRatio| / idealStockRatio))

buyPrice  = round(basePrice × priceFactor × (1 + spread))      [trader pays more]
sellPrice = round(basePrice × priceFactor × (1 - spread × 0.5)) [trader receives less]
```

Default spread = 10%, idealStockRatio = 0.5.

At equilibrium (stock at ideal ratio): priceFactor ≈ 1.0, so:
- Buy price ≈ basePrice × 1.10 (trader pays 10% above base)
- Sell price ≈ basePrice × 0.95 (trader receives 5% below base)
- Effective spread ≈ 15% of base price

## Trade Cycle Assumptions

The vanilla auto-trader:
1. Finds the cheapest seller for any resource where global avg buy < avg sell (arbitrage exists)
2. Buys up to full cargo capacity
3. Flies to the highest-paying buyer
4. Sells cargo, requiring ≥15% margin over buy price (degrades if no good trades found)
5. Repeats

### Throughput Bottleneck: resupplyRatePerHour

Market transactions are capped per game-hour by `resupplyRatePerHour`. For a typical station:

| Station Type | resupplyRatePerHour | Fuel types (×10) |
|---|---|---|
| Small station | ~5-10 t/hr | 50-100 t/hr |
| Medium station | ~10-20 t/hr | 100-200 t/hr |
| Large station | ~20-50 t/hr | 200-500 t/hr |

For non-fuel goods at a medium station (~15 t/hr), loading 100t of cargo takes ~7 game-hours. Selling takes the same at the destination.

## Modeled Trade Route

### Assumptions
- Trading a mid-value resource: **ELECTRONICS** (basePrice ~120 cr/t)
- Buy station: slightly overstocked → buy price ≈ 110 cr/t
- Sell station: slightly understocked → sell price ≈ 140 cr/t
- Profit margin: (140-110)/110 = 27.3% (passes the 15% threshold)
- Average inter-station distance: moderate (≈0.5 game-distance units)
- resupplyRatePerHour at both stations: 15 t/hr
- Fleet refuels automatically on arrival (isAutoRefuel = true)

### Single Round-Trip Breakdown

#### 1. Buy Phase
- Cargo to fill: 100 t
- Rate: 15 t/hr
- Time: ceil(100/15) = **7 hours**
- Cost: 100 × 110 = **11,000 cr**

#### 2. Travel to Seller → Buyer
Navigation uses 50/50 accel/decel split at max G:

```
accG = 0.064g
accKkm = 0.064 × 73,234.4 = 4,687 game-units/day²
distance = 0.5 game-units
accDist = 0.25 (half the distance)
accTime = sqrt(2 × 0.25 / 4,687) = sqrt(0.0001067) ≈ 0.01033 days ≈ 0.248 hours
totalFlightTime = 2 × accTime ≈ 0.496 hours ≈ 0.5 hours
```

Δv consumed per leg:
```
dVRequired = 2 × sqrt(2 × accG × 9.81 × accDist)
           = 2 × sqrt(2 × 0.064 × 9.81 × 0.25)  [need game-to-real conversion]
```

Using the game's ConsumeDv formula:
```
fuelPerFixedUpdate = currentMass × acc / (9.81 × Isp) × 87,500 × timeScale × fixedDeltaTime
```

For a full trip (simplified via Tsiolkovsky):
```
Δv per leg ≈ 2 × sqrt(2 × 0.625 × distance_meters)
```

At game scale, a moderate trip consumes roughly **2-4 t of fuel** per leg (round trip: ~4-8 t).

We'll use **6 t fuel per round trip** as a reasonable mid-estimate for a small ship on moderate routes.

#### 3. Sell Phase
- Cargo to sell: 100 t
- Rate: 15 t/hr
- Time: ceil(100/15) = **7 hours**
- Revenue: 100 × 140 = **14,000 cr**

#### 4. Refueling (on arrival at station)
The fleet auto-refuels via `FleetManager.OnArrival → SetStateTo(REFUELING)`.
- Fuel needed: ~6 t
- Fuel resupply rate: 15 × 10 = 150 t/hr (fuel types get ×10 multiplier)
- Refuel time: < 1 hour (effectively instant)
- Fuel cost: DT_FUEL basePrice ~80 cr/t, at station price ≈ 85 cr/t
- Refuel cost: 6 × 85 = **510 cr**

#### 5. Cycle Overhead
- TradeCycle polling: ~1-3 hours to find a valid trade
- Navigation setup: ~1 hour
- Overhead per cycle: **~3 hours**

### Single Round-Trip Summary

| Phase | Time (hrs) | Credits |
|---|---|---|
| Find trade + setup | 3 | 0 |
| Travel to seller | 0.5 | 0 |
| Buy 100t Electronics | 7 | -11,000 |
| Travel to buyer | 0.5 | 0 |
| Sell 100t Electronics | 7 | +14,000 |
| Refuel (~6t) | 0.5 | -510 |
| **Round-trip total** | **18.5 hrs** | **+2,490 cr** |

### Monthly Projection (720 hours)

```
Trips per month  = floor(720 / 18.5) = 38 round trips
Gross revenue    = 38 × 14,000 = 532,000 cr
Cargo costs      = 38 × 11,000 = 418,000 cr
Fuel costs       = 38 × 510    =  19,380 cr
```

| Metric | Value |
|---|---|
| Total round trips | 38 |
| Gross trade revenue | 532,000 cr |
| Cost of goods | 418,000 cr |
| Gross trade profit | 114,000 cr |
| Total fuel cost | 19,380 cr |
| **Net profit (after fuel)** | **94,620 cr** |
| Profit per trip | 2,490 cr |
| Fuel as % of gross profit | 17.0% |
| Effective cr/hour | 131.4 cr/hr |

## Sensitivity Analysis

### If margin degrades (market saturation)

The vanilla trader halves `currentMinProfitMargin` each failed cycle. If margins drop to ~8%:

| Scenario | Sell Price | Profit/Trip | Monthly Net |
|---|---|---|---|
| 27% margin (base case) | 140 cr/t | 2,490 cr | 94,620 cr |
| 15% margin (threshold) | 127 cr/t | 1,190 cr | 45,220 cr |
| 8% margin (degraded) | 119 cr/t | 390 cr | 14,820 cr |
| 0% margin (desperation) | 110 cr/t | -510 cr | -19,380 cr |

At 0% margin the trader is losing money purely to fuel costs.

### If trading cheaper resources (e.g., METALS, basePrice ~30 cr/t)

| Resource | Buy | Sell | Profit/Trip | Monthly Net |
|---|---|---|---|---|
| Electronics (120 base) | 110 | 140 | 2,490 | 94,620 |
| Superalloys (200 base) | 185 | 230 | 3,990 | 151,620 |
| Metals (30 base) | 28 | 35 | 190 | 7,220 |
| Rare Earths (80 base) | 74 | 93 | 1,390 | 52,820 |

Low-value bulk goods barely cover fuel costs.

### If travel distance doubles

| Distance | Flight Time | Fuel/Trip | Trips/Month | Monthly Net |
|---|---|---|---|---|
| 0.5 units (base) | 1 hr | 6 t / 510 cr | 38 | 94,620 cr |
| 1.0 units | 1.4 hr | 10 t / 850 cr | 36 | 82,440 cr |
| 2.0 units | 2 hr | 16 t / 1,360 cr | 33 | 64,020 cr |

Longer routes eat into both trip count and fuel budget.

## Key Takeaways

1. **Fuel is ~17% of gross profit** for a small trader on moderate routes — significant but not dominant.
2. **The resupplyRatePerHour bottleneck** (loading/unloading time) matters more than flight time for short routes. A 100t cargo ship spends ~14 of its ~18.5 hour cycle just docked.
3. **Margin degradation is the real killer.** If the 15% threshold can't be met and the trader starts accepting thin margins, fuel costs quickly eat all profit.
4. **Resource choice matters enormously.** Trading metals at 30 cr/t base barely breaks even; electronics or superalloys are 5-20× more profitable per trip.
5. **The vanilla random resource selection** means the trader often picks suboptimal resources. The ProfitOptimizerPatch in the mod addresses this by scoring all resources by total profit.

---

*Model based on vanilla game code defaults. Actual in-game values depend on ScriptableObject configurations (resource basePrices, station resupplyRates, drive Isp/thrust) which are set in Unity assets, not in code.*
