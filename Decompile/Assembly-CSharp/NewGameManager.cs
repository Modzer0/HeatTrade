// Decompiled with JetBrains decompiler
// Type: NewGameManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class NewGameManager : MonoBehaviour
{
  public static NewGameManager current;
  private FactionsManager fm;
  private ShipSpawner ss;
  private SaveLoadSystem sls;
  [SerializeField]
  private TrackList tl;
  private bool isInit;
  [SerializeField]
  private GameObject fleetPF;
  [SerializeField]
  private uiAnimator coverUIA;

  private void Awake() => NewGameManager.current = this;

  private void Start()
  {
    this.Init();
    this.StartCoroutine((IEnumerator) this.DelayCoverFade());
  }

  private void Init()
  {
    if (this.isInit)
      return;
    this.fm = FactionsManager.current;
    this.ss = ShipSpawner.current;
    this.sls = SaveLoadSystem.current;
    this.isInit = true;
  }

  private void Setup(bool isPrometheus)
  {
    this.Init();
    if (isPrometheus)
      return;
    this.SetupNonPrometheus();
  }

  private void SetupNonPrometheus()
  {
    MonoBehaviour.print((object) "SETUP NON PROMETHEUS");
    AtlasFleetTimer objectOfType = Object.FindObjectOfType<AtlasFleetTimer>();
    objectOfType.enabled = false;
    objectOfType.gameObject.SetActive(false);
    foreach (FleetManager fleetManager in Object.FindObjectsOfType<FleetManager>())
    {
      if (fleetManager.name == "FLEET - Prometheus")
        Object.Destroy((Object) fleetManager.gameObject);
    }
    if (this.fm.playerBackground.startingShips.Count <= 0)
      return;
    FleetManager component = Object.Instantiate<GameObject>(this.fleetPF).GetComponent<FleetManager>();
    component.gameObject.SetActive(true);
    component.GetComponent<Track>().SetName("Main");
    component.GetComponent<Track>().factionID = 1;
    foreach (S_Ship startingShip in this.fm.playerBackground.startingShips)
      this.ss.SpawnShip(startingShip.bp.PrefabKey, 1, startingShip.bp.ClassName).transform.SetParent(component.transform);
    component.UpdateFleet();
  }

  public void SetupNewGame(bool isPrometheus)
  {
    MonoBehaviour.print((object) ("SETUP NEW GAME. is prometheus: " + isPrometheus.ToString()));
    this.Setup(isPrometheus);
  }

  public void SetupLoadGame(bool isPrometheus, string saveName)
  {
    MonoBehaviour.print((object) ("SETUP LOAD GAME. is prometheus: " + isPrometheus.ToString()));
    this.Setup(isPrometheus);
    this.StartCoroutine((IEnumerator) this.DelayLoadGame(saveName));
  }

  public void SetupAutosave(bool isPrometheus, string saveName)
  {
    MonoBehaviour.print((object) ("SETUP AUTOSAVE. is prometheus: " + isPrometheus.ToString()));
    this.Setup(isPrometheus);
  }

  public IEnumerator DelayLoadGame(string saveName)
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.sls.LoadGame(saveName);
    this.tl.RefreshList();
  }

  private IEnumerator DelayCoverFade()
  {
    yield return (object) new WaitForSeconds(0.2f);
    this.CoverFade();
  }

  private void CoverFade() => this.coverUIA.Hide();
}
