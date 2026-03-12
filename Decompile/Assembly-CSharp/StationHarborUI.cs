// Decompiled with JetBrains decompiler
// Type: StationHarborUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class StationHarborUI : MonoBehaviour
{
  public static StationHarborUI current;
  private TimeManager tm;
  private FleetPanels fps;
  [Header("UI")]
  [SerializeField]
  private GameObject harborPanel;
  [SerializeField]
  private TMP_Text stationName;
  public StationManager station;
  private Market market;
  private FleetManager fleet;
  [Header("PORT")]
  [SerializeField]
  private FleetPortUI fleetPortPF;
  [SerializeField]
  private ShipVicUI shipPortPF;
  [SerializeField]
  private GameObject dockingBayPF;
  [SerializeField]
  private Transform portList;
  [Header("TRANSFER TO FLEET")]
  public FleetManager transferToThisFleet;
  private Transform shipToTransfer;
  [SerializeField]
  private GameObject shipTransferPanel;
  [SerializeField]
  private Transform shipTransferList;
  [SerializeField]
  private TransferFleetButton transferFleetButtonPF;
  [Header("NEW FLEET")]
  [SerializeField]
  private TMP_InputField fleetNameInput;
  [SerializeField]
  private GameObject fleetPF;
  [Header("= SERVICES ====================")]
  [Header("Repair")]
  [SerializeField]
  private TMP_Text repairRateText;
  [SerializeField]
  private ResourceDefinition metals;
  [SerializeField]
  private TMP_Text metalsBarText;
  [SerializeField]
  private uiBar metalsBar;
  [SerializeField]
  private TMP_Text metalsPriceText;
  [SerializeField]
  private ResourceDefinition rareEarths;
  [SerializeField]
  private TMP_Text rareEarthsBarText;
  [SerializeField]
  private uiBar rareEarthsBar;
  [SerializeField]
  private TMP_Text rareEarthsPriceText;
  [SerializeField]
  private ResourceDefinition superalloys;
  [SerializeField]
  private TMP_Text superalloysBarText;
  [SerializeField]
  private uiBar superalloysBar;
  [SerializeField]
  private TMP_Text superalloysPriceText;
  [SerializeField]
  private ResourceDefinition exoticMatter;
  [SerializeField]
  private TMP_Text exoticMatterPriceText;
  [SerializeField]
  private uiBar exoticMatterBar;
  [SerializeField]
  private TMP_Text exoticMatterBarText;
  [SerializeField]
  private TMP_Text repairFleetText;
  [SerializeField]
  private Transform shipsListRepair;
  [SerializeField]
  private ShipModulesCard shipModulesPF;
  [Header("Resupply")]
  [SerializeField]
  private TMP_Text resupplyRateText;
  [SerializeField]
  private ResourceDefinition bMunitions;
  [SerializeField]
  private TMP_Text bMunitionsBarText;
  [SerializeField]
  private uiBar bMunitionsBar;
  [SerializeField]
  private TMP_Text bMunitionsPriceText;
  [SerializeField]
  private ResourceDefinition aMunitions;
  [SerializeField]
  private TMP_Text aMunitionsBarText;
  [SerializeField]
  private uiBar aMunitionsBar;
  [SerializeField]
  private TMP_Text aMunitionsPriceText;
  [SerializeField]
  private ResourceDefinition eMunitions;
  [SerializeField]
  private TMP_Text eMunitionsPriceText;
  [SerializeField]
  private uiBar eMunitionsBar;
  [SerializeField]
  private TMP_Text eMunitionsBarText;
  [SerializeField]
  private TMP_Text resupplyFleetText;
  [SerializeField]
  private Transform shipsListResupply;
  [SerializeField]
  private ShipModulesCard shipMunitionsPF;
  [Header("Refuel")]
  [SerializeField]
  private TMP_Text refuelRateText;
  [SerializeField]
  private ResourceDefinition tFuel;
  [SerializeField]
  private TMP_Text tFuelPriceText;
  [SerializeField]
  private TMP_Text tFuelBarText;
  [SerializeField]
  private uiBar tFuelBar;
  [SerializeField]
  private ResourceDefinition hFuel;
  [SerializeField]
  private TMP_Text hFuelPriceText;
  [SerializeField]
  private TMP_Text hFuelBarText;
  [SerializeField]
  private uiBar hFuelBar;
  [SerializeField]
  private ResourceDefinition aFuel;
  [SerializeField]
  private TMP_Text aFuelPriceText;
  [SerializeField]
  private TMP_Text aFuelBarText;
  [SerializeField]
  private uiBar aFuelBar;
  [SerializeField]
  private TMP_Text refuelFleetText;
  [SerializeField]
  private TMP_Text fleetDvText;
  [SerializeField]
  private uiBar fleetDvBar;
  [SerializeField]
  private Transform shipsListRefuel;
  [SerializeField]
  private ShipFuelUI shipFuelPF;
  [Header("Rearm")]
  [SerializeField]
  private TMP_Text rearmPriceText;
  [SerializeField]
  private TMP_Text rearmBarText;
  [SerializeField]
  private uiBar rearmBar;
  [Header("Fleet Service Toggles")]
  [SerializeField]
  private Toggle fleetRepairToggle;
  private bool wasRepairing;
  [SerializeField]
  private Toggle fleetResupplyToggle;
  private bool wasResupplying;
  [SerializeField]
  private Toggle fleetRefuelToggle;
  private bool wasRefueling;

  public FleetManager Fleet => this.fleet;

  private void Awake() => StationHarborUI.current = this;

  private void Start()
  {
    this.tm = TimeManager.current;
    this.fps = FleetPanels.current;
  }

  private void Update()
  {
    if ((bool) (UnityEngine.Object) this.fleet)
      this.UpdateAllFleetData();
    if ((bool) (UnityEngine.Object) this.station)
      this.UpdateAllStationData();
    this.UpdateToggles();
    this.GetInput();
  }

  private void GetInput()
  {
    if (!Input.GetKeyDown(KeyCode.Escape))
      return;
    this.SetOff();
  }

  private void UpdateToggles()
  {
    if (!(bool) (UnityEngine.Object) this.fleet || !this.harborPanel.activeSelf)
      return;
    bool isRepairing = this.fleet.isRepairing;
    bool isResupplying = this.fleet.isResupplying;
    bool isRefueling = this.fleet.isRefueling;
    if (isRepairing != this.wasRepairing)
    {
      this.fleetRepairToggle.SetIsOnWithoutNotify(this.fleet.isRepairing);
      this.wasRepairing = isRepairing;
    }
    if (isResupplying != this.wasResupplying)
    {
      this.fleetResupplyToggle.SetIsOnWithoutNotify(this.fleet.isResupplying);
      this.wasResupplying = isResupplying;
    }
    if (isRefueling == this.wasRefueling)
      return;
    this.fleetRefuelToggle.SetIsOnWithoutNotify(this.fleet.isRefueling);
    this.wasRefueling = isRefueling;
  }

  public void Refresh()
  {
    this.stationName.text = $"WELCOME TO {this.station.GetComponent<Track>().publicName.ToUpper()} STATION!";
    IEnumerator enumerator1 = (IEnumerator) this.portList.GetEnumerator();
    try
    {
      while (enumerator1.MoveNext())
        UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator1.Current).gameObject);
    }
    finally
    {
      if (enumerator1 is IDisposable disposable)
        disposable.Dispose();
    }
    bool flag = true;
    IEnumerator enumerator2 = (IEnumerator) this.station.transform.GetEnumerator();
    try
    {
      while (enumerator2.MoveNext())
      {
        Transform current = (Transform) enumerator2.Current;
        if ((bool) (UnityEngine.Object) current.GetComponent<FleetManager>())
        {
          FleetManager component = current.GetComponent<FleetManager>();
          if (component.GetComponent<Track>().factionID == 1)
          {
            FleetPortUI fleetPortUi = UnityEngine.Object.Instantiate<FleetPortUI>(this.fleetPortPF, this.portList);
            fleetPortUi.SetFleet(component);
            if (flag)
            {
              fleetPortUi.SelectThisFleet(true);
              fleetPortUi.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
              flag = false;
            }
            foreach (S_Ship ship in component.ships)
              UnityEngine.Object.Instantiate<ShipVicUI>(this.shipPortPF, fleetPortUi.transform).GetComponent<ShipVicUI>().SetShip(ship);
          }
        }
      }
    }
    finally
    {
      if (enumerator2 is IDisposable disposable)
        disposable.Dispose();
    }
  }

  private void UpdateAllFleetData() => this.UpdateFleetFuelData();

  private void UpdateAllStationData()
  {
    this.UpdateStationRepairData();
    this.UpdateStationResupplyData();
    this.UpdateStationFuelData();
  }

  public void NewFleet()
  {
    if (string.IsNullOrEmpty(this.fleetNameInput.text) || this.fleetNameInput.text == "" || this.fleetNameInput.text == " ")
      return;
    this.NewFleetCustom(this.fleetNameInput.text);
    this.fleetNameInput.text = "";
  }

  private IEnumerator DelayedRefresh()
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.Refresh();
  }

  public void SelectFleet(FleetManager newFleet)
  {
    this.fleet = newFleet;
    if (!(bool) (UnityEngine.Object) newFleet)
    {
      this.NoFleet();
    }
    else
    {
      this.UpdateAllStationData();
      string fullName = this.fleet.GetComponent<Track>().GetFullName();
      this.repairFleetText.text = fullName;
      this.fleetRepairToggle.gameObject.SetActive(true);
      this.resupplyFleetText.text = fullName;
      this.fleetResupplyToggle.gameObject.SetActive(true);
      this.refuelFleetText.text = fullName;
      this.fleetRefuelToggle.gameObject.SetActive(true);
      this.UpdateAllFleetData();
      this.ClearLists();
      this.PopulateLists();
    }
  }

  private void ClearLists()
  {
    IEnumerator enumerator1 = (IEnumerator) this.shipsListRepair.GetEnumerator();
    try
    {
      while (enumerator1.MoveNext())
        UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator1.Current).gameObject);
    }
    finally
    {
      if (enumerator1 is IDisposable disposable)
        disposable.Dispose();
    }
    IEnumerator enumerator2 = (IEnumerator) this.shipsListRefuel.GetEnumerator();
    try
    {
      while (enumerator2.MoveNext())
        UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator2.Current).gameObject);
    }
    finally
    {
      if (enumerator2 is IDisposable disposable)
        disposable.Dispose();
    }
    IEnumerator enumerator3 = (IEnumerator) this.shipsListResupply.GetEnumerator();
    try
    {
      while (enumerator3.MoveNext())
        UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator3.Current).gameObject);
    }
    finally
    {
      if (enumerator3 is IDisposable disposable)
        disposable.Dispose();
    }
  }

  private void PopulateLists()
  {
    foreach (S_Ship ship in this.fleet.ships)
    {
      UnityEngine.Object.Instantiate<ShipModulesCard>(this.shipModulesPF, this.shipsListRepair).Setup(ship);
      UnityEngine.Object.Instantiate<ShipModulesCard>(this.shipMunitionsPF, this.shipsListResupply).Setup(ship);
      UnityEngine.Object.Instantiate<ShipFuelUI>(this.shipFuelPF, this.shipsListRefuel).SetOn(ship);
    }
  }

  private void NoFleet()
  {
    this.UpdateStationFuelData();
    this.repairFleetText.text = "";
    this.fleetRepairToggle.gameObject.SetActive(false);
    this.resupplyFleetText.text = "";
    this.fleetResupplyToggle.gameObject.SetActive(false);
    this.refuelFleetText.text = "";
    this.fleetRefuelToggle.gameObject.SetActive(false);
    this.UpdateFleetFuelData();
    this.ClearLists();
  }

  public void SetShipToTransfer(Transform newShipToTransfer)
  {
    this.shipToTransfer = newShipToTransfer;
  }

  public void OnClickTransferShipButton(Transform newShipToTransfer)
  {
    if (this.station.GetComponent<Attachments>().attachedFleets.Count < 2)
      return;
    this.TryTransferShip(newShipToTransfer);
  }

  public void TryTransferShip(Transform newShipToTransfer)
  {
    MonoBehaviour.print((object) ("try transfer ship: " + newShipToTransfer.name));
    this.shipToTransfer = newShipToTransfer;
    IEnumerator enumerator = (IEnumerator) this.shipTransferList.GetEnumerator();
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
    foreach (FleetManager attachedFleet in this.station.GetComponent<Attachments>().attachedFleets)
    {
      if (attachedFleet.GetComponent<Track>().factionID == 1)
        UnityEngine.Object.Instantiate<TransferFleetButton>(this.transferFleetButtonPF, this.shipTransferList).GetComponent<TransferFleetButton>().SetFleet(attachedFleet);
    }
    this.StartCoroutine((IEnumerator) this.DelayTransferActive());
  }

  private IEnumerator DelayTransferActive()
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.shipTransferPanel.SetActive(true);
  }

  public void TransferToThisFleet(FleetManager fleetToTransferTo)
  {
    if (!(bool) (UnityEngine.Object) fleetToTransferTo || !(bool) (UnityEngine.Object) this.shipToTransfer)
      this.FleetTransferEnd();
    else if (fleetToTransferTo.ships.Count >= 10)
    {
      this.FleetTransferEnd();
    }
    else
    {
      FleetManager fleetManager = (FleetManager) null;
      if ((bool) (UnityEngine.Object) this.shipToTransfer.transform.parent && (bool) (UnityEngine.Object) this.shipToTransfer.transform.parent.GetComponent<FleetManager>())
        fleetManager = this.shipToTransfer.transform.parent.GetComponent<FleetManager>();
      this.shipToTransfer.transform.parent = fleetToTransferTo.transform;
      if ((bool) (UnityEngine.Object) fleetManager)
        fleetManager.UpdateFleet();
      fleetToTransferTo.UpdateFleet();
      this.FleetTransferEnd();
    }
  }

  private void FleetTransferEnd()
  {
    this.Refresh();
    this.shipTransferPanel.SetActive(false);
    this.fps.UpdateData();
  }

  private void UpdateFleetRepairData()
  {
  }

  private void UpdateStationRepairData()
  {
    if (!(bool) (UnityEngine.Object) this.market)
      return;
    float quantityAvailable1 = this.market.GetQuantityAvailable(this.metals);
    float num1 = (float) Mathf.RoundToInt(this.market.GetTotalQuantity());
    float percentage1 = Mathf.Clamp(quantityAvailable1 / num1, 0.0f, 1f);
    float quantityAvailable2 = this.market.GetQuantityAvailable(this.rareEarths);
    float num2 = (float) Mathf.RoundToInt(this.market.GetTotalQuantity());
    float percentage2 = Mathf.Clamp(quantityAvailable2 / num2, 0.0f, 1f);
    float quantityAvailable3 = this.market.GetQuantityAvailable(this.superalloys);
    float num3 = (float) Mathf.RoundToInt(this.market.GetTotalQuantity());
    float percentage3 = Mathf.Clamp(quantityAvailable3 / num3, 0.0f, 1f);
    float quantityAvailable4 = this.market.GetQuantityAvailable(this.exoticMatter);
    float num4 = (float) Mathf.RoundToInt(this.market.GetTotalQuantity());
    float percentage4 = Mathf.Clamp(quantityAvailable4 / num4, 0.0f, 1f);
    this.metalsPriceText.text = this.market.GetCurrentPrice(this.metals, true).ToString() + " cr/t";
    this.metalsBarText.text = ((int) quantityAvailable1).ToString() + "t";
    this.metalsBar.SetBarSize(percentage1);
    this.rareEarthsPriceText.text = this.market.GetCurrentPrice(this.rareEarths, true).ToString() + " cr/t";
    this.rareEarthsBarText.text = ((int) quantityAvailable2).ToString() + "t";
    this.rareEarthsBar.SetBarSize(percentage2);
    this.superalloysPriceText.text = this.market.GetCurrentPrice(this.superalloys, true).ToString() + " cr/t";
    this.superalloysBarText.text = ((int) quantityAvailable3).ToString() + "t";
    this.superalloysBar.SetBarSize(percentage3);
    this.exoticMatterPriceText.text = this.market.GetCurrentPrice(this.exoticMatter, true).ToString() + " cr/t";
    this.exoticMatterBarText.text = ((int) quantityAvailable4).ToString() + "t";
    this.exoticMatterBar.SetBarSize(percentage4);
  }

  public void SetCurrentFleetRepair(bool isRepair)
  {
    MonoBehaviour.print((object) "set current fleet repair");
    this.fleet.isRepairing = isRepair;
    this.fleet.SetStateTo(FleetState.REPAIRING);
    this.fleet.station = this.station;
  }

  private void UpdateStationResupplyData()
  {
    float quantityAvailable1 = this.market.GetQuantityAvailable(this.bMunitions);
    float num1 = (float) Mathf.RoundToInt(this.market.GetTotalQuantity());
    float percentage1 = Mathf.Clamp(quantityAvailable1 / num1, 0.0f, 1f);
    float quantityAvailable2 = this.market.GetQuantityAvailable(this.aMunitions);
    float num2 = (float) Mathf.RoundToInt(this.market.GetTotalQuantity());
    float percentage2 = Mathf.Clamp(quantityAvailable2 / num2, 0.0f, 1f);
    float quantityAvailable3 = this.market.GetQuantityAvailable(this.eMunitions);
    float num3 = (float) Mathf.RoundToInt(this.market.GetTotalQuantity());
    float percentage3 = Mathf.Clamp(quantityAvailable3 / num3, 0.0f, 1f);
    this.bMunitionsPriceText.text = this.market.GetCurrentPrice(this.bMunitions, true).ToString() + " cr/t";
    this.bMunitionsBarText.text = ((int) quantityAvailable1).ToString() + "t";
    this.bMunitionsBar.SetBarSize(percentage1);
    this.aMunitionsPriceText.text = this.market.GetCurrentPrice(this.aMunitions, true).ToString() + " cr/t";
    this.aMunitionsBarText.text = ((int) quantityAvailable2).ToString() + "t";
    this.aMunitionsBar.SetBarSize(percentage2);
    this.eMunitionsPriceText.text = this.market.GetCurrentPrice(this.eMunitions, true).ToString() + " cr/t";
    this.eMunitionsBarText.text = ((int) quantityAvailable3).ToString() + "t";
    this.eMunitionsBar.SetBarSize(percentage3);
  }

  public void SetCurrentFleetResupply(bool isResupply)
  {
    MonoBehaviour.print((object) "set current fleet resupply");
    this.fleet.isResupplying = isResupply;
    this.fleet.SetStateTo(FleetState.RESUPPLYING);
    this.fleet.station = this.station;
  }

  private void UpdateFleetFuelData()
  {
    if (!(bool) (UnityEngine.Object) this.fleet)
    {
      this.fleetDvText.text = "";
      this.fleetDvBar.SetBarSize(0.0f);
    }
    else
    {
      float fleetDv = this.fleet.fleetDv;
      float fleetDvMax = this.fleet.fleetDvMax;
      this.fleetDvText.text = $"Dv: {Mathf.RoundToInt(fleetDv).ToString("#,0")}/{fleetDvMax.ToString("#,0")} km/s";
      this.fleetDvBar.SetBarSize(fleetDv / fleetDvMax);
    }
  }

  private void UpdateStationFuelData()
  {
    float quantityAvailable1 = this.market.GetQuantityAvailable(this.tFuel);
    float num1 = (float) Mathf.RoundToInt(this.market.GetTotalQuantity());
    float percentage1 = Mathf.Clamp(quantityAvailable1 / num1, 0.0f, 1f);
    float quantityAvailable2 = this.market.GetQuantityAvailable(this.hFuel);
    float num2 = (float) Mathf.RoundToInt(this.market.GetTotalQuantity());
    float percentage2 = Mathf.Clamp(quantityAvailable2 / num2, 0.0f, 1f);
    float quantityAvailable3 = this.market.GetQuantityAvailable(this.aFuel);
    float num3 = (float) Mathf.RoundToInt(this.market.GetTotalQuantity());
    float percentage3 = Mathf.Clamp(quantityAvailable3 / num3, 0.0f, 1f);
    this.tFuelPriceText.text = this.market.GetCurrentPrice(this.tFuel, true).ToString() + " cr/t";
    this.tFuelBarText.text = ((int) quantityAvailable1).ToString() + "t";
    this.tFuelBar.SetBarSize(percentage1);
    this.hFuelPriceText.text = this.market.GetCurrentPrice(this.hFuel, true).ToString() + " cr/t";
    this.hFuelBarText.text = ((int) quantityAvailable2).ToString() + "t";
    this.hFuelBar.SetBarSize(percentage2);
    this.aFuelPriceText.text = this.market.GetCurrentPrice(this.aFuel, true).ToString() + " cr/t";
    this.aFuelBarText.text = ((int) quantityAvailable3).ToString() + "t";
    this.aFuelBar.SetBarSize(percentage3);
  }

  public void SetCurrentFleetRefuel(bool isRefuel)
  {
    MonoBehaviour.print((object) "set current fleet refuel");
    this.fleet.isRefueling = isRefuel;
    this.fleet.SetStateTo(FleetState.REFUELING);
    this.fleet.station = this.station;
  }

  public void SetOn(StationManager newStation)
  {
    this.harborPanel.SetActive(true);
    this.station = newStation;
    this.market = this.station.GetComponent<Market>();
    this.refuelRateText.text = $"REFUEL RATE: {this.market.resupplyRatePerHour.ToString()}t/day";
    this.Refresh();
  }

  public void SetOff()
  {
    if (!this.harborPanel.activeSelf)
      return;
    this.DeleteEmptyFleets();
    this.harborPanel.SetActive(false);
    this.ClearLists();
    this.tm.timeScale = 0.01f;
  }

  private void DeleteEmptyFleets()
  {
    if (!(bool) (UnityEngine.Object) this.station)
      return;
    foreach (FleetManager attachedFleet in this.station.GetComponent<Attachments>().attachedFleets)
    {
      if (attachedFleet.ships.Count == 0)
      {
        IEnumerator enumerator = (IEnumerator) attachedFleet.transform.GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            Transform current = (Transform) enumerator.Current;
            if ((bool) (UnityEngine.Object) current.GetComponent<Camera>())
              current.transform.parent = (Transform) null;
          }
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
        UnityEngine.Object.Destroy((UnityEngine.Object) attachedFleet.gameObject);
      }
    }
    this.StartCoroutine((IEnumerator) this.DelayAttachmentRefresh());
  }

  private IEnumerator DelayAttachmentRefresh()
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.station.GetComponent<Attachments>().RefreshList();
    this.station = (StationManager) null;
  }

  public bool HasPlayerFleet()
  {
    if (!(bool) (UnityEngine.Object) this.station)
      return false;
    IEnumerator enumerator = (IEnumerator) this.station.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if ((bool) (UnityEngine.Object) current.GetComponent<Track>() && current.GetComponent<Track>().factionID == 1 && (bool) (UnityEngine.Object) current.GetComponent<FleetManager>())
          return true;
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    return false;
  }

  public FleetManager GetPlayerFleet()
  {
    if (!(bool) (UnityEngine.Object) this.station)
      return (FleetManager) null;
    IEnumerator enumerator1 = (IEnumerator) this.station.transform.GetEnumerator();
    try
    {
      while (enumerator1.MoveNext())
      {
        Transform current = (Transform) enumerator1.Current;
        if ((bool) (UnityEngine.Object) current.GetComponent<Track>() && current.GetComponent<Track>().factionID == 1 && (bool) (UnityEngine.Object) current.GetComponent<FleetManager>() && current.GetComponent<FleetManager>().ships.Count < 10)
          return current.GetComponent<FleetManager>();
      }
    }
    finally
    {
      if (enumerator1 is IDisposable disposable)
        disposable.Dispose();
    }
    IEnumerator enumerator2 = (IEnumerator) this.station.transform.GetEnumerator();
    try
    {
      while (enumerator2.MoveNext())
      {
        Transform current = (Transform) enumerator2.Current;
        if ((bool) (UnityEngine.Object) current.GetComponent<Track>() && current.GetComponent<Track>().factionID == 1 && (bool) (UnityEngine.Object) current.GetComponent<FleetManager>())
          return current.GetComponent<FleetManager>();
      }
    }
    finally
    {
      if (enumerator2 is IDisposable disposable)
        disposable.Dispose();
    }
    return (FleetManager) null;
  }

  public FleetManager NewFleetCustom(string newName)
  {
    FleetManager component = UnityEngine.Object.Instantiate<GameObject>(this.fleetPF).GetComponent<FleetManager>();
    component.gameObject.SetActive(true);
    component.GetComponent<Track>().SetName(newName);
    component.GetComponent<Track>().factionID = 1;
    component.transform.SetParent(this.station.transform);
    this.station.GetComponent<Attachments>().RefreshList();
    this.StartCoroutine((IEnumerator) this.DelayedRefresh());
    MonoBehaviour.print((object) "new fleet finished");
    return component;
  }
}
