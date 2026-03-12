// Decompiled with JetBrains decompiler
// Type: TrackDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TrackDisplayer : MonoBehaviour
{
  public static TrackDisplayer current;
  private FactionsManager fm;
  private AudioManager am;
  public Color ownedColor;
  [SerializeField]
  private Color friendlyColor;
  [SerializeField]
  private Color neutralColor;
  [SerializeField]
  private Color hostileColor;
  [SerializeField]
  private Color unknownColor;
  [SerializeField]
  private Color naturalColor;
  public List<Track> allTracks;
  [SerializeField]
  private bool isSetTracksVisible;
  private HashSet<string> trackIds = new HashSet<string>();
  [SerializeField]
  private bool isTactical;
  private float targetZoom;

  private void Awake()
  {
    if ((Object) TrackDisplayer.current != (Object) null && (Object) TrackDisplayer.current != (Object) this)
      Object.Destroy((Object) this.gameObject);
    else
      TrackDisplayer.current = this;
  }

  private void Start()
  {
    this.fm = FactionsManager.current;
    this.am = AudioManager.current;
  }

  public void AddTrackID(string newID) => this.trackIds.Add(newID);

  public void AddTrack(Track newTrack)
  {
    if ((Object) this.fm == (Object) null)
      this.fm = FactionsManager.current;
    if ((Object) this.am == (Object) null)
      this.am = AudioManager.current;
    if (this.allTracks.Contains(newTrack))
      return;
    foreach (Track allTrack in this.allTracks)
    {
      if (allTrack.id == newTrack.id)
        newTrack.id = this.GetTrackNumber();
    }
    this.allTracks.Add(newTrack);
    this.trackIds.Add(newTrack.id);
    if ((Object) newTrack.GetComponent<Target>() == (Object) null)
    {
      newTrack.gameObject.AddComponent<Target>();
      Target component = newTrack.GetComponent<Target>();
      if (!this.isSetTracksVisible)
        return;
      newTrack.UpdateIFF();
      Color iffColor = this.GetIFFColor(newTrack);
      component.targetColor = iffColor;
      if (newTrack.id == null || newTrack.id == "")
        newTrack.id = this.GetTrackNumber();
      if (newTrack.trackName == null || newTrack.trackName == "")
        newTrack.SetTrackName();
      if (newTrack.iff == IFF.OWNED)
        component.isOwned = true;
      component.track = newTrack;
      component.targetName = newTrack.trackName;
      component.isTactical = this.isTactical;
      component.needArrowIndicator = true;
      if ((bool) (Object) newTrack.GetComponent<OrbitRenderer>())
        newTrack.GetComponent<OrbitRenderer>().SetColor(iffColor);
    }
    if (!this.isSetTracksVisible)
      return;
    this.am.PlaySFX(2);
  }

  public Color GetIFFColor(Track track)
  {
    Color iffColor = this.unknownColor;
    if (track.iff == IFF.OWNED)
      iffColor = this.ownedColor;
    else if (track.iff == IFF.FRIENDLY)
      iffColor = this.friendlyColor;
    else if (track.iff == IFF.NEUTRAL)
      iffColor = this.neutralColor;
    else if (track.iff == IFF.HOSTILE)
      iffColor = this.hostileColor;
    else if (track.isNatural)
      iffColor = this.naturalColor;
    return iffColor;
  }

  public string GetTrackNumber()
  {
    string trackNumber = "NULL";
    bool flag = true;
    for (int index = 0; index < 9999; ++index)
    {
      int num1 = Random.Range(0, 10);
      int num2 = Random.Range(0, 10);
      int num3 = Random.Range(0, 10);
      int num4 = Random.Range(0, 10);
      string str = num1.ToString() + num2.ToString() + num3.ToString() + num4.ToString();
      if (!this.trackIds.Contains(str))
      {
        trackNumber = str;
        this.trackIds.Add(str);
        flag = false;
        break;
      }
    }
    if (flag)
      MonoBehaviour.print((object) "UNABLE TO GENERATE UNIQUE TRACK NUMBER AFTER 9999 ATTEMPTS");
    return trackNumber;
  }

  public Track GetTrackFromID(string id)
  {
    if (id == null || id == "" || string.IsNullOrEmpty(id))
      return (Track) null;
    Track trackFromId = (Track) null;
    foreach (Track allTrack in this.allTracks)
    {
      if (allTrack.id == id)
      {
        trackFromId = allTrack;
        break;
      }
    }
    return trackFromId;
  }
}
