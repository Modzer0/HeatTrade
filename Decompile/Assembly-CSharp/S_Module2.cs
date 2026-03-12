// Decompiled with JetBrains decompiler
// Type: S_Module2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class S_Module2 : MonoBehaviour
{
  public ModuleBP bp;
  public List<S_Mount2> mounts;
  public float health;
  public float armorHealth;
  public float supplies;
  public ResourceInventory inv;
  public bool isInitInv;

  private void Start()
  {
    this.inv = this.GetComponent<ResourceInventory>();
    if (!(bool) (Object) this.inv || this.isInitInv)
      return;
    this.inv.SetStorageMax(this.bp.GetSetVolume() * this.bp.Density);
    this.inv.Init();
    this.isInitInv = true;
  }

  public void NewModule(ModuleBP newBP)
  {
    this.bp = newBP;
    this.health = 100f;
    this.armorHealth = this.bp.ArmorHealthMax;
    this.supplies = 0.0f;
  }

  public void AddHealth(float mod)
  {
    this.health += mod;
    this.health = Mathf.Clamp(this.health, 0.0f, 100f);
  }

  public void AddArmorHealth(float mod)
  {
    this.armorHealth += mod;
    this.armorHealth = Mathf.Clamp(this.armorHealth, 0.0f, this.bp.ArmorHealthMax);
  }

  public void AddSupplies(int mod)
  {
    this.supplies += (float) mod;
    this.supplies = Mathf.Clamp(this.supplies, 0.0f, (this.bp as IResupplyable).GetResourceMax());
  }

  public float GetHealthRatio()
  {
    return (double) this.health == 0.0 ? 0.0f : Mathf.Clamp01(this.health / 100f);
  }

  public float GetSupplyRatio()
  {
    return (double) this.supplies == 0.0 || !(this.bp is IResupplyable) ? 0.0f : Mathf.Clamp01(this.supplies / (this.bp as IResupplyable).GetResourceMax());
  }

  public float AddCargo(ResourceDefinition resource, float quantity)
  {
    return !(bool) (Object) this.inv ? 0.0f : this.inv.AddResource(resource, quantity);
  }

  public float RemoveCargo(ResourceDefinition resource, float quantity)
  {
    return !(bool) (Object) this.inv ? 0.0f : this.inv.RemoveResource(resource, quantity);
  }

  public TacticalModuleData ToTacticalData()
  {
    TacticalModuleData tacticalData = new TacticalModuleData();
    tacticalData.bpKey = this.bp.PrefabKey;
    tacticalData.health = this.health;
    tacticalData.armorHealth = this.armorHealth;
    tacticalData.resource = this.supplies;
    if ((bool) (Object) this.inv)
      tacticalData.inventoryData = this.inv.GetInventoryData();
    List<TacticalMountData> tacticalMountDataList = new List<TacticalMountData>();
    foreach (S_Mount2 mount in this.mounts)
      tacticalMountDataList.Add(mount.ToTacticalData());
    tacticalData.mounts = tacticalMountDataList;
    return tacticalData;
  }

  public void InitModuleData(TacticalModuleData moduleData)
  {
    if (!(bool) (Object) this.inv)
      this.inv = this.GetComponent<ResourceInventory>();
    if ((bool) (Object) this.inv)
    {
      this.inv.SetStorageMax(this.bp.GetSetVolume() * this.bp.Density);
      if (moduleData.inventoryData != null && moduleData.inventoryData.resources != null && moduleData.inventoryData.resources.Count > 0)
        this.inv.SetFromData(moduleData.inventoryData);
      this.isInitInv = true;
    }
    this.health = moduleData.health;
    this.armorHealth = moduleData.armorHealth;
    this.supplies = moduleData.resource;
  }

  public InventoryData GetInventoryData()
  {
    return !(bool) (Object) this.inv ? (InventoryData) null : this.inv.GetInventoryData();
  }
}
