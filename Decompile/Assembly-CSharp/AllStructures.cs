// Decompiled with JetBrains decompiler
// Type: AllStructures
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AllStructures : MonoBehaviour
{
  public static AllStructures current;
  private SaveLoadSystem sls;
  public List<S_Ship> ships = new List<S_Ship>();

  private void Awake()
  {
    if ((UnityEngine.Object) AllStructures.current != (UnityEngine.Object) null && (UnityEngine.Object) AllStructures.current != (UnityEngine.Object) this)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    else
      AllStructures.current = this;
  }

  private void Start()
  {
    this.sls = SaveLoadSystem.current;
    this.sls.OnLoadGame += new Action(this.CallDelayedRefresh);
    this.CallDelayedRefresh();
  }

  private void CallDelayedRefresh() => this.StartCoroutine((IEnumerator) this.DelayedRefresh());

  private IEnumerator DelayedRefresh()
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.Refresh();
  }

  public void Refresh()
  {
    this.ships = new List<S_Ship>();
    foreach (S_Ship sShip in UnityEngine.Object.FindObjectsOfType<S_Ship>())
      this.ships.Add(sShip);
  }

  public void AddShip(S_Ship newShip) => this.ships.Add(newShip);

  public S_Ship GetShipFromID(string trackID)
  {
    MonoBehaviour.print((object) ("getting ship from id: " + trackID));
    foreach (S_Ship ship in this.ships)
    {
      if (ship.trackID == trackID)
      {
        MonoBehaviour.print((object) ("ship found!: " + ship.name));
        return ship;
      }
    }
    MonoBehaviour.print((object) "ship not found");
    return (S_Ship) null;
  }
}
