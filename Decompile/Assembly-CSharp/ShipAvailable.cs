// Decompiled with JetBrains decompiler
// Type: ShipAvailable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ShipAvailable : MonoBehaviour
{
  private FactionsManager fm;
  private Shipyard shipyard;
  private ShipBP ship;
  public bool isSkirmish;
  private SkirmishSetup skirm;
  [Header("UI")]
  [SerializeField]
  private Image iconBG;
  [SerializeField]
  private Image iconFG;
  [SerializeField]
  private TMP_Text nameText;
  [SerializeField]
  private TMP_Text classText;
  [SerializeField]
  private TMP_Text priceText;
  private bool isInit;

  private void Start() => this.Init();

  private void Init()
  {
    if (this.isInit)
      return;
    this.shipyard = Shipyard.current;
    this.fm = FactionsManager.current;
    this.skirm = SkirmishSetup.current;
    this.isInit = true;
  }

  public void Setup(ShipBP newShip)
  {
    this.Init();
    this.ship = newShip;
    this.nameText.text = this.ship.ClassName;
    this.classText.text = this.ship.TypeFull;
    this.priceText.text = this.ship.Value.ToString("#,0") + " cr";
    int factionId = this.ship.FactionID;
    if (factionId == 0)
      return;
    this.iconBG.color = this.fm.GetPrimaryColor(factionId);
    this.iconFG.color = this.fm.GetSecondaryColor(factionId);
  }

  public void OnClick()
  {
    if (this.isSkirmish)
    {
      this.skirm.SelectShip(this.ship);
    }
    else
    {
      if (!(bool) (Object) this.shipyard || !(bool) (Object) this.ship)
        return;
      this.shipyard.SelectShip(this.ship);
    }
  }
}
