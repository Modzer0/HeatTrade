// Decompiled with JetBrains decompiler
// Type: PlayerFleetPanelManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PlayerFleetPanelManager : MonoBehaviour
{
  public static PlayerFleetPanelManager current;
  private FleetManager fleet;
  private FleetState previousState;
  [Header("UI")]
  [Header("STATE TOGGLES")]
  [SerializeField]
  private Toggle idleToggle;
  [SerializeField]
  private Toggle tradeToggle;
  [SerializeField]
  private Toggle attackToggle;
  [SerializeField]
  private Toggle repairToggle;
  [SerializeField]
  private Toggle resupplyToggle;
  [SerializeField]
  private Toggle refuelToggle;
  [Header("AUTO TOGGLES")]
  [SerializeField]
  private Toggle autoRefuelToggle;
  [SerializeField]
  private Toggle autoRepairToggle;
  [SerializeField]
  private Toggle autoRearmToggle;
  [SerializeField]
  private FleetPanel fleetPanel;

  private void Awake() => PlayerFleetPanelManager.current = this;

  private void Update()
  {
    if (!(bool) (Object) this.fleet || this.fleet.state == this.previousState)
      return;
    this.UpdateStateToggles();
  }

  private void UpdateStateToggles()
  {
    this.idleToggle.SetIsOnWithoutNotify(false);
    this.tradeToggle.SetIsOnWithoutNotify(false);
    this.attackToggle.SetIsOnWithoutNotify(false);
    this.repairToggle.SetIsOnWithoutNotify(false);
    this.resupplyToggle.SetIsOnWithoutNotify(false);
    this.refuelToggle.SetIsOnWithoutNotify(false);
    if (this.fleet.state == FleetState.IDLE)
      this.idleToggle.SetIsOnWithoutNotify(true);
    else if (this.fleet.state == FleetState.TRADING)
      this.tradeToggle.SetIsOnWithoutNotify(true);
    else if (this.fleet.state == FleetState.ATTACKING)
      this.attackToggle.SetIsOnWithoutNotify(true);
    else if (this.fleet.state == FleetState.REPAIRING)
      this.repairToggle.SetIsOnWithoutNotify(true);
    else if (this.fleet.state == FleetState.RESUPPLYING)
      this.resupplyToggle.SetIsOnWithoutNotify(true);
    else if (this.fleet.state == FleetState.REFUELING)
      this.refuelToggle.SetIsOnWithoutNotify(true);
    this.previousState = this.fleet.state;
  }

  public void NewSelected(FleetManager newFleet)
  {
    this.fleet = newFleet;
    this.previousState = this.fleet.state;
    this.fleetPanel.Setup(this.fleet);
    this.autoRefuelToggle.isOn = this.fleet.isAutoRefuel;
    this.autoRepairToggle.isOn = this.fleet.isAutoRepair;
    if (this.fleet.isAutoResupply)
      this.autoRearmToggle.isOn = true;
    else
      this.autoRearmToggle.isOn = false;
  }

  public void ToggleRefuel(bool isOn)
  {
    if (this.fleet.isAutoRefuel == isOn)
      return;
    this.fleet.isAutoRefuel = isOn;
  }

  public void ToggleRepair(bool isOn)
  {
    if (this.fleet.isAutoRepair == isOn)
      return;
    this.fleet.isAutoRepair = isOn;
  }

  public void ToggleRearm(bool isOn)
  {
    if (this.fleet.isAutoResupply == isOn)
      return;
    this.fleet.isAutoResupply = isOn;
  }
}
