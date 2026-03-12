// Decompiled with JetBrains decompiler
// Type: S_Module
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class S_Module : MonoBehaviour
{
  private PartsLibraryManager plm;
  [Header("= VARIABLES ====================")]
  public Structure structure;
  [Header("INFO")]
  public string prefabName;
  public string productName;
  public PartType partType;
  public SizeClass sizeClass;
  public ResourceQuantity repairQuantity;
  public ResupplyCycle resupplyCycle;
  [Header("SIZE")]
  public float length;
  public float diameter;
  public float density;
  public float volume;
  [Header("DATA")]
  public float health;
  public float healthMax;
  public float resource;
  public float resourceMax;
  public int value;
  public float mass;
  [Header("POWER AND HEAT")]
  public float power;
  public float heat;
  public HeatClass heatClass;
  [Header("= MOUNT ====================")]
  public List<S_Mount> mounts = new List<S_Mount>();

  public virtual void Start()
  {
    this.plm = PartsLibraryManager.current;
    this.structure = this.transform.parent.GetComponent<Structure>();
    this.SetVolume();
  }

  public float GetHealthRatio()
  {
    return (double) this.healthMax == 0.0 ? 0.0f : Mathf.Clamp01(this.health / this.healthMax);
  }

  public float GetResourceRatio()
  {
    return (double) this.resourceMax == 0.0 ? 0.0f : Mathf.Clamp01(this.resource / this.resourceMax);
  }

  public void AddHealth(float mod)
  {
    this.health += mod;
    this.health = Mathf.Clamp(this.health, 0.0f, this.healthMax);
  }

  public void AddResource(int mod)
  {
    this.resource += (float) mod;
    this.resource = Mathf.Clamp(this.resource, 0.0f, this.resourceMax);
  }

  private void SetDataFromPartsLibrary()
  {
    this.SetFromPD(this.plm.library.GetPart(this.prefabName));
  }

  private void SetVolume()
  {
    this.volume = 3.14159274f * Mathf.Pow(this.diameter / 2f, 2f) * this.length;
  }

  public TacticalModuleData ToTacticalData()
  {
    TacticalModuleData tacticalData = new TacticalModuleData();
    tacticalData.health = this.health;
    tacticalData.resource = this.resource;
    List<TacticalMountData> tacticalMountDataList = new List<TacticalMountData>();
    foreach (S_Mount mount in this.mounts)
      tacticalMountDataList.Add(mount.ToTacticalData());
    tacticalData.mounts = tacticalMountDataList;
    return tacticalData;
  }

  public void SetFromPD(PartPrefabData mpd)
  {
    if (mpd == null)
      return;
    this.productName = mpd.productName;
    this.partType = mpd.partType;
    this.healthMax = (float) mpd.healthMax;
    this.resourceMax = (float) mpd.resourceMax;
    this.sizeClass = mpd.sizeClass;
    this.repairQuantity = mpd.repairQuantity;
    this.resupplyCycle = mpd.resupplyCycle;
    if (this.repairQuantity == null)
      MonoBehaviour.print((object) "no repair quantity");
    if ((double) mpd.length != 0.0)
      this.length = mpd.length;
    if ((double) mpd.diameter != 0.0)
      this.diameter = mpd.diameter;
    if ((double) mpd.density != 0.0)
      this.density = mpd.density;
    if ((double) mpd.length != 0.0 && (double) mpd.diameter != 0.0 && (double) mpd.density != 0.0)
    {
      this.length = mpd.length;
      this.diameter = mpd.diameter;
      this.density = mpd.density;
      this.SetVolume();
      this.mass = (float) ((double) this.volume * (double) this.density / 1000.0);
    }
    else
    {
      this.SetVolume();
      this.mass = mpd.mass;
    }
    this.power = mpd.power;
    this.heat = mpd.heat;
    this.heatClass = mpd.heatClass;
  }
}
