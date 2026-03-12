// Decompiled with JetBrains decompiler
// Type: S_Propulsion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class S_Propulsion : S_Module
{
  private TimeManager tm;
  private bool isInitialized;
  [Header("PROPULSION")]
  public PropulsionType propulsionType;
  public ResourceDefinition fuelType;
  private S_FuelStorage fuelStorage = new S_FuelStorage();
  [Header("OUTPUT")]
  [SerializeField]
  private float thermalOutput;
  [SerializeField]
  private float efficiency;
  [SerializeField]
  private float thermalThrottle;
  public float isp;
  public float massFlowRate;
  public float maxThrust;

  public override void Start()
  {
    if (this.isInitialized)
      return;
    this.Init();
  }

  public void Init()
  {
    base.Start();
    this.tm = TimeManager.current;
    this.SetOutput();
    this.isInitialized = true;
  }

  private void SetOutput()
  {
    this.power = this.thermalOutput * this.efficiency;
    this.heat = this.thermalOutput - this.power;
    this.maxThrust = (float) ((double) this.massFlowRate * (double) this.isp * 9.8100004196167 / 1000000.0);
  }

  public void TryAddFuel(S_FuelStorage newFuelStorage)
  {
    if (!((Object) this.fuelStorage != (Object) newFuelStorage))
      return;
    this.fuelStorage = newFuelStorage;
  }

  public void ConsumeDv(float acc)
  {
    this.structure.UpdateMass();
    float fuelUsed = (float) ((double) this.structure.currentMass * (double) acc / (9.8100004196167 * (double) this.isp) * 87500.0) * this.tm.timeScale * Time.fixedDeltaTime;
    if ((double) fuelUsed <= 0.0)
      return;
    this.fuelStorage.ConsumeFuel(fuelUsed);
  }
}
