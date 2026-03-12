// Decompiled with JetBrains decompiler
// Type: ShipVicUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ShipVicUI : MonoBehaviour
{
  private StationHarborUI shui;
  [SerializeField]
  private S_ShipDataUI shipUI;
  private S_Ship ship;

  private void Start() => this.shui = StationHarborUI.current;

  public void SetShip(S_Ship newShip)
  {
    this.ship = newShip;
    this.shipUI.SetStructure(this.ship);
  }

  public void TransferThis() => this.shui.OnClickTransferShipButton(this.ship.transform);
}
