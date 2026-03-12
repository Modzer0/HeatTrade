// Decompiled with JetBrains decompiler
// Type: NavigationData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class NavigationData
{
  public NavigationState state;
  public FlightType flightType;
  public ArrivalType arrivalType;
  public string targetID;
  public SerializableVector3 currentDir;
  public SerializableVector3 currentVel;
  public float accG;
  public float decG;
  public float accKkm;
  public float decKkm;
  public SerializableVector3 startMarkerPos;
  public SerializableVector3 accMarkerPos;
  public SerializableVector3 decelMarkerPos;
  public SerializableVector3 stopMarkerPos;
}
