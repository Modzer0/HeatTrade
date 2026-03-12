// Decompiled with JetBrains decompiler
// Type: TrackData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class TrackData : MonoBehaviour
{
  private MapInputs mi;
  private FactionsManager fm;
  private TrackDisplayer td;
  public Track track;
  private CamInfo camInfo;
  private bool isOwnedByPlayer;
  [SerializeField]
  private TMP_Text headerText;
  [SerializeField]
  private TMP_Text nameText;
  [SerializeField]
  private TMP_Text naturalText;
  [SerializeField]
  private TMP_Text typeText;
  [SerializeField]
  private TMP_Text factionText;

  private void Start()
  {
  }

  private void SetSingletons()
  {
    this.mi = MapInputs.current;
    this.fm = FactionsManager.current;
    this.td = TrackDisplayer.current;
  }

  public void OnClick()
  {
    if (!(bool) (Object) this.track)
      return;
    this.mi.EnterTarget(this.camInfo);
    this.mi.SetTarget(this.camInfo);
  }

  public void SetTrack(Track newTrack)
  {
    if ((Object) newTrack == (Object) null)
      return;
    if ((Object) this.mi == (Object) null)
      this.SetSingletons();
    this.track = newTrack;
    this.camInfo = this.track.GetComponent<CamInfo>();
    this.isOwnedByPlayer = this.track.iff == IFF.OWNED;
    this.headerText.text = this.track.trackName;
    this.headerText.color = this.td.GetIFFColor(this.track);
    this.nameText.text = this.track.publicName;
    this.naturalText.text = this.track.isNatural ? "TRUE" : "FALSE";
    this.typeText.text = this.track.type.ToString();
    this.factionText.text = this.fm.GetFactionCode(this.track.factionID);
  }
}
