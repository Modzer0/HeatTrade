// Decompiled with JetBrains decompiler
// Type: Tutorial2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class Tutorial2 : MonoBehaviour
{
  private TacticalCamera tc;
  private MapInputs mi;
  private AudioManager am;
  private StationHarborUI shui;
  private SelectedPanelManager spm;
  private FactionsManager fm;
  [SerializeField]
  private Shipyard shipyard;
  [SerializeField]
  private int step;
  [Header("TIPS")]
  [SerializeField]
  private Transform tips;
  [SerializeField]
  private uiAnimator tips1;
  [SerializeField]
  private uiAnimator tips2;
  [SerializeField]
  private uiAnimator tips3;
  [SerializeField]
  private uiAnimator tips4;
  [SerializeField]
  private uiAnimator tips5;
  [SerializeField]
  private uiAnimator tips5p1;
  [SerializeField]
  private uiAnimator tips6;
  [SerializeField]
  private uiAnimator tips7;
  [SerializeField]
  private uiAnimator tips7p1;
  [SerializeField]
  private uiAnimator tips8;
  [SerializeField]
  private uiAnimator tips9;
  [SerializeField]
  private uiAnimator tips10;
  [Header("SARA")]
  [SerializeField]
  private TerminalTyper ttSara;
  private uiMover saraMover;
  [SerializeField]
  private GameObject replyButton;
  [SerializeField]
  private TMP_Text replyText;
  [SerializeField]
  private List<SaraStep> saraSteps;
  [SerializeField]
  private uiAnimator endScreen;
  [Header("LINE")]
  [SerializeField]
  private LinePointer lp;
  [SerializeField]
  private Transform lineStart;
  [SerializeField]
  private LineRenderer line;
  [SerializeField]
  private Transform indicatorsList;
  private Indicator targetIndicator;
  [SerializeField]
  private Track mainFleetTrack;
  [SerializeField]
  private FleetManager mainFleet;
  [SerializeField]
  private Navigation mfNav;
  [SerializeField]
  private Transform t3_TargetPanel;
  [SerializeField]
  private Transform t5_selectButton;
  [SerializeField]
  private Transform t6_selectPanel;
  [SerializeField]
  private Transform t10_navTargetButton;
  [SerializeField]
  private Transform t11_movementPanel;
  [SerializeField]
  private Transform t12_startFlightButton;
  [SerializeField]
  private Transform t14_hailButton;
  [SerializeField]
  private Transform t15_merryMakerMoveButton;
  [SerializeField]
  private Transform t16_mainFleetButton;
  [SerializeField]
  private Transform t17_repairButton;
  [SerializeField]
  private Transform t21_attachmentsTab;
  [SerializeField]
  private Transform t24_selectedFleetTab;
  [Header("STEPS")]
  [SerializeField]
  private GameObject navPanel;
  [SerializeField]
  private Track rubiconTrack;
  [SerializeField]
  private GameObject stationPanel;
  private Vector3 merryMakerTransferButtonPos = new Vector3(846f, 830f, 0.0f);
  private Vector3 serviceButtonPos = new Vector3(1352f, 736f, 0.0f);
  private Vector3 mfButtonPos = new Vector3(468f, 822f, 0.0f);
  private Vector3 startFlightButtonPos = new Vector3(1744f, 158f, 0.0f);
  [SerializeField]
  private uiAnimator stationServicesCover;
  [SerializeField]
  private S_Ship viper;
  [SerializeField]
  private S_Mount2 supplyMount;
  private bool isShipBought;

  private void Awake()
  {
    this.fm = FactionsManager.current;
    this.fm.SetupNewGame(true, (Faction) null);
  }

  private void Start()
  {
    this.mi = MapInputs.current;
    this.am = AudioManager.current;
    this.shui = StationHarborUI.current;
    this.spm = SelectedPanelManager.current;
    this.fm = FactionsManager.current;
    IEnumerator enumerator = (IEnumerator) this.tips.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
        ((Component) enumerator.Current).gameObject.SetActive(true);
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    this.shipyard.onPlayerBuyShip += new Action(this.OnPlayerBuyShip);
  }

  private void Update()
  {
    if (this.step == 2)
    {
      if (!this.mi.HasTarget())
        return;
      this.NextStep();
    }
    else if (this.step == 3)
      this.lp.NewTargetUI(this.t3_TargetPanel.position);
    else if (this.step == 4)
    {
      if ((bool) (UnityEngine.Object) this.targetIndicator && this.targetIndicator.gameObject.activeSelf)
      {
        if ((UnityEngine.Object) this.targetIndicator.track != (UnityEngine.Object) this.mainFleetTrack)
          this.SetTargetIndicator(this.mainFleetTrack);
        if ((bool) (UnityEngine.Object) this.targetIndicator)
          this.lp.NewTargetUI(this.targetIndicator.transform.position);
      }
      else
      {
        this.SetTargetIndicator(this.mainFleetTrack);
        if (!(bool) (UnityEngine.Object) this.targetIndicator)
          this.lp.Off();
      }
      if (!this.mi.HasTarget() || !((UnityEngine.Object) this.mi.GetTargetTrack() == (UnityEngine.Object) this.mainFleetTrack))
        return;
      this.lp.Off();
      this.NextStep();
    }
    else if (this.step == 5)
    {
      this.lp.NewTargetUI(this.t5_selectButton.position);
      if (!(bool) (UnityEngine.Object) this.mi.selectedFleet || !((UnityEngine.Object) this.mi.selectedFleet == (UnityEngine.Object) this.mainFleet))
        return;
      this.NextStep();
    }
    else if (this.step == 6)
      this.lp.NewTargetUI(this.t6_selectPanel.position);
    else if (this.step == 9)
    {
      if ((bool) (UnityEngine.Object) this.targetIndicator && this.targetIndicator.gameObject.activeSelf)
      {
        if ((UnityEngine.Object) this.targetIndicator.track != (UnityEngine.Object) this.rubiconTrack)
          this.SetTargetIndicator(this.rubiconTrack);
        if ((bool) (UnityEngine.Object) this.targetIndicator)
          this.lp.NewTargetUI(this.targetIndicator.transform.position);
      }
      else
      {
        this.SetTargetIndicator(this.rubiconTrack);
        if (!(bool) (UnityEngine.Object) this.targetIndicator)
          this.lp.Off();
      }
      if (!this.mi.HasTarget() || !((UnityEngine.Object) this.mi.GetTargetTrack() == (UnityEngine.Object) this.rubiconTrack))
        return;
      this.lp.Off();
      this.NextStep();
    }
    else if (this.step == 10)
    {
      this.lp.NewTargetUI(this.t10_navTargetButton.position);
      if (!((UnityEngine.Object) this.mfNav.target == (UnityEngine.Object) this.rubiconTrack.GetComponent<Transform>()))
        return;
      this.NextStep();
    }
    else if (this.step == 11)
    {
      this.lp.NewTargetUI(this.t11_movementPanel.position);
      if (this.mfNav.currentState != NavigationState.Accelerating)
        return;
      this.NextStep();
    }
    else if (this.step == 12)
    {
      this.lp.NewTargetUI(this.t12_startFlightButton.position);
      if (this.mfNav.currentState != NavigationState.Accelerating)
        return;
      this.NextStep();
    }
    else if (this.step == 13)
    {
      this.lp.Off();
      if (!(bool) (UnityEngine.Object) this.mainFleet.transform.parent || !(this.mainFleet.transform.parent.GetComponent<Track>().publicName == "Rubicon"))
        return;
      this.NextStep();
    }
    else if (this.step == 14)
    {
      this.lp.NewTargetUI(this.t14_hailButton.position);
      if (!this.stationPanel.activeSelf)
        return;
      this.NextStep();
    }
    else if (this.step == 15)
    {
      this.lp.NewTargetUI(this.t15_merryMakerMoveButton.position);
      if (this.mainFleet.ships.Count <= 2)
        return;
      this.NextStep();
    }
    else if (this.step == 16 /*0x10*/)
    {
      this.lp.NewTargetUI(this.t16_mainFleetButton.position);
      if (!((UnityEngine.Object) this.shui.Fleet == (UnityEngine.Object) this.mainFleet))
        return;
      this.NextStep();
    }
    else if (this.step == 17)
    {
      this.lp.NewTargetUI(this.t17_repairButton.position);
      if ((double) this.viper.GetHealthRatio() < 0.99900001287460327)
        return;
      this.NextStep();
    }
    else if (this.step == 18)
    {
      this.lp.Off();
      if ((double) this.supplyMount.resource < (double) (this.supplyMount.bp as IResupplyable).GetResourceMax())
        return;
      this.NextStep();
    }
    else if (this.step == 19)
    {
      if ((double) this.viper.GetFuelRatio() < 0.75)
        return;
      this.NextStep();
    }
    else if (this.step == 20)
    {
      if (this.stationPanel.activeSelf)
        return;
      this.NextStep();
    }
    else if (this.step == 21)
    {
      this.lp.NewTargetUI(this.t21_attachmentsTab.position);
      if (!this.mi.HasTarget() || !((UnityEngine.Object) this.mi.GetTargetTrack() == (UnityEngine.Object) this.mainFleetTrack))
        return;
      this.NextStep();
    }
    else if (this.step == 22)
    {
      if (!(bool) (UnityEngine.Object) this.mi.selectedFleet || !((UnityEngine.Object) this.mi.selectedFleet == (UnityEngine.Object) this.mainFleet))
        return;
      this.NextStep();
    }
    else if (this.step == 24)
    {
      this.lp.NewTargetUI(this.t24_selectedFleetTab.position);
      if (!this.mainFleet.isAutoRefuel)
        return;
      this.NextStep();
    }
    else if (this.step == 25)
    {
      if (!this.mainFleet.GetComponent<Trader>().isAutoTrading)
        return;
      this.NextStep();
    }
    else if (this.step == 26)
    {
      if (this.fm.playerFaction.credits < 100000)
        return;
      this.NextStep();
    }
    else if (this.step == 28)
    {
      if (this.mainFleet.GetComponent<Trader>().isAutoTrading)
        return;
      this.NextStep();
    }
    else if (this.step == 29)
    {
      if (this.mainFleet.GetComponent<Trader>().isAutoTrading || !(bool) (UnityEngine.Object) this.mainFleet.transform.parent)
        return;
      this.NextStep();
    }
    else
    {
      if (this.step != 30)
        return;
      this.SetTargetIndicator(this.mainFleet.transform.parent.GetComponent<Track>());
      if ((bool) (UnityEngine.Object) this.targetIndicator)
        this.lp.NewTargetUI(this.targetIndicator.transform.position);
      if (this.mainFleet.GetComponent<Trader>().isAutoTrading || !(bool) (UnityEngine.Object) this.mainFleet.transform.parent || !this.stationPanel.activeSelf)
        return;
      this.NextStep();
    }
  }

  private void OnPlayerBuyShip()
  {
    if (this.step != 31 /*0x1F*/)
      return;
    this.NextStep();
  }

  public void StartTutorial()
  {
    this.step = 0;
    this.NextStep();
  }

  private IEnumerator DelaySara()
  {
    yield return (object) new WaitForSeconds(10f);
    this.ttSara.GetComponent<uiAnimator>().Show();
  }

  public void NextStep()
  {
    this.lp.Off();
    ++this.step;
    if (this.step == 1)
    {
      this.StartCoroutine((IEnumerator) this.DelayStartTip(this.tips1));
      this.StartCoroutine((IEnumerator) this.DelaySara());
    }
    else if (this.step == 34)
    {
      this.ttSara.GetComponent<uiAnimator>().Hide();
      this.endScreen.Show();
    }
    else if (this.step > 1)
    {
      SaraStep saraStep = this.saraSteps[this.step];
      this.ttSara.TypeThis(saraStep.saraText);
      this.replyText.text = saraStep.replyText;
      this.replyButton.SetActive(saraStep.isShowReplyButton);
      if (this.step == 2)
      {
        this.tips1.Hide();
        this.tips2.Show();
      }
      else if (this.step == 4)
      {
        this.SetTargetIndicator(this.mainFleetTrack);
        this.tips2.Hide();
        this.tips3.Show();
      }
      else if (this.step == 6)
      {
        this.tips3.Hide();
        this.tips4.Show();
      }
      else if (this.step == 8)
      {
        this.tips4.Hide();
        this.SetTargetIndicator(this.rubiconTrack);
      }
      else if (this.step == 11)
        this.tips5p1.Show();
      else if (this.step == 12)
      {
        this.tips5p1.Hide();
        this.tips6.Show();
      }
      else if (this.step == 13)
      {
        this.tips6.Hide();
        this.tips7.Show();
      }
      else if (this.step != 15)
      {
        if (this.step == 17)
        {
          this.tips7.Hide();
          this.tips7p1.Show();
          this.stationServicesCover.Hide();
        }
        else if (this.step == 20)
        {
          this.mi.ClearSelected();
          this.spm.ClearSelected();
        }
        else if (this.step == 21)
        {
          this.tips7p1.Hide();
          this.tips8.Show();
        }
        else if (this.step == 25)
        {
          this.tips8.Hide();
          this.tips9.Show();
        }
        else if (this.step == 30)
        {
          this.tips9.Hide();
          this.tips10.Show();
        }
        else if (this.step == 31 /*0x1F*/)
          this.fm.ModPlayerCredits("REWARD", 1000000);
      }
    }
    this.am.PlaySFX(6);
  }

  private void SetTargetIndicator(Track targetTrack)
  {
    this.targetIndicator = (Indicator) null;
    foreach (Indicator componentsInChild in this.indicatorsList.GetComponentsInChildren<Indicator>())
    {
      if ((UnityEngine.Object) componentsInChild.track == (UnityEngine.Object) targetTrack && componentsInChild.indicatorType == IndicatorType.BOX)
      {
        this.targetIndicator = componentsInChild;
        break;
      }
    }
  }

  private IEnumerator DelayStartTip(uiAnimator tip)
  {
    yield return (object) new WaitForSeconds(0.5f);
    this.StartTip(tip);
  }

  private void StartTip(uiAnimator tip) => tip.Show();
}
