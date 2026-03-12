// Decompiled with JetBrains decompiler
// Type: ParticleProjectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ParticleProjectile : MonoBehaviour
{
  [SerializeField]
  private ParticleSystem ps;
  private List<ParticleCollisionEvent> colEvents = new List<ParticleCollisionEvent>();

  private void OnParticleCollision(GameObject other)
  {
    int collisionEvents = this.ps.GetCollisionEvents(other, (List<ParticleCollisionEvent>) this.colEvents);
    int num = 0;
    while (num < collisionEvents)
      ++num;
  }
}
