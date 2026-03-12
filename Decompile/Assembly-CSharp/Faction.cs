// Decompiled with JetBrains decompiler
// Type: Faction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class Faction
{
  public int factionID;
  public string factionName;
  public string factionWords;
  [TextArea]
  public string factionDescription;
  public string factionCode;
  public int credits;
  public Color colorPrimary;
  public Color colorSecondary;
  public Dictionary<int, int> relations = new Dictionary<int, int>();
  public Sprite factionIcon;

  public Faction(FactionData fd, Sprite icon)
  {
    this.factionID = fd.factionID;
    this.factionName = fd.factionName;
    this.factionWords = fd.factionWords;
    this.factionDescription = fd.factionDescription;
    this.factionCode = fd.factionCode;
    this.credits = fd.credits;
    this.relations = fd.relations.ToDictionary();
    this.colorPrimary = fd.colorDataPrimary.ToColor();
    this.colorSecondary = fd.colorDataSecondary.ToColor();
    this.factionIcon = icon;
  }

  public Faction(Faction newFaction)
  {
    this.factionID = newFaction.factionID;
    this.factionName = newFaction.factionName;
    this.factionWords = newFaction.factionWords;
    this.factionDescription = newFaction.factionDescription;
    this.factionCode = newFaction.factionCode;
    this.credits = newFaction.credits;
    this.factionIcon = newFaction.factionIcon;
    this.colorPrimary = newFaction.colorPrimary;
    this.colorSecondary = newFaction.colorSecondary;
  }

  public Faction(
    string name,
    string words,
    string description,
    string code,
    Color newColorPrimary,
    Color newColorSecondary,
    Sprite newIcon)
  {
    this.factionID = 1;
    this.factionName = name;
    this.factionWords = words;
    this.factionDescription = description;
    this.factionCode = code;
    this.credits = 0;
    this.factionIcon = newIcon;
    this.colorPrimary = newColorPrimary;
    this.colorSecondary = newColorSecondary;
  }

  public void SetFactionRelation(int targetFactionID, int relation)
  {
    if (this.relations == null)
      this.relations = new Dictionary<int, int>();
    if (!this.relations.ContainsKey(targetFactionID))
      this.relations.Add(targetFactionID, relation);
    else
      this.relations[targetFactionID] = relation;
  }

  public List<int> GetHostiles()
  {
    List<int> hostiles = new List<int>();
    foreach (KeyValuePair<int, int> relation in this.relations)
    {
      if (relation.Value <= -80)
        hostiles.Add(relation.Key);
    }
    return hostiles;
  }

  public int GetRelations(int id) => !this.relations.ContainsKey(id) ? 0 : this.relations[id];
}
