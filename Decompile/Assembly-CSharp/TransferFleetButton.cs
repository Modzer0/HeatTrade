// Decompiled with JetBrains decompiler
// Type: TransferFleetButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class TransferFleetButton : MonoBehaviour
{
  private StationHarborUI shui;
  public FleetManager fleet;
  [SerializeField]
  private TMP_Text fleetName;

  private void Start() => this.shui = StationHarborUI.current;

  public void SetFleet(FleetManager newFleet)
  {
    this.fleet = newFleet;
    this.fleetName.text = newFleet.GetComponent<Track>().trackName;
  }

  public void OnClick() => this.shui.TransferToThisFleet(this.fleet);
}
