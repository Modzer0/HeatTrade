// Decompiled with JetBrains decompiler
// Type: S_Mount
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class S_Mount
{
  [Header("INFO")]
  public string prefabName;
  public string productName;
  public PartType partType;
  public SizeClass sizeClass;
  public ResourceQuantity repairQuantity;
  public ResupplyCycle resupplyCycle;
  [Header("DATA")]
  public float health;
  public float healthMax;
  public float resource;
  public int resourceMax;
  public int value;
  public int mass;
  [Header("POWER AND HEAT")]
  public float power;
  public float heat;
  public HeatClass heatClass;

  public TacticalMountData ToTacticalData()
  {
    return new TacticalMountData()
    {
      productName = this.productName,
      health = this.health,
      resource = this.resource
    };
  }

  public void SetFromPD(PartPrefabData pd)
  {
    this.productName = pd.productName;
    this.partType = pd.partType;
    this.healthMax = (float) pd.healthMax;
    this.resourceMax = pd.resourceMax;
    this.mass = Mathf.RoundToInt(pd.mass);
    this.sizeClass = pd.sizeClass;
    this.repairQuantity = pd.repairQuantity;
    this.resupplyCycle = pd.resupplyCycle;
    this.power = pd.power;
    this.heat = pd.heat;
    this.heatClass = pd.heatClass;
  }

  public float GetHealthRatio()
  {
    return (double) this.healthMax == 0.0 ? 0.0f : Mathf.Clamp01(this.health / this.healthMax);
  }

  public float GetResourceRatio()
  {
    return this.resourceMax == 0 ? 0.0f : Mathf.Clamp01(this.resource / (float) this.resourceMax);
  }

  public void AddHealth(float mod)
  {
    this.health += mod;
    this.health = Mathf.Clamp(this.health, 0.0f, this.healthMax);
  }

  public void AddResource(int mod)
  {
    this.resource += (float) mod;
    this.resource = Mathf.Clamp(this.resource, 0.0f, (float) this.resourceMax);
  }
}
