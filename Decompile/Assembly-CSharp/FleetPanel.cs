// Decompiled with JetBrains decompiler
// Type: FleetPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class FleetPanel : MonoBehaviour
{
  private FleetManager fleet;
  [SerializeField]
  private GameObject fleetPanel;
  [SerializeField]
  private Transform targetShipList;
  [SerializeField]
  private S_ShipDataUI targetStructureUIPF;

  public void Setup(FleetManager newFleet)
  {
    if (!(bool) (UnityEngine.Object) newFleet || !(bool) (UnityEngine.Object) this || !(bool) (UnityEngine.Object) this.gameObject)
      return;
    this.fleet = newFleet;
    IEnumerator enumerator = (IEnumerator) this.targetShipList.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
        UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator.Current).gameObject);
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    foreach (S_Ship ship in this.fleet.GetComponent<FleetManager>().ships)
      UnityEngine.Object.Instantiate<S_ShipDataUI>(this.targetStructureUIPF, this.targetShipList).SetStructure(ship);
  }

  public void UpdateData() => this.Setup(this.fleet);
}
