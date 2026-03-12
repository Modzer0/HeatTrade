// Decompiled with JetBrains decompiler
// Type: AttachmentsPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class AttachmentsPanel : MonoBehaviour
{
  private Attachments root;
  private int prevAttachedFleets;
  [SerializeField]
  private GameObject attachmentsPanel;
  [SerializeField]
  private Transform attachmentsList;
  [SerializeField]
  private SmallFleetData smallFleetDataPF;

  private void Start() => this.StartCoroutine((IEnumerator) this.CheckLoop());

  private IEnumerator CheckLoop()
  {
    while (true)
    {
      if ((bool) (UnityEngine.Object) this.root && this.root.attachedFleets.Count != this.prevAttachedFleets)
        this.Setup(this.root);
      yield return (object) new WaitForSeconds(0.2f);
    }
  }

  public void Setup(Attachments newRoot)
  {
    this.root = newRoot;
    this.prevAttachedFleets = this.root.attachedFleets.Count;
    IEnumerator enumerator = (IEnumerator) this.attachmentsList.transform.GetEnumerator();
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
    if ((UnityEngine.Object) this.root.station != (UnityEngine.Object) null && this.root.GetComponent<Track>().type != TrackType.STATION)
      UnityEngine.Object.Instantiate<SmallFleetData>(this.smallFleetDataPF, this.attachmentsList).SetData(this.root.station.GetComponent<Track>());
    foreach (FleetManager attachedFleet in this.root.attachedFleets)
    {
      if ((UnityEngine.Object) attachedFleet == (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      else
        UnityEngine.Object.Instantiate<SmallFleetData>(this.smallFleetDataPF, this.attachmentsList).SetData(attachedFleet.GetComponent<Track>());
    }
  }
}
