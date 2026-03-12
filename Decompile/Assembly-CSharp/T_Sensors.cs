// Decompiled with JetBrains decompiler
// Type: T_Sensors
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class T_Sensors : T_Module, IInitable
{
  private TrackDisplayer td;
  [Header("SENSORS")]
  public Track parentTrack;
  public ShipController ship;
  private List<WeaponSystem> weapons = new List<WeaponSystem>();
  private List<Track> defenseTargets = new List<Track>();
  private List<Track> offenseTargets = new List<Track>();
  private List<(Track, float)> threatsUnsorted = new List<(Track, float)>();
  private List<Track> threatsSorted = new List<Track>();
  private List<Track> removeList = new List<Track>();
  [SerializeField]
  [Tooltip("Seconds between scans")]
  private float scanRate = 1f;
  private float nextSecUpdate;

  public override void Start()
  {
    base.Start();
    this.td = TrackDisplayer.current;
  }

  public void Init(Track newTrack, Rigidbody newRB)
  {
    this.parentTrack = newTrack;
    this.ship = this.parentTrack.GetComponent<ShipController>();
    this.weapons = this.ship.weapons;
  }

  private void FixedUpdate()
  {
    if ((double) Time.fixedTime < (double) this.nextSecUpdate)
      return;
    this.ThreatDetection();
    this.TargetAssignment();
    this.nextSecUpdate = (float) ((double) Time.fixedTime + (double) this.scanRate + 10.0 * (1.0 - (double) this.healthRatio));
  }

  private void ThreatDetection()
  {
    this.threatsSorted.Clear();
    this.threatsUnsorted.Clear();
    foreach (Track allTrack in this.td.allTracks)
    {
      if ((UnityEngine.Object) allTrack != (UnityEngine.Object) null && allTrack.factionID != this.parentTrack.factionID && allTrack.type != TrackType.SQUADRON && (!(bool) (UnityEngine.Object) allTrack.GetComponent<ShipController>() || !allTrack.GetComponent<ShipController>().isDead))
      {
        float num = Vector3.Distance(this.transform.position, allTrack.transform.position);
        this.threatsUnsorted.Add((allTrack, num));
      }
    }
    this.threatsUnsorted.Sort((Comparison<(Track, float)>) ((a, b) => a.Item2.CompareTo(b.Item2)));
    this.defenseTargets.Clear();
    this.offenseTargets.Clear();
    foreach ((Track, float) tuple in this.threatsUnsorted)
    {
      if (tuple.Item1.type == TrackType.MISSILE)
        this.defenseTargets.Add(tuple.Item1);
      else
        this.offenseTargets.Add(tuple.Item1);
      this.threatsSorted.Add(tuple.Item1);
    }
  }

  private void TargetAssignment()
  {
    this.removeList.Clear();
    foreach (WeaponSystem weapon in this.weapons)
    {
      if (weapon.currentMode == WeaponMode.DEFENSE)
      {
        foreach (Track defenseTarget in this.defenseTargets)
        {
          if (!this.removeList.Contains(defenseTarget) && weapon.TryTarget(defenseTarget))
            this.removeList.Add(defenseTarget);
        }
      }
      else if (weapon.currentMode == WeaponMode.OFFENSE)
      {
        foreach (Track offenseTarget in this.offenseTargets)
          weapon.TryTarget(offenseTarget);
      }
      else if (weapon.currentMode == WeaponMode.TARGET)
        weapon.AllTarget(this.ship.currentTarget);
      else if (weapon.currentMode == WeaponMode.NEAREST)
      {
        foreach (Track newTarget in this.threatsSorted)
        {
          weapon.TryTarget(newTarget);
          this.removeList.Add(newTarget);
        }
      }
    }
  }
}
