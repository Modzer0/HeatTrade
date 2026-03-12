// Decompiled with JetBrains decompiler
// Type: FleetManagerUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class FleetManagerUI : MonoBehaviour
{
  public static FleetManagerUI current;
  private MapInputs mi;
  private SelectedPanelManager spm;
  private FleetManager fleet1;
  private FleetManager fleet2;
  [Header("UI")]
  [SerializeField]
  private FleetManager rootFleet;
  [SerializeField]
  private GameObject fleetManagerPanel;
  [SerializeField]
  private Transform fleetList1;
  [SerializeField]
  private Transform fleetList2;
  [SerializeField]
  private Transform shipList1;
  [SerializeField]
  private Transform shipList2;
  [SerializeField]
  private FleetNearbyUI fleetNearbyUIPF1;
  [SerializeField]
  private FleetNearbyUI fleetNearbyUIPF2;
  [SerializeField]
  private ShipTransferUI shipTransferUIPF1;
  [SerializeField]
  private ShipTransferUI shipTransferUIPF2;
  [SerializeField]
  private GameObject fleetPF;
  [SerializeField]
  private TMP_InputField fleetNameInput;

  private void Awake() => FleetManagerUI.current = this;

  private void Start()
  {
    this.mi = MapInputs.current;
    this.spm = SelectedPanelManager.current;
  }

  public void SelectThisFleet(FleetManager newFleet, int panelIndex)
  {
    if (panelIndex == 1)
    {
      this.fleet1 = newFleet;
      this.UpdateShipList(this.shipList1, this.fleet1, this.shipTransferUIPF1);
    }
    else
    {
      if (panelIndex != 2)
        return;
      this.fleet2 = newFleet;
      this.UpdateShipList(this.shipList2, this.fleet2, this.shipTransferUIPF2);
    }
  }

  public void NewFleet()
  {
    if (string.IsNullOrEmpty(this.fleetNameInput.text) || this.fleetNameInput.text == "" || this.fleetNameInput.text == " ")
      return;
    FleetManager component = UnityEngine.Object.Instantiate<GameObject>(this.fleetPF).GetComponent<FleetManager>();
    component.gameObject.SetActive(true);
    component.GetComponent<Track>().SetName(this.fleetNameInput.text);
    component.GetComponent<Track>().factionID = 1;
    component.transform.SetParent(this.rootFleet.transform);
    this.StartCoroutine((IEnumerator) this.DelayedNewFleet());
    MonoBehaviour.print((object) "new fleet finished");
  }

  private IEnumerator DelayedNewFleet()
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.UpdateFleetLists();
  }

  private void UpdateFleetLists()
  {
    this.rootFleet.GetComponent<Attachments>().RefreshList();
    this.ClearFleetLists();
    this.PopulateFleetLists();
    this.UpdateFleetToggles();
  }

  private void ClearFleetLists()
  {
    IEnumerator enumerator1 = (IEnumerator) this.fleetList1.GetEnumerator();
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
    IEnumerator enumerator2 = (IEnumerator) this.fleetList2.GetEnumerator();
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
  }

  private void PopulateFleetLists()
  {
    this.AddFleet(this.rootFleet);
    foreach (FleetManager attachedFleet in this.rootFleet.GetComponent<Attachments>().attachedFleets)
      this.AddFleet(attachedFleet);
  }

  private void AddFleet(FleetManager fleet)
  {
    UnityEngine.Object.Instantiate<FleetNearbyUI>(this.fleetNearbyUIPF1, this.fleetList1).Setup(fleet);
    UnityEngine.Object.Instantiate<FleetNearbyUI>(this.fleetNearbyUIPF2, this.fleetList2).Setup(fleet);
  }

  private void UpdateFleetToggles()
  {
    IEnumerator enumerator1 = (IEnumerator) this.fleetList1.GetEnumerator();
    try
    {
      while (enumerator1.MoveNext())
      {
        FleetNearbyUI component = ((Component) enumerator1.Current).GetComponent<FleetNearbyUI>();
        if ((bool) (UnityEngine.Object) component && (UnityEngine.Object) component.fm == (UnityEngine.Object) this.fleet1)
          component.SetToggle(true);
        else
          component.SetToggle(false);
      }
    }
    finally
    {
      if (enumerator1 is IDisposable disposable)
        disposable.Dispose();
    }
    IEnumerator enumerator2 = (IEnumerator) this.fleetList2.GetEnumerator();
    try
    {
      while (enumerator2.MoveNext())
      {
        FleetNearbyUI component = ((Component) enumerator2.Current).GetComponent<FleetNearbyUI>();
        if ((bool) (UnityEngine.Object) component && (UnityEngine.Object) component.fm == (UnityEngine.Object) this.fleet2)
          component.SetToggle(true);
        else
          component.SetToggle(false);
      }
    }
    finally
    {
      if (enumerator2 is IDisposable disposable)
        disposable.Dispose();
    }
  }

  private void UpdateShipLists()
  {
    this.UpdateShipList(this.shipList1, this.fleet1, this.shipTransferUIPF1);
    this.UpdateShipList(this.shipList2, this.fleet2, this.shipTransferUIPF2);
  }

  private void UpdateShipList(Transform list, FleetManager fleet, ShipTransferUI stuipf)
  {
    if (!(bool) (UnityEngine.Object) fleet)
      return;
    IEnumerator enumerator = (IEnumerator) list.GetEnumerator();
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
    foreach (S_Ship ship in fleet.ships)
      UnityEngine.Object.Instantiate<ShipTransferUI>(stuipf, list).Setup(ship);
  }

  public void TransferShip(S_Ship shipToTransfer, int listIndex)
  {
    if (listIndex == 1 && (bool) (UnityEngine.Object) this.fleet2)
      this.AddShipToFleet(shipToTransfer, this.fleet2);
    else if (listIndex == 2 && (bool) (UnityEngine.Object) this.fleet1)
      this.AddShipToFleet(shipToTransfer, this.fleet1);
    this.UpdateShipLists();
  }

  private void AddShipToFleet(S_Ship shipToTransfer, FleetManager fleet)
  {
    if (fleet.ships.Count >= 10)
      return;
    FleetManager component = shipToTransfer.transform.parent.GetComponent<FleetManager>();
    shipToTransfer.transform.parent = fleet.transform;
    component.UpdateFleet();
    fleet.UpdateFleet();
  }

  public void On()
  {
    MonoBehaviour.print((object) "fmui on");
    if ((UnityEngine.Object) this.mi.selectedFleet == (UnityEngine.Object) null)
      return;
    this.fleetManagerPanel.SetActive(true);
    this.ClearFleetLists();
    this.rootFleet = !(bool) (UnityEngine.Object) this.mi.selectedFleet.transform.parent || !(bool) (UnityEngine.Object) this.mi.selectedFleet.transform.parent.GetComponent<Attachments>() ? this.mi.selectedFleet : this.mi.selectedFleet.transform.parent.GetComponent<FleetManager>();
    this.fleet1 = this.rootFleet;
    this.rootFleet.GetComponent<Attachments>().RefreshList();
    this.PopulateFleetLists();
    if (this.mi.selectedFleet.GetComponent<Attachments>().attachedFleets.Count > 0)
      this.fleet2 = this.mi.selectedFleet.GetComponent<Attachments>().attachedFleets[0];
    this.UpdateFleetToggles();
    this.UpdateShipLists();
  }

  public void Off()
  {
    MonoBehaviour.print((object) "fmui off");
    this.DeleteEmptyFleets();
    this.fleetManagerPanel.SetActive(false);
    this.spm.Refresh();
  }

  private IEnumerator DelayedRefresh()
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.rootFleet.GetComponent<Attachments>().RefreshList();
  }

  private void DeleteEmptyFleets()
  {
    MonoBehaviour.print((object) ("delete empty fleets in " + this.rootFleet.transform.name));
    List<GameObject> gameObjectList = new List<GameObject>();
    if ((bool) (UnityEngine.Object) this.rootFleet && this.rootFleet.ships.Count == 0)
    {
      MonoBehaviour.print((object) ("checking for empty fleets. total children: " + this.rootFleet.transform.childCount.ToString()));
      IEnumerator enumerator = (IEnumerator) this.rootFleet.transform.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          Transform current = (Transform) enumerator.Current;
          MonoBehaviour.print((object) ("checking child: " + current.name));
          if ((bool) (UnityEngine.Object) current.GetComponent<Camera>() || (bool) (UnityEngine.Object) current.GetComponent<FleetManager>())
          {
            MonoBehaviour.print((object) "child has camera or fleet");
            if ((bool) (UnityEngine.Object) current.GetComponent<Navigation>())
              current.GetComponent<Navigation>().currentState = NavigationState.Orbiting;
            current.SetParent((Transform) null);
            if ((bool) (UnityEngine.Object) current.GetComponent<Attachments>())
              current.GetComponent<Attachments>().CheckEnable();
          }
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      gameObjectList.Add(this.rootFleet.gameObject);
      MonoBehaviour.print((object) "finished checking");
    }
    IEnumerator enumerator1 = (IEnumerator) this.rootFleet.transform.GetEnumerator();
    try
    {
      while (enumerator1.MoveNext())
      {
        Transform current = (Transform) enumerator1.Current;
        if ((bool) (UnityEngine.Object) current.GetComponent<FleetManager>() && current.GetComponent<FleetManager>().ships.Count == 0)
          gameObjectList.Add(current.gameObject);
      }
    }
    finally
    {
      if (enumerator1 is IDisposable disposable)
        disposable.Dispose();
    }
    if (!gameObjectList.Contains(this.rootFleet.gameObject))
      this.StartCoroutine((IEnumerator) this.DelayedRefresh());
    foreach (GameObject gameObject in gameObjectList)
    {
      MonoBehaviour.print((object) ("destroying empty fleet: " + gameObject.name));
      IEnumerator enumerator2 = (IEnumerator) gameObject.transform.GetEnumerator();
      try
      {
        while (enumerator2.MoveNext())
        {
          Transform current = (Transform) enumerator2.Current;
          MonoBehaviour.print((object) ("child: " + current.name));
          if ((bool) (UnityEngine.Object) current.GetComponent<Camera>())
            current.SetParent((Transform) null);
        }
      }
      finally
      {
        if (enumerator2 is IDisposable disposable)
          disposable.Dispose();
      }
      UnityEngine.Object.Destroy((UnityEngine.Object) gameObject);
    }
  }
}
