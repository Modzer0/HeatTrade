// Decompiled with JetBrains decompiler
// Type: MachineGunController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MachineGunController : AntiMissileGunController
{
  [Header("Child class")]
  [Tooltip("parameters of child class")]
  public Animator _animator;
  private int _animatorFireHash = Animator.StringToHash("Fire");

  protected override void Start() => base.Start();

  protected override void Fire()
  {
    this._animator.SetBool(this._animatorFireHash, false);
    if (!((Object) this.target != (Object) null) || (double) this.ProjectileCount <= 0.0 || (double) Time.time <= (double) this.nextFireAllowed || !this.IsAiming)
      return;
    for (int index = 0; index < this.Barrel.Length; ++index)
    {
      if (this.UsePooling)
      {
        PoolManager.instance.ReuseObject(this.ProjectilePrefab, this.Barrel[index].position, this.Barrel[index].rotation, this.predictedTargetPosition, this.ProjectileSpeed);
      }
      else
      {
        AntiMissileProjectile component = Object.Instantiate<GameObject>(this.ProjectilePrefab, this.Barrel[index].position, this.Barrel[index].rotation).GetComponent<AntiMissileProjectile>();
        component.transform.LookAt(this.predictedTargetPosition);
        component.Speed = this.ProjectileSpeed;
      }
      --this.ProjectileCount;
    }
    this.nextFireAllowed = Time.time + this.FireRate;
    if ((Object) this.BulletShellFX != (Object) null)
    {
      this.bulletShellFX_PS.Play();
      this.Invoke("StopBulletShellEffect", 1.2f);
    }
    this._animator.SetBool(this._animatorFireHash, true);
    if ((Object) this.ShootFX == (Object) null)
      return;
    this.shootFX_PS.Play();
  }
}
