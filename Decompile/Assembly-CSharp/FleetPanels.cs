// Decompiled with JetBrains decompiler
// Type: FleetPanels
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FleetPanels : MonoBehaviour
{
  public static FleetPanels current;
  private List<FleetPanel> panels = new List<FleetPanel>();

  private void Awake() => FleetPanels.current = this;

  private void Start()
  {
    MonoBehaviour.print((object) "FLEET PANEL START");
    this.GetNewPanels();
  }

  private void GetNewPanels()
  {
    for (int index = this.panels.Count - 1; index >= 0; --index)
    {
      if ((Object) this.panels[index] == (Object) null)
        this.panels.RemoveAt(index);
    }
    foreach (FleetPanel fleetPanel in Object.FindObjectsOfType<FleetPanel>())
      this.panels.Add(fleetPanel);
  }

  private IEnumerator LateGetNewPanels()
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.GetNewPanels();
  }

  public void UpdateData()
  {
    bool flag = false;
    foreach (FleetPanel panel in this.panels)
    {
      if ((Object) panel == (Object) null || !panel.gameObject.activeSelf || (Object) panel.gameObject == (Object) null)
      {
        flag = true;
        break;
      }
      panel.UpdateData();
    }
    if (!flag)
      return;
    this.StartCoroutine((IEnumerator) this.LateGetNewPanels());
  }
}
