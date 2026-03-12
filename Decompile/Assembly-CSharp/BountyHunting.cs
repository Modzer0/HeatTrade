// Decompiled with JetBrains decompiler
// Type: BountyHunting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BountyHunting : MonoBehaviour
{
  private ShipSpawner ss;
  private TrackDisplayer td;
  private SaveLoadSystem sls;
  [SerializeField]
  private Track earthTrack;
  [SerializeField]
  private Track lunaTrack;
  [SerializeField]
  private List<BountyFleet> bountyFleets = new List<BountyFleet>();
  [SerializeField]
  private GameObject fleetPF;
  private bool isSpawningBounties;
  [SerializeField]
  private List<FleetManager> spawnedFleets = new List<FleetManager>();
  [Header("UI")]
  [SerializeField]
  private GameObject bountyPanel;
  [SerializeField]
  private BountyButton bountyButtonPF;
  [SerializeField]
  private Transform bountyButtonsList;
  [Header("UI")]
  [SerializeField]
  private List<string> namesList = new List<string>();

  private void Start()
  {
    this.ss = ShipSpawner.current;
    this.td = TrackDisplayer.current;
    this.sls = SaveLoadSystem.current;
    this.sls.OnLoadGame += new Action(this.OnLoadGame);
    this.StartCoroutine((IEnumerator) this.SpawnBountiesRoutine());
  }

  private void OnLoadGame() => this.StartCoroutine((IEnumerator) this.SpawnBountiesRoutine());

  private IEnumerator SpawnBountiesRoutine()
  {
    if (!this.isSpawningBounties)
    {
      this.isSpawningBounties = true;
      yield return (object) new WaitForSeconds(1f);
      MonoBehaviour.print((object) "===== SPAWNING BOUNTIES =====");
      this.SpawnBounties();
      this.UpdateUI();
      this.isSpawningBounties = false;
    }
  }

  private void UpdateUI()
  {
    IEnumerator enumerator = (IEnumerator) this.bountyButtonsList.GetEnumerator();
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
    foreach (FleetManager spawnedFleet in this.spawnedFleets)
      UnityEngine.Object.Instantiate<BountyButton>(this.bountyButtonPF, this.bountyButtonsList).GetComponent<BountyButton>().Setup(spawnedFleet);
  }

  private void SpawnBounties()
  {
    this.spawnedFleets.Clear();
    int num = 5;
    for (int spawnIndex = 0; spawnIndex < num; ++spawnIndex)
      this.SpawnFleet(UnityEngine.Random.Range(0, this.bountyFleets.Count), spawnIndex);
  }

  private void SpawnFleet(int bountyIndex, int spawnIndex)
  {
    BountyFleet bountyFleet = this.bountyFleets[bountyIndex];
    FleetManager component1 = UnityEngine.Object.Instantiate<GameObject>(this.fleetPF).GetComponent<FleetManager>();
    this.spawnedFleets.Add(component1);
    Track track = this.earthTrack;
    if (UnityEngine.Random.Range(0, 2) == 1)
      track = this.lunaTrack;
    Track component2 = component1.GetComponent<Track>();
    component2.factionID = 99;
    component2.publicName = "BOUNTY " + spawnIndex.ToString();
    this.td.AddTrack(component2);
    component1.transform.name = "FLEET - " + component2.publicName;
    component1.gameObject.SetActive(true);
    Orbiter component3 = component1.GetComponent<Orbiter>();
    component3.parent = track.transform;
    component3.parentOrbiter = track.GetComponent<Orbiter>();
    component3.StartRandomOrbit();
    component1.state = FleetState.IDLE;
    for (int index = 0; index < bountyFleet.ships.Count; ++index)
    {
      string names = this.namesList[UnityEngine.Random.Range(0, this.namesList.Count)];
      S_Ship sShip = this.ss.SpawnShip(bountyFleet.ships[index].PrefabKey, 99, names);
      sShip.FullResupply();
      sShip.transform.parent = component1.transform;
      component1.ships.Add(sShip);
    }
    component1.UpdateDv();
  }
}
