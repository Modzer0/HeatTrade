// Decompiled with JetBrains decompiler
// Type: BlueprintLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "BlueprintLibrary", menuName = "ScriptableObjects/BlueprintLibrary")]
public class BlueprintLibrary : ScriptableObject
{
  public List<ShipBP> shipBlueprints = new List<ShipBP>();
  public List<ModuleBP> moduleBlueprints = new List<ModuleBP>();
  public List<MountBP> mountBlueprints = new List<MountBP>();

  private void Start()
  {
  }

  public ShipBP GetShipBP(string newKey)
  {
    foreach (ShipBP shipBlueprint in this.shipBlueprints)
    {
      if (shipBlueprint.PrefabKey == newKey)
        return shipBlueprint;
    }
    return (ShipBP) null;
  }

  public ModuleBP GetModuleBP(string newKey)
  {
    foreach (ModuleBP moduleBlueprint in this.moduleBlueprints)
    {
      if (moduleBlueprint.PrefabKey == newKey)
        return moduleBlueprint;
    }
    Debug.LogError((object) ("MODULE Key not found! " + newKey));
    return (ModuleBP) null;
  }

  public MountBP GetMountBP(string newKey)
  {
    foreach (MountBP mountBlueprint in this.mountBlueprints)
    {
      if (mountBlueprint.PrefabKey == newKey)
        return mountBlueprint;
    }
    Debug.LogError((object) ("MOUNT Key not found! " + newKey));
    return (MountBP) null;
  }
}
