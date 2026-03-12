// Decompiled with JetBrains decompiler
// Type: TacticalShipData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class TacticalShipData
{
  public int factionID;
  public string trackID;
  public string publicName;
  public float cruiseAcceleration;
  public List<TacticalModuleData> modules = new List<TacticalModuleData>();
  public ShipBP bp;
}
