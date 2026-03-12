// Decompiled with JetBrains decompiler
// Type: PartsLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "PartsLibrary", menuName = "ScriptableObjects/PartsLibrary")]
public class PartsLibrary : ScriptableObject
{
  [Header("MODULES")]
  public List<PartsLibrary.PrefabEntry> drivePrefabs = new List<PartsLibrary.PrefabEntry>();
  public List<PartsLibrary.PrefabEntry> heatsinkPrefabs = new List<PartsLibrary.PrefabEntry>();
  public List<PartsLibrary.PrefabEntry> cargoPrefabs = new List<PartsLibrary.PrefabEntry>();
  public List<PartsLibrary.PrefabEntry> crewPrefabs = new List<PartsLibrary.PrefabEntry>();
  public List<PartsLibrary.PrefabEntry> sensorsPrefabs = new List<PartsLibrary.PrefabEntry>();
  public List<PartsLibrary.PrefabEntry> weaponPrefabs = new List<PartsLibrary.PrefabEntry>();
  public List<PartsLibrary.PrefabEntry> nosePrefabs = new List<PartsLibrary.PrefabEntry>();
  private List<List<PartsLibrary.PrefabEntry>> moduleLists = new List<List<PartsLibrary.PrefabEntry>>();
  [Header("MOUNTS")]
  public List<PartsLibrary.PrefabEntry> radiatorPrefabs = new List<PartsLibrary.PrefabEntry>();
  public List<PartsLibrary.PrefabEntry> kineticsPrefabs = new List<PartsLibrary.PrefabEntry>();
  public List<PartsLibrary.PrefabEntry> missileSystemPrefabs = new List<PartsLibrary.PrefabEntry>();
  private List<List<PartsLibrary.PrefabEntry>> mountLists = new List<List<PartsLibrary.PrefabEntry>>();
  public Dictionary<string, PartPrefabData> prefabs;

  public PartPrefabData GetPart(string key)
  {
    if (this.prefabs == null)
      this.PopulatePrefabsList();
    if (this.prefabs.ContainsKey(key))
      return this.prefabs[key];
    Debug.Log((object) $"PL: {key} not found!");
    return (PartPrefabData) null;
  }

  private void PopulatePrefabsList()
  {
    this.prefabs = new Dictionary<string, PartPrefabData>();
    foreach (PartsLibrary.PrefabEntry drivePrefab in this.drivePrefabs)
    {
      if (!this.prefabs.ContainsKey(drivePrefab.key))
        this.prefabs.Add(drivePrefab.key, drivePrefab.mpd);
    }
    foreach (PartsLibrary.PrefabEntry heatsinkPrefab in this.heatsinkPrefabs)
    {
      if (!this.prefabs.ContainsKey(heatsinkPrefab.key))
        this.prefabs.Add(heatsinkPrefab.key, heatsinkPrefab.mpd);
    }
    foreach (PartsLibrary.PrefabEntry cargoPrefab in this.cargoPrefabs)
    {
      if (!this.prefabs.ContainsKey(cargoPrefab.key))
        this.prefabs.Add(cargoPrefab.key, cargoPrefab.mpd);
    }
    foreach (PartsLibrary.PrefabEntry crewPrefab in this.crewPrefabs)
    {
      if (!this.prefabs.ContainsKey(crewPrefab.key))
        this.prefabs.Add(crewPrefab.key, crewPrefab.mpd);
    }
    foreach (PartsLibrary.PrefabEntry sensorsPrefab in this.sensorsPrefabs)
    {
      if (!this.prefabs.ContainsKey(sensorsPrefab.key))
        this.prefabs.Add(sensorsPrefab.key, sensorsPrefab.mpd);
    }
    foreach (PartsLibrary.PrefabEntry weaponPrefab in this.weaponPrefabs)
    {
      if (!this.prefabs.ContainsKey(weaponPrefab.key))
        this.prefabs.Add(weaponPrefab.key, weaponPrefab.mpd);
    }
    foreach (PartsLibrary.PrefabEntry nosePrefab in this.nosePrefabs)
    {
      if (!this.prefabs.ContainsKey(nosePrefab.key))
        this.prefabs.Add(nosePrefab.key, nosePrefab.mpd);
    }
    foreach (PartsLibrary.PrefabEntry radiatorPrefab in this.radiatorPrefabs)
    {
      if (!this.prefabs.ContainsKey(radiatorPrefab.key))
        this.prefabs.Add(radiatorPrefab.key, radiatorPrefab.mpd);
    }
    foreach (PartsLibrary.PrefabEntry kineticsPrefab in this.kineticsPrefabs)
    {
      if (!this.prefabs.ContainsKey(kineticsPrefab.key))
        this.prefabs.Add(kineticsPrefab.key, kineticsPrefab.mpd);
    }
    foreach (PartsLibrary.PrefabEntry missileSystemPrefab in this.missileSystemPrefabs)
    {
      if (!this.prefabs.ContainsKey(missileSystemPrefab.key))
        this.prefabs.Add(missileSystemPrefab.key, missileSystemPrefab.mpd);
    }
  }

  [Serializable]
  public class PrefabEntry
  {
    public string key;
    public PartPrefabData mpd;
  }
}
