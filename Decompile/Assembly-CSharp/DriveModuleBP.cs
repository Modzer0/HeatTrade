// Decompiled with JetBrains decompiler
// Type: DriveModuleBP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "DriveModuleBP", menuName = "ScriptableObjects/DriveModuleBP")]
public class DriveModuleBP : ModuleBP
{
  [Header("= DRIVE MODULE ====================")]
  [SerializeField]
  private PropulsionType propulsionType;
  [SerializeField]
  private ResourceDefinition fuelType;
  [Header("POWER")]
  [SerializeField]
  private float totalOutput;
  [SerializeField]
  private float electricOutput;
  [SerializeField]
  private float thermalOutput;
  [SerializeField]
  private float thermalEfficiency;
  [SerializeField]
  private float thrustOutput;
  [SerializeField]
  private float wasteOutput;
  [Header("PROPULSION")]
  [SerializeField]
  private float isp;
  [SerializeField]
  private float massFlowRate;
  [SerializeField]
  private float cruiseThrustN;

  public PropulsionType PropulsionType => this.propulsionType;

  public ResourceDefinition FuelType => this.fuelType;

  public float TotalOutput => this.totalOutput;

  public float ElectricOutput => this.electricOutput;

  public float ThermalOutput => this.thermalOutput;

  public float ThermalEfficiency => this.thermalEfficiency;

  public float ThrustOutput => this.thrustOutput;

  public float WasteOutput => this.wasteOutput;

  public float Isp => this.isp;

  public float ExhaustVelocity => this.isp * 9.81f;

  public float MassFlowRate => this.massFlowRate;

  public float CruiseThrustN => this.cruiseThrustN;
}
