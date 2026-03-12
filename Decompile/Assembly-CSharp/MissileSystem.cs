// Decompiled with JetBrains decompiler
// Type: MissileSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MissileSystem : WeaponSystem
{
  [Header("MISSILE SYSTEM")]
  public List<Pathfinder> missiles = new List<Pathfinder>();
  public Pathfinder missilePrefab;
  public MissileGuidance mg;
  public MissileMode missileMode;
  private MissileType currentMissileType;
  public int deltaV;
  public int salvoCount = 1;
  public Track target;
  private List<Pathfinder> removeList = new List<Pathfinder>();
  [SerializeField]
  private float staggerLaunchDelay = 0.5f;
  public float launchVel = 2f;
  public LayerMask targetMask;

  public override void Start() => base.Start();

  public override void Init(Track newTrack, Rigidbody newRB)
  {
    base.Init(newTrack, newRB);
    this.parentTrack = newTrack;
    this.parentRB = newRB;
    this.mg = this.GetComponent<MissileGuidance>();
    this.missileMode = MissileMode.STRIKE;
  }

  public void NewTarget(Track newTarget)
  {
    if ((Object) newTarget == (Object) null)
    {
      this.target = newTarget;
      if (!(bool) (Object) this.mg)
        return;
      this.mg.target = (Transform) null;
    }
    else
    {
      this.target = newTarget;
      if (!(bool) (Object) this.mg)
        return;
      this.mg.SetTarget(newTarget.transform);
    }
  }

  public void LaunchSalvo()
  {
    if ((Object) this.mg.target == (Object) null)
      return;
    if (this.salvoCount > this.missiles.Count)
      this.salvoCount = this.missiles.Count;
    if (this.missileMode == MissileMode.MOVE)
      this.SetMissiles(PathfinderState.MOVE);
    else if (this.missileMode == MissileMode.STRIKE)
      this.SetMissiles(PathfinderState.COLLIDE);
    this.salvoCount = 1;
  }

  public int GetMissileCount() => Mathf.CeilToInt(this.resource);

  public void SetMissiles(PathfinderState state)
  {
    if (this.missiles == null || this.missiles.Count == 0)
      return;
    int salvoCount = this.salvoCount;
    if (this.salvoCount > this.missiles.Count)
      salvoCount = this.salvoCount;
    this.resource -= (float) this.salvoCount;
    this.StartCoroutine((IEnumerator) this.StaggerLaunch(salvoCount, state));
  }

  private IEnumerator StaggerLaunch(int missilesToFire, PathfinderState state)
  {
    MissileSystem missileSystem = this;
    for (int i = 0; i < missilesToFire && !((Object) missileSystem.mg.target == (Object) null); ++i)
    {
      Pathfinder missile = (Pathfinder) null;
      foreach (Pathfinder missile1 in missileSystem.missiles)
      {
        if (!missileSystem.removeList.Contains(missile1))
        {
          missile = missile1;
          break;
        }
      }
      if (!((Object) missile == (Object) null))
      {
        missileSystem.mg.missilesToGuide.Add(missile);
        missileSystem.removeList.Add(missile);
        yield return (object) new WaitForSeconds(missileSystem.staggerLaunchDelay + (float) (10.0 * (1.0 - (double) missileSystem.healthRatio)));
        if ((double) missileSystem.healthRatio > 0.0)
          missileSystem.LaunchMissile(missile, state);
        missile = (Pathfinder) null;
      }
      else
        break;
    }
    foreach (Pathfinder remove in missileSystem.removeList)
      missileSystem.missiles.Remove(remove);
    missileSystem.removeList.Clear();
  }

  public virtual void LaunchMissile(Pathfinder missile, PathfinderState state)
  {
  }
}
