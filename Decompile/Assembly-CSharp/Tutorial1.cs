// Decompiled with JetBrains decompiler
// Type: Tutorial1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class Tutorial1 : MonoBehaviour
{
  private TacticalCamera tc;
  private TacticalInputs ti;
  private AudioManager am;
  private FactionsManager fm;
  [SerializeField]
  private Transform predawnBlue;
  [SerializeField]
  private int step;
  [Header("CONSOLE")]
  [SerializeField]
  private TerminalTyper tt;
  [SerializeField]
  private GameObject acceptButton;
  [SerializeField]
  private uiAnimator consolePanel;
  [SerializeField]
  private uiAnimator heatDeath;
  [Header("SARA")]
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
  private Transform indicatorsList;
  private Indicator targetIndicator;
  [SerializeField]
  private Transform target8;
  [SerializeField]
  private Transform target12;
  [SerializeField]
  private Transform target15;
  [Header("REPAIR")]
  [SerializeField]
  private uiAnimator tips1;
  [SerializeField]
  private uiAnimator tips2;
  [SerializeField]
  private uiAnimator tips3p1;
  [SerializeField]
  private uiAnimator tips3p2;
  [SerializeField]
  private uiAnimator tips3p3;
  [SerializeField]
  private uiAnimator tips4;
  [SerializeField]
  private T_Module crewModule;
  [SerializeField]
  private T_Module driveModule;
  [Header("BEACONS")]
  [SerializeField]
  private GameObject beacon1;
  [SerializeField]
  private GameObject beacon2;
  [SerializeField]
  private GameObject beacon3;
  [Header("MISSILE DEFENSE")]
  [SerializeField]
  private GameObject weaponsCover;
  [SerializeField]
  private Captain hephaestus;
  [SerializeField]
  private WeaponSystem predawnBluePDC;
  [SerializeField]
  private MissileSystem hephaestusMissileBay;
  [SerializeField]
  private ShipController hephaestusSC;
  [SerializeField]
  private uiAnimator endScreen;
  private bool isFollowing;
  [Header("TEXTS")]
  [SerializeField]
  private List<BigText> texts;
  [SerializeField]
  private List<BigText> replyTexts;
  [SerializeField]
  private List<SaraStep> saraSteps;
  [Header("AUDIO")]
  [SerializeField]
  private AudioSource damageLoopA;
  [SerializeField]
  private AudioSource damageLoopB;

  private void Awake()
  {
    this.fm = FactionsManager.current;
    this.fm.SetupNewGame(true, (Faction) null);
  }

  private void Start()
  {
    this.tc = TacticalCamera.current;
    this.StartCoroutine((IEnumerator) this.DelayFocus());
    this.tt.OnTypingFinished += new Action(this.OnTypingFinished);
    this.tt.TypeThis(this.texts[this.step].text);
    this.ti = TacticalInputs.current;
    this.am = AudioManager.current;
  }

  private void Update()
  {
    if (this.step == 7)
    {
      this.SetTargetIndicator(this.predawnBlue.GetComponent<Track>());
      if ((bool) (UnityEngine.Object) this.targetIndicator)
        this.lp.NewTargetUI(this.targetIndicator.transform.position);
      else
        this.lp.Off();
      if (!((UnityEngine.Object) this.ti.selectedShip == (UnityEngine.Object) this.predawnBlue.GetComponent<ShipController>()))
        return;
      this.lp.Off();
      this.NextStep();
    }
    else if (this.step == 8)
    {
      this.lp.NewTargetUI(this.target8.position);
      if ((double) this.crewModule.health < 50.0 || (double) this.driveModule.health < 50.0)
        return;
      this.lp.Off();
      this.heatDeath.Hide();
      this.damageLoopB.loop = false;
      this.NextStep();
    }
    else if (this.step == 9)
    {
      this.SetTargetIndicator(this.beacon1.GetComponent<Track>());
      if ((bool) (UnityEngine.Object) this.targetIndicator)
        this.lp.NewTargetUI(this.targetIndicator.transform.position);
      else
        this.lp.Off();
      if ((double) Vector3.Distance(this.beacon1.transform.position, this.predawnBlue.transform.position) >= 10.0)
        return;
      this.lp.Off();
      this.NextStep();
    }
    else if (this.step == 10)
    {
      this.SetTargetIndicator(this.beacon2.GetComponent<Track>());
      if ((bool) (UnityEngine.Object) this.targetIndicator)
        this.lp.NewTargetUI(this.targetIndicator.transform.position);
      else
        this.lp.Off();
      if ((double) Vector3.Distance(this.beacon2.transform.position, this.predawnBlue.transform.position) >= 10.0)
        return;
      this.lp.Off();
      this.NextStep();
    }
    else if (this.step == 11)
    {
      this.SetTargetIndicator(this.beacon3.GetComponent<Track>());
      if ((bool) (UnityEngine.Object) this.targetIndicator)
        this.lp.NewTargetUI(this.targetIndicator.transform.position);
      else
        this.lp.Off();
      if ((double) Vector3.Distance(this.beacon3.transform.position, this.predawnBlue.transform.position) >= 10.0)
        return;
      this.lp.Off();
      this.NextStep();
    }
    else if (this.step == 12)
    {
      if (this.predawnBluePDC.currentMode != WeaponMode.DEFENSE)
        return;
      this.NextStep();
    }
    else if (this.step == 13)
    {
      if (this.hephaestusMissileBay.missiles.Count >= 8)
        return;
      this.tc.SetFocus(this.hephaestus.transform, 1);
      this.NextStep();
    }
    else if (this.step == 14)
    {
      if (this.hephaestusMissileBay.mg.missilesToGuide.Count > 0)
        return;
      this.hephaestus.GetComponent<ShipController>().isTakeDamage = true;
      this.NextStep();
    }
    else
    {
      if (this.step != 15)
        return;
      if (this.predawnBluePDC.currentMode == WeaponMode.OFFENSE && !this.isFollowing)
      {
        this.tc.SetFocus(this.hephaestus.transform, 2);
        this.isFollowing = true;
      }
      if (!this.hephaestusSC.isDead)
        return;
      this.NextStep();
    }
  }

  private IEnumerator DelayFocus()
  {
    yield return (object) new WaitForSeconds(1f);
    this.tc.SetFocus(this.predawnBlue, 1);
  }

  private IEnumerator DelaySara()
  {
    yield return (object) new WaitForSeconds(20f);
    this.ttSara.GetComponent<uiAnimator>().Show();
    this.NextStep();
  }

  private void OnTypingFinished()
  {
    if (this.step == 0)
    {
      this.acceptButton.SetActive(true);
    }
    else
    {
      if (this.step != 1)
        return;
      this.NextStep();
    }
  }

  public void NextStep()
  {
    ++this.step;
    if (this.step < this.texts.Count)
    {
      if (this.step <= 1)
        this.tt.TypeThis(this.texts[this.step].text);
      else if (this.step >= 3)
      {
        this.ttSara.TypeThis(this.texts[this.step].text);
        this.replyText.text = this.replyTexts[this.step].text;
      }
    }
    if (this.step == 2)
    {
      this.StartCoroutine((IEnumerator) this.DelayStartTip(this.tips1));
      this.consolePanel.Hide();
      this.damageLoopA.loop = false;
      this.StartCoroutine((IEnumerator) this.DelaySara());
      this.beacon1.GetComponent<Target>().enabled = false;
      this.beacon2.GetComponent<Target>().enabled = false;
      this.beacon3.GetComponent<Target>().enabled = false;
      this.predawnBluePDC.isOn = false;
    }
    else if (this.step == 17)
    {
      this.ttSara.GetComponent<uiAnimator>().Hide();
      this.endScreen.Show();
    }
    else if (this.step >= 3)
    {
      SaraStep saraStep = this.saraSteps[this.step];
      this.ttSara.TypeThis(saraStep.saraText);
      this.replyText.text = saraStep.replyText;
      this.replyButton.SetActive(saraStep.isShowReplyButton);
      if (this.step != 4 && this.step != 7)
      {
        if (this.step == 8)
        {
          this.tips1.Hide();
          this.tips2.Show();
        }
        else if (this.step == 9)
        {
          this.tips2.Hide();
          this.tips3p1.Show();
          this.beacon1.GetComponent<Target>().enabled = true;
        }
        else if (this.step == 10)
        {
          this.beacon1.SetActive(false);
          this.beacon2.GetComponent<Target>().enabled = true;
          this.tips3p1.Hide();
          this.tips3p2.Show();
        }
        else if (this.step == 11)
        {
          this.beacon2.SetActive(false);
          this.beacon3.GetComponent<Target>().enabled = true;
          this.tips3p2.Hide();
          this.tips3p3.Show();
        }
        else if (this.step == 12)
        {
          this.beacon3.SetActive(false);
          this.tips3p3.Hide();
          this.tips4.Show();
          this.weaponsCover.SetActive(false);
          this.predawnBluePDC.isOn = true;
          this.predawnBluePDC.TryTarget((Track) null);
        }
        else if (this.step == 13)
          this.hephaestus.SetLaunchChance(100);
        else if (this.step == 16 /*0x10*/)
          this.tips4.Hide();
      }
    }
    this.am.PlaySFX(2);
  }

  private IEnumerator DelayStartTip(uiAnimator tip)
  {
    yield return (object) new WaitForSeconds(0.5f);
    this.StartTip(tip);
  }

  private void StartTip(uiAnimator tip) => tip.Show();

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
}
