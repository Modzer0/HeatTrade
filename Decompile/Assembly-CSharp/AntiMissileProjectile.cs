// Decompiled with JetBrains decompiler
// Type: AntiMissileProjectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class AntiMissileProjectile : PoolObject
{
  [Header("Projectile settings")]
  [Tooltip("Projectile traveling speed")]
  [HideInInspector]
  public float Speed;
  [Tooltip("Projectile life time")]
  public float TimeTodestroy;
  [Tooltip("Projectile Explosion FX (Optional)")]
  public GameObject Explosion;

  private void Update() => this.transform.Translate(Vector3.forward * this.Speed * Time.deltaTime);

  private void OnEnable() => this.StartCoroutine((IEnumerator) this.DestroyDelay());

  protected virtual void OnCollisionEnter(Collision col)
  {
    if (this.IsPooling)
      this.Destroy(this.gameObject);
    else
      Object.Destroy((Object) this.gameObject, 0.1f);
    if (!((Object) this.Explosion != (Object) null))
      return;
    Object.Instantiate<GameObject>(this.Explosion, this.transform.position, this.transform.rotation);
  }

  private IEnumerator DestroyDelay()
  {
    AntiMissileProjectile missileProjectile = this;
    yield return (object) new WaitForSeconds(missileProjectile.TimeTodestroy);
    if (missileProjectile.IsPooling)
      missileProjectile.Destroy(missileProjectile.gameObject);
    else
      Object.Destroy((Object) missileProjectile.gameObject, 0.1f);
    if ((Object) missileProjectile.Explosion != (Object) null)
      Object.Instantiate<GameObject>(missileProjectile.Explosion, missileProjectile.transform.position, missileProjectile.transform.rotation);
  }

  public override void OnobjectReuse(Vector3 target, float speed)
  {
    this.transform.LookAt(target);
    this.Speed = speed;
  }
}
