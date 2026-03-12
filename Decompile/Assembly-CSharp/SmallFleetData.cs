// Decompiled with JetBrains decompiler
// Type: SmallFleetData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SmallFleetData : MonoBehaviour
{
  private TrackDisplayer td;
  private MapInputs mi;
  private Track track;
  [SerializeField]
  private Image image;
  [SerializeField]
  private TMP_Text nameText;
  private bool isInit;

  private void Start()
  {
    if (this.isInit)
      return;
    this.Init();
  }

  private void Init()
  {
    if (this.isInit)
      return;
    this.td = TrackDisplayer.current;
    this.mi = MapInputs.current;
    this.isInit = true;
  }

  public void SetData(Track newTrack)
  {
    if ((Object) newTrack == (Object) null)
      return;
    if (!this.isInit)
      this.Init();
    this.track = newTrack;
    this.nameText.text = this.track.trackName;
    this.nameText.color = this.td.GetIFFColor(this.track);
  }

  public void SelectFleet()
  {
    if (!(bool) (Object) this.track)
      return;
    this.mi.EnterTarget(this.track.GetComponent<CamInfo>());
    this.mi.SetTarget(this.track.GetComponent<CamInfo>());
  }
}
