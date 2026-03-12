// Decompiled with JetBrains decompiler
// Type: GameLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GameLoader : MonoBehaviour
{
  public static GameLoader current;
  private TimeManager tm;
  private FactionsManager fm;
  private TrackDisplayer td;
  [SerializeField]
  private S_StructureLibrary ssl;
  [SerializeField]
  private AllRecipes allRecipes;
  [SerializeField]
  private AllResources allResources;
  private AllStructures allStructures;
  [SerializeField]
  private BlueprintLibrary bl;
  private SelectedPanelManager spm;
  private UIManager uim;
  private GameSaveData gsd;
  [SerializeField]
  private List<Transform> bodies;
  [SerializeField]
  private List<FleetManager> fleets = new List<FleetManager>();
  [SerializeField]
  private List<StationManager> stations = new List<StationManager>();
  [SerializeField]
  private GameObject stationPF;
  [SerializeField]
  private GameObject fleetPF;

  private void Awake()
  {
    if ((UnityEngine.Object) GameLoader.current != (UnityEngine.Object) null && (UnityEngine.Object) GameLoader.current != (UnityEngine.Object) this)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      GameLoader.current = this;
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
    }
  }

  private void Start()
  {
    this.tm = TimeManager.current;
    this.fm = FactionsManager.current;
    this.td = TrackDisplayer.current;
    this.allStructures = AllStructures.current;
    this.spm = SelectedPanelManager.current;
    this.uim = UIManager.current;
  }

  public bool StartNew(GameSaveData newGSD)
  {
    MonoBehaviour.print((object) "start new");
    this.gsd = newGSD;
    this.spm.ClearSelected();
    this.uim.ClearInfo();
    this.Reset();
    this.LoadTime();
    this.LoadBodies();
    this.LoadFactions();
    this.LoadStations();
    this.LoadFleets();
    this.LoadAttachments();
    this.StartCoroutine((IEnumerator) this.DelayedLoad());
    MonoBehaviour.print((object) "===== LOADING DONE =====");
    return true;
  }

  private IEnumerator DelayedLoad()
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.LoadNavData();
  }

  private void Reset()
  {
    Camera.main.transform.parent = (Transform) null;
    this.td.allTracks.Clear();
    this.allStructures.ships.Clear();
  }

  private void LoadTime()
  {
    this.tm.timeScale = this.gsd.timeScale;
    this.tm.mins = this.gsd.mins;
    this.tm.hours = this.gsd.hours;
    this.tm.days = this.gsd.days;
    this.tm.months = this.gsd.months;
    this.tm.years = this.gsd.years;
    this.tm.SetTotalDays(this.gsd.totalDays);
  }

  private void LoadBodies()
  {
    foreach (BodyData body1 in this.gsd.bodies)
    {
      foreach (Transform body2 in this.bodies)
      {
        if (body2.name == body1.bodyName)
        {
          body2.GetComponent<Track>().id = body1.id;
          body2.SetLocalPositionAndRotation(body1.position.ToVector3(), body1.rotation.ToQuaternion());
          Orbiter component = body2.GetComponent<Orbiter>();
          component.currentAngle = body1.currentAngle;
          component.mass = body1.mass;
          component.rocheLimitRadius = body1.rocheLimitRadius;
          component.soiRadius = body1.soiRadius;
          this.td.AddTrack(body2.GetComponent<Track>());
        }
      }
    }
  }

  private void LoadFactions()
  {
    this.fm.factions.Clear();
    foreach (FactionData faction in this.gsd.factions)
      this.fm.NewFaction(faction);
    SaveLoadSystem.current.UpdateGameName();
  }

  private void LoadStations()
  {
    foreach (Component component in UnityEngine.Object.FindObjectsOfType<StationManager>())
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    this.stations.Clear();
    foreach (StationData station in this.gsd.stations)
    {
      StationManager component1 = UnityEngine.Object.Instantiate<GameObject>(this.stationPF).GetComponent<StationManager>();
      Track trackFromId = this.td.GetTrackFromID(station.parentID);
      if ((UnityEngine.Object) trackFromId != (UnityEngine.Object) null)
      {
        component1.transform.parent = trackFromId.transform;
        component1.transform.localPosition = Vector3.zero;
      }
      else
        component1.transform.localPosition = station.position.ToVector3();
      Orbiter component2 = component1.GetComponent<Orbiter>();
      component2.parentOrbiter = this.td.GetTrackFromID(station.parentOrbiterID).GetComponent<Orbiter>();
      if (station.orbit.isInheritAngleFromParent)
        component2.SetFromData(station.orbit);
      Track component3 = component1.GetComponent<Track>();
      component3.factionID = station.factionID;
      component3.id = station.id;
      component3.publicName = station.publicName;
      component1.transform.name = "STATION - " + station.id;
      component1.gameObject.SetActive(true);
      this.stations.Add(component1);
      this.td.AddTrack(component1.GetComponent<Track>());
      component1.GetComponent<Market>().SetFromData(station.market, station.inventory);
      component1.GetComponent<Factory>().SetFromData(station.factory);
    }
  }

  private void LoadFleets()
  {
    foreach (Component component in UnityEngine.Object.FindObjectsOfType<FleetManager>())
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    this.fleets.Clear();
    foreach (FleetData fleet in this.gsd.fleets)
    {
      FleetManager component1 = UnityEngine.Object.Instantiate<GameObject>(this.fleetPF).GetComponent<FleetManager>();
      Track trackFromId = this.td.GetTrackFromID(fleet.parentID);
      if ((UnityEngine.Object) trackFromId != (UnityEngine.Object) null)
      {
        component1.transform.parent = trackFromId.transform;
        component1.transform.localPosition = Vector3.zero;
      }
      else
        component1.transform.localPosition = fleet.position.ToVector3();
      Track component2 = component1.GetComponent<Track>();
      component2.factionID = fleet.factionID;
      component2.id = fleet.id;
      component2.publicName = fleet.publicName;
      this.td.AddTrack(component2);
      component1.transform.name = "FLEET - " + fleet.id;
      component1.gameObject.SetActive(true);
      this.fleets.Add(component1);
      component1.state = fleet.state;
      component1.isAutoRepair = fleet.isAutoRepair;
      component1.isAutoResupply = fleet.isAutoResupply;
      component1.isAutoRefuel = fleet.isAutoRefuel;
      component1.isRepairing = fleet.isRepairing;
      component1.isRefueling = fleet.isResupplying;
      component1.isResupplying = fleet.isRefueling;
      this.LoadShipsInFleet(fleet, component1);
      component1.UpdateDv();
    }
  }

  private void InitTrader(FleetManager fleet) => fleet.GetComponent<Trader>().SetCargoStorages();

  private void LoadNavData()
  {
    this.ClearEmptyFleets();
    this.LoadFleetNavData();
    this.LoadFleetTraderData();
    this.LoadStationNavData();
  }

  private void LoadAttachments()
  {
    foreach (Attachments attachments in UnityEngine.Object.FindObjectsOfType<Attachments>())
      attachments.RefreshList();
  }

  private void LoadShipsInFleet(FleetData fd, FleetManager fleetM)
  {
    foreach (ShipData ship in fd.ships)
    {
      GameObject prefab = this.ssl.GetPrefab(this.bl.GetShipBP(ship.bpKey));
      if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("Missing prefab for: " + ship.bpKey));
      }
      else
      {
        GameObject newShip = UnityEngine.Object.Instantiate<GameObject>(prefab, fleetM.transform);
        if ((bool) (UnityEngine.Object) newShip.GetComponent<S_Ship>())
        {
          S_Ship component = newShip.GetComponent<S_Ship>();
          component.factionID = ship.factionID;
          component.trackID = ship.id;
          component.publicName = ship.publicName;
          component.UpdateDv();
        }
        this.LoadModulesInShip(ship, newShip);
      }
    }
    fleetM.Init();
  }

  private void LoadModulesInShip(ShipData shipData, GameObject newShip)
  {
    List<S_Module2> sModule2List = new List<S_Module2>();
    foreach (TacticalModuleData module in shipData.modules)
    {
      if (!(module.bpKey == "") && module.bpKey != null && !string.IsNullOrEmpty(module.bpKey))
      {
        IEnumerator enumerator = (IEnumerator) newShip.transform.GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            Transform current = (Transform) enumerator.Current;
            if ((bool) (UnityEngine.Object) current.GetComponent<S_Module2>())
            {
              S_Module2 component = current.GetComponent<S_Module2>();
              if (!sModule2List.Contains(component) && !((UnityEngine.Object) component.bp != (UnityEngine.Object) this.bl.GetModuleBP(module.bpKey)))
              {
                component.InitModuleData(module);
                sModule2List.Add(component);
                this.LoadMountsInModule(module, component);
                break;
              }
            }
          }
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      }
    }
  }

  private void LoadMountsInModule(TacticalModuleData moduleData, S_Module2 module)
  {
    if (moduleData.mounts.Count == 0)
      return;
    List<S_Mount2> sMount2List = new List<S_Mount2>();
    foreach (TacticalMountData mount1 in moduleData.mounts)
    {
      foreach (S_Mount2 mount2 in module.mounts)
      {
        if (!sMount2List.Contains(mount2) && !((UnityEngine.Object) mount2.bp != (UnityEngine.Object) this.bl.GetMountBP(mount1.bpKey)))
        {
          mount2.InitMountData(mount1);
          sMount2List.Add(mount2);
          break;
        }
      }
    }
  }

  private void ClearEmptyFleets()
  {
    List<FleetManager> fleetManagerList = new List<FleetManager>();
    foreach (FleetManager fleet in this.fleets)
    {
      if ((UnityEngine.Object) fleet != (UnityEngine.Object) null)
        fleetManagerList.Add(fleet);
    }
    this.fleets = fleetManagerList;
  }

  private void LoadFleetNavData()
  {
    foreach (FleetData fleet1 in this.gsd.fleets)
    {
      FleetManager fleetManager = (FleetManager) null;
      foreach (FleetManager fleet2 in this.fleets)
      {
        if (fleet2.GetComponent<Track>().id == fleet1.id)
        {
          fleetManager = fleet2;
          break;
        }
      }
      if (!((UnityEngine.Object) fleetManager == (UnityEngine.Object) null))
      {
        NavigationData navigationData = fleet1.navigationData;
        Orbiter component1 = fleetManager.GetComponent<Orbiter>();
        Navigation component2 = fleetManager.GetComponent<Navigation>();
        component1.parentOrbiter = this.td.GetTrackFromID(fleet1.parentOrbiterID).GetComponent<Orbiter>();
        Transform newTarget = (Transform) null;
        string targetId = navigationData.targetID;
        if (targetId != "NULL" && (bool) (UnityEngine.Object) this.td)
        {
          Track trackFromId = this.td.GetTrackFromID(targetId);
          if (!((UnityEngine.Object) trackFromId == (UnityEngine.Object) null))
            newTarget = trackFromId.transform;
          else
            continue;
        }
        if (navigationData.state == NavigationState.Orbiting)
        {
          foreach (Transform body in this.bodies)
          {
            if (body.GetComponent<Track>().id == fleet1.parentOrbiterID)
            {
              component1.SetTarget(body.GetComponent<Orbiter>());
              break;
            }
          }
          component1.isOrbiting = true;
          component2.currentState = navigationData.state;
          component2.flightType = navigationData.flightType;
          component2.arrivalType = navigationData.arrivalType;
        }
        else if (navigationData.state == NavigationState.Accelerating || navigationData.state == NavigationState.Drifting || navigationData.state == NavigationState.Decelerating)
        {
          component1.isOrbiting = false;
          component2.currentState = navigationData.state;
          component2.flightType = navigationData.flightType;
          component2.arrivalType = navigationData.arrivalType;
          component2.SetTarget(newTarget);
          component2.currentDir = navigationData.currentDir.ToVector3();
          component2.currentVel = navigationData.currentVel.ToVector3();
          component2.accG = navigationData.accG;
          component2.decG = navigationData.decG;
          component2.accKkm = navigationData.accKkm;
          component2.decKkm = navigationData.decKkm;
          component2.startMarkerPos = navigationData.startMarkerPos.ToVector3();
          component2.accMarkerPos = navigationData.accMarkerPos.ToVector3();
          component2.decelMarkerPos = navigationData.decelMarkerPos.ToVector3();
          component2.stopMarkerPos = navigationData.stopMarkerPos.ToVector3();
          component2.targetInterceptPosition = component2.stopMarkerPos;
          if (fleet1.factionID == 1)
            component2.DrawInterceptLines();
        }
      }
    }
  }

  private void LoadFleetTraderData()
  {
    foreach (FleetData fleet1 in this.gsd.fleets)
    {
      FleetManager fleet2 = (FleetManager) null;
      foreach (FleetManager fleet3 in this.fleets)
      {
        if (fleet3.GetComponent<Track>().id == fleet1.id)
        {
          fleet2 = fleet3;
          break;
        }
      }
      if (!((UnityEngine.Object) fleet2 == (UnityEngine.Object) null))
      {
        Market market1 = (Market) null;
        Market market2 = (Market) null;
        if (fleet1.currentMarketID != "" && fleet1.currentMarketID != null && !string.IsNullOrEmpty(fleet1.currentMarketID))
          market1 = this.td.GetTrackFromID(fleet1.currentMarketID).GetComponent<Market>();
        if (fleet1.targetMarketID != "" && fleet1.targetMarketID != null && !string.IsNullOrEmpty(fleet1.targetMarketID))
          market2 = this.td.GetTrackFromID(fleet1.targetMarketID).GetComponent<Market>();
        Trader component = fleet2.GetComponent<Trader>();
        component.isAutoTrading = fleet1.isAutoTrading;
        component.isTrading = fleet1.isTrading;
        int num1 = (bool) (UnityEngine.Object) market1 ? 1 : 0;
        int num2 = (bool) (UnityEngine.Object) market2 ? 1 : 0;
        component.currentMarket = market1;
        component.targetMarket = market2;
        component.targetResource = this.allResources.GetResource(fleet1.targetResourceName);
        component.targetQuantity = fleet1.targetQuantity;
        component.isBuying = fleet1.isBuying;
        component.buyPrice = fleet1.buyPrice;
        component.bestSellPrice = fleet1.bestSellPrice;
        component.profitMargin = fleet1.profitMargin;
        component.minProfitMargin = fleet1.minProfitMargin;
        component.currentMinProfitMargin = fleet1.currentMinProfitMargin;
        component.SetHasArrived(fleet1.hasArrived);
        if ((bool) (UnityEngine.Object) market2 && (bool) (UnityEngine.Object) component.targetResource && (double) component.targetQuantity != 0.0)
          component.isTradeValid = true;
        this.InitTrader(fleet2);
      }
    }
  }

  private void LoadStationNavData()
  {
    foreach (StationData station1 in this.gsd.stations)
    {
      if (!station1.orbit.isInheritAngleFromParent)
      {
        StationManager stationManager = (StationManager) null;
        foreach (StationManager station2 in this.stations)
        {
          if (station2.GetComponent<Track>().id == station1.id)
          {
            stationManager = station2;
            break;
          }
        }
        if (!((UnityEngine.Object) stationManager == (UnityEngine.Object) null))
        {
          Orbiter component = stationManager.GetComponent<Orbiter>();
          if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
            component.SetFromData(station1.orbit);
        }
      }
    }
  }
}
