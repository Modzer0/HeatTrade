// Decompiled with JetBrains decompiler
// Type: Attachments
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Attachments : MonoBehaviour
{
  public Target target;
  private Orbiter orbiter;
  public bool isStation;
  public StationManager station;
  public List<FleetManager> attachedFleets = new List<FleetManager>();
  private bool isInit;

  private void Awake() => this.Init();

  private void Init()
  {
    if (this.isInit)
      return;
    this.target = this.GetComponent<Target>();
    this.orbiter = this.GetComponent<Orbiter>();
    if ((bool) (UnityEngine.Object) this.GetComponent<StationManager>())
    {
      this.isStation = true;
      this.station = this.GetComponent<StationManager>();
    }
    IEnumerator enumerator = (IEnumerator) this.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if ((bool) (UnityEngine.Object) current.GetComponent<StationManager>())
          this.station = current.GetComponent<StationManager>();
        else if ((bool) (UnityEngine.Object) current.GetComponent<FleetManager>())
          this.attachedFleets.Add(current.GetComponent<FleetManager>());
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    this.isInit = true;
  }

  private void Update()
  {
    if ((bool) (UnityEngine.Object) this.target || !(bool) (UnityEngine.Object) this.GetComponent<Target>())
      return;
    this.target = this.GetComponent<Target>();
    this.CheckEnable();
  }

  public void RefreshList()
  {
    this.Init();
    this.attachedFleets.RemoveAll((Predicate<FleetManager>) (item => (UnityEngine.Object) item == (UnityEngine.Object) null));
    if (this.transform.childCount <= 0)
      return;
    IEnumerator enumerator = (IEnumerator) this.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if ((bool) (UnityEngine.Object) current.GetComponent<StationManager>())
          this.station = current.GetComponent<StationManager>();
        if ((bool) (UnityEngine.Object) current.GetComponent<FleetManager>() && !this.attachedFleets.Contains(current.GetComponent<FleetManager>()))
          this.attachedFleets.Add(current.GetComponent<FleetManager>());
        if ((bool) (UnityEngine.Object) current.GetComponent<Attachments>())
          current.GetComponent<Attachments>().CheckEnable();
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
  }

  public void CheckEnable()
  {
    if (!(bool) (UnityEngine.Object) this.target)
      return;
    if ((UnityEngine.Object) this.transform.parent != (UnityEngine.Object) null && (bool) (UnityEngine.Object) this.transform.parent.GetComponent<Attachments>())
    {
      if (this.target.enabled)
        this.target.enabled = false;
      if (this.orbiter.isOrbiting)
        this.orbiter.isOrbiting = false;
      this.transform.localPosition = Vector3.zero;
    }
    else
    {
      if (!this.target.enabled)
        this.target.enabled = true;
      if (this.orbiter.isOrbiting)
        return;
      this.orbiter.isOrbiting = true;
    }
  }

  public void Remove(FleetManager fleet) => this.attachedFleets.Remove(fleet);
}
