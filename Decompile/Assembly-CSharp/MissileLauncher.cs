// Decompiled with JetBrains decompiler
// Type: MissileLauncher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class MissileLauncher : MonoBehaviour
{
  public int factionID;
  public Rigidbody parentRB;
  public bool isLoadingMissile;
  private float launchTime;
  private Pathfinder currentMissile;
  private PathfinderState state;
  public Transform target;
  public MissileGuidance mg;
  public int size;
  public float missileLoadTime;
  public MissileLauncher.LaunchFace launchFace;
  public MissileLauncher.LaunchThrust launchThrust;
  public float launchVel;

  private void Start()
  {
  }

  private void Update()
  {
    if (!this.isLoadingMissile || (double) Time.time < (double) this.launchTime || !((Object) this.currentMissile != (Object) null))
      return;
    if (this.factionID == 0)
      this.factionID = this.transform.parent.GetComponent<WeaponSystem>().parentTrack.factionID;
    this.isLoadingMissile = false;
    this.currentMissile.GetComponent<Track>().enabled = true;
    this.currentMissile.GetComponent<Track>().factionID = this.factionID;
    this.currentMissile.gameObject.SetActive(true);
    this.currentMissile.transform.position = this.transform.position;
    this.currentMissile.transform.rotation = this.transform.rotation;
    Rigidbody component = this.currentMissile.GetComponent<Rigidbody>();
    component.isKinematic = false;
    if (this.launchFace == MissileLauncher.LaunchFace.FRONT)
      component.AddForce(this.transform.forward * this.launchVel, ForceMode.Impulse);
    else if (this.launchFace == MissileLauncher.LaunchFace.SIDE)
      component.AddForce(this.transform.up * this.launchVel, ForceMode.Impulse);
    component.AddForce(this.parentRB.velocity, ForceMode.Impulse);
    this.currentMissile.transform.parent = (Transform) null;
    this.StartCoroutine((IEnumerator) this.LaunchDelay(this.currentMissile));
    this.currentMissile = (Pathfinder) null;
  }

  public void LoadMissile(Pathfinder missile, PathfinderState newState, Transform newTarget)
  {
    this.target = newTarget;
    this.isLoadingMissile = true;
    this.launchTime = Time.time + this.missileLoadTime;
    this.currentMissile = missile;
    this.state = newState;
    if (!(bool) (Object) missile.GetComponent<Missile2>())
      return;
    missile.GetComponent<Missile2>().target = this.target.transform;
  }

  private IEnumerator LaunchDelay(Pathfinder missile)
  {
    yield return (object) new WaitForSeconds(0.5f);
    missile.isRCSOnly = false;
    if (this.launchThrust == MissileLauncher.LaunchThrust.COLD)
    {
      missile.TrySetStateBrake();
      yield return (object) new WaitForSeconds(0.5f);
    }
    if (!((Object) missile == (Object) null))
    {
      while ((bool) (Object) missile.gameObject && !this.CheckLOS(missile.transform.position, this.mg.target.position))
        yield return (object) null;
      if (this.state == PathfinderState.COLLIDE && (bool) (Object) missile.GetComponent<Missile2>())
        missile.GetComponent<Missile2>().isArmed = true;
      missile.targetPos = this.mg.GetTargetPos(missile);
      missile.TrySetState(this.state);
    }
  }

  private bool CheckLOS(Vector3 origin, Vector3 targetPos)
  {
    if ((Object) this.target == (Object) null)
      return false;
    bool flag = false;
    RaycastHit hitInfo;
    if (Physics.Linecast(origin, targetPos, out hitInfo))
    {
      if ((Object) hitInfo.transform == (Object) this.target.transform)
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

  public enum LaunchFace
  {
    FRONT,
    SIDE,
  }

  public enum LaunchThrust
  {
    HOT,
    COLD,
  }
}
