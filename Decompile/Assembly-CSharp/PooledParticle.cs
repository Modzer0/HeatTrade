// Decompiled with JetBrains decompiler
// Type: PooledParticle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class PooledParticle : MonoBehaviour, IPool
{
  private Pool pool;
  [SerializeField]
  private string poolKey;
  private ParticleSystem ps;
  [SerializeField]
  private float lifeTime = 2f;

  private void Start()
  {
    this.pool = Pool.current;
    this.ps = this.GetComponent<ParticleSystem>();
  }

  public void OnSpawn()
  {
    if ((bool) (Object) this.ps)
      this.ps.Play();
    this.StartCoroutine((IEnumerator) this.StartLifetime());
  }

  public void OnDespawn()
  {
    if ((bool) (Object) this.ps)
      this.ps.Stop();
    this.StopCoroutine((IEnumerator) this.StartLifetime());
  }

  private IEnumerator StartLifetime()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    PooledParticle pooledParticle = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      pooledParticle.pool.Return(pooledParticle.poolKey, (IPool) pooledParticle);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(pooledParticle.lifeTime);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  [SpecialName]
  GameObject IPool.get_gameObject() => this.gameObject;
}
