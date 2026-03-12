// Decompiled with JetBrains decompiler
// Type: S_FuelStorage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (ResourceInventory))]
public class S_FuelStorage : S_Module
{
  [Header("FUEL STORAGE")]
  private bool isInitialized;
  private ResourceInventory inventory;
  [SerializeField]
  private List<ResourceQuantity> initialResources = new List<ResourceQuantity>();
  public ResourceDefinition fuelResource;

  public override void Start()
  {
    if (this.isInitialized)
      return;
    this.Init();
  }

  public void Init()
  {
    base.Start();
    this.SetFuelMass();
    this.inventory = this.GetComponent<ResourceInventory>();
    this.isInitialized = true;
  }

  private void SetFuelMass()
  {
    double num1 = (double) this.volume * 0.40000000596046448;
    float num2 = this.volume * 0.6f;
    int num3 = 162;
    int num4 = 317;
    int num5 = 59;
    double num6 = (double) num3;
    float num7 = (float) (num1 * num6 / 1000.0);
    float num8 = 0.0f;
    float num9;
    if (this.fuelResource.type == ResourceType.DT_FUEL)
      num9 = (float) ((double) num2 * (double) num5 / 1000.0);
    else if (this.fuelResource.type == ResourceType.DH_FUEL)
    {
      num9 = (float) ((double) num2 * (double) num4 / 1000.0);
    }
    else
    {
      if (this.fuelResource.type != ResourceType.ANTIMATTER)
        return;
      this.resourceMax = num7 + num8;
    }
  }

  public void AddFuel(float mod)
  {
    double num = (double) this.inventory.AddResource(this.fuelResource, mod);
  }

  public float GetMissingFuel() => this.inventory.GetFreeSpace();

  public void ConsumeFuel(float fuelUsed)
  {
    double num = (double) this.inventory.RemoveResource(this.fuelResource, fuelUsed);
  }

  public float GetFuelMass()
  {
    if (!this.isInitialized)
      this.Init();
    return this.inventory.GetQuantityOf(this.fuelResource);
  }

  public float GetFuelMax()
  {
    if (!this.isInitialized)
      this.Init();
    return this.inventory.storageMax;
  }

  public float GetFuelRatio()
  {
    return (double) this.GetFuelMax() == 0.0 ? 0.0f : Mathf.Clamp01(this.GetFuelMass() / this.GetFuelMax());
  }
}
