// Decompiled with JetBrains decompiler
// Type: S_CargoStorage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (ResourceInventory))]
public class S_CargoStorage : S_Module
{
  private ResourceInventory inventory;
  [SerializeField]
  private List<ResourceQuantity> initialResources = new List<ResourceQuantity>();

  public override void Start()
  {
    base.Start();
    this.inventory = this.GetComponent<ResourceInventory>();
    this.inventory.SetStorageMax(this.volume);
    this.inventory.GiveResources(this.initialResources);
  }

  public bool HasCargo() => (double) this.inventory.GetTotalQuantity() > 0.0;

  public ResourceDefinition GetResourceDefinition() => this.inventory.GetResource();

  public float GetQuantityOf(ResourceDefinition resource) => this.inventory.GetQuantityOf(resource);

  public float AddResource(ResourceDefinition resource, float quantity)
  {
    return this.inventory.AddResource(resource, quantity);
  }

  public float RemoveResource(ResourceDefinition resource, float quantity)
  {
    return this.inventory.RemoveResource(resource, quantity);
  }

  public float GetStorageSpace() => this.inventory.storageMax;

  public ResourceInventory GetInventory() => this.inventory;
}
