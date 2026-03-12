// Decompiled with JetBrains decompiler
// Type: T_StructureLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "T_StructureLibrary", menuName = "ScriptableObjects/T_StructureLibrary")]
public class T_StructureLibrary : ScriptableObject
{
  public List<PrefabEntry> tacticalPrefabs = new List<PrefabEntry>();
  [SerializeField]
  private Dictionary<ShipBP, GameObject> prefabs;

  private void InitPrefabs()
  {
    this.prefabs = new Dictionary<ShipBP, GameObject>();
    foreach (PrefabEntry tacticalPrefab in this.tacticalPrefabs)
    {
      if (!this.prefabs.ContainsKey(tacticalPrefab.bp))
        this.prefabs.Add(tacticalPrefab.bp, tacticalPrefab.prefab);
    }
  }

  public GameObject GetPrefab(ShipBP newBP)
  {
    if (this.prefabs == null)
      this.InitPrefabs();
    if (this.prefabs.ContainsKey(newBP))
      return this.prefabs[newBP];
    Debug.LogWarning((object) $"Prefab with bp '{newBP.GetFullClassName()}' not found.");
    return (GameObject) null;
  }
}
