// Decompiled with JetBrains decompiler
// Type: GameSaveData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class GameSaveData
{
  public float timeScale;
  public int mins;
  public int hours;
  public int days;
  public int months;
  public int years;
  public int totalDays;
  public List<BodyData> bodies;
  public List<FactionData> factions;
  public List<StationData> stations;
  public List<FleetData> fleets;
  public string version;
}
