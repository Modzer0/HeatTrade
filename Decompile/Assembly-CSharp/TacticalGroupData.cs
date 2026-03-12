// Decompiled with JetBrains decompiler
// Type: TacticalGroupData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class TacticalGroupData
{
  public int factionID;
  public string trackID;
  public string publicName;
  public List<TacticalShipData> objects;
  public Vector3 initPos;
  public Vector3 spawnPos;
  public Vector3 direction;
}
