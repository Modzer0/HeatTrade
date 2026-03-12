# Auto-Trading System Notes

Game: Spacefleet - Heat Death  
Source: Decompiled Assembly-CSharp

## Overview

The auto-trading system is driven by the `Trader` component attached to fleet GameObjects. When enabled, it runs a continuous loop: find a profitable trade, fly to the target market, execute the trade, repeat. The `GlobalMarket` singleton coordinates market discovery, and each station's `Market` component handles local pricing and inventory.

## Core Loop (`Trader.cs`)

1. `SetOn(true)` sets `isAutoTrading = true` and starts the coroutine `StartTradeCycle`.
2. The coroutine polls `TradeCycle()` every 1 second until a valid trade is found.
3. Once valid, the fleet's navigation is configured and the fleet flies to the target market.
4. On arrival, `NewHour()` fires each game-hour and calls `Trade()` to execute buy/sell.
5. When the trade is complete (quantity fulfilled, credits exhausted, or market can't transact), `StopTrade()` restarts the cycle with a 0.1s delay.

## Trade Decision Rules (`TradeCycle`)

### Selling (cargo already has goods)
- If any cargo module has resources, the trader enters SELL mode (`isBuying = false`).
- It picks the first resource type found in cargo and sums up the total quantity across all cargo modules.
- Calls `GlobalMarket.GetBestBuyerOfExceptHostile` to find the highest-paying non-hostile market.
- Profit margin check: `bestSellPrice >= buyPrice * (1 + currentMinProfitMargin)`.
  - Default `minProfitMargin` = 15%.
  - If the margin check fails, `currentMinProfitMargin` is halved each retry.
  - Once it drops below 0.001, it's set to -100000 (effectively disabling the margin requirement so the trader will sell at any price to clear cargo).
  - A successful margin check resets `currentMinProfitMargin` back to the default 15%.

### Buying (cargo is empty)
- If no cargo has resources, the trader enters BUY mode (`isBuying = true`).
- `targetQuantity` = total cargo storage space.
- Calls `GlobalMarket.GetBestSellerAnyExceptHostile` which iterates a randomized list of all resource types and finds the cheapest non-hostile seller.
- A resource is only considered for buying if its average buy price < average sell price across all markets (i.e., there's a global arbitrage opportunity).

### Validation
A trade is valid only if all three are non-null/non-zero: `targetMarket`, `targetResource`, `targetQuantity`.

## Navigation & Safety Checks

Before flying, the coroutine adjusts acceleration parameters:
- Sets `accG` to the fleet's max, with 50/50 accel/decel ratio.
- If max acceleration is exceeded, insufficient delta-v, or collision warning: halves all navigation parameters and retries.
- If fleet delta-v drops below 10% of max: switches fleet to `REFUELING` state and aborts the trade cycle.

## Market Pricing (`Market.cs`)

Prices are dynamic, based on supply/demand at each station:

- Each resource has a `basePrice` (from `ResourceDefinition`).
- Price scales with a stock ratio: `currentStock / minStockQuantity` vs `idealStockRatio` (default 0.5).
- Sensitivity constants: buy = 0.09, sell = 0.11.
- Max fluctuation ratio: 0.8.
- A `transactionSpread` (default 10%) creates a bid-ask spread — buy prices are higher, sell prices are lower.
- Prices update after every trade execution.

### CanBuy (purchase limit per tick)
The amount a fleet can buy per hour is capped by the minimum of:
- Requested quantity
- Available stock above the market's minimum stock threshold
- Total stock on hand
- What the faction can afford at current price
- `resupplyRatePerHour` (×10 for VOLATILES, DT_FUEL, DH_FUEL)

### Selling to a market
Selling is similarly capped by `resupplyRatePerHour` (×10 for fuel types). The market adds the resource to its inventory and pays the seller.

## Global Market Discovery (`GlobalMarket.cs`)

### Finding a Buyer (`GetBestBuyerOfExceptHostile`)
- Filters out markets belonging to hostile factions.
- From remaining markets, picks the one offering the highest sell price (from the trader's perspective).
- Skips markets with full inventories.
- Ties are broken randomly.

### Finding a Seller (`GetBestSellerAnyExceptHostile`)
- Filters out hostile faction markets.
- Iterates resources in random order.
- Only considers resources where global average buy price < global average sell price (arbitrage exists).
- For each resource, finds the cheapest seller whose price is at or below the global average buy price.
- The seller must have the resource in stock and pass the `CanBuy` check.
- Ties broken randomly.

## Faction & Hostility

- Traders never trade with hostile factions. Hostility is checked via `Faction.GetHostiles()`.
- Player faction (factionID == 1) gets credit changes logged through `FactionsManager.ModPlayerCredits`.
- AI factions modify credits directly on their `Faction` object.

## Cargo Management

- Cargo is stored in `S_Module2` components of type `CARGO` across all ships in the fleet.
- When buying, resources are distributed across cargo modules until full.
- When selling, resources are removed from cargo modules until the quantity is met.
- If no cargo modules exist on a player fleet, a notification warns "No cargo ships in fleet!".

## Trade Execution Details (`Trade` method)

- Buying: positive `realQuantity` passed to `Market.ExecuteTrade`. Records `buyPrice = totalCost / quantity` for later profit margin checks.
- Selling: negative `realQuantity`. If the sell returns 0 quantity (market can't absorb), trade stops immediately. If the effective sell price drops below the profit margin threshold, the margin is halved (same degradation as in TradeCycle) and trade stops.
- Trade stops when: target quantity is fulfilled, credits run out, or the market can no longer transact.

## Key Constants & Defaults

| Parameter | Value | Notes |
|---|---|---|
| `minProfitMargin` | 0.15 (15%) | Required markup over buy price |
| `buySensitivity` | 0.09 | Price sensitivity when buying |
| `sellSensitivity` | 0.11 | Price sensitivity when selling |
| `MaxFluctuationRatio` | 0.8 | Caps price swing |
| `idealStockRatio` | 0.5 | Stock level where price = base price |
| `transactionSpread` | 0.1 (10%) | Bid-ask spread |
| Fuel resupply multiplier | ×10 | For VOLATILES, DT_FUEL, DH_FUEL |
| Delta-v abort threshold | 10% | Fleet switches to REFUELING |
