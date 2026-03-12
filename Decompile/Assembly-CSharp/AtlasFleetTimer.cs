// Decompiled with JetBrains decompiler
// Type: AtlasFleetTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class AtlasFleetTimer : MonoBehaviour
{
  private TimeManager tm;
  private SceneTransitionManager stm;
  private SaveLoadSystem sls;
  private AllStructures allStructures;
  [SerializeField]
  private AudioSource sfx;
  [SerializeField]
  private int daysBeforeArrival;
  private int daysRemaining;
  [SerializeField]
  private FleetManager playerFleet;
  [SerializeField]
  private GameObject atlasFleetPF;
  [Header("UI - Timer")]
  [SerializeField]
  private TMP_Text timer;
  [SerializeField]
  private uiBar timerBar;
  [SerializeField]
  private uiAnimator encounterUI;
  [Header("UI - End Screen")]
  private bool isShowingEndScreen;
  [SerializeField]
  private ParticleSystem ps1;
  [SerializeField]
  private ParticleSystem ps2;
  [SerializeField]
  private uiAnimator uiaEndScreen;
  [SerializeField]
  private uiAnimator uiaTop;
  [SerializeField]
  private GameObject defeat;
  [SerializeField]
  private GameObject victory;
  [SerializeField]
  private uiAnimator uiaMid;
  [SerializeField]
  private TerminalTyper tt;
  [SerializeField]
  private uiAnimator uiaBot;
  [SerializeField]
  private uiAnimator uiaEnd;

  private void Start()
  {
    this.stm = SceneTransitionManager.current;
    this.tm = TimeManager.current;
    this.tm.NewDay += new Action(this.NewDay);
    this.sls = SaveLoadSystem.current;
    this.sls.OnLoadGame += new Action(this.OnLoadGame);
    this.allStructures = AllStructures.current;
    this.StartCoroutine((IEnumerator) this.LateStart());
    this.NewDay();
  }

  private void OnLoadGame() => this.StartCoroutine((IEnumerator) this.LateStart());

  private IEnumerator ShowEndScreen(bool isVictory)
  {
    this.uiaEndScreen.Show();
    this.sfx.Play();
    this.uiaTop.Show();
    if (isVictory)
    {
      this.victory.SetActive(true);
      yield return (object) new WaitForSeconds(1f);
      this.ps1.Play();
      this.ps2.Play();
    }
    else
      this.defeat.SetActive(true);
    yield return (object) new WaitForSeconds(5f);
    this.uiaMid.Show();
    this.tt.TypeCurrent();
    yield return (object) new WaitForSeconds(12f);
    this.uiaBot.Show();
    yield return (object) new WaitForSeconds(5f);
    this.uiaEnd.Show();
  }

  private IEnumerator LateStart()
  {
    AtlasFleetTimer atlasFleetTimer = this;
    yield return (object) new WaitForSeconds(0.1f);
    yield return (object) new WaitForSeconds(0.1f);
    atlasFleetTimer.CalculateDaysRemaining();
    atlasFleetTimer.UpdateDisplay();
    if (atlasFleetTimer.daysRemaining <= 0 && !atlasFleetTimer.isShowingEndScreen)
    {
      atlasFleetTimer.isShowingEndScreen = true;
      atlasFleetTimer.StartCoroutine((IEnumerator) atlasFleetTimer.ShowEndScreen(atlasFleetTimer.stm.isPlayerVictory));
    }
  }

  private void CalculateDaysRemaining()
  {
    this.daysRemaining = this.daysBeforeArrival - this.tm.TotalDays;
  }

  private void NewDay()
  {
    this.CalculateDaysRemaining();
    this.UpdateDisplay();
    if (this.daysRemaining > 0)
      return;
    this.tm.NewDay -= new Action(this.NewDay);
    this.EncounterSetup();
  }

  private void UpdateDisplay()
  {
    this.timer.text = this.daysRemaining.ToString() + " DAYS";
    this.timerBar.SetBarSize((float) this.daysRemaining / (float) this.daysBeforeArrival);
  }

  private IEnumerator LateTest()
  {
    yield return (object) new WaitForSeconds(2f);
    this.EncounterSetup();
  }

  private void EncounterSetup()
  {
    List<TacticalGroupData> newGroups = new List<TacticalGroupData>();
    this.encounterUI.Show();
    int num = 0;
    foreach (FleetManager fleetManager in UnityEngine.Object.FindObjectsOfType<FleetManager>())
    {
      if (num < 10)
      {
        if (fleetManager.GetComponent<Track>().factionID == 1)
        {
          foreach (Component ship in fleetManager.ships)
          {
            ship.transform.SetParent(this.playerFleet.transform);
            ++num;
            if (num >= 10)
              break;
          }
        }
      }
      else
        break;
    }
    if (num == 0)
    {
      this.encounterUI.Hide();
      this.StartCoroutine((IEnumerator) this.ShowEndScreen(false));
    }
    else
    {
      this.playerFleet.UpdateFleet();
      newGroups.Add(this.playerFleet.ToTacticalGroup());
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.atlasFleetPF, new Vector3(this.playerFleet.transform.position.x + 1f, this.playerFleet.transform.position.y, this.playerFleet.transform.position.z + 1f), this.playerFleet.transform.rotation);
      foreach (S_Ship ship in gameObject.GetComponent<FleetManager>().ships)
        this.allStructures.AddShip(ship);
      newGroups.Add(gameObject.GetComponent<FleetManager>().ToTacticalGroup());
      UnityEngine.Object.Destroy((UnityEngine.Object) gameObject);
      this.stm.SetupEngagement(newGroups, this.transform.position);
    }
  }
}
