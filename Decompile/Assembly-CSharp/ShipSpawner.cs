// Decompiled with JetBrains decompiler
// Type: ShipSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ShipSpawner : MonoBehaviour
{
  public static ShipSpawner current;
  private TrackDisplayer td;
  [SerializeField]
  private BlueprintLibrary bl;
  [SerializeField]
  private List<ShipBP> shipBPs = new List<ShipBP>();
  [SerializeField]
  private S_Ship shipPF;
  [SerializeField]
  private S_Module2 modulePF;
  [SerializeField]
  private S_Mount2 mountPF;
  private bool isInit;

  public List<ShipBP> ShipBPs => this.shipBPs;

  private void Awake() => ShipSpawner.current = this;

  private void Start() => this.Init();

  private void Init()
  {
    if (this.isInit)
      return;
    this.td = TrackDisplayer.current;
    this.shipBPs.Clear();
    foreach (ShipBP shipBlueprint in this.bl.shipBlueprints)
      this.shipBPs.Add(shipBlueprint);
    this.isInit = true;
  }

  public S_Ship SpawnShip(string toSpawnKey, int factionID, string newName)
  {
    this.Init();
    ShipBP newBP = (ShipBP) null;
    foreach (ShipBP shipBp in this.shipBPs)
    {
      if (shipBp.PrefabKey == toSpawnKey)
      {
        newBP = shipBp;
        break;
      }
    }
    if ((Object) newBP == (Object) null)
    {
      Debug.LogError((object) $"SHIP KEY: {toSpawnKey} NOT FOUND!");
      return (S_Ship) null;
    }
    S_Ship spawnedShip = Object.Instantiate<S_Ship>(this.shipPF);
    spawnedShip.NewShip(newBP, factionID, this.td.GetTrackNumber(), newName);
    foreach (ModuleBP module in newBP.Modules)
    {
      S_Module2 sModule2 = Object.Instantiate<S_Module2>(this.modulePF, spawnedShip.transform);
      sModule2.NewModule(module);
      if (module is ICargo)
      {
        sModule2.gameObject.AddComponent<ResourceInventory>();
        sModule2.GetComponent<ResourceInventory>().InitResources();
      }
      foreach (MountBP mount in module.Mounts)
      {
        S_Mount2 sMount2 = Object.Instantiate<S_Mount2>(this.mountPF, sModule2.transform);
        sMount2.NewMount(mount);
        sModule2.mounts.Add(sMount2);
      }
    }
    spawnedShip.Init();
    spawnedShip.FullResupply();
    this.StartCoroutine((IEnumerator) this.DelayedFullRefuel(spawnedShip));
    return spawnedShip;
  }

  private IEnumerator DelayedFullRefuel(S_Ship spawnedShip)
  {
    yield return (object) new WaitForSeconds(0.1f);
    spawnedShip.AddFuel(spawnedShip.GetFuelMax());
  }
}
