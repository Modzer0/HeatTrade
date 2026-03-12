// Decompiled with JetBrains decompiler
// Type: CombatManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CombatManager : MonoBehaviour
{
  public static CombatManager current;
  [SerializeField]
  private TacticalSceneLoader tsl;
  private SceneTransitionManager stm;
  private TacticalInputs ti;
  private bool isCombatOver;
  private AudioManager am;
  public TacticalGroupData playerFleet = new TacticalGroupData();
  public TacticalGroupData hostileFleet = new TacticalGroupData();
  public List<ShipController> playerShips = new List<ShipController>();
  public List<ShipController> hostileShips = new List<ShipController>();
  private List<ShipController> deadShips = new List<ShipController>();
  [Header("UI")]
  [SerializeField]
  private ShipDataUI sduiPF;
  [SerializeField]
  private Transform playerShipsUnscathed;
  [SerializeField]
  private Transform playerShipsDamaged;
  [SerializeField]
  private Transform playerShipsDestroyed;
  [SerializeField]
  private Transform playerShipsDisintegrated;
  [SerializeField]
  private Transform hostileShipsUnscathed;
  [SerializeField]
  private Transform hostileShipsDamaged;
  [SerializeField]
  private Transform hostileShipsDestroyed;
  [SerializeField]
  private Transform hostileShipsDisintegrated;
  [SerializeField]
  private uiAnimator resultsScreen;

  private void Awake() => CombatManager.current = this;

  private void Start()
  {
    this.tsl = UnityEngine.Object.FindObjectOfType<TacticalSceneLoader>();
    this.stm = SceneTransitionManager.current;
    this.ti = TacticalInputs.current;
    this.am = AudioManager.current;
  }

  public void AddShip(bool isPlayerShip, ShipController sc)
  {
    if (isPlayerShip)
      this.playerShips.Add(sc);
    else
      this.hostileShips.Add(sc);
    sc.OnShipDeath += new Action<int, ShipController, bool>(this.OnShipDeath);
  }

  public void AddFleet(TacticalGroupData group)
  {
    if (group.factionID == 1)
      this.playerFleet = group;
    else
      this.hostileFleet = group;
  }

  private void OnShipDeath(int factionID, ShipController sc, bool isHardDeath)
  {
    if (this.deadShips.Contains(sc))
      return;
    if (factionID == 1)
      this.playerShips.Remove(sc);
    else
      this.hostileShips.Remove(sc);
    int count = this.playerShips.Count;
    string str1 = count.ToString();
    count = this.hostileShips.Count;
    string str2 = count.ToString();
    MonoBehaviour.print((object) $"player ships: {str1} hostile ships: {str2}");
    if (this.playerShips.Count == 0)
      this.StartCoroutine((IEnumerator) this.DelayCombatEnd(false));
    else if (this.hostileShips.Count == 0)
      this.StartCoroutine((IEnumerator) this.DelayCombatEnd(true));
    if (isHardDeath)
      this.ListShip(4, sc);
    else
      this.ListShip(3, sc);
    this.deadShips.Add(sc);
  }

  private IEnumerator DelayCombatEnd(bool isPlayerWin)
  {
    MonoBehaviour.print((object) ("delay combat end: " + isPlayerWin.ToString()));
    yield return (object) new WaitForSeconds(3f);
    this.CombatEndSetup(isPlayerWin);
  }

  private void CombatEndSetup(bool isPlayerWin)
  {
    MonoBehaviour.print((object) "===== combat end setup =====");
    if (this.isCombatOver)
      return;
    List<ShipController> shipControllerList;
    TacticalGroupData tacticalGroupData;
    if (isPlayerWin)
    {
      MonoBehaviour.print((object) "PLAYER WINS");
      shipControllerList = this.playerShips;
      tacticalGroupData = this.playerFleet;
    }
    else
    {
      MonoBehaviour.print((object) "PLAYER LOSES");
      shipControllerList = this.hostileShips;
      tacticalGroupData = this.hostileFleet;
    }
    List<TacticalShipData> tacticalShipDataList = new List<TacticalShipData>();
    foreach (TacticalShipData tacticalShipData in tacticalGroupData.objects)
    {
      MonoBehaviour.print((object) $"winning ship data: {tacticalShipData.publicName} trackID: {tacticalShipData.trackID}");
      bool flag = false;
      foreach (ShipController shipController in shipControllerList)
      {
        MonoBehaviour.print((object) $"======= TACTICAL SAVE: winning ship: {shipController.name} track: {shipController.track.id} shipData track: {tacticalShipData.trackID}");
        if (!((UnityEngine.Object) shipController == (UnityEngine.Object) null) && shipController.track.id == tacticalShipData.trackID)
        {
          MonoBehaviour.print((object) "still alive!");
          tacticalShipData.modules = shipController.GetTacticalModuleData();
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        tacticalShipDataList.Add(tacticalShipData);
        MonoBehaviour.print((object) ("dead ship: " + tacticalShipData.publicName));
      }
    }
    foreach (TacticalShipData tacticalShipData in tacticalShipDataList)
    {
      if (tacticalGroupData.objects.Contains(tacticalShipData))
        tacticalGroupData.objects.Remove(tacticalShipData);
    }
    this.stm.winningGroup = tacticalGroupData;
    foreach (ShipController sc in shipControllerList)
    {
      if ((double) sc.totalHealthRatio == 1.0)
        this.ListShip(1, sc);
      else
        this.ListShip(2, sc);
    }
    this.StartCoroutine((IEnumerator) this.DelayListRefresh());
    if (!this.ti.isHudOn)
      this.ti.ToggleHUD();
    this.resultsScreen.Show();
    this.am.PlaySFX(6);
    this.isCombatOver = true;
    this.StartCoroutine((IEnumerator) this.WaitForExit());
  }

  private IEnumerator DelayListRefresh()
  {
    this.SetAllLists(true);
    yield return (object) new WaitForSeconds(0.1f);
    this.SetAllLists(false);
    yield return (object) new WaitForSeconds(0.1f);
    this.SetAllLists(true);
  }

  private void SetAllLists(bool isOn)
  {
    this.playerShipsUnscathed.gameObject.SetActive(isOn);
    this.playerShipsDamaged.gameObject.SetActive(isOn);
    this.playerShipsDestroyed.gameObject.SetActive(isOn);
    this.playerShipsDisintegrated.gameObject.SetActive(isOn);
    this.hostileShipsUnscathed.gameObject.SetActive(isOn);
    this.hostileShipsDamaged.gameObject.SetActive(isOn);
    this.hostileShipsDestroyed.gameObject.SetActive(isOn);
    this.hostileShipsDisintegrated.gameObject.SetActive(isOn);
  }

  private void ListShip(int listIndex, ShipController sc)
  {
    int num = sc.track.factionID == 1 ? 1 : 0;
    Transform parent = (Transform) null;
    if (num != 0)
    {
      switch (listIndex)
      {
        case 1:
          parent = this.playerShipsUnscathed;
          break;
        case 2:
          parent = this.playerShipsDamaged;
          break;
        case 3:
          parent = this.playerShipsDestroyed;
          break;
        case 4:
          parent = this.playerShipsDisintegrated;
          break;
      }
    }
    else
    {
      switch (listIndex)
      {
        case 1:
          parent = this.hostileShipsUnscathed;
          break;
        case 2:
          parent = this.hostileShipsDamaged;
          break;
        case 3:
          parent = this.hostileShipsDestroyed;
          break;
        case 4:
          parent = this.hostileShipsDisintegrated;
          break;
      }
    }
    if (!(bool) (UnityEngine.Object) parent)
      return;
    UnityEngine.Object.Instantiate<ShipDataUI>(this.sduiPF, parent).SetShip(sc);
  }

  private IEnumerator WaitForExit()
  {
    while (!Input.GetKeyDown(KeyCode.Space))
      yield return (object) null;
    this.EndCombat();
  }

  public void EndCombat()
  {
    Time.timeScale = 1f;
    this.stm.EndCombat();
  }

  public void PlayerSurrender()
  {
    List<ShipController> shipControllerList = new List<ShipController>();
    foreach (ShipController playerShip in this.playerShips)
    {
      if (!playerShip.isDead)
        shipControllerList.Add(playerShip);
    }
    foreach (ShipController shipController in shipControllerList)
      shipController.SoftDeath();
  }
}
