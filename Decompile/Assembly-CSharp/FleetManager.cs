// Decompiled with JetBrains decompiler
// Type: FleetManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class FleetManager : MonoBehaviour
{
  [SerializeField]
  private S_StructureLibrary ssl;
  [SerializeField]
  private PartsLibrary pl;
  private Track track;
  public Orbiter orbiter;
  public Navigation navigation;
  private Trader trader;
  private TimeManager tm;
  private NotificationsManager notifs;
  private bool isInitialized;
  public List<S_Ship> ships = new List<S_Ship>();
  public int attachedFleets;
  public StationManager station;
  public bool isAutoRepair;
  public bool isAutoResupply;
  public bool isAutoRefuel;
  public bool isRepairing;
  public bool isResupplying;
  public bool isRefueling;
  public float fleetDv;
  public float fleetDvMax;
  public float fleetMaxAcceleration = -1f;
  public float maxAccG;
  [Header("LIMITERS")]
  private float DvLimit = float.PositiveInfinity;
  public string DvLimiter;
  private float accLimit = float.PositiveInfinity;
  public string AccLimiter;
  [Header("CONTROL")]
  [SerializeField]
  private FleetState defaultState;
  public FleetState state;

  private void Start()
  {
    if (this.isInitialized)
      return;
    this.Init();
  }

  public void Init()
  {
    if (this.isInitialized)
      return;
    this.track = this.GetComponent<Track>();
    this.orbiter = this.GetComponent<Orbiter>();
    this.navigation = this.GetComponent<Navigation>();
    this.ships = ((IEnumerable<S_Ship>) this.GetComponentsInChildren<S_Ship>()).ToList<S_Ship>();
    this.trader = this.GetComponent<Trader>();
    this.notifs = NotificationsManager.current;
    this.tm = TimeManager.current;
    this.state = this.defaultState;
    if (this.defaultState == FleetState.TRADING)
      this.trader.SetOn(true);
    if (this.track.factionID != 1)
    {
      this.isAutoRefuel = true;
      this.isAutoResupply = true;
      this.isAutoRepair = true;
    }
    this.navigation.onArrival += new Action(this.OnArrival);
    if ((bool) (UnityEngine.Object) this.tm)
      this.tm.NewHour += new Action(this.NewHour);
    this.UpdateFleet();
    this.SetupState();
    this.isInitialized = true;
  }

  private void Update()
  {
    if (this.ships.Count == 0 || (double) this.fleetMaxAcceleration == -1.0)
      this.UpdateFleet();
    this.UpdateDv();
  }

  private bool CheckStation()
  {
    if ((bool) (UnityEngine.Object) this.transform.parent && (bool) (UnityEngine.Object) this.transform.parent.GetComponent<StationManager>())
      this.station = this.transform.parent.GetComponent<StationManager>();
    return (UnityEngine.Object) this.station != (UnityEngine.Object) null;
  }

  private void NewHour()
  {
    if (!(bool) (UnityEngine.Object) this.station)
      return;
    if (this.isRepairing)
      this.Repair();
    else if (this.isRefueling)
    {
      this.Refuel();
    }
    else
    {
      if (!this.isResupplying)
        return;
      this.Resupply();
    }
  }

  private void Repair()
  {
    Market component = this.station.GetComponent<Market>();
    bool flag1 = true;
    int num1 = 0;
    float num2 = 0.0f;
    string str = "";
    foreach (S_Ship ship in this.ships)
    {
      bool flag2 = false;
      bool flag3 = false;
      S_Module2 sModule2 = (S_Module2) null;
      S_Mount2 sMount2 = (S_Mount2) null;
      foreach (S_Module2 module in ship.modules)
      {
        if ((double) module.health < 100.0)
        {
          flag2 = true;
          flag3 = true;
          sModule2 = module;
          break;
        }
        foreach (S_Mount2 mount in module.mounts)
        {
          if ((double) mount.health < 100.0)
          {
            flag2 = true;
            sMount2 = mount;
            break;
          }
        }
      }
      if (flag2)
      {
        flag1 = false;
        int credits = this.track.GetFaction().credits;
        int totalCost = 0;
        ResourceQuantity resourceQuantity = !flag3 ? sMount2.bp.RepairQuantity : sModule2.bp.RepairQuantity;
        float quantity = resourceQuantity.quantity;
        component.ExecuteTrade(resourceQuantity.resource, ref quantity, ref credits, ref totalCost);
        this.track.GetFaction().credits = credits;
        num1 += totalCost;
        num2 += quantity;
        if (totalCost > credits)
          str = "INSUFFICIENT FUNDS";
        else if ((double) quantity < (double) resourceQuantity.quantity)
        {
          str = "NO AVAILABLE MATERIALS";
        }
        else
        {
          if (flag3)
            sModule2.AddHealth(50f);
          else
            sMount2.AddHealth(50f);
          if ((double) quantity == 0.0)
            break;
        }
      }
    }
    if (this.track.factionID == 1)
    {
      if ((double) num2 > 0.0)
        this.Notify($"REPAIRED: -{num1.ToString()}cr", "yellow");
      else
        this.Notify("NO REPAIR: " + str, "orange");
    }
    if (!flag1)
      return;
    this.ResetState();
  }

  private void Resupply()
  {
    Market component = this.station.GetComponent<Market>();
    bool flag1 = true;
    int num1 = 0;
    float num2 = 0.0f;
    string str = "UNKNOWN";
    foreach (S_Ship ship in this.ships)
    {
      bool flag2 = false;
      bool flag3 = false;
      S_Module2 sModule2 = (S_Module2) null;
      S_Mount2 sMount2 = (S_Mount2) null;
      IResupplyable resupplyable = (IResupplyable) null;
      foreach (S_Module2 module in ship.modules)
      {
        resupplyable = (IResupplyable) null;
        if (module.bp is IResupplyable)
        {
          resupplyable = module.bp as IResupplyable;
          if ((double) module.supplies < (double) resupplyable.GetResourceMax())
          {
            flag2 = true;
            flag3 = true;
            sModule2 = module;
            break;
          }
          foreach (S_Mount2 mount in module.mounts)
          {
            if (mount.bp is IResupplyable)
            {
              resupplyable = mount.bp as IResupplyable;
              if ((double) mount.resource < (double) resupplyable.GetResourceMax())
              {
                flag2 = true;
                sMount2 = mount;
                break;
              }
            }
          }
          if (flag2)
            break;
        }
      }
      if (flag2 && (!flag3 || (bool) (UnityEngine.Object) sModule2) && (flag3 || (bool) (UnityEngine.Object) sMount2) && resupplyable != null)
      {
        flag1 = false;
        int credits = this.track.GetFaction().credits;
        int totalCost = 0;
        ResupplyCycle resupplyCycle = resupplyable.GetResupplyCycle();
        float quantity = resupplyCycle.input.quantity;
        component.ExecuteTrade(resupplyCycle.input.resource, ref quantity, ref credits, ref totalCost);
        this.track.GetFaction().credits = credits;
        num1 += totalCost;
        num2 += quantity;
        if (totalCost > credits)
          str = "INSUFFICIENT FUNDS";
        else if ((double) quantity < (double) resupplyCycle.input.quantity)
        {
          str = "NO AVAILABLE SUPPLIES";
        }
        else
        {
          if (flag3)
            sModule2.AddSupplies(resupplyable.GetResupplyCycle().output);
          else
            sMount2.AddResource(resupplyable.GetResupplyCycle().output);
          if ((double) quantity == 0.0)
            break;
        }
      }
    }
    if (this.track.factionID == 1)
    {
      if ((double) num2 > 0.0)
        this.Notify($"RESUPPLIED: -{num1.ToString()}cr", "yellow");
      else
        this.Notify("NO RESUPPLY: " + str, "orange");
    }
    if (!flag1)
      return;
    this.ResetState();
  }

  private void Refuel()
  {
    Market component = this.station.GetComponent<Market>();
    bool flag = true;
    int num1 = 0;
    float num2 = 0.0f;
    string str = "";
    foreach (S_Ship sShip in this.ships.OrderBy<S_Ship, float>((Func<S_Ship, float>) (s => s.GetDvOnly())).ToList<S_Ship>())
    {
      float missingFuel1 = sShip.GetMissingFuel();
      if ((double) missingFuel1 > 0.0)
      {
        int credits = this.track.GetFaction().credits;
        ResourceDefinition fuelResource = sShip.GetFuelResource();
        int totalCost = 0;
        component.ExecuteTrade(fuelResource, ref missingFuel1, ref credits, ref totalCost);
        this.track.GetFaction().credits = credits;
        num1 += totalCost;
        float mod = missingFuel1 * 10f;
        num2 += mod;
        if (totalCost > credits)
          str = "INSUFFICIENT FUNDS";
        else if ((double) mod <= 0.0)
        {
          str = "NO AVAILABLE FUEL";
        }
        else
        {
          sShip.AddFuel(mod);
          float missingFuel2 = sShip.GetMissingFuel();
          if ((double) mod != 0.0 && (double) missingFuel2 > 0.99000000953674316)
            flag = false;
        }
      }
    }
    if (this.track.factionID == 1)
    {
      if ((double) num2 > 0.0)
        this.Notify($"REFUELED: -{num1.ToString()}cr", "yellow");
      else
        this.Notify("NO REFUEL: " + str, "orange");
    }
    if (!flag)
      return;
    this.ResetState();
  }

  private void Notify(string message, string color)
  {
    this.notifs.NewNotif($"[{this.track.GetFullName()}] {message}", color);
  }

  private void ResetState()
  {
    if (this.trader.isAutoTrading)
      this.SetStateTo(FleetState.TRADING);
    else
      this.SetStateTo(this.defaultState);
  }

  private void OnArrival()
  {
    if (!(bool) (UnityEngine.Object) this.transform.parent)
      return;
    this.station = !(bool) (UnityEngine.Object) this.transform.parent.GetComponent<StationManager>() ? (StationManager) null : this.transform.parent.GetComponent<StationManager>();
    if (!(bool) (UnityEngine.Object) this.station || !this.isAutoRefuel)
      return;
    this.SetStateTo(FleetState.REFUELING);
  }

  private void SetupState()
  {
    if (this.state == FleetState.IDLE)
      return;
    if (this.state == FleetState.TRADING)
    {
      this.trader.isTrading = true;
    }
    else
    {
      if (this.state == FleetState.REFUELING)
        return;
      int state = (int) this.state;
    }
  }

  public void SetStateTo(FleetState newState)
  {
    if (this.state == newState)
      return;
    this.state = newState;
    this.trader.isTrading = false;
    this.isRepairing = false;
    this.isResupplying = false;
    this.isRefueling = false;
    if (this.state == FleetState.IDLE)
      return;
    if (this.state == FleetState.TRADING)
    {
      if (!this.trader.isAutoTrading)
        return;
      this.trader.isTrading = true;
    }
    else if (this.state == FleetState.REPAIRING)
      this.isRepairing = true;
    else if (this.state == FleetState.RESUPPLYING)
      this.isResupplying = true;
    else if (this.state == FleetState.REFUELING)
    {
      this.isRefueling = true;
    }
    else
    {
      int state = (int) this.state;
    }
  }

  public void UpdateDv()
  {
    this.DvLimit = float.PositiveInfinity;
    float num = 0.0f;
    if (this.ships.Count == 0)
    {
      this.fleetDv = 0.0f;
      this.fleetDvMax = 0.0f;
      this.maxAccG = 0.0f;
    }
    else
    {
      foreach (S_Ship ship in this.ships)
      {
        float dv = 0.0f;
        float dvMax = 0.0f;
        ship.GetDvAndMax(out dv, out dvMax);
        if ((double) dv < (double) this.DvLimit)
        {
          this.DvLimit = dv;
          num = dvMax;
          this.DvLimiter = $"{ship.trackID} - {ship.publicName}";
        }
      }
      this.fleetDv = this.DvLimit;
      this.fleetDvMax = num;
    }
  }

  public void UpdateFleet()
  {
    this.ships.Clear();
    IEnumerator enumerator = (IEnumerator) this.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if ((UnityEngine.Object) current == (UnityEngine.Object) null)
          this.ships.Remove(current.GetComponent<S_Ship>());
        else if ((bool) (UnityEngine.Object) current.GetComponent<S_Ship>() && !this.ships.Contains(current.GetComponent<S_Ship>()))
          this.ships.Add(current.GetComponent<S_Ship>());
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    this.accLimit = float.PositiveInfinity;
    foreach (S_Ship ship in this.ships)
    {
      ship.Init();
      float maxAcceleration = ship.maxAcceleration;
      if ((double) maxAcceleration < (double) this.accLimit)
      {
        this.accLimit = maxAcceleration;
        this.AccLimiter = $"{ship.trackID} - {ship.publicName}";
      }
    }
    this.UpdateDv();
    this.fleetMaxAcceleration = this.accLimit;
    this.maxAccG = this.fleetMaxAcceleration / 9.81f;
  }

  public void ConsumeDv(float acc)
  {
    foreach (S_Ship ship in this.ships)
      ship.ConsumeDv(acc);
  }

  public TacticalGroupData ToTacticalGroup()
  {
    Track component = this.GetComponent<Track>();
    TacticalGroupData tacticalGroup = new TacticalGroupData()
    {
      factionID = component.factionID,
      trackID = component.id,
      publicName = component.publicName,
      objects = new List<TacticalShipData>(),
      initPos = this.transform.position
    };
    foreach (S_Ship ship in this.ships)
      tacticalGroup.objects.Add(ship.ToTacticalData());
    return tacticalGroup;
  }

  public void DestroyOldShips()
  {
    IEnumerator enumerator = (IEnumerator) this.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if ((bool) (UnityEngine.Object) current.GetComponent<S_Ship>())
          UnityEngine.Object.Destroy((UnityEngine.Object) current.gameObject);
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
  }

  public void PopulateFleet(List<TacticalShipData> objects)
  {
    MonoBehaviour.print((object) $"======== POPULATING {this.name} spawning: {objects.Count.ToString()} objects. Ships: {this.ships.Count<S_Ship>().ToString()} children: {this.transform.childCount.ToString()}");
    this.ships.Clear();
    foreach (TacticalShipData objectData in objects)
      this.LoadShipInFleet(objectData);
    this.Init();
  }

  private void LoadShipInFleet(TacticalShipData objectData)
  {
    MonoBehaviour.print((object) $"======= loading ship: {objectData.publicName} {objectData.bp.GetFullClassName()}");
    GameObject prefab = this.ssl.GetPrefab(objectData.bp);
    MonoBehaviour.print((object) ("spawning prefab: " + prefab.name));
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("Missing prefab for: " + objectData.bp.GetFullClassName()));
    }
    else
    {
      GameObject shipGO = UnityEngine.Object.Instantiate<GameObject>(prefab, this.transform);
      S_Ship component = shipGO.GetComponent<S_Ship>();
      this.ships.Add(component);
      MonoBehaviour.print((object) ("spawning ship: " + shipGO.name));
      component.factionID = objectData.factionID;
      component.trackID = objectData.trackID;
      component.publicName = objectData.publicName;
      component.UpdateDv();
      this.LoadModulesInShip(objectData, shipGO);
    }
  }

  private void LoadModulesInShip(TacticalShipData objectData, GameObject shipGO)
  {
    List<S_Module2> sModule2List = new List<S_Module2>();
    MonoBehaviour.print((object) ("loading modules in ship: " + objectData.publicName));
    foreach (TacticalModuleData module in objectData.modules)
    {
      MonoBehaviour.print((object) ("===== loading module from data: " + module.bpKey));
      if (!(module.bpKey == "") && module.bpKey != null && !string.IsNullOrEmpty(module.bpKey))
      {
        IEnumerator enumerator = (IEnumerator) shipGO.transform.GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            Transform current = (Transform) enumerator.Current;
            if ((bool) (UnityEngine.Object) current.GetComponent<S_Module2>())
            {
              S_Module2 component = current.GetComponent<S_Module2>();
              if (!sModule2List.Contains(component))
              {
                if (component.bp.PrefabKey != module.bpKey)
                {
                  MonoBehaviour.print((object) ("module doesn't match: " + component.bp.PrefabKey));
                }
                else
                {
                  MonoBehaviour.print((object) ("FOUND MODULE in S_Ship: " + component.name));
                  component.InitModuleData(module);
                  sModule2List.Add(component);
                  this.LoadMountsInModule(module, component);
                  break;
                }
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
        if (!sMount2List.Contains(mount2) && !(mount2.bp.PrefabKey != mount1.bpKey))
        {
          mount2.InitMountData(mount1);
          sMount2List.Add(mount2);
          break;
        }
      }
    }
  }

  private void OnDestroy()
  {
    if ((bool) (UnityEngine.Object) this.tm)
      this.tm.NewHour -= new Action(this.NewHour);
    this.StopAllCoroutines();
  }
}
