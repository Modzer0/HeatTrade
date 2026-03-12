// Decompiled with JetBrains decompiler
// Type: GameSaver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GameSaver : MonoBehaviour
{
  public static GameSaver current;
  private TimeManager tm;
  private FactionsManager fm;
  [SerializeField]
  private List<Transform> bodies;

  private void Awake()
  {
    if ((Object) GameSaver.current != (Object) null && (Object) GameSaver.current != (Object) this)
    {
      Object.Destroy((Object) this.gameObject);
    }
    else
    {
      GameSaver.current = this;
      Object.DontDestroyOnLoad((Object) this.gameObject);
    }
  }

  private void Start()
  {
    this.tm = TimeManager.current;
    this.fm = FactionsManager.current;
  }

  public GameSaveData GetSaveData()
  {
    MonoBehaviour.print((object) "===== SAVING!");
    List<BodyData> bodyDataList = new List<BodyData>();
    foreach (Transform body in this.bodies)
    {
      Orbiter component = body.GetComponent<Orbiter>();
      BodyData bodyData = new BodyData()
      {
        id = body.GetComponent<Track>().id,
        bodyName = body.name,
        position = new SerializableVector3(body.position),
        rotation = new SerializableQuaternion(body.rotation),
        currentAngle = component.currentAngle,
        mass = component.mass,
        rocheLimitRadius = component.rocheLimitRadius,
        soiRadius = component.soiRadius
      };
      bodyDataList.Add(bodyData);
    }
    List<FactionData> factionDataList = new List<FactionData>();
    foreach (Faction faction in this.fm.factions)
      factionDataList.Add(this.ConvertFactionToData(faction));
    List<StationData> stationDataList = new List<StationData>();
    foreach (StationManager sm in Object.FindObjectsOfType<StationManager>())
      stationDataList.Add(this.ConvertStationToData(sm));
    List<FleetData> fleetDataList = new List<FleetData>();
    foreach (FleetManager fleet in Object.FindObjectsOfType<FleetManager>())
    {
      if (fleet.GetComponent<Track>().factionID != 99)
        fleetDataList.Add(this.ConvertFleetToData(fleet));
    }
    return new GameSaveData()
    {
      timeScale = this.tm.timeScale,
      mins = this.tm.mins,
      hours = this.tm.hours,
      days = this.tm.days,
      months = this.tm.months,
      years = this.tm.years,
      totalDays = this.tm.TotalDays,
      bodies = bodyDataList,
      factions = factionDataList,
      stations = stationDataList,
      fleets = fleetDataList,
      version = Application.version
    };
  }

  private FactionData ConvertFactionToData(Faction faction)
  {
    return new FactionData()
    {
      factionID = faction.factionID,
      factionName = faction.factionName,
      factionWords = faction.factionWords,
      factionDescription = faction.factionDescription,
      factionCode = faction.factionCode,
      credits = faction.credits,
      relations = new SerializableIntIntDictionary(faction.relations),
      colorDataPrimary = new ColorData(faction.colorPrimary),
      colorDataSecondary = new ColorData(faction.colorSecondary)
    };
  }

  private StationData ConvertStationToData(StationManager sm)
  {
    Track component = sm.GetComponent<Track>();
    SerializableVector3 serializableVector3 = new SerializableVector3(sm.transform.position);
    string str = "";
    if ((Object) sm.transform.parent != (Object) null && (bool) (Object) sm.transform.parent.GetComponent<Track>())
      str = sm.transform.parent.GetComponent<Track>().id;
    InventoryData inventoryData = (InventoryData) null;
    MarketData marketData = (MarketData) null;
    FactoryData factoryData = (FactoryData) null;
    OrbitData orbitData = (OrbitData) null;
    if ((bool) (Object) sm.GetComponent<Market>())
    {
      inventoryData = sm.GetComponent<Market>().inventory.GetInventoryData();
      marketData = sm.GetComponent<Market>().GetMarketData();
    }
    if ((bool) (Object) sm.GetComponent<Factory>())
      factoryData = sm.GetComponent<Factory>().GetFactoryData();
    if ((bool) (Object) sm.GetComponent<Orbiter>())
      orbitData = sm.GetComponent<Orbiter>().GetOrbitData();
    return new StationData()
    {
      factionID = component.factionID,
      id = component.id,
      publicName = component.publicName,
      position = serializableVector3,
      parentID = str,
      parentOrbiterID = sm.GetComponent<Orbiter>().parentOrbiter.GetComponent<Track>().id,
      station = sm,
      inventory = inventoryData,
      market = marketData,
      factory = factoryData,
      orbit = orbitData
    };
  }

  private FleetData ConvertFleetToData(FleetManager fleet)
  {
    Track component1 = fleet.GetComponent<Track>();
    SerializableVector3 serializableVector3 = new SerializableVector3(fleet.transform.position);
    string str1 = "";
    if ((Object) fleet.transform.parent != (Object) null && (bool) (Object) fleet.transform.parent.GetComponent<Track>())
      str1 = fleet.transform.parent.GetComponent<Track>().id;
    Navigation component2 = fleet.GetComponent<Navigation>();
    List<ShipData> shipDataList = new List<ShipData>();
    foreach (S_Ship ship in fleet.ships)
      shipDataList.Add(this.ConvertShipToData(ship));
    string str2 = "NULL";
    if ((Object) component2.target != (Object) null && (bool) (Object) component2.target.GetComponent<Track>())
      str2 = component2.target.GetComponent<Track>().id;
    Trader component3 = fleet.GetComponent<Trader>();
    string str3 = "";
    if ((Object) component3.targetResource != (Object) null)
      str3 = component3.targetResource.resourceName;
    return new FleetData()
    {
      factionID = component1.factionID,
      id = component1.id,
      publicName = component1.publicName,
      position = serializableVector3,
      parentID = str1,
      parentOrbiterID = fleet.GetComponent<Orbiter>().parentOrbiter.GetComponent<Track>().id,
      navigationData = new NavigationData()
      {
        state = component2.currentState,
        flightType = component2.flightType,
        arrivalType = component2.arrivalType,
        targetID = str2,
        currentDir = new SerializableVector3(component2.currentDir),
        currentVel = new SerializableVector3(component2.currentVel),
        accG = component2.accG,
        decG = component2.decG,
        accKkm = component2.accKkm,
        decKkm = component2.decKkm,
        startMarkerPos = new SerializableVector3(component2.startMarkerPos),
        accMarkerPos = new SerializableVector3(component2.accMarkerPos),
        decelMarkerPos = new SerializableVector3(component2.decelMarkerPos),
        stopMarkerPos = new SerializableVector3(component2.stopMarkerPos)
      },
      state = fleet.state,
      isAutoRepair = fleet.isAutoRepair,
      isAutoResupply = fleet.isAutoResupply,
      isAutoRefuel = fleet.isAutoRefuel,
      isRepairing = fleet.isRepairing,
      isResupplying = fleet.isRefueling,
      isRefueling = fleet.isResupplying,
      isAutoTrading = component3.isAutoTrading,
      isTrading = component3.isTrading,
      currentMarketID = component3.GetCurrentMarketID(),
      targetMarketID = component3.GetTargetMarketID(),
      targetResourceName = str3,
      targetQuantity = component3.targetQuantity,
      isBuying = component3.isBuying,
      buyPrice = component3.buyPrice,
      bestSellPrice = component3.bestSellPrice,
      profitMargin = component3.profitMargin,
      minProfitMargin = component3.minProfitMargin,
      currentMinProfitMargin = component3.currentMinProfitMargin,
      hasArrived = component3.HasArrived,
      ships = shipDataList
    };
  }

  private ShipData ConvertShipToData(S_Ship s)
  {
    List<TacticalModuleData> tacticalModuleDataList = new List<TacticalModuleData>();
    foreach (S_Module2 module in s.modules)
      tacticalModuleDataList.Add(this.ConvertModuleToData(module, s));
    return new ShipData()
    {
      bpKey = s.bp.PrefabKey,
      factionID = s.factionID,
      id = s.trackID,
      publicName = s.publicName,
      modules = tacticalModuleDataList
    };
  }

  private TacticalModuleData ConvertModuleToData(S_Module2 module, S_Ship s)
  {
    List<TacticalMountData> tacticalMountDataList = new List<TacticalMountData>();
    foreach (S_Mount2 mount in module.mounts)
      tacticalMountDataList.Add(this.ConvertMountToData(mount));
    return new TacticalModuleData()
    {
      bpKey = module.bp.PrefabKey,
      health = module.health,
      armorHealth = module.armorHealth,
      resource = module.supplies,
      inventoryData = module.GetInventoryData(),
      mounts = tacticalMountDataList
    };
  }

  private TacticalMountData ConvertMountToData(S_Mount2 mount)
  {
    if ((Object) mount == (Object) null)
      return (TacticalMountData) null;
    return new TacticalMountData()
    {
      bpKey = mount.bp.PrefabKey,
      productName = mount.bp.PartNameFull,
      health = mount.health,
      resource = mount.resource
    };
  }
}
