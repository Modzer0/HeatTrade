// Decompiled with JetBrains decompiler
// Type: ShipTransferUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class ShipTransferUI : MonoBehaviour
{
  private FleetManagerUI fmui;
  private ShipInfoManager sim;
  private S_Ship ship;
  [SerializeField]
  private int listIndex;
  [Header("UI")]
  [SerializeField]
  private TMP_Text nameText;

  private void Start()
  {
    this.fmui = FleetManagerUI.current;
    this.sim = ShipInfoManager.current;
  }

  public void Setup(S_Ship newShip)
  {
    this.ship = newShip;
    this.nameText.text = $"{this.ship.trackID} - {this.ship.publicName}";
  }

  public void ShowInfo() => this.sim.NewShipInfo(this.ship);

  public void Transfer() => this.fmui.TransferShip(this.ship, this.listIndex);
}
