// Decompiled with JetBrains decompiler
// Type: FactionsManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable enable
public class FactionsManager : MonoBehaviour
{
  [SerializeField]
  private 
  #nullable disable
  FactionIconsLibrary fil;
  private NotificationsManager notifs;
  private ColorManager cm;
  private AudioManager am;
  private SceneTransitionManager stm;
  public List<Faction> factions;
  public Faction playerFaction;
  public Faction prometheusFactionPreset;
  public PlayerBackground playerBackground;
  public string flagsPath;

  public static FactionsManager current { get; private set; }

  private void Awake()
  {
    if ((Object) FactionsManager.current != (Object) null && (Object) FactionsManager.current != (Object) this)
    {
      Object.Destroy((Object) this.gameObject);
    }
    else
    {
      FactionsManager.current = this;
      Object.DontDestroyOnLoad((Object) this.gameObject);
      this.flagsPath = Path.Combine(Application.persistentDataPath, "PlayerFlags");
      if (Directory.Exists(this.flagsPath))
        return;
      Directory.CreateDirectory(this.flagsPath);
    }
  }

  private void Start()
  {
    this.notifs = NotificationsManager.current;
    this.cm = ColorManager.current;
    this.stm = SceneTransitionManager.current;
  }

  public void SetupNewGame(bool isPrometheus, Faction playerFaction)
  {
    if (isPrometheus)
      this.NewGame(this.prometheusFactionPreset, isPrometheus);
    else
      this.NewGame(playerFaction, isPrometheus);
  }

  private void NewGame(Faction newPlayerFaction, bool isPrometheus)
  {
    this.stm.gst = !isPrometheus ? this.stm.newCustomGST : this.stm.newPrometheusGST;
    this.factions.Remove(this.GetFactionFromID(1));
    this.playerFaction = new Faction(newPlayerFaction);
    this.factions.Add(this.playerFaction);
    if (!isPrometheus)
      this.ModPlayerCredits((string) null, this.playerBackground.startingCredits);
    foreach (Faction faction1 in this.factions)
    {
      foreach (Faction faction2 in this.factions)
        faction1.SetFactionRelation(faction2.factionID, 0);
    }
    foreach (Faction faction in this.factions)
    {
      if (isPrometheus)
      {
        if (faction.factionID == 1)
        {
          faction.SetFactionRelation(12, -100);
          faction.SetFactionRelation(99, -100);
        }
        else if (faction.factionID == 12)
          faction.SetFactionRelation(1, -100);
        else if (faction.factionID == 99)
          faction.SetFactionRelation(1, -100);
      }
      else if (faction.factionID == 1)
        faction.SetFactionRelation(99, -100);
      else if (faction.factionID == 99)
        faction.SetFactionRelation(1, -100);
    }
  }

  public void ModPlayerCredits(
  #nullable enable
  string? header, int mod)
  {
    MonoBehaviour.print((object) ("MOD PLAYER CREDITS: " + mod.ToString()));
    if (!(bool) (Object) this.notifs)
      this.notifs = NotificationsManager.current;
    if (!(bool) (Object) this.am)
      this.am = AudioManager.current;
    int num = mod > 0 ? 1 : 0;
    this.playerFaction.credits += mod;
    string str = "+";
    if (!(bool) (Object) this.cm)
      this.cm = ColorManager.current;
    string color = "green";
    if (num == 0)
    {
      str = "";
      color = "orange";
    }
    if (!(bool) (Object) this.notifs)
      return;
    this.notifs.NewNotif($"{header}: {str}{mod.ToString()}cr", color);
  }

  public 
  #nullable disable
  Faction GetFactionFromID(int id)
  {
    foreach (Faction faction in this.factions)
    {
      if (faction.factionID == id)
        return faction;
    }
    return (Faction) null;
  }

  public Faction NewFaction(FactionData fd)
  {
    Sprite icon = fd.factionID != 1 || !(fd.factionName != "Prometheus") ? this.fil.GetIconFromFactionID(fd.factionID) : this.GetCustomSprite(fd.factionName);
    Faction faction = new Faction(fd, icon);
    if (faction.factionID == 1)
    {
      this.playerFaction = faction;
      if (!this.factions.Contains(this.playerFaction))
        this.factions.Add(this.playerFaction);
    }
    else
      this.factions.Add(faction);
    return faction;
  }

  public Sprite GetCustomSprite(string customName)
  {
    Sprite customSprite = (Sprite) null;
    string path = Path.Combine(this.flagsPath, customName + ".png");
    if (!File.Exists(path))
      return (Sprite) null;
    byte[] data = File.ReadAllBytes(path);
    Texture2D texture2D = new Texture2D(2, 2);
    if (texture2D.LoadImage(data))
    {
      Rect rect = new Rect(0.0f, 0.0f, (float) texture2D.width, (float) texture2D.height);
      customSprite = Sprite.Create(texture2D, rect, new Vector2(0.5f, 0.5f));
    }
    return customSprite;
  }

  public void AddFaction(Faction faction)
  {
    if (this.factions.Contains(faction))
      return;
    this.factions.Add(faction);
  }

  public string GetFactionCode(int id)
  {
    string factionCode = "";
    if (id == 1)
    {
      factionCode = this.playerFaction.factionCode;
    }
    else
    {
      foreach (Faction faction in this.factions)
      {
        if (faction.factionID == id)
          factionCode = faction.factionCode;
      }
    }
    return factionCode;
  }

  public IFF GetIFF(int trackFactionID)
  {
    IFF iff = IFF.UNKNOWN;
    Dictionary<int, int> relations = this.playerFaction.relations;
    if (trackFactionID == 1)
      iff = IFF.OWNED;
    else if (relations != null)
    {
      if (relations.ContainsKey(trackFactionID) && relations[trackFactionID] >= 80 /*0x50*/)
        iff = IFF.FRIENDLY;
      else if (relations.ContainsKey(trackFactionID) && relations[trackFactionID] <= -80)
        iff = IFF.HOSTILE;
      else if (relations.ContainsKey(trackFactionID) && relations[trackFactionID] < 80 /*0x50*/ && relations[trackFactionID] > -80)
        iff = IFF.NEUTRAL;
    }
    return iff;
  }

  public Color GetPrimaryColor(int id)
  {
    Faction factionFromId = this.GetFactionFromID(id);
    return factionFromId == null ? Color.black : factionFromId.colorPrimary;
  }

  public Color GetSecondaryColor(int id)
  {
    Faction factionFromId = this.GetFactionFromID(id);
    return factionFromId == null ? Color.black : factionFromId.colorSecondary;
  }
}
