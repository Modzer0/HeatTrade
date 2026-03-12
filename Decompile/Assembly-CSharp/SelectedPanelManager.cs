// Decompiled with JetBrains decompiler
// Type: SelectedPanelManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SelectedPanelManager : MonoBehaviour
{
  public static SelectedPanelManager current;
  private NavPanel np;
  private TraderPanelManager tpm;
  private PlayerFleetPanelManager pfpm;
  [SerializeField]
  private AttachmentsPanel ap;
  private FleetPanels fps;
  private Track selected;
  [Header("UI")]
  [SerializeField]
  private TMP_Text selectedName;
  [SerializeField]
  private TMP_InputField nameInput;
  [SerializeField]
  private GameObject selectedPanel;
  [Header("TOGGLES: PANEL")]
  [SerializeField]
  private Toggle navToggle;
  [SerializeField]
  private Toggle fleetToggle;
  [SerializeField]
  private Toggle tradeToggle;
  [SerializeField]
  private Toggle cargoToggle;
  [SerializeField]
  private Toggle attachmentsToggle;
  [Header("TOGGLES: AUTO")]
  [SerializeField]
  private Toggle autoRefuelToggle;
  [SerializeField]
  private Toggle autoRepairToggle;
  [SerializeField]
  private Toggle autoResupplyToggle;
  [Header("TOGGLES: STATE")]
  [SerializeField]
  private Toggle fleetIdleToggle;
  [SerializeField]
  private Toggle fleetTradeToggle;
  [SerializeField]
  private Toggle fleetAttackToggle;
  [SerializeField]
  private Toggle fleetRepairToggle;
  [SerializeField]
  private Toggle fleetResupplyToggle;
  [SerializeField]
  private Toggle fleetRefuelToggle;

  private void Awake() => SelectedPanelManager.current = this;

  private void Start()
  {
    this.np = NavPanel.current;
    this.tpm = TraderPanelManager.current;
    this.pfpm = PlayerFleetPanelManager.current;
    this.fps = FleetPanels.current;
  }

  public void NewSelected(Track newSelected)
  {
    this.selected = newSelected;
    this.selectedName.text = this.selected.GetFactionCode() + this.selected.id;
    this.nameInput.text = this.selected.publicName;
    this.SetPanels();
    this.SetFleetToggles();
  }

  public void ClearSelected()
  {
    this.selected = (Track) null;
    this.selectedPanel.SetActive(false);
  }

  public void Refresh() => this.StartCoroutine((IEnumerator) this.DelayedRefresh());

  private IEnumerator DelayedRefresh()
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.SetPanels();
    this.SetFleetToggles();
  }

  private void SetFleetToggles()
  {
    if (!(bool) (UnityEngine.Object) this.selected || !(bool) (UnityEngine.Object) this.selected.GetComponent<FleetManager>())
      return;
    FleetManager component = this.selected.GetComponent<FleetManager>();
    this.autoRefuelToggle.isOn = component.isAutoRefuel;
    this.autoRepairToggle.isOn = component.isAutoRepair;
    this.autoResupplyToggle.isOn = component.isAutoResupply;
    if (component.state == FleetState.IDLE)
      this.fleetIdleToggle.SetIsOnWithoutNotify(true);
    else if (component.state == FleetState.TRADING)
      this.fleetTradeToggle.SetIsOnWithoutNotify(true);
    else if (component.state == FleetState.ATTACKING)
      this.fleetAttackToggle.SetIsOnWithoutNotify(true);
    else if (component.state == FleetState.REPAIRING)
      this.fleetRepairToggle.SetIsOnWithoutNotify(true);
    else if (component.state == FleetState.RESUPPLYING)
    {
      this.fleetResupplyToggle.SetIsOnWithoutNotify(true);
    }
    else
    {
      if (component.state != FleetState.REFUELING)
        return;
      this.fleetRefuelToggle.SetIsOnWithoutNotify(true);
    }
  }

  private void SetPanels()
  {
    this.navToggle.gameObject.SetActive(false);
    this.fleetToggle.gameObject.SetActive(false);
    this.tradeToggle.gameObject.SetActive(false);
    this.attachmentsToggle.gameObject.SetActive(false);
    if ((UnityEngine.Object) this.selected == (UnityEngine.Object) null || !(bool) (UnityEngine.Object) this.selected.GetComponent<Track>())
    {
      this.selectedPanel.SetActive(false);
    }
    else
    {
      this.selectedPanel.SetActive(true);
      FleetManager component1 = this.selected.GetComponent<FleetManager>();
      Trader component2 = this.selected.GetComponent<Trader>();
      Attachments component3 = this.selected.GetComponent<Attachments>();
      if ((bool) (UnityEngine.Object) this.selected.GetComponent<Navigation>() && (bool) (UnityEngine.Object) component1)
      {
        this.navToggle.gameObject.SetActive(true);
        this.np.SetNewFleet(component1);
      }
      if ((bool) (UnityEngine.Object) component1)
      {
        this.fleetToggle.gameObject.SetActive(true);
        this.pfpm.NewSelected(component1);
      }
      if ((bool) (UnityEngine.Object) component2)
      {
        this.tradeToggle.gameObject.SetActive(true);
        this.tpm.NewSelected(component2);
      }
      if (!(bool) (UnityEngine.Object) this.selected.GetComponent<Attachments>())
        return;
      this.attachmentsToggle.gameObject.SetActive(true);
      this.ap.Setup(component3);
    }
  }

  public void RenameSelectedTrack()
  {
    if (!(bool) (UnityEngine.Object) this.selected)
      return;
    this.selected.publicName = this.nameInput.text;
    this.fps.UpdateData();
  }

  public void SetFleetStateTo(string newState)
  {
    this.selected.GetComponent<FleetManager>().SetStateTo((FleetState) Enum.Parse(typeof (FleetState), newState));
  }
}
