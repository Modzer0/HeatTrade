// Decompiled with JetBrains decompiler
// Type: HeatManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HeatManager : MonoBehaviour
{
  private ShipController ship;
  public List<T_Module> modules = new List<T_Module>();
  public List<T_Mount> mounts = new List<T_Mount>();
  public List<IHealth> healths = new List<IHealth>();
  public List<T_Radiator> radiators = new List<T_Radiator>();
  public List<T_Nozzle> nozzles = new List<T_Nozzle>();
  public List<T_HeatSink> heatsinks = new List<T_HeatSink>();
  public float netHeatFlow;
  public float heatPercent;
  public float currentHeat;
  public float heatsinkCurrent;
  public float heatsinkMax;
  public float heatsinkMaxMax;
  public float capacity;
  private float overflowMaxMW = 10000f;
  public float timeToOverflow;
  public float heatIn;
  public float heatOut;
  public bool isRadiatorsExtended = true;

  public void Init(List<T_Module> allModules)
  {
    this.ship = this.GetComponent<ShipController>();
    foreach (T_Module allModule in allModules)
    {
      this.modules.Add(allModule);
      this.healths.Add(allModule.GetComponent<IHealth>());
      if (allModule.partType == PartType.HEATSINK)
      {
        this.heatsinkMaxMax += allModule.heatCapacityMaxMJ;
        this.heatsinks.Add(allModule.GetComponent<T_HeatSink>());
      }
      if ((bool) (Object) allModule.GetComponent<T_Nozzle>())
        this.nozzles.Add(allModule.GetComponent<T_Nozzle>());
      foreach (T_Mount mount in allModule.mounts)
      {
        this.mounts.Add(mount);
        this.healths.Add(mount.GetComponent<IHealth>());
        if ((bool) (Object) mount.GetComponent<T_Radiator>() && (Object) mount != (Object) null)
          this.radiators.Add(mount.GetComponent<T_Radiator>());
      }
    }
  }

  private void Update()
  {
    if (!(bool) (Object) this.ship || this.ship.isDead)
      return;
    this.UpdateCurrentHeat();
    this.UpdateHeatsinkMax();
    this.UpdateHeatsinkCurrent();
    this.timeToOverflow = 0.0f;
    this.timeToOverflow = this.capacity / this.netHeatFlow;
    this.Overflow();
    this.ApplyRadiatorGlow();
  }

  private void UpdateCurrentHeat()
  {
    if ((double) this.currentHeat < 0.0)
      this.currentHeat = 0.0f;
    this.netHeatFlow = 0.0f;
    this.heatIn = 0.0f;
    this.heatOut = 0.0f;
    foreach (T_Module module in this.modules)
    {
      if (module.isOn)
      {
        this.netHeatFlow += module.currentHeat;
        if ((double) module.currentHeat > 0.0)
          this.heatIn += module.currentHeat;
        if ((double) module.currentHeat < 0.0)
          this.heatOut += module.currentHeat;
      }
    }
    foreach (T_Mount mount in this.mounts)
    {
      if (!((Object) mount == (Object) null) && mount.isOn)
      {
        this.netHeatFlow += mount.currentHeat;
        if ((double) mount.currentHeat > 0.0)
          this.heatIn += mount.currentHeat;
        if ((double) mount.currentHeat < 0.0)
          this.heatOut += mount.currentHeat;
      }
    }
    this.currentHeat += this.netHeatFlow * Time.deltaTime;
  }

  private void UpdateHeatsinkMax()
  {
    this.heatsinkMax = 0.0f;
    foreach (T_HeatSink heatsink in this.heatsinks)
      this.heatsinkMax += heatsink.heatCapacityMJ * heatsink.healthRatio;
    this.capacity = this.heatsinkMax - this.heatsinkCurrent;
  }

  private void UpdateHeatsinkCurrent()
  {
    if ((double) this.heatsinkCurrent >= 0.0 && (double) this.capacity > 0.0 || (double) this.currentHeat < 0.0 && (double) this.capacity <= 0.0)
    {
      if ((double) this.heatsinkCurrent + (double) this.currentHeat > (double) this.heatsinkMax)
      {
        this.heatsinkCurrent = this.heatsinkMax;
        this.currentHeat -= this.capacity;
      }
      else
      {
        this.heatsinkCurrent += this.currentHeat;
        this.currentHeat = 0.0f;
      }
    }
    else
    {
      if ((double) this.heatsinkCurrent >= 0.0)
        return;
      this.heatsinkCurrent = 0.0f;
    }
  }

  private void Overflow()
  {
    this.heatPercent = this.currentHeat / this.overflowMaxMW;
    if (float.IsNaN(this.heatPercent))
      this.heatPercent = 0.0f;
    double num1 = (double) Mathf.Clamp01(this.heatPercent);
    if ((double) this.currentHeat > (double) this.overflowMaxMW)
    {
      this.ship.SoftDeath();
      this.currentHeat = this.overflowMaxMW;
    }
    else if ((double) this.currentHeat > 100.0)
    {
      float num2 = Mathf.Lerp(1f, 200f, this.heatPercent);
      int num3 = 0;
      IHealth health;
      do
      {
        do
        {
          health = this.healths[Random.Range(0, this.healths.Count)];
        }
        while (health == null);
        if ((double) health.GetHealth() == 0.0)
          ++num3;
        else
          goto label_9;
      }
      while (num3 < this.healths.Count);
      goto label_10;
label_9:
      health.ModifyHealth(-num2 * Time.deltaTime);
    }
label_10:
    foreach (T_Module module in this.modules)
      module.UpdateHeatFX(this.heatPercent);
  }

  public void ToggleRadiators(bool? isOn = null)
  {
    this.isRadiatorsExtended = !isOn.HasValue ? !this.isRadiatorsExtended : isOn.Value;
    foreach (T_Radiator radiator in this.radiators)
      radiator.Toggle(this.isRadiatorsExtended);
  }

  private void ApplyRadiatorGlow()
  {
    float f = 0.0f;
    if ((double) this.heatOut != 0.0)
      f = this.heatIn / this.heatOut;
    float num = Mathf.Abs(f);
    for (int index = 0; index < this.radiators.Count; ++index)
      this.radiators[index].targetDissipationRatio = num;
  }
}
