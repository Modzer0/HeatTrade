// Decompiled with JetBrains decompiler
// Type: HeatsinkModuleBP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "HeatsinkModuleBP", menuName = "ScriptableObjects/HeatsinkModuleBP")]
public class HeatsinkModuleBP : ModuleBP
{
  [Header("= HEATSINK MODULE ====================")]
  [SerializeField]
  private float heatCapacity;
  [SerializeField]
  private HeatsinkType heatsinkType;

  public float HeatCapacity => this.heatCapacity;

  public HeatsinkType HeatsinkType => this.heatsinkType;
}
