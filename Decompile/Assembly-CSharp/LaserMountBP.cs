// Decompiled with JetBrains decompiler
// Type: LaserMountBP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "LaserMountBP", menuName = "ScriptableObjects/LaserMountBP")]
public class LaserMountBP : MountBP
{
  [Header("= LASER MOUNT ====================")]
  [SerializeField]
  private float powerDraw;
  [SerializeField]
  private float efficiency;
  [SerializeField]
  private float wavelength;
  [SerializeField]
  private float apertureDiameter;

  public float PowerDraw => this.powerDraw;

  public float Efficiency => this.efficiency;

  public float Wavelength => this.wavelength;

  public float ApertureDiameter => this.apertureDiameter;
}
