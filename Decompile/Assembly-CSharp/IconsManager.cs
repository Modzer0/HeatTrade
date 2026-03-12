// Decompiled with JetBrains decompiler
// Type: IconsManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class IconsManager : MonoBehaviour
{
  public static IconsManager current;
  [Header("ICONS")]
  [Header("Modules")]
  [SerializeField]
  private Sprite cargo;
  [SerializeField]
  private Sprite crew;
  [SerializeField]
  private Sprite drive;
  [SerializeField]
  private Sprite ewar;
  [SerializeField]
  private Sprite fuel;
  [SerializeField]
  private Sprite heatsink;
  [SerializeField]
  private Sprite missiles;
  [SerializeField]
  private Sprite nose;
  [SerializeField]
  private Sprite nozzle;
  [SerializeField]
  private Sprite sensors;
  [SerializeField]
  private Sprite weapon;
  [Header("Mounts")]
  [SerializeField]
  private Sprite kinetics;
  [SerializeField]
  private Sprite laser;
  [SerializeField]
  private Sprite missile;
  [SerializeField]
  private Sprite pd;
  [SerializeField]
  private Sprite radiator;

  private void Awake() => IconsManager.current = this;

  public Sprite GetIconFor(PartType type)
  {
    switch (type)
    {
      case PartType.DRIVE:
        return this.drive;
      case PartType.HEATSINK:
        return this.heatsink;
      case PartType.CARGO:
        return this.cargo;
      case PartType.CREW:
        return this.crew;
      case PartType.SENSORS:
        return this.sensors;
      case PartType.WEAPON:
        return this.weapon;
      case PartType.NOSE:
        return this.nose;
      case PartType.RADIATORS:
        return this.radiator;
      case PartType.KINETIC:
        return this.kinetics;
      case PartType.BEAM:
        return this.laser;
      case PartType.MISSILE:
        return this.missile;
      case PartType.EWAR:
        return this.ewar;
      case PartType.PD:
        return this.pd;
      case PartType.FUEL:
        return this.fuel;
      case PartType.NOZZLE:
        return this.nozzle;
      default:
        return (Sprite) null;
    }
  }
}
