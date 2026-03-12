// Decompiled with JetBrains decompiler
// Type: CaptainDataUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CaptainDataUI : MonoBehaviour
{
  private ColorManager cm;
  private TacticalInputs ti;
  private Captain ship;
  private Track targetTrack;
  private HeatManager hm;
  private bool isDead;
  [Header("HEADER")]
  [SerializeField]
  private Gradient colorGradient;
  [SerializeField]
  private TMP_Text nameText;
  [SerializeField]
  private TMP_Text classText;
  [SerializeField]
  private TMP_Text indexText;
  [SerializeField]
  private ShipButton indexButton;
  [SerializeField]
  private Transform orderDelayImg;
  [Header("MAIN - Info")]
  [SerializeField]
  private uiBar healthBar;
  [SerializeField]
  private TMP_Text powerText;
  [SerializeField]
  private TMP_Text heatText;
  [SerializeField]
  private TMP_Text ammoText;
  [SerializeField]
  private TMP_Text missileText;
  [SerializeField]
  private TMP_Text crewText;
  [SerializeField]
  private TMP_Text targetText;
  [SerializeField]
  private Slider rangeSlider;
  [SerializeField]
  private TMP_Text rangeText;
  [Header("MAIN - Control")]
  [SerializeField]
  private ButtonGroup taskButtons;
  [SerializeField]
  private ButtonGroup weaponsButtons;
  [SerializeField]
  private ButtonGroup missileRoleButtons;
  [SerializeField]
  private ButtonGroup directionButtons;
  [SerializeField]
  private ButtonGroup distanceButtons;
  [SerializeField]
  private ButtonGroup missileSpendButtons;

  private void Start() => this.Init();

  private void Init()
  {
    this.cm = ColorManager.current;
    this.ti = TacticalInputs.current;
  }

  private void Update()
  {
    if (!(bool) (Object) this.ship)
      return;
    this.UpdateData();
  }

  private void UpdateData()
  {
    if ((bool) (Object) this.targetTrack)
      this.targetText.text = this.targetTrack.trackName;
    if (this.ship.isTakingNewOrders)
    {
      this.orderDelayImg.gameObject.SetActive(true);
      this.orderDelayImg.Rotate(-Vector3.forward, Time.deltaTime * 360f);
    }
    else
    {
      this.orderDelayImg.gameObject.SetActive(false);
      this.orderDelayImg.rotation = Quaternion.identity;
    }
    if ((Object) this.ship.ship != (Object) null)
      this.healthBar.SetBarSize(this.ship.ship.totalHealthRatio);
    this.taskButtons.SetIndex((int) this.ship.command);
    this.weaponsButtons.SetIndex((int) this.ship.weapons);
    this.missileRoleButtons.SetIndex((int) this.ship.missileRole);
    this.directionButtons.SetIndex((int) this.ship.direction);
    this.distanceButtons.SetIndex((int) this.ship.range);
    this.missileSpendButtons.SetIndex((int) this.ship.missileSpend);
    float num = 0.0f;
    if ((double) this.hm.heatsinkMax > 0.0)
      num = Mathf.Clamp01(this.hm.heatsinkCurrent / this.hm.heatsinkMax);
    this.heatText.color = this.colorGradient.Evaluate(1f - num);
  }

  public void SetShip(Captain newShip)
  {
    this.Init();
    if ((Object) newShip == (Object) null || (Object) newShip.gameObject == (Object) null)
    {
      Object.Destroy((Object) this.gameObject);
    }
    else
    {
      this.ship = newShip;
      this.hm = this.ship.GetComponent<HeatManager>();
      if ((bool) (Object) this.ship.target)
        this.targetTrack = this.ship.target.GetComponent<Track>();
      if ((bool) (Object) this.nameText)
        this.nameText.text = this.ship.track.trackName;
      if ((bool) (Object) this.classText)
        this.classText.text = $"{this.ship.ship.className} {this.ship.ship.classType}";
      if ((bool) (Object) this.indexText)
      {
        int num = -1;
        string str = "";
        if (this.ti.shipsOwned != null && this.ti.shipsOwned.Contains(this.ship.ship))
        {
          num = this.ti.shipsOwned.IndexOf(this.ship.ship) + 1;
          str = num != 10 ? num.ToString() : "0";
        }
        this.indexText.text = str;
        this.indexButton.index = num;
      }
      if ((bool) (Object) this.rangeSlider)
        this.rangeSlider.value = this.ship.customRange;
      this.rangeText.text = this.ship.customRange.ToString("F0") + "km";
      this.UpdateData();
    }
  }

  public void SetTask(int i) => this.ship.command = (CaptainCommand) i;

  public void SetWeapons(int i)
  {
    this.ship.weapons = (CaptainWeapons) i;
    this.ship.SetWeapons();
  }

  public void SetMissileRole(int i) => this.ship.missileRole = (CaptainMissileRole) i;

  public void SetDirection(int i) => this.ship.direction = (CaptainDirection) i;

  public void SetDistance(int i) => this.ship.range = (CaptainRange) i;

  public void SetMissileSpend(int i) => this.ship.missileSpend = (CaptainMissileSpend) i;

  public void SetCustomRange(float newRange)
  {
    this.ship.customRange = newRange * 100f;
    this.rangeText.text = newRange.ToString("F0") + "km";
  }

  private void OnShipDeath(int i, ShipController ship, bool isHardDeath)
  {
  }
}
