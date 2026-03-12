// Decompiled with JetBrains decompiler
// Type: Track
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Track : MonoBehaviour
{
  private TrackDisplayer td;
  private FactionsManager fm;
  public bool isNatural;
  public bool isIFFOn;
  public IFF iff;
  public TrackType type;
  public int factionID;
  public string id;
  public string publicName;
  public string trackName;
  public string mission;
  public int radarSignature;
  public int heatSignature;
  public int trackRadius = 10;
  private bool isInit;

  private void Start() => this.Init();

  private void Init()
  {
    if (this.isInit)
      return;
    this.fm = FactionsManager.current;
    this.td = TrackDisplayer.current;
    this.td.AddTrack(this);
    this.isInit = true;
  }

  public void SetTeam(int newTeam)
  {
  }

  public void SetName(string newName)
  {
    MonoBehaviour.print((object) ("set name: " + newName));
    this.publicName = newName;
  }

  public void SetTrackName()
  {
    if ((Object) this.fm == (Object) null)
      this.fm = FactionsManager.current;
    if ((Object) this.td == (Object) null)
      this.td = TrackDisplayer.current;
    if (this.id == null || this.fm.GetFactionCode(this.factionID) == null)
      return;
    if (this.id == "" || this.id == " " || string.IsNullOrEmpty(this.id))
      this.id = this.td.GetTrackNumber();
    if (this.iff == IFF.OWNED)
      this.trackName = $"{this.id} - {this.publicName}";
    else
      this.trackName = $"{this.fm.GetFactionCode(this.factionID)} {this.id} - {this.publicName}";
  }

  public Faction GetFaction() => this.fm.GetFactionFromID(this.factionID);

  public string GetFactionCode()
  {
    if (!(bool) (Object) this.fm)
      this.Init();
    return this.fm.GetFactionCode(this.factionID);
  }

  public string GetFullName() => $"{this.GetFactionCode()}{this.id} - {this.publicName}";

  public void UpdateIFF()
  {
    this.Init();
    this.iff = this.fm.GetIFF(this.factionID);
  }
}
