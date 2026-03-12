// Decompiled with JetBrains decompiler
// Type: StationPort
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class StationPort : MonoBehaviour
{
  public StationManager station;
  public S_FuelStorage fuel;
  public float repairRate;
  public float repairCost;
  public float resupplyRate;
  public float resupplyCost;
  public float refuelRate;
  public int refuelCost;
  public float rearmRate;
  public float rearmCost;

  private void Start()
  {
  }
}
