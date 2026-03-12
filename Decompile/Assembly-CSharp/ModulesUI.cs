// Decompiled with JetBrains decompiler
// Type: ModulesUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class ModulesUI : MonoBehaviour
{
  public static ModulesUI current;
  private TacticalInputs ti;
  private ShipController ship;
  [SerializeField]
  private GameObject modulesPanel;
  [SerializeField]
  private Transform modulesList;
  [Header("ICONS")]
  [Header("Engineering")]
  [SerializeField]
  private List<Color> engineeringColors;
  [SerializeField]
  private Sprite nozzleIcon;
  [SerializeField]
  private Sprite driveIcon;
  [SerializeField]
  private Sprite radiatorIcon;
  [SerializeField]
  private Sprite heatsinkIcon;
  [Header("Logistics")]
  [SerializeField]
  private List<Color> logisticsColors;
  [SerializeField]
  private Sprite fuelIcon;
  [SerializeField]
  private Sprite cargoIcon;
  [SerializeField]
  private Sprite crewIcon;
  [Header("Weapons")]
  [SerializeField]
  private List<Color> weaponsColors;
  [SerializeField]
  private Sprite weaponIcon;
  [SerializeField]
  private Sprite pdIcon;
  [SerializeField]
  private Sprite kineticsIcon;
  [SerializeField]
  private Sprite missileIcon;
  [SerializeField]
  private Sprite laserIcon;
  [Header("Electronics")]
  [SerializeField]
  private List<Color> electronicsColors;
  [SerializeField]
  private Sprite sensorsIcon;
  [SerializeField]
  private Sprite ewarIcon;
  [Header("Others")]
  [SerializeField]
  private Sprite noseIcon;
  [SerializeField]
  private GameObject noseIconPF;
  [Header("DAMAGE CONTROL")]
  [SerializeField]
  private TMP_Text dcFreeText;
  [SerializeField]
  private uiBar materialsBar;
  [Header("HEALTH")]
  [SerializeField]
  private GameObject healthRatioBarBG;
  [SerializeField]
  private uiBar healthRatioBar;
  [SerializeField]
  private Gradient healthGradient;
  [Header("PREFABS")]
  [SerializeField]
  private PartHealthUI modulePF;
  [SerializeField]
  private PartHealthUI mountPF;

  private void Awake() => ModulesUI.current = this;

  private void Start()
  {
    this.ti = TacticalInputs.current;
    this.ti.newSelection += new Action(this.NewShipSelected);
  }

  private void Update()
  {
    if ((bool) (UnityEngine.Object) this.ship)
    {
      this.healthRatioBarBG.SetActive(true);
      float totalHealthRatio = this.ship.totalHealthRatio;
      this.healthRatioBar.SetBarSize(totalHealthRatio);
      this.healthRatioBar.SetBarColor(this.healthGradient.Evaluate(totalHealthRatio));
      this.dcFreeText.text = this.ship.GetFreeDC().ToString();
      this.materialsBar.SetBarSize(this.ship.GetMaterialsRatio());
    }
    else
      this.healthRatioBarBG.SetActive(false);
  }

  private void NewShipSelected()
  {
    this.ship = this.ti.selectedShip;
    if ((UnityEngine.Object) this.ship == (UnityEngine.Object) null)
    {
      this.modulesPanel.SetActive(false);
    }
    else
    {
      this.modulesPanel.SetActive(true);
      IEnumerator enumerator = (IEnumerator) this.modulesList.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator.Current).gameObject);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      foreach (T_Module allModule in this.ship.allModules)
      {
        PartHealthUI partHealthUi = UnityEngine.Object.Instantiate<PartHealthUI>(this.modulePF, this.modulesList);
        Sprite sprite = (Sprite) null;
        if (allModule.mounts.Count > 0)
          sprite = this.GetIcon(allModule.mounts[0].partType);
        T_Module newModule = allModule;
        Sprite icon = this.GetIcon(allModule.partType);
        Sprite mountIcon = sprite;
        partHealthUi.Init(newModule, icon, mountIcon);
      }
    }
  }

  private Sprite GetIcon(PartType partType)
  {
    switch (partType)
    {
      case PartType.DRIVE:
        return this.driveIcon;
      case PartType.HEATSINK:
        return this.heatsinkIcon;
      case PartType.CARGO:
        return this.cargoIcon;
      case PartType.CREW:
        return this.crewIcon;
      case PartType.SENSORS:
        return this.sensorsIcon;
      case PartType.WEAPON:
        return this.weaponIcon;
      case PartType.NOSE:
        return this.noseIcon;
      case PartType.RADIATORS:
        return this.radiatorIcon;
      case PartType.KINETIC:
        return this.kineticsIcon;
      case PartType.BEAM:
        return this.laserIcon;
      case PartType.MISSILE:
        return this.missileIcon;
      case PartType.EWAR:
        return this.ewarIcon;
      case PartType.PD:
        return this.pdIcon;
      case PartType.FUEL:
        return this.fuelIcon;
      case PartType.NOZZLE:
        return this.nozzleIcon;
      default:
        return (Sprite) null;
    }
  }

  private List<Color> GetColors(PartType partType)
  {
    switch (partType)
    {
      case PartType.DRIVE:
        return this.engineeringColors;
      case PartType.HEATSINK:
        return this.engineeringColors;
      case PartType.CARGO:
        return this.logisticsColors;
      case PartType.CREW:
        return this.logisticsColors;
      case PartType.SENSORS:
        return this.electronicsColors;
      case PartType.WEAPON:
        return this.weaponsColors;
      case PartType.RADIATORS:
        return this.engineeringColors;
      case PartType.KINETIC:
        return this.weaponsColors;
      case PartType.BEAM:
        return this.weaponsColors;
      case PartType.MISSILE:
        return this.weaponsColors;
      case PartType.EWAR:
        return this.electronicsColors;
      case PartType.PD:
        return this.weaponsColors;
      case PartType.FUEL:
        return this.logisticsColors;
      default:
        return (List<Color>) null;
    }
  }
}
