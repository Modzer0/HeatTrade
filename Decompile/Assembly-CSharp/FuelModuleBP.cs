// Decompiled with JetBrains decompiler
// Type: FuelModuleBP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "FuelModuleBP", menuName = "ScriptableObjects/FuelModuleBP")]
public class FuelModuleBP : ModuleBP, ICargo
{
  [Header("= FUEL STORAGE MODULE ====================")]
  [SerializeField]
  private ResourceDefinition fuelResource;

  public ResourceDefinition FuelResource => this.fuelResource;

  public float FuelDensity => this.Density;

  public float FuelCapacity => this.Mass;

  public float GetCargoDensity() => this.Density;
}
