// Decompiled with JetBrains decompiler
// Type: FleetData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class FleetData
{
  public string id;
  public int factionID;
  public string publicName;
  public SerializableVector3 position;
  public string parentID;
  public string parentOrbiterID;
  public NavigationData navigationData;
  public List<ShipData> ships;
  public FleetState state;
  public bool isAutoRepair;
  public bool isAutoResupply;
  public bool isAutoRefuel;
  public bool isRepairing;
  public bool isResupplying;
  public bool isRefueling;
  public bool isAutoTrading;
  public bool isTrading;
  public string currentMarketID;
  public string targetMarketID;
  public string targetResourceName;
  public float targetQuantity;
  public bool isBuying;
  public int buyPrice;
  public int bestSellPrice;
  public float profitMargin;
  public float minProfitMargin;
  public float currentMinProfitMargin;
  public bool hasArrived;
}
