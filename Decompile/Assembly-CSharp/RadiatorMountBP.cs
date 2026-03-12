// Decompiled with JetBrains decompiler
// Type: RadiatorMountBP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "RadiatorMountBP", menuName = "ScriptableObjects/RadiatorMountBP")]
public class RadiatorMountBP : MountBP
{
  [Header("= RADIATOR MOUNT ====================")]
  [SerializeField]
  private float length;
  [SerializeField]
  private float height;
  [SerializeField]
  private float area;
  [SerializeField]
  private float operatingTemp;
  [SerializeField]
  private float heatDissipation;

  public float Length => this.length;

  public float Height => this.height;

  public float Area => this.area;

  public float OperatingTemp => this.operatingTemp;

  public float HeatDissipation => this.heatDissipation;
}
