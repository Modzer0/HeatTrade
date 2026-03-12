// Decompiled with JetBrains decompiler
// Type: T_Module
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class T_Module : MonoBehaviour, IHealth
{
  public ShipController sc;
  [Header("Blueprint")]
  [SerializeField]
  private ModuleBP bp;
  [Header("Tactical Module")]
  public string prefabName;
  public string productName;
  public float health;
  public float healthRatio;
  public float armorThickness;
  public float armorHealth;
  public float armorHealthMax;
  public float resource;
  public InventoryData inventoryData;
  [Header("MPD DATA")]
  public PartType partType;
  public float healthMax;
  public float resourceMax;
  public float mass;
  [Header("POWER AND HEAT")]
  public bool isOn;
  public bool isDead;
  public float power;
  public float heat;
  public float currentPower;
  public float currentHeat;
  public HeatClass heatClass;
  public float heatCapacityMJ;
  public float heatCapacityMaxMJ;
  [Header("MOUNT")]
  [SerializeField]
  public List<T_Mount> mounts = new List<T_Mount>();
  [Header("HEAT FX")]
  [SerializeField]
  private float intensity = 3f;
  [SerializeField]
  private Renderer moduleRenderer;
  [SerializeField]
  private Color heatColor;
  private Color orange;
  [Header("DAMAGE CONTROL")]
  public bool isBeingFixed;
  [Header("DAMAGE FX")]
  [SerializeField]
  private Material mat;

  public ModuleBP BP => this.bp;

  public virtual void Start()
  {
    this.sc = this.transform.root.GetComponent<ShipController>();
    this.healthMax = 100f;
    this.healthRatio = Mathf.Clamp01(this.health / this.healthMax);
    this.armorHealthMax = (float) ((double) this.transform.localScale.x * (double) this.transform.localScale.y * (double) this.transform.localScale.z * 100.0);
    this.armorHealth = this.armorHealthMax;
    if ((bool) (Object) this.GetComponent<Renderer>() && !(bool) (Object) this.moduleRenderer)
      this.moduleRenderer = this.GetComponent<Renderer>();
    if ((bool) (Object) this.moduleRenderer)
      this.mat = this.moduleRenderer.material;
    if ((bool) (Object) this.mat)
      this.mat.SetVector("_TileScale", (Vector4) this.transform.localScale);
    this.orange = new Color(1f, 0.098f, 0.0f, 1f);
    this.isOn = true;
  }

  public virtual void InitBP()
  {
    if (!(bool) (Object) this.bp)
      return;
    this.prefabName = this.bp.PrefabKey;
    this.productName = this.bp.PartNameFull;
    this.partType = this.bp.PartType;
    this.mass = this.bp.Mass;
  }

  private void Update()
  {
    if ((double) this.healthRatio > 0.0)
      return;
    this.isOn = false;
  }

  public void InitMounts()
  {
    this.mounts.Clear();
    foreach (T_Mount componentsInChild in this.transform.GetComponentsInChildren<T_Mount>())
    {
      if (!this.mounts.Contains(componentsInChild))
        this.mounts.Add(componentsInChild);
    }
  }

  public virtual void SetOn(bool newIsOn) => this.isOn = newIsOn;

  public void UpdateHeatFX(float heatPercent)
  {
    heatPercent = Mathf.Clamp01(heatPercent);
    float num = 1000f * heatPercent;
    if ((Object) this.moduleRenderer == (Object) null)
    {
      if (!((Object) this.GetComponent<Renderer>() != (Object) null))
        return;
      this.moduleRenderer = this.GetComponent<Renderer>();
    }
    this.moduleRenderer.material.SetFloat("_Temperature", num);
  }

  public float GetHealth() => this.health;

  public bool TryDamageKinetic(float relativeMagnitude)
  {
    if ((bool) (Object) this.sc && this.sc.isDead || !this.sc.isTakeDamage)
      return true;
    if (!(bool) (Object) this.sc)
      return false;
    bool flag = false;
    relativeMagnitude = Mathf.Abs(relativeMagnitude);
    float num1 = 0.1f;
    float damage = relativeMagnitude * num1;
    double num2 = (double) this.armorThickness - (double) damage;
    this.DamageArmor(damage);
    if (num2 < 0.0)
    {
      float num3 = relativeMagnitude;
      flag = true;
      this.ModifyHealth(-num3);
    }
    return flag;
  }

  private void DamageArmor(float damage)
  {
    this.armorHealth -= damage;
    if ((double) this.armorHealth > 0.0)
      return;
    this.armorHealth = 0.0f;
    this.armorThickness = 0.0f;
  }

  public void TryDamagePhotonic(float tryDamage)
  {
    if (!(bool) (Object) this.sc || this.sc.isDead || !this.sc.isTakeDamage)
      return;
    float num1 = 1f / 1000f;
    float damage = tryDamage * num1;
    MonoBehaviour.print((object) ("try damage photonic. actual damage: " + damage.ToString()));
    if ((double) this.armorHealth > 0.0 || (double) this.armorThickness > 0.0)
    {
      float num2 = this.armorHealth - damage;
      this.DamageArmor(damage);
      if ((double) this.armorHealth > 0.0)
        return;
      this.ModifyHealth(num2 * 10f);
    }
    else
    {
      this.armorHealth = 0.0f;
      this.ModifyHealth((float) (-(double) damage * 10.0));
    }
  }

  public void ModifyHealth(float mod)
  {
    if ((double) mod < 0.0 && !this.sc.isTakeDamage)
      return;
    this.CheckIfDCTeamKilled(mod);
    this.health += mod;
    if ((double) this.health > (double) this.healthMax)
      this.health = this.healthMax;
    else if ((double) this.health < 0.0)
    {
      this.health = 0.0f;
      this.sc.ApplyDamageRandom(mod * 0.1f);
      if (!this.isDead)
        this.SetIsDead();
    }
    this.healthRatio = (double) this.healthMax > 0.0 ? Mathf.Clamp01(this.health / this.healthMax) : 0.0f;
    if (!(bool) (Object) this.mat)
      return;
    this.mat.SetFloat("_HealthRatio", this.healthRatio);
  }

  public virtual void SetIsDead()
  {
    this.isDead = true;
    this.KillAllMounts();
    if (!this.isBeingFixed)
      return;
    this.isBeingFixed = false;
    this.sc.DCTeamFreed();
  }

  private void CheckIfDCTeamKilled(float mod)
  {
    if (!this.isBeingFixed || (double) mod >= 0.0)
      return;
    if ((double) this.health - (double) mod <= 0.0)
    {
      this.DCTeamKilled();
    }
    else
    {
      if ((double) Random.Range(0, 101) > (double) Mathf.Abs(mod))
        return;
      this.DCTeamKilled();
    }
  }

  private void DCTeamKilled()
  {
    this.isBeingFixed = false;
    this.sc.DCTeamKilled();
  }

  private void KillAllMounts()
  {
    foreach (T_Mount mount in this.mounts)
      mount.ModifyHealth(-mount.health);
  }

  public TacticalModuleData ToTacticalData()
  {
    List<TacticalMountData> tacticalMountDataList = new List<TacticalMountData>();
    foreach (T_Mount mount in this.mounts)
      tacticalMountDataList.Add(mount.ToTacticalData());
    string str = "";
    if ((Object) this.bp != (Object) null)
      str = this.bp.PrefabKey;
    return new TacticalModuleData()
    {
      bpKey = str,
      health = this.health,
      resource = this.resource,
      mounts = tacticalMountDataList,
      inventoryData = this.inventoryData
    };
  }
}
