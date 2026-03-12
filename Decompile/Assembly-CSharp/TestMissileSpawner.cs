// Decompiled with JetBrains decompiler
// Type: TestMissileSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class TestMissileSpawner : MonoBehaviour
{
  [SerializeField]
  private GameObject missilePrefab;
  [SerializeField]
  private Transform target;
  [SerializeField]
  private float spawnDist;
  [SerializeField]
  private bool isSpawnAroundTarget;
  private MissileGuidance mg;
  [SerializeField]
  private bool isArmOnSpawn;
  [SerializeField]
  private bool isAuto;
  [SerializeField]
  private int spawnIntervalSeconds;
  private float nextSpawnTime;

  private void Start()
  {
    this.mg = this.GetComponent<MissileGuidance>();
    this.mg.SetTarget(this.target);
  }

  private void Update()
  {
    if (!this.isAuto || (double) Time.time <= (double) this.nextSpawnTime)
      return;
    this.SpawnMissile();
    this.nextSpawnTime = Time.time + (float) this.spawnIntervalSeconds;
  }

  private void SpawnMissile()
  {
    Vector3 zero = Vector3.zero;
    GameObject newMissile = Object.Instantiate<GameObject>(this.missilePrefab, !this.isSpawnAroundTarget ? this.transform.position + new Vector3(Random.Range(-this.spawnDist, this.spawnDist), Random.Range(-this.spawnDist, this.spawnDist), Random.Range(-this.spawnDist, this.spawnDist)) : this.target.position + new Vector3(Random.Range(-this.spawnDist, this.spawnDist), Random.Range(-this.spawnDist, this.spawnDist), Random.Range(-this.spawnDist, this.spawnDist)), Quaternion.identity);
    newMissile.SetActive(true);
    newMissile.GetComponent<Missile2>().Activate(0);
    this.StartCoroutine((IEnumerator) this.MissileInit(newMissile));
  }

  private IEnumerator MissileInit(GameObject newMissile)
  {
    yield return (object) new WaitForSeconds(0.1f);
    Pathfinder component1 = newMissile.GetComponent<Pathfinder>();
    component1.targetPos = this.mg.GetTargetPos(component1);
    component1.TrySetState(PathfinderState.COLLIDE);
    Missile2 component2 = newMissile.GetComponent<Missile2>();
    if (this.isArmOnSpawn)
      component2.isArmed = true;
    component2.target = this.target.transform;
  }
}
