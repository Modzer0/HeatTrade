// Decompiled with JetBrains decompiler
// Type: Pool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Pool : MonoBehaviour
{
  public static Pool current;
  private Dictionary<string, Queue<IPool>> pools = new Dictionary<string, Queue<IPool>>();
  private Dictionary<string, IPool> prefabs = new Dictionary<string, IPool>();
  [Header("PROJECTILES")]
  public Projectile pdcBulletPF;
  public Projectile pdcBullet4PF;
  public Projectile pdcBulletS2PF;
  public Projectile railgunSlug3kmsPF;
  public Projectile railgunSlug5kmsPF;
  public Projectile railgunSlug7p5kmsPF;
  [Header("MISSILES")]
  public Missile2 shurikenPF;
  public Missile2 razePF;
  [Header("PARTICLES")]
  public PooledParticle pdcDebrisPF;
  public PooledParticle rgDebrisPF;
  [Header("DECALS")]
  public Decal pdcHolePF;
  public Decal rgHolePF;

  private void Awake()
  {
    if ((Object) Pool.current != (Object) null && (Object) Pool.current != (Object) this)
      Object.Destroy((Object) this.gameObject);
    else
      Pool.current = this;
  }

  private void Start()
  {
    this.RegisterPrefab("pdcBullet", (IPool) this.pdcBulletPF, 100);
    if ((bool) (Object) this.pdcBullet4PF)
      this.RegisterPrefab("pdcBullet4", (IPool) this.pdcBullet4PF, 50);
    if ((bool) (Object) this.pdcBulletS2PF)
      this.RegisterPrefab("pdcBulletS2", (IPool) this.pdcBulletS2PF, 100);
    this.RegisterPrefab("railgunSlug", (IPool) this.railgunSlug3kmsPF, 50);
    this.RegisterPrefab("railgunSlug5", (IPool) this.railgunSlug5kmsPF, 25);
    this.RegisterPrefab("railgunSlug7.5", (IPool) this.railgunSlug7p5kmsPF, 10);
    this.RegisterPrefab("shuriken", (IPool) this.shurikenPF, 50);
    this.RegisterPrefab("raze", (IPool) this.razePF, 50);
    this.RegisterPrefab("pdcDebris", (IPool) this.pdcDebrisPF, 50);
    this.RegisterPrefab("rgDebris", (IPool) this.rgDebrisPF, 50);
    this.RegisterPrefab("pdcHole", (IPool) this.pdcHolePF, 50);
    this.RegisterPrefab("rgHole", (IPool) this.rgHolePF, 50);
  }

  public void RegisterPrefab(string key, IPool prefab, int initialSize)
  {
    if (this.prefabs.ContainsKey(key))
      return;
    this.prefabs[key] = prefab;
    this.pools[key] = new Queue<IPool>();
    for (int index = 0; index < initialSize; ++index)
      this.CreateNew(key, this.transform);
  }

  private IPool CreateNew(string key, Transform parent)
  {
    IPool component = Object.Instantiate<GameObject>(this.prefabs[key].gameObject, parent).GetComponent<IPool>();
    component.gameObject.SetActive(false);
    this.pools[key].Enqueue(component);
    return component;
  }

  public IPool Get(string key, Vector3 position, Quaternion rotation, Transform parent = null)
  {
    if (!this.pools.ContainsKey(key) || key == string.Empty)
    {
      Debug.LogError((object) $"Pool for key '{key}' does not exist!");
      return (IPool) null;
    }
    if (this.pools[key].Count == 0)
      this.CreateNew(key, parent);
    IPool pool = this.pools[key].Dequeue();
    pool.gameObject.transform.SetPositionAndRotation(position, rotation);
    pool.gameObject.SetActive(true);
    pool.OnSpawn();
    return pool;
  }

  public void Return(string key, IPool obj)
  {
    obj.OnDespawn();
    obj.gameObject.transform.parent = this.transform;
    obj.gameObject.SetActive(false);
    this.pools[key].Enqueue(obj);
  }
}
