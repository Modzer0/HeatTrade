// Decompiled with JetBrains decompiler
// Type: BountyButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class BountyButton : MonoBehaviour
{
  private MapInputs mi;
  [SerializeField]
  private TMP_Text nameText;
  [SerializeField]
  private TMP_Text rewardText;
  [SerializeField]
  private CamInfo fleetCI;

  public void Setup(FleetManager fleet)
  {
    this.mi = MapInputs.current;
    this.fleetCI = fleet.GetComponent<CamInfo>();
    this.nameText.text = fleet.GetComponent<Track>().GetFullName();
    int num = 0;
    foreach (S_Ship ship in fleet.ships)
      num += ship.bp.Value;
    this.rewardText.text = num.ToString("#,0") + " cr";
  }

  public void OnClick()
  {
    this.mi.EnterTarget(this.fleetCI);
    this.mi.SetTarget(this.fleetCI);
    this.mi.CenterCam();
  }
}
