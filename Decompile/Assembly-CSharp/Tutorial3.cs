// Decompiled with JetBrains decompiler
// Type: Tutorial3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class Tutorial3 : MonoBehaviour
{
  private TacticalCamera tc;
  private TacticalInputs ti;
  private AudioManager am;
  private SquadronAssignment sa;
  private PlayerFleetUI pfui;
  private FactionsManager fm;
  [SerializeField]
  private int step;
  [Header("TIPS")]
  [SerializeField]
  private uiAnimator tips1;
  [SerializeField]
  private uiAnimator tips2;
  [SerializeField]
  private uiAnimator tips3;
  [SerializeField]
  private uiAnimator tips4;
  [SerializeField]
  private uiAnimator tips4p1;
  [SerializeField]
  private uiAnimator tips5;
  [SerializeField]
  private uiAnimator tips6;
  [SerializeField]
  private uiAnimator tips7;
  [SerializeField]
  private uiAnimator tips8;
  [Header("SARA")]
  [SerializeField]
  private uiAnimator startBlack;
  [SerializeField]
  private TerminalTyper ttSara;
  private uiMover saraMover;
  [SerializeField]
  private GameObject replyButton;
  [SerializeField]
  private TMP_Text replyText;
  [Header("LINE")]
  [SerializeField]
  private LinePointer lp;
  [SerializeField]
  private Transform lineStart;
  [SerializeField]
  private Transform targetHeatToggle;
  [Header("TARGETS")]
  [SerializeField]
  private Transform heatTab;
  [Header("HEAT MANAGEMENT and ADVANCED WEAPONS")]
  [SerializeField]
  private GameObject heatPanel;
  [SerializeField]
  private ShipController fitz;
  [SerializeField]
  private Captain fitzCaptain;
  [SerializeField]
  private Transform casket;
  [SerializeField]
  private WeaponSystem laserBay;
  [SerializeField]
  private ShipController funeralSinger;
  [SerializeField]
  private Captain funeralSingerCaptain;
  [SerializeField]
  private ShipController fiend;
  [SerializeField]
  private MissileSystem funeralSingerMissiles;
  [Header("FLEET COMMAND")]
  [SerializeField]
  private GameObject fleetPanel;
  [SerializeField]
  private List<ShipController> playerShips = new List<ShipController>();
  [SerializeField]
  private List<ShipController> playerSquadron1 = new List<ShipController>();
  [SerializeField]
  private List<ShipController> playerSquadron2 = new List<ShipController>();
  [SerializeField]
  private List<ShipController> playerSquadron3 = new List<ShipController>();
  [SerializeField]
  private List<Squadron> playerSquadrons = new List<Squadron>();
  [SerializeField]
  private List<ShipController> hostileShips = new List<ShipController>();
  [Header("TEXTS")]
  [SerializeField]
  private List<SaraStep> saraSteps;
  [SerializeField]
  private uiAnimator endScreen;

  private void Awake()
  {
    this.fm = FactionsManager.current;
    this.fm.SetupNewGame(true, (Faction) null);
  }

  private void Start()
  {
    this.tc = TacticalCamera.current;
    this.ti = TacticalInputs.current;
    this.am = AudioManager.current;
    this.pfui = PlayerFleetUI.current;
    this.fitzCaptain = this.fitz.GetComponent<Captain>();
    this.funeralSingerCaptain = this.funeralSinger.GetComponent<Captain>();
    this.StartCoroutine((IEnumerator) this.DelaySara());
    this.startBlack.Hide();
    foreach (ShipController playerShip in this.playerShips)
      this.ti.AddPlayerShip(playerShip);
  }

  private void Update()
  {
    if (this.step == 7)
    {
      this.lp.NewTargetUI(this.targetHeatToggle.position);
      if (!this.heatPanel.activeSelf || !((Object) this.ti.selectedShip == (Object) this.fitz))
        return;
      this.lp.Off();
      this.NextStep();
    }
    else if (this.step == 9)
    {
      if (this.fitz.GetComponent<HeatManager>().isRadiatorsExtended)
        return;
      this.NextStep();
    }
    else if (this.step == 11)
    {
      if (!this.fitz.GetComponent<HeatManager>().isRadiatorsExtended)
        return;
      this.tips1.Hide();
      this.NextStep();
    }
    else if (this.step == 16 /*0x10*/)
    {
      if (!((Object) this.fitz.currentTarget == (Object) this.casket.GetComponent<Track>()))
        return;
      this.NextStep();
    }
    else if (this.step == 17)
    {
      if (!this.tips2.gameObject.activeSelf && this.laserBay.currentMode == WeaponMode.TARGET)
      {
        this.tips2.Show();
      }
      else
      {
        if ((double) this.casket.GetComponent<ShipController>().totalHealthRatio > 0.30000001192092896 && !((Object) this.casket.gameObject == (Object) null))
          return;
        this.NextStep();
      }
    }
    else if (this.step == 18)
    {
      if (!((Object) this.ti.selectedShip == (Object) this.funeralSinger))
        return;
      this.NextStep();
    }
    else if (this.step == 19)
    {
      if (!((Object) this.funeralSinger.currentTarget == (Object) this.fiend.GetComponent<Track>()))
        return;
      this.NextStep();
    }
    else if (this.step == 20)
    {
      if ((double) this.funeralSingerMissiles.resource > 20.0)
        return;
      this.NextStep();
    }
    else if (this.step == 21)
    {
      if ((double) this.fiend.totalHealthRatio > 0.5)
        return;
      this.NextStep();
    }
    else if (this.step == 24)
    {
      if (!this.playerSquadrons[0].GetShips().Contains(this.funeralSingerCaptain))
        return;
      this.NextStep();
    }
    else if (this.step == 25)
    {
      if (this.playerSquadrons[0].command != SquadronCommand.MOVE)
        return;
      this.NextStep();
    }
    else if (this.step == 28)
    {
      if (this.playerSquadrons[0].command != SquadronCommand.ESCORT)
        return;
      this.NextStep();
    }
    else if (this.step == 29)
    {
      if (!this.playerSquadrons[1].GetShips().Contains(this.fitzCaptain))
        return;
      this.NextStep();
    }
    else
    {
      if (this.step != 35 || !this.AreHostilesDead())
        return;
      this.NextStep();
    }
  }

  private bool AreHostilesDead()
  {
    foreach (ShipController hostileShip in this.hostileShips)
    {
      if (!hostileShip.isDead)
        return false;
    }
    return true;
  }

  private IEnumerator DelaySara()
  {
    yield return (object) new WaitForSeconds(3f);
    this.ttSara.GetComponent<uiAnimator>().Show();
    this.SetSaraStep();
  }

  public void NextStep()
  {
    ++this.step;
    if (this.step == 37)
    {
      this.ttSara.GetComponent<uiAnimator>().Hide();
      this.endScreen.Show();
    }
    else if (this.step > 0)
    {
      this.SetSaraStep();
      if (this.step != 7)
      {
        if (this.step == 8)
        {
          this.tc.SetFocus(this.fitz.transform, 2);
          this.StartCoroutine((IEnumerator) this.DelayFitzEvade());
          this.tips1.Show();
        }
        else if (this.step == 16 /*0x10*/)
          this.tc.SetFocus(this.casket.transform, 1);
        else if (this.step == 18)
        {
          this.ti.SelectNew(this.fitz);
          this.fitz.isSelectable = false;
          this.laserBay.SetMode(WeaponMode.DEFENSE);
          this.casket.gameObject.SetActive(false);
          this.fiend.gameObject.SetActive(true);
          this.funeralSinger.gameObject.SetActive(true);
          this.tc.SetFocus(this.funeralSinger.transform, 1);
          this.funeralSingerCaptain.command = CaptainCommand.EVASIVE_HARD;
          this.funeralSingerCaptain.UpdateCommand();
        }
        else if (this.step == 19)
          this.tc.SetFocus(this.fiend.transform, 1);
        else if (this.step == 21)
        {
          this.tips2.Hide();
          this.tips3.Show();
        }
        else if (this.step == 23)
        {
          this.fiend.gameObject.SetActive(false);
          foreach (Component component in this.playerSquadron1)
            component.gameObject.SetActive(true);
          this.playerSquadrons[0].gameObject.SetActive(true);
          this.tc.SetFocus(this.playerSquadrons[0].GetShips()[0].transform, 2);
          this.tips3.Hide();
          this.tips4.Show();
          this.fleetPanel.SetActive(true);
          this.StartCoroutine((IEnumerator) this.DelaySquadronSelect(0));
        }
        else if (this.step == 25)
        {
          this.tips4.Hide();
          this.tips4p1.Show();
        }
        else if (this.step == 29)
        {
          this.fitz.isSelectable = true;
          foreach (Component component in this.playerSquadron2)
            component.gameObject.SetActive(true);
          this.StartCoroutine((IEnumerator) this.DelaySquadronSelect(1));
          this.StartCoroutine((IEnumerator) this.DelayFocusOn(this.playerSquadrons[1].transform));
        }
        else if (this.step == 30)
        {
          this.tips4p1.Hide();
          this.tips5.Show();
        }
        else if (this.step == 31 /*0x1F*/)
        {
          this.tips5.Hide();
          this.tips6.Show();
          foreach (Component component in this.playerSquadron3)
            component.gameObject.SetActive(true);
          this.StartCoroutine((IEnumerator) this.DelaySquadronSelect(2));
          this.StartCoroutine((IEnumerator) this.DelayFocusOn(this.playerSquadrons[2].transform));
        }
        else if (this.step == 34)
        {
          this.tips6.Hide();
          this.tips7.Show();
        }
        else if (this.step == 35)
        {
          foreach (Component hostileShip in this.hostileShips)
            hostileShip.gameObject.SetActive(true);
          this.tc.SetFocus(this.hostileShips[0].transform, 1);
        }
      }
    }
    this.am.PlaySFX(2);
  }

  private void SetSaraStep()
  {
    SaraStep saraStep = this.saraSteps[this.step];
    this.ttSara.TypeThis(saraStep.saraText);
    this.replyText.text = saraStep.replyText;
    this.replyButton.SetActive(saraStep.isShowReplyButton);
  }

  private IEnumerator DelayFitzEvade()
  {
    yield return (object) new WaitForSeconds(2f);
    this.fitzCaptain.command = CaptainCommand.EVASIVE_HARD;
    this.fitzCaptain.UpdateCommand();
    this.casket.GetComponent<Captain>().command = CaptainCommand.EVASIVE_HARD;
    this.casket.GetComponent<Captain>().UpdateCommand();
  }

  private IEnumerator DelayStartTip(uiAnimator tip)
  {
    yield return (object) new WaitForSeconds(0.5f);
    this.StartTip(tip);
  }

  private void StartTip(uiAnimator tip)
  {
    tip.Show();
    this.StartCoroutine((IEnumerator) this.DelayMove(tip.GetComponent<uiMover>()));
  }

  private IEnumerator DelayMove(uiMover tip)
  {
    yield return (object) new WaitForSeconds(1f);
    tip.MoveTo(new Vector2(-793f, 524f));
  }

  private IEnumerator DelaySquadronSelect(int i)
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.pfui.SetToggle(i);
  }

  private IEnumerator DelayFocusOn(Transform target)
  {
    yield return (object) new WaitForSeconds(2f);
    this.tc.SetFocus(target, 1);
  }
}
