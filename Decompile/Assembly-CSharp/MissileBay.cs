// Decompiled with JetBrains decompiler
// Type: MissileBay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MissileBay : MissileSystem
{
  private List<Pathfinder> mbRemoveList = new List<Pathfinder>();
  public List<MissileLauncher> missileLaunchers = new List<MissileLauncher>();
  private List<Pathfinder> missilesToLaunch = new List<Pathfinder>();
  [SerializeField]
  private float missileLoadTime;
  private PathfinderState state;
  private bool launchersHaveGuidance;

  public override void Start()
  {
    base.Start();
    if ((double) this.resource > (double) this.resourceMax)
      this.resource = this.resourceMax;
    for (int index = 0; index < Mathf.CeilToInt(this.resource); ++index)
    {
      Pathfinder pathfinder = UnityEngine.Object.Instantiate<Pathfinder>(this.missilePrefab, (Transform) null);
      this.missiles.Add(pathfinder);
      pathfinder.GetComponent<Collider>().enabled = false;
      pathfinder.gameObject.SetActive(false);
    }
  }

  public override void Init(Track newTrack, Rigidbody newRB)
  {
    base.Init(newTrack, newRB);
    this.AssignGuidance();
  }

  private void AssignGuidance()
  {
    foreach (MissileLauncher missileLauncher in this.missileLaunchers)
    {
      if ((UnityEngine.Object) missileLauncher == (UnityEngine.Object) null)
        break;
      missileLauncher.parentRB = this.parentRB;
      missileLauncher.missileLoadTime = this.missileLoadTime;
      missileLauncher.mg = this.mg;
      missileLauncher.launchVel = this.launchVel;
      if ((UnityEngine.Object) this.mg != (UnityEngine.Object) null)
        this.launchersHaveGuidance = true;
    }
  }

  private void Update()
  {
    if ((UnityEngine.Object) this.parentRB == (UnityEngine.Object) null)
      return;
    if (this.mbRemoveList.Count > 0)
      this.missiles.RemoveAll((Predicate<Pathfinder>) (missile => this.mbRemoveList.Contains(missile)));
    if (this.missilesToLaunch.Count > 0)
    {
      foreach (Pathfinder missile in this.missilesToLaunch)
      {
        foreach (MissileLauncher missileLauncher in this.missileLaunchers)
        {
          if (!(bool) (UnityEngine.Object) missileLauncher.parentRB)
            missileLauncher.parentRB = this.parentRB;
          if (!missileLauncher.isLoadingMissile)
          {
            missileLauncher.LoadMissile(missile, this.state, this.target.transform);
            this.mbRemoveList.Add(missile);
            break;
          }
        }
      }
    }
    if (this.mbRemoveList.Count <= 0)
      return;
    this.missilesToLaunch.RemoveAll((Predicate<Pathfinder>) (missile => this.mbRemoveList.Contains(missile)));
  }

  public override void LaunchMissile(Pathfinder missile, PathfinderState newState)
  {
    this.missilesToLaunch.Add(missile);
    missile.GetComponent<Missile2>().Activate(this.parentTrack.factionID);
    this.state = newState;
  }
}
