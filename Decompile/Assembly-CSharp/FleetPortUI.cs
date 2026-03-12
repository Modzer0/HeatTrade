// Decompiled with JetBrains decompiler
// Type: FleetPortUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FleetPortUI : MonoBehaviour
{
  private StationHarborUI shui;
  private bool isSelected;
  [SerializeField]
  private ShipVicUI svuiPF;
  private List<ShipVicUI> svuis = new List<ShipVicUI>();
  private FleetManager fleet;
  [SerializeField]
  private TMP_Text nameText;

  private void Update()
  {
    if (!(bool) (Object) this.fleet || !((Object) this.fleet.transform.parent == (Object) null))
      return;
    if (this.isSelected)
      this.SelectThisFleet(false);
    Object.Destroy((Object) this.gameObject);
  }

  public void SetFleet(FleetManager newFleet)
  {
    this.shui = StationHarborUI.current;
    this.fleet = newFleet;
    this.nameText.text = $"{this.fleet.GetComponent<Track>().GetFullName()} ({this.fleet.ships.Count.ToString()}/10)";
    this.isSelected = false;
    this.transform.GetChild(0).GetComponent<Toggle>().group = this.transform.parent.GetComponent<ToggleGroup>();
  }

  public void SelectThisFleet(bool newIsSelected)
  {
    this.isSelected = newIsSelected;
    if (this.isSelected)
      this.shui.SelectFleet(this.fleet);
    else
      this.shui.SelectFleet((FleetManager) null);
  }
}
