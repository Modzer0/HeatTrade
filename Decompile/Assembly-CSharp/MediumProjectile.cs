// Decompiled with JetBrains decompiler
// Type: MediumProjectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MediumProjectile : AntiMissileProjectile
{
  [Header("Child class")]
  [Tooltip("parameters of child class")]
  public GameObject BodyExplosion_FX;

  protected override void OnCollisionEnter(Collision col)
  {
    if (this.IsPooling)
      this.Destroy(this.gameObject);
    else
      Object.Destroy((Object) this.gameObject, 0.1f);
    switch (LayerMask.LayerToName(col.gameObject.layer))
    {
      case "EnemyPlane":
      case "EnemyTank":
        if (!((Object) this.Explosion != (Object) null))
          break;
        Object.Instantiate<GameObject>(this.Explosion, this.transform.position, this.transform.rotation);
        break;
      case "EnemySoldier":
        if (!((Object) this.BodyExplosion_FX != (Object) null))
          break;
        Object.Instantiate<GameObject>(this.BodyExplosion_FX, this.transform.position, this.transform.rotation);
        break;
      default:
        if (!((Object) this.Explosion != (Object) null))
          break;
        Object.Instantiate<GameObject>(this.Explosion, this.transform.position, this.transform.rotation);
        break;
    }
  }
}
