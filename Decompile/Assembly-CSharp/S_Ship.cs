// Decompiled with JetBrains decompiler
// Type: S_Ship
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class S_Ship : MonoBehaviour
{
  private AllStructures allStructures;
  private TimeManager tm;
  private TrackDisplayer td;
  public ShipBP bp;
  public int factionID;
  public string trackID;
  public string publicName;
  public List<S_Module2> modules;
  [SerializeField]
  private DriveModuleBP driveBP;
  [SerializeField]
  private S_Module2 drive;
  [SerializeField]
  private FuelModuleBP fuelBP;
  [SerializeField]
  private S_Module2 fuel;
  [SerializeField]
  private ResourceInventory fuelInv;
  public float maxAcceleration;
  [SerializeField]
  private float dv;
  [SerializeField]
  private float dvMax;
  public float armorMass;
  public float dryMass;
  public float fullMass;
  public float currentMass;
  private bool isInit;

  private void Start() => this.Init();

  public void Init()
  {
    if (this.isInit)
      return;
    this.allStructures = AllStructures.current;
    this.tm = TimeManager.current;
    this.td = TrackDisplayer.current;
    if ((bool) (UnityEngine.Object) this.allStructures)
      this.allStructures.AddShip(this);
    IEnumerator enumerator = (IEnumerator) this.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if ((bool) (UnityEngine.Object) current.GetComponent<S_Module2>())
          this.modules.Add(current.GetComponent<S_Module2>());
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    foreach (S_Module2 module in this.modules)
    {
      if (module.bp is DriveModuleBP bp2)
      {
        this.drive = module;
        this.driveBP = bp2;
      }
      else if (module.bp is FuelModuleBP bp1)
      {
        this.fuel = module;
        this.fuelBP = bp1;
        this.fuelInv = module.GetComponent<ResourceInventory>();
      }
    }
    this.UpdateDv();
    this.maxAcceleration = this.driveBP.CruiseThrustN / (this.currentMass * 1000f);
    if (this.trackID == null || this.trackID == "")
      this.trackID = this.td.GetTrackNumber();
    else
      this.td.AddTrackID(this.trackID);
    this.isInit = true;
  }

  public void NewShip(ShipBP newBP, int newFactionID, string newTrackID, string newPublicName)
  {
    this.bp = newBP;
    this.factionID = newFactionID;
    this.trackID = newTrackID;
    this.publicName = newPublicName;
  }

  private void UpdateMass()
  {
    float quantityOf = this.fuelInv.GetQuantityOf(this.fuelBP.FuelResource);
    float storageMax = this.fuelInv.storageMax;
    float num1 = (float) ((double) this.bp.ArmorThickness * 60.0 / 1000.0);
    this.armorMass = 0.0f;
    float num2 = 0.0f;
    foreach (S_Module2 module in this.modules)
    {
      float num3 = num1 * 3.14159274f * module.bp.Diameter * module.bp.Length;
      this.armorMass += num3;
      num2 += module.bp.Mass + num3;
    }
    this.dryMass = num2;
    this.currentMass = num2 + quantityOf;
    this.fullMass = num2 + storageMax;
  }

  public void UpdateDv()
  {
    if (!(bool) (UnityEngine.Object) this.driveBP)
      return;
    this.UpdateMass();
    float exhaustVelocity = this.driveBP.ExhaustVelocity;
    float num1 = Mathf.Log(this.currentMass / this.dryMass);
    this.dv = (float) ((double) exhaustVelocity * (double) num1 / 1000.0);
    float num2 = Mathf.Log(this.fullMass / this.dryMass);
    this.dvMax = (float) ((double) exhaustVelocity * (double) num2 / 1000.0);
  }

  public void GetDvAndMax(out float dv, out float dvMax)
  {
    this.UpdateDv();
    dv = this.dv;
    dvMax = this.dvMax;
  }

  public float GetDvOnly()
  {
    this.UpdateDv();
    return this.dv;
  }

  public void ConsumeDv(float acc)
  {
    this.UpdateMass();
    float quantity = (float) ((double) this.currentMass * (double) acc / (9.8100004196167 * (double) this.driveBP.Isp) * 87500.0) * this.tm.timeScale * Time.fixedDeltaTime;
    if ((double) quantity <= 0.0)
      return;
    double num = (double) this.fuelInv.RemoveResource(this.fuelBP.FuelResource, quantity);
  }

  public float GetMissingFuel() => this.fuelInv.GetFreeSpace();

  public ResourceDefinition GetFuelResource() => this.fuelBP.FuelResource;

  public void AddFuel(float mod)
  {
    double num = (double) this.fuelInv.AddResource(this.fuelBP.FuelResource, mod);
  }

  public float GetFuelMass() => this.fuelInv.GetQuantityOf(this.fuelBP.FuelResource);

  public float GetFuelMax() => this.fuelInv.storageMax;

  public float GetFuelRatio()
  {
    return (double) this.GetFuelMax() == 0.0 ? 0.0f : Mathf.Clamp01(this.GetFuelMass() / this.GetFuelMax());
  }

  public float GetHealthRatio()
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    foreach (S_Module2 module in this.modules)
    {
      num1 += module.health;
      num2 += 100f;
      foreach (S_Mount2 mount in module.mounts)
      {
        num1 += mount.health;
        num2 += 100f;
      }
    }
    return (double) num2 == 0.0 ? 0.0f : Mathf.Clamp01(num1 / num2);
  }

  public List<S_Module2> GetModulesOfType(PartType type)
  {
    List<S_Module2> modulesOfType = new List<S_Module2>();
    foreach (S_Module2 module in this.modules)
    {
      if (module.bp.PartType == type)
        modulesOfType.Add(module);
    }
    return modulesOfType;
  }

  public void FullResupply()
  {
    foreach (S_Module2 module in this.modules)
    {
      if (module.bp is IResupplyable)
        module.supplies = (module.bp as IResupplyable).GetResourceMax();
      foreach (S_Mount2 mount in module.mounts)
      {
        if (mount.bp is IResupplyable)
          mount.resource = (mount.bp as IResupplyable).GetResourceMax();
      }
    }
  }

  public TacticalShipData ToTacticalData()
  {
    TacticalShipData tacticalData1 = new TacticalShipData();
    tacticalData1.factionID = this.factionID;
    if (string.IsNullOrEmpty(this.trackID) || this.trackID == null)
      Debug.LogError((object) "This object has no track ID! An ID is always required.");
    tacticalData1.trackID = this.trackID;
    tacticalData1.publicName = this.publicName;
    tacticalData1.bp = this.bp;
    tacticalData1.cruiseAcceleration = this.bp.CruiseAccMps;
    IEnumerator enumerator = (IEnumerator) this.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if ((bool) (UnityEngine.Object) current.GetComponent<S_Module2>())
        {
          TacticalModuleData tacticalData2 = current.GetComponent<S_Module2>().ToTacticalData();
          tacticalData1.modules.Add(tacticalData2);
        }
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    return tacticalData1;
  }
}
