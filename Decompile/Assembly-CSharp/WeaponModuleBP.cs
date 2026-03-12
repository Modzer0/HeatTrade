// Decompiled with JetBrains decompiler
// Type: WeaponModuleBP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "WeaponModuleBP", menuName = "ScriptableObjects/WeaponModuleBP")]
public class WeaponModuleBP : ModuleBP, IResupplyable
{
  [Header("= WEAPON MODULE ====================")]
  [SerializeField]
  private float suppliesMax;
  [SerializeField]
  private ResupplyCycle resupplyCycle;

  public float ResourceMax => this.suppliesMax;

  public ResupplyCycle ResupplyCycle => this.resupplyCycle;

  public float GetResourceMax() => this.suppliesMax;

  public ResupplyCycle GetResupplyCycle() => this.resupplyCycle;
}
