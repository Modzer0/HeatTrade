// Decompiled with JetBrains decompiler
// Type: S_ShipDataUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class S_ShipDataUI : 
  MonoBehaviour,
  ITooltipable,
  IPointerEnterHandler,
  IEventSystemHandler,
  IPointerExitHandler
{
  private MapInputs mi;
  private FactionsManager fm;
  private TrackDisplayer td;
  private TooltipSystem ts;
  private ShipInfoManager sim;
  private bool isSetupDone;
  private ShipController sc;
  private S_Ship ship;
  private bool isPlayerShip;
  [SerializeField]
  private uiBar healthBar;
  [SerializeField]
  private Image iconBG;
  [SerializeField]
  private Image iconFG;
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
  private static LTDescr delay;

  private void Start()
  {
    if (this.isSetupDone)
      return;
    this.Setup();
  }

  private void Setup()
  {
    this.mi = MapInputs.current;
    this.fm = FactionsManager.current;
    this.td = TrackDisplayer.current;
    this.ts = TooltipSystem.current;
    this.sim = ShipInfoManager.current;
    this.isSetupDone = true;
  }

  public void SetStructure(S_Ship s)
  {
    MonoBehaviour.print((object) ("setting structure for shipdataUI. ship: " + s.name));
    if (!this.isSetupDone)
      this.Setup();
    if ((UnityEngine.Object) s == (UnityEngine.Object) null)
    {
      MonoBehaviour.print((object) "no structure for shipdataUI");
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      MonoBehaviour.print((object) ("2 setting structure for shipdataUI. ship: " + s.name));
      this.ship = s;
      this.iconBG.color = this.fm.GetPrimaryColor(this.ship.factionID);
      this.iconFG.color = this.fm.GetSecondaryColor(this.ship.factionID);
      this.nameText.text = $"{this.ship.trackID} - {this.ship.publicName}";
      this.classText.text = this.ship.bp.GetShortClassName();
      this.healthBar.SetBarSize(this.ship.GetHealthRatio());
    }
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    S_ShipDataUI.delay = LeanTween.delayedCall(0.5f, (Action) (() => { }));
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (!(bool) (UnityEngine.Object) this.ts)
      return;
    this.ts.Hide();
  }

  public void GetInfo()
  {
    if (!(bool) (UnityEngine.Object) this.sim)
      return;
    this.sim.NewShipInfo(this.ship);
  }
}
