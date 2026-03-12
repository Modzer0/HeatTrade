// Decompiled with JetBrains decompiler
// Type: ExplosionPhysicsForce
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ExplosionPhysicsForce : MonoBehaviour
{
  public float ExplosionForce = 4f;
  public float Radius = 10f;

  private IEnumerator Start()
  {
    ExplosionPhysicsForce explosionPhysicsForce = this;
    yield return (object) null;
    float num = 10f;
    Collider[] colliderArray = Physics.OverlapSphere(explosionPhysicsForce.transform.position, num);
    List<Rigidbody> rigidbodyList = new List<Rigidbody>();
    foreach (Collider collider in colliderArray)
    {
      if ((Object) collider.attachedRigidbody != (Object) null && !rigidbodyList.Contains(collider.attachedRigidbody))
        rigidbodyList.Add(collider.attachedRigidbody);
    }
    foreach (Rigidbody rigidbody in rigidbodyList)
      rigidbody.AddExplosionForce(explosionPhysicsForce.ExplosionForce, explosionPhysicsForce.transform.position, num, 10f, ForceMode.Impulse);
  }
}
