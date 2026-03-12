// Decompiled with JetBrains decompiler
// Type: QuickLaunchSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class QuickLaunchSystem : MissileSystem
{
  private List<Pathfinder> qlsRemoveList = new List<Pathfinder>();
  [SerializeField]
  private List<Transform> slots;

  public override void Start()
  {
    base.Start();
    foreach (Transform slot in this.slots)
    {
      if (slot.childCount > 0)
        UnityEngine.Object.Destroy((UnityEngine.Object) slot.GetChild(0).gameObject);
    }
    if ((double) this.resource > (double) this.slots.Count || (double) this.resource > (double) this.resourceMax)
      this.resource = this.resourceMax;
    for (int index = 0; index < Mathf.CeilToInt(this.resource); ++index)
    {
      Pathfinder pathfinder = UnityEngine.Object.Instantiate<Pathfinder>(this.missilePrefab);
      pathfinder.transform.parent = this.slots[index];
      pathfinder.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
      this.missiles.Add(pathfinder);
      pathfinder.GetComponent<Collider>().enabled = false;
      pathfinder.GetComponent<Track>().enabled = false;
    }
  }

  private void Update()
  {
    foreach (Pathfinder missile in this.missiles)
    {
      if ((UnityEngine.Object) missile == (UnityEngine.Object) null)
        this.qlsRemoveList.Add(missile);
    }
    if (this.qlsRemoveList.Count <= 0)
      return;
    this.missiles.RemoveAll((Predicate<Pathfinder>) (missile => this.qlsRemoveList.Contains(missile)));
  }

  public override void LaunchMissile(Pathfinder missile, PathfinderState state)
  {
    Rigidbody component = missile.GetComponent<Rigidbody>();
    missile.GetComponent<Missile2>().Activate(this.parentTrack.factionID);
    component.AddRelativeForce(0.0f, this.launchVel, 0.0f, ForceMode.Impulse);
    component.AddForce(this.parentRB.velocity, ForceMode.Impulse);
    missile.transform.parent = (Transform) null;
    missile.GetComponent<Collider>().enabled = true;
    this.StartCoroutine((IEnumerator) this.LaunchDelay(missile, state));
  }

  private IEnumerator LaunchDelay(Pathfinder missile, PathfinderState state)
  {
    QuickLaunchSystem quickLaunchSystem = this;
    yield return (object) new WaitForSeconds(0.25f);
    missile.targetThrust = missile.maxThrust;
    missile.TrySetStateBrake();
    while ((bool) (UnityEngine.Object) missile && (double) missile.GetComponent<Rigidbody>().velocity.magnitude > 0.10000000149011612)
      yield return (object) null;
    while (!quickLaunchSystem.CheckLOS(missile.transform.position, quickLaunchSystem.mg.target.position))
      yield return (object) null;
    if ((UnityEngine.Object) quickLaunchSystem.target != (UnityEngine.Object) null)
    {
      if (state == PathfinderState.COLLIDE)
      {
        missile.GetComponent<Missile2>().isArmed = true;
        missile.GetComponent<Missile2>().target = quickLaunchSystem.target.transform;
      }
      missile.targetPos = quickLaunchSystem.mg.GetTargetPos(missile);
      missile.TrySetState(state);
    }
  }

  private bool CheckLOS(Vector3 origin, Vector3 targetPos)
  {
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      return false;
    bool flag = false;
    RaycastHit hitInfo;
    if (Physics.Linecast(origin, targetPos, out hitInfo, (int) this.targetMask))
    {
      if ((UnityEngine.Object) hitInfo.transform == (UnityEngine.Object) this.target.transform)
        flag = true;
    }
    else
      flag = true;
    if (flag)
      Debug.DrawLine(origin, targetPos, Color.green, 1f);
    else
      Debug.DrawLine(origin, targetPos, Color.red, 1f);
    return flag;
  }
}
