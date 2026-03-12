// Decompiled with JetBrains decompiler
// Type: SkirmishMapManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SkirmishMapManager : MonoBehaviour
{
  private SceneTransitionManager stm;
  [SerializeField]
  private GameObject mainBodies;
  [SerializeField]
  private List<SkirmishMap> maps = new List<SkirmishMap>();
  [SerializeField]
  private Transform saturnRing;

  private void Start()
  {
    this.stm = SceneTransitionManager.current;
    if (!(bool) (Object) this.stm || this.stm.gst.nla != GameStartType.NLA.Skirmish)
      return;
    this.LoadMap();
  }

  private void LoadMap()
  {
    this.mainBodies.SetActive(false);
    foreach (SkirmishMap map in this.maps)
    {
      if (map.mapName == this.stm.skirmishMapName)
        map.mapGO.SetActive(true);
      if (map.mapName == SkirmishMapName.Saturn)
        this.saturnRing.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }
  }
}
