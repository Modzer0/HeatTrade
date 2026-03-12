// Decompiled with JetBrains decompiler
// Type: S_StructureLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "S_StructureLibrary", menuName = "ScriptableObjects/S_StructureLibrary")]
public class S_StructureLibrary : ScriptableObject
{
  public List<PrefabEntry> strategicPrefabs = new List<PrefabEntry>();
  [SerializeField]
  private Dictionary<ShipBP, GameObject> prefabs;

  private void InitPrefabs()
  {
    this.prefabs = new Dictionary<ShipBP, GameObject>();
    foreach (PrefabEntry strategicPrefab in this.strategicPrefabs)
    {
      if (!this.prefabs.ContainsKey(strategicPrefab.bp))
        this.prefabs.Add(strategicPrefab.bp, strategicPrefab.prefab);
    }
  }

  public GameObject GetPrefab(ShipBP newBP)
  {
    if (this.prefabs == null)
      this.InitPrefabs();
    if (this.prefabs.ContainsKey(newBP))
      return this.prefabs[newBP];
    Debug.LogWarning((object) $"Prefab with bp '{newBP}' not found.");
    return (GameObject) null;
  }
}
