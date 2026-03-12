// Decompiled with JetBrains decompiler
// Type: KineticMountBP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "KineticMountBP", menuName = "ScriptableObjects/KineticMountBP")]
public class KineticMountBP : MountBP, IResupplyable
{
  [Header("= KINETIC MOUNT ====================")]
  [SerializeField]
  private float suppliesMax;
  [SerializeField]
  private ResupplyCycle resupplyCycle;
  [SerializeField]
  private float muzzleVelocity;
  [SerializeField]
  private float shotCooldown;
  [SerializeField]
  private float accuracyError;
  [SerializeField]
  private string projectileKey;

  public float SuppliesMax => this.suppliesMax;

  public ResupplyCycle ResupplyCycle => this.resupplyCycle;

  public float MuzzleVelocity => this.muzzleVelocity;

  public float ShotCooldown => this.shotCooldown;

  public float AccuracyError => this.accuracyError;

  public string ProjectileKey => this.projectileKey;

  public float GetResourceMax() => this.suppliesMax;

  public ResupplyCycle GetResupplyCycle() => this.resupplyCycle;
}
