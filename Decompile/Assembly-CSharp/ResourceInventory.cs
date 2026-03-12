// Decompiled with JetBrains decompiler
// Type: ResourceInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ResourceInventory : MonoBehaviour
{
  public AllResources allResources;
  [Tooltip("The total maximum cargo volume (in cubic meters/units) this entity can hold.")]
  public float storageUsed;
  [Tooltip("The current volume used by stored resources. Calculated dynamically.")]
  public float storageMax;
  [SerializeField]
  private List<ResourceQuantity> resources = new List<ResourceQuantity>();

  private void Start()
  {
    this.UpdateStorageUsed();
    this.InitResources();
  }

  private void UpdateStorageUsed()
  {
    this.storageUsed = 0.0f;
    if (this.resources.Count <= 0)
      return;
    foreach (ResourceQuantity resource in this.resources)
    {
      if ((double) resource.quantity > 0.0)
        this.storageUsed += resource.quantity;
    }
  }

  public void InitResources()
  {
    if ((Object) this.allResources == (Object) null)
      this.allResources = GlobalMarket.current.allResources;
    int count = this.allResources.resources.Count;
    if (this.resources.Count == count)
      return;
    if (this.resources.Count < count)
      this.resources.Clear();
    foreach (ResourceDefinition resource in this.allResources.resources)
      this.resources.Add(new ResourceQuantity(resource, 0.0f));
  }

  public void SetStorageMax(float newMax) => this.storageMax = newMax;

  public void Init()
  {
    if ((Object) this.allResources == (Object) null)
      this.allResources = GlobalMarket.current.allResources;
    int count = this.resources.Count;
  }

  public void GiveResources(List<ResourceQuantity> newResources)
  {
    foreach (ResourceQuantity newResource in newResources)
    {
      if ((Object) newResource.resource != (Object) null)
      {
        double num = (double) this.AddResource(newResource.resource, newResource.quantity);
      }
    }
  }

  public bool HasCargo() => (double) this.storageUsed > 0.0;

  public float GetQuantityOf(ResourceDefinition resource)
  {
    foreach (ResourceQuantity resource1 in this.resources)
    {
      if ((Object) resource1.resource == (Object) resource)
        return resource1.quantity;
    }
    return 0.0f;
  }

  public float GetTotalQuantity() => this.storageUsed;

  public void ModifyResource(ResourceDefinition resource, float mod)
  {
    foreach (ResourceQuantity resource1 in this.resources)
    {
      if ((Object) resource1.resource == (Object) resource)
      {
        resource1.quantity += mod;
        this.storageUsed += mod;
      }
    }
  }

  public bool HasResource(ResourceDefinition resource)
  {
    foreach (ResourceQuantity resource1 in this.resources)
    {
      if ((Object) resource1.resource == (Object) resource && (double) resource1.quantity > 0.0)
        return true;
    }
    return false;
  }

  public ResourceDefinition GetResource()
  {
    foreach (ResourceQuantity resource in this.resources)
    {
      if ((double) resource.quantity > 0.0)
        return resource.resource;
    }
    return (ResourceDefinition) null;
  }

  public bool IsFull() => (double) this.storageUsed >= (double) this.storageMax;

  public float GetCapacityRatio()
  {
    return (double) this.storageMax == 0.0 ? 0.0f : this.storageUsed / this.storageMax;
  }

  public float AddResource(ResourceDefinition resource, float quantity)
  {
    if ((double) quantity <= 0.0)
      return 0.0f;
    float b = this.storageMax - this.storageUsed;
    float mod = Mathf.Min(quantity, b);
    this.ModifyResource(resource, mod);
    return mod;
  }

  public float RemoveResource(ResourceDefinition resource, float quantity)
  {
    if ((double) quantity <= 0.0)
      return 0.0f;
    float quantityOf = this.GetQuantityOf(resource);
    float num = Mathf.Min(quantity, quantityOf);
    if ((double) num > 0.0)
      this.ModifyResource(resource, -num);
    return num;
  }

  public List<ResourceQuantity> GetAllStock() => this.resources;

  public float GetFreeSpace() => this.storageMax - this.storageUsed;

  public InventoryData GetInventoryData()
  {
    InventoryData inventoryData = new InventoryData();
    foreach (ResourceQuantity resource in this.resources)
      inventoryData.resources.Add(resource.GetResourceData());
    return inventoryData;
  }

  public void SetFromData(InventoryData newInventory)
  {
    this.resources.Clear();
    foreach (ResourceDefinition resource in this.allResources.resources)
      this.resources.Add(new ResourceQuantity(resource, 0.0f));
    foreach (ResourceData resource in newInventory.resources)
    {
      ResourceQuantity resourceQuantity = new ResourceQuantity(this.allResources.GetResource(resource.resourceName), resource.quantity);
      double num = (double) this.AddResource(resourceQuantity.resource, resourceQuantity.quantity);
    }
  }
}
