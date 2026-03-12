// Decompiled with JetBrains decompiler
// Type: ShipDataUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ShipDataUI : MonoBehaviour
{
  private TacticalInputs ti;
  private FactionsManager fm;
  private TrackDisplayer td;
  private bool isSetupDone;
  private ShipController ship;
  private Structure structure;
  private bool isPlayerShip;
  [SerializeField]
  private TMP_Text indexText;
  [SerializeField]
  private TMP_Text nameText;
  [SerializeField]
  private TMP_Text statusText;
  [SerializeField]
  private TMP_Text factionText;
  [SerializeField]
  private TMP_Text classText;
  [SerializeField]
  private TMP_Text typeText;
  [SerializeField]
  private TMP_Text dvText;
  [SerializeField]
  private Transform dvBar;
  [SerializeField]
  private TMP_Text power;
  [SerializeField]
  private TMP_Text heat;
  [SerializeField]
  private TMP_Text ammo;
  [SerializeField]
  private TMP_Text missile;
  [SerializeField]
  private TMP_Text crew;
  [SerializeField]
  private Image primaryImg;
  [SerializeField]
  private Image secondaryImg;

  private void Setup()
  {
    this.ti = TacticalInputs.current;
    this.fm = FactionsManager.current;
    this.td = TrackDisplayer.current;
    this.isSetupDone = true;
  }

  private void Start()
  {
    if (this.isSetupDone)
      return;
    this.Setup();
  }

  public void SetShip(ShipController sc)
  {
    if (!this.isSetupDone)
      this.Setup();
    this.ship = sc;
    if (this.ship.GetComponent<Track>().factionID == 1)
      this.isPlayerShip = true;
    if ((bool) (Object) this.indexText)
    {
      if (this.isPlayerShip)
        this.indexText.text = (this.ti.shipsOwned.IndexOf(this.ship) + 1).ToString();
      else
        this.indexText.gameObject.SetActive(false);
    }
    Faction factionFromId = this.fm.GetFactionFromID(this.ship.GetComponent<Track>().factionID);
    string factionCode = factionFromId.factionCode;
    string trackName = this.ship.GetComponent<Track>().trackName;
    if ((bool) (Object) this.nameText)
      this.nameText.text = factionCode + trackName;
    if ((bool) (Object) this.statusText)
      this.statusText.text = this.ship.GetComponent<ShipController>().status;
    if ((bool) (Object) this.factionText)
      this.factionText.text = factionCode;
    if ((bool) (Object) this.classText)
      this.classText.text = this.ship.GetComponent<ShipController>().className;
    if ((bool) (Object) this.typeText)
      this.typeText.text = this.ship.GetComponent<ShipController>().classType;
    if ((bool) (Object) this.dvBar && (bool) (Object) this.dvBar.GetComponent<uiBar>() && (bool) (Object) this.ship)
      this.dvBar.GetComponent<uiBar>().SetBarSize(this.ship.totalHealthRatio);
    else
      this.dvBar.GetComponent<uiBar>().SetBarSize(0.0f);
    if ((bool) (Object) this.primaryImg)
      this.primaryImg.color = factionFromId.colorPrimary;
    if (!(bool) (Object) this.secondaryImg)
      return;
    this.secondaryImg.color = factionFromId.colorSecondary;
  }

  public void OnClick() => this.ti.SelectNew(this.ship);
}
