// Decompiled with JetBrains decompiler
// Type: CargoModuleBP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "CargoModuleBP", menuName = "ScriptableObjects/CargoModuleBP")]
public class CargoModuleBP : ModuleBP, ICargo
{
  public float GetCargoDensity() => this.Density;
}
