// Decompiled with JetBrains decompiler
// Type: StationData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class StationData
{
  public string id;
  public int factionID;
  public string publicName;
  public SerializableVector3 position;
  public string parentID;
  public string parentOrbiterID;
  public StationManager station;
  public InventoryData inventory;
  public MarketData market;
  public FactoryData factory;
  public OrbitData orbit;
}
