// Decompiled with JetBrains decompiler
// Type: TrackList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TrackList : MonoBehaviour
{
  private TrackDisplayer td;
  [SerializeField]
  private GameObject bodiesPanel;
  [SerializeField]
  private GameObject ownedPanel;
  [SerializeField]
  private GameObject alliesPanel;
  [SerializeField]
  private GameObject neutralPanel;
  [SerializeField]
  private GameObject hostilePanel;
  [SerializeField]
  private List<GameObject> panels;
  [SerializeField]
  private Transform bodiesList;
  [SerializeField]
  private Transform ownedList;
  [SerializeField]
  private Transform alliesList;
  [SerializeField]
  private Transform neutralList;
  [SerializeField]
  private Transform hostileList;
  [SerializeField]
  private List<Transform> lists;
  [SerializeField]
  private GameObject trackDataPF;

  private void Start()
  {
    this.td = TrackDisplayer.current;
    this.lists = new List<Transform>()
    {
      this.bodiesList,
      this.ownedList,
      this.alliesList,
      this.neutralList,
      this.hostileList
    };
    this.panels = new List<GameObject>()
    {
      this.bodiesPanel,
      this.ownedPanel,
      this.alliesPanel,
      this.neutralPanel,
      this.hostilePanel
    };
    this.RefreshList();
  }

  public void SetPanel(int index)
  {
    if (this.panels.Count == 0)
      return;
    foreach (GameObject panel in this.panels)
      panel.SetActive(false);
    this.panels[index].SetActive(true);
    this.RefreshList();
  }

  public void RefreshList()
  {
    foreach (Transform list in this.lists)
    {
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
    }
    foreach (Track allTrack in this.td.allTracks)
    {
      Transform parent = this.bodiesList;
      if (allTrack.iff == IFF.OWNED)
        parent = this.ownedList;
      else if (allTrack.iff == IFF.FRIENDLY)
        parent = this.alliesList;
      else if (allTrack.iff == IFF.NEUTRAL)
        parent = this.neutralList;
      else if (allTrack.iff == IFF.HOSTILE)
        parent = this.hostileList;
      UnityEngine.Object.Instantiate<GameObject>(this.trackDataPF, parent).GetComponent<TrackData>().SetTrack(allTrack);
    }
  }
}
