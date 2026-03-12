# Ship & Module Architecture — Spacefleet: Heat Death

*Derived from decompiled Assembly-CSharp. All classes are in the global namespace, inheriting MonoBehaviour or ScriptableObject.*

---

## Blueprint Hierarchy (ScriptableObjects)

All ship/module/mount definitions are Unity ScriptableObjects stored in asset bundles (not accessible in decompiled code). The inheritance chain:

```
ScriptableObject
├── ShipBP                    — Ship definition (hull-level)
└── PartBP                    — Base for all parts
    ├── ModuleBP              — Cylindrical module (has volume, mounts)
    │   ├── DriveModuleBP     — Engine module (thrust, Isp, fuel type)
    │   └── FuelModuleBP      — Fuel tank (implements ICargo)
    └── MountBP               — Weapon/equipment mount point (empty subclass)
```

### PartBP (Base Class)
Every part — module or mount — inherits from `PartBP`:

| Field | Type | Purpose |
|---|---|---|
| `prefabKey` | string | Unique lookup key for BlueprintLibrary |
| `partNameFull` | string | Display name |
| `partNameShort` | string | Abbreviated name |
| `description` | string | Flavor text |
| `partType` | PartType | Enum categorizing the part |
| `sizeClass` | SizeClass | S1, S2, or S3 |
| `repairCycle` | RepairCycle | Resource cost to repair |
| `density` | float | Used for storage capacity calculation |
| `mass` | float | Part mass in tonnes |
| `armorHealthMax` | float | Max armor HP |
| `armorThickness` | float | Armor thickness (affects armor mass) |

### PartType Enum
```
DRIVE, HEATSINK, CARGO, CREW, SENSORS, WEAPON, NOSE, BATTERY,
RADIATORS, KINETIC, BEAM, MISSILE, EWAR, PD, FUEL, NOZZLE
```

### SizeClass Enum
```
S1, S2, S3
```

### PropulsionType Enum
```
TRITORCH, HITORCH, ATORCH
```

---

## ModuleBP — Cylindrical Ship Sections

Extends `PartBP` with physical dimensions:

| Field | Type | Purpose |
|---|---|---|
| `length` | float | Module length (meters) |
| `diameter` | float | Module diameter (meters) |
| `volume` | float | Calculated: π × (diameter/2)² × length |
| `mounts` | List\<MountBP\> | Weapon/equipment slots on this module |

Volume is computed by `GetSetVolume()` and used with `density` to determine storage capacity:
```
storageMax = volume × density
```

This is set at runtime in `S_Module2.Start()` via `inv.SetStorageMax(bp.GetSetVolume() * bp.Density)`.

---

## DriveModuleBP — Engine Modules

Extends `ModuleBP` with propulsion stats:

| Field | Type | Purpose |
|---|---|---|
| `propulsionType` | PropulsionType | TRITORCH, HITORCH, or ATORCH |
| `fuelType` | ResourceDefinition | What fuel this drive burns |
| `totalOutput` | float | Total power output |
| `electricOutput` | float | Electrical power |
| `thermalOutput` | float | Thermal power |
| `thermalEfficiency` | float | Heat efficiency ratio |
| `thrustOutput` | float | Thrust power |
| `wasteOutput` | float | Waste heat |
| `isp` | float | Specific impulse (seconds) |
| `massFlowRate` | float | Propellant consumption rate |
| `cruiseThrustN` | float | Cruise thrust in Newtons |

Derived property:
```
ExhaustVelocity = Isp × 9.81  (m/s)
```
