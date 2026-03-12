// Decompiled with JetBrains decompiler
// Type: PlayerFleetUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PlayerFleetUI : MonoBehaviour
{
  public static PlayerFleetUI current;
  private TacticalInputs ti;
  private Squadron currentSquadron;
  [SerializeField]
  private TMP_Text targetText;
  [SerializeField]
  private TMP_Text commandText;
  [SerializeField]
  private TMP_Text dispersionText;
  [SerializeField]
  private Slider dispersionSlider;
  [SerializeField]
  private Transform orderDelayImg;
  [SerializeField]
  private Transform uiShipList;
  [SerializeField]
  private CaptainDataUI captainDataUIPF;
  [SerializeField]
  private ShipDataUI shipDataPF;
  [SerializeField]
  private List<Toggle> fleetToggles = new List<Toggle>();
  [SerializeField]
  private List<SquadronToggle> fleetButtons = new List<SquadronToggle>();
  [SerializeField]
  private bool isVersion2;
  [SerializeField]
  private SquadronToggle squadronTogglePF;
  [SerializeField]
  private CaptainToggle captainTogglePF;

  private void Awake() => PlayerFleetUI.current = this;

  private void Start()
  {
    this.ti = TacticalInputs.current;
    this.ti.newSelection += new Action(this.NewSelection);
    this.StartCoroutine((IEnumerator) this.LateStart());
  }

  private IEnumerator LateStart()
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.NewSquadronData();
  }

  private void Update()
  {
    if (this.isVersion2)
      return;
    this.UpdateSquadronData();
  }

  private void NewSelection() => this.NewSquadronData();

  public void SetFleet(int index)
  {
    MonoBehaviour.print((object) ("===== new fleet: " + index.ToString()));
    this.NewSquadronData();
  }

  public void SetToggle(int index)
  {
    if (this.isVersion2)
    {
      this.SetFleet(index);
      this.NewSquadronData();
    }
    else
    {
      if (this.fleetToggles[index].isOn)
        this.ti.SetSelectedSquadronIndex(index);
      else
        this.fleetToggles[index].isOn = true;
      this.NewSquadronData();
    }
  }

  public void RefreshSquadronList()
  {
    this.ClearShipList();
    this.UpdateList();
  }

  private void NewSquadronData()
  {
    if (this.isVersion2)
      this.UpdateList();
    else if (!(bool) (UnityEngine.Object) this.ti.selectedSquadron)
    {
      this.ClearShipList();
    }
    else
    {
      this.currentSquadron = this.ti.selectedSquadron;
      this.dispersionSlider.value = (float) (this.currentSquadron.dispersion / 100);
      this.UpdateList();
    }
  }

  private void UpdateSquadronData()
  {
    if (!(bool) (UnityEngine.Object) this.currentSquadron || this.isVersion2)
      return;
    this.targetText.text = !(bool) (UnityEngine.Object) this.currentSquadron.targetTrack ? "NULL" : this.currentSquadron.targetTrack.trackName;
    this.commandText.text = this.currentSquadron.command == SquadronCommand.NULL ? "NULL" : this.currentSquadron.command.ToString();
    this.dispersionText.text = this.dispersionSlider.value.ToString("F0") + "km";
    if (this.currentSquadron.isTakingNewOrders)
    {
      this.orderDelayImg.gameObject.SetActive(true);
      this.orderDelayImg.Rotate(-Vector3.forward, Time.deltaTime * 360f);
    }
    else
    {
      this.orderDelayImg.gameObject.SetActive(false);
      this.orderDelayImg.rotation = Quaternion.identity;
    }
  }

  public void SetDispersion()
  {
    if (!(bool) (UnityEngine.Object) this.currentSquadron)
      return;
    this.currentSquadron.SetDispersion((int) this.dispersionSlider.value * 100);
  }

  public void NewShipSelected(int index)
  {
    MonoBehaviour.print((object) ("===== new ship selected: " + index.ToString()));
    this.NewSquadronData();
  }

  private void ClearShipList()
  {
    if (this.isVersion2)
    {
      IEnumerator enumerator = (IEnumerator) this.uiShipList.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          Transform current = (Transform) enumerator.Current;
          if (current.gameObject.activeSelf)
            UnityEngine.Object.Destroy((UnityEngine.Object) current.gameObject);
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }
    else
    {
      IEnumerator enumerator = (IEnumerator) this.uiShipList.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator.Current).gameObject);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }
  }

  private void UpdateList()
  {
    this.ClearShipList();
    if (this.isVersion2)
    {
      this.fleetButtons.Clear();
      for (int index = this.ti.squadronsOwned.Count - 1; index >= 0; --index)
      {
        Squadron squadron = this.ti.squadronsOwned[index];
        foreach (Captain ship in squadron.GetShips())
          this.NewShip(ship);
        this.NewSquadron(squadron);
      }
    }
    else
    {
      foreach (Captain ship in this.currentSquadron.GetShips())
        this.NewShip(ship);
    }
  }

  private void NewSquadron(Squadron squadron)
  {
    SquadronToggle squadronToggle = UnityEngine.Object.Instantiate<SquadronToggle>(this.squadronTogglePF, this.uiShipList);
    squadronToggle.gameObject.SetActive(true);
    squadronToggle.SetSquadron(squadron);
    this.fleetButtons.Add(squadronToggle);
    if ((UnityEngine.Object) squadron == (UnityEngine.Object) this.ti.selectedSquadron)
      squadronToggle.Toggle(true);
    else
      squadronToggle.Toggle(false);
  }

  private void NewShip(Captain ship)
  {
    if (!(bool) (UnityEngine.Object) ship || !(bool) (UnityEngine.Object) ship.gameObject || !ship.gameObject.activeSelf)
      return;
    if (this.isVersion2)
    {
      CaptainToggle captainToggle = UnityEngine.Object.Instantiate<CaptainToggle>(this.captainTogglePF, this.uiShipList);
      captainToggle.gameObject.SetActive(true);
      captainToggle.SetShip(ship);
      if ((UnityEngine.Object) ship.ship == (UnityEngine.Object) this.ti.selectedShip)
        captainToggle.Toggle(true);
      else
        captainToggle.Toggle(false);
    }
    else
      UnityEngine.Object.Instantiate<CaptainDataUI>(this.captainDataUIPF, this.uiShipList).SetShip(ship);
  }

  public void AddShip(ShipController sc)
  {
  }
}
