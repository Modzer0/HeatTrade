// Decompiled with JetBrains decompiler
// Type: ShipyardModuleCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ShipyardModuleCard : MonoBehaviour
{
  private IconsManager im;
  private bool isInit;
  private ModuleBP module;
  private RectTransform rt;
  [Header("MODULE")]
  [SerializeField]
  private Image moduleIcon;
  [SerializeField]
  private TMP_Text moduleNameText;
  [SerializeField]
  private RectTransform moduleRT;
  [Header("===============")]
  [SerializeField]
  private TMP_Text moduleSizeText;
  [SerializeField]
  private TMP_Text moduleTypeText;
  [SerializeField]
  private TMP_Text moduleRepairText;
  [SerializeField]
  private TMP_Text moduleMassText;
  [SerializeField]
  private TMP_Text moduleValueText;
  [Header("SPECIAL ===============")]
  [SerializeField]
  private GameObject leftTextPF;
  [SerializeField]
  private GameObject rightTextPF;
  [SerializeField]
  private Transform specialLeftModule;
  [SerializeField]
  private Transform specialRightModule;
  [Space(10f)]
  [Header("MOUNTS")]
  [SerializeField]
  private RectTransform mountsRT;
  [SerializeField]
  private GameObject mountsPanel;
  [SerializeField]
  private Image mountIcon;
  [SerializeField]
  private TMP_Text mountNameText;
  [SerializeField]
  private TMP_Text mountCountText;
  [Header("===============")]
  [SerializeField]
  private TMP_Text mountSizeText;
  [SerializeField]
  private TMP_Text mountTypeText;
  [SerializeField]
  private TMP_Text mountRepairText;
  [SerializeField]
  private TMP_Text mountMassText;
  [SerializeField]
  private TMP_Text mountValueText;
  [Header("SPECIAL ===============")]
  [SerializeField]
  private Transform specialLeftMount;
  [SerializeField]
  private Transform specialRightMount;

  private void Init()
  {
    if (this.isInit)
      return;
    this.im = IconsManager.current;
    this.rt = this.GetComponent<RectTransform>();
    this.isInit = true;
  }

  public void Setup(ModuleBP newModule)
  {
    this.gameObject.SetActive(true);
    this.Init();
    this.module = newModule;
    this.SetModuleData();
    this.SetMountData();
    this.StartCoroutine((IEnumerator) this.SetSizesDelayed());
  }

  private IEnumerator SetSizesDelayed()
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.SetSizes();
  }

  private void SetSizes()
  {
    float y1 = this.specialLeftModule.GetComponent<RectTransform>().sizeDelta.y;
    float y2 = this.specialLeftMount.GetComponent<RectTransform>().sizeDelta.y;
    this.moduleRT.sizeDelta = new Vector2(this.moduleRT.sizeDelta.x, 102f + y1);
    this.mountsRT.sizeDelta = new Vector2(this.mountsRT.sizeDelta.x, 102f + y2);
    this.rt.sizeDelta = new Vector2(this.rt.sizeDelta.x, 104f + Mathf.Max(y1, y2));
  }

  private void NewText(string leftText, string rightText)
  {
    GameObject gameObject1 = Object.Instantiate<GameObject>(this.leftTextPF, this.specialLeftModule);
    GameObject gameObject2 = Object.Instantiate<GameObject>(this.rightTextPF, this.specialRightModule);
    gameObject1.GetComponent<TMP_Text>().text = leftText;
    gameObject2.GetComponent<TMP_Text>().text = rightText;
    gameObject1.SetActive(true);
    gameObject2.SetActive(true);
  }

  private void NewTextMount(string leftText, string rightText)
  {
    GameObject gameObject1 = Object.Instantiate<GameObject>(this.leftTextPF, this.specialLeftMount);
    GameObject gameObject2 = Object.Instantiate<GameObject>(this.rightTextPF, this.specialRightMount);
    gameObject1.GetComponent<TMP_Text>().text = leftText;
    gameObject2.GetComponent<TMP_Text>().text = rightText;
    gameObject1.SetActive(true);
    gameObject2.SetActive(true);
  }

  private void SetModuleData()
  {
    this.moduleIcon.sprite = this.im.GetIconFor(this.module.PartType);
    this.moduleNameText.text = this.module.PartNameFull;
    this.moduleSizeText.text = this.module.SizeClass.ToString();
    this.moduleTypeText.text = this.module.PartType.ToString();
    ResourceQuantity repairQuantity = this.module.RepairQuantity;
    this.moduleRepairText.text = $"{repairQuantity.resource.resourceName} x{repairQuantity.quantity.ToString()}";
    this.moduleMassText.text = this.module.Mass.ToString() + " t";
    this.moduleValueText.text = " cr";
    if (this.module is CargoModuleBP)
    {
      CargoModuleBP module = this.module as CargoModuleBP;
      this.NewText("Cargo Density", "0.05 t/m^3");
      this.NewText("Cargo Capacity", module.Mass.ToString() + " t");
    }
    else if (this.module is CrewModuleBP)
    {
      CrewModuleBP module = this.module as CrewModuleBP;
      this.NewText("Crew Count", module.CrewCount.ToString());
      this.NewText("DC Teams", module.DCTeams.ToString());
      this.NewText("Materials", module.Materials.ToString());
    }
    else if (this.module is DriveModuleBP)
    {
      DriveModuleBP module = this.module as DriveModuleBP;
      this.NewText("Propulsion Type", module.PropulsionType.ToString());
      this.NewText("Fuel Type", module.FuelType.resourceName);
      float num = module.TotalOutput;
      this.NewText("Total Output", num.ToString() + " GW");
      num = module.ElectricOutput;
      this.NewText("Electric Output", num.ToString() + " GW");
      num = module.ThermalOutput;
      this.NewText("Thermal Output", num.ToString() + " GW");
      num = module.ThermalEfficiency * 100f;
      this.NewText("Thermal Efficiency", num.ToString() + "%");
      num = module.ThrustOutput;
      this.NewText("Thrust Output", num.ToString() + " GW");
      num = module.WasteOutput;
      this.NewText("Waste Heat", num.ToString() + " GW");
      num = module.Isp;
      this.NewText("ISP (Cruise)", num.ToString() + " s");
      num = module.MassFlowRate;
      this.NewText("Mass Flow Rate", num.ToString() + " kg/s");
      num = module.CruiseThrustN;
      this.NewText("Max Thrust", num.ToString() + " N");
    }
    else if (this.module is FuelModuleBP)
    {
      FuelModuleBP module = this.module as FuelModuleBP;
      this.NewText("Fuel Resource", module.FuelResource.resourceName);
      float num = module.FuelDensity;
      this.NewText("Fuel Density", num.ToString() + " t/m^3");
      num = module.FuelCapacity;
      this.NewText("Fuel Capacity", num.ToString() + " t");
    }
    else if (this.module is HeatsinkModuleBP)
    {
      HeatsinkModuleBP module = this.module as HeatsinkModuleBP;
      this.NewText("Heat Capacity", module.HeatCapacity.ToString() + " MJ");
      this.NewText("Type", module.HeatsinkType.typeName);
    }
    else if (this.module is MissilesModuleBP)
      this.NewText("Missile", (this.module as MissilesModuleBP).MissilePrefab.name);
    else if (this.module is NoseModuleBP)
    {
      ModuleBP module = this.module;
      this.NewText("", "");
    }
    else if (this.module is SensorsModuleBP)
    {
      ModuleBP module = this.module;
      this.NewText("", "");
    }
    else
    {
      if (!(this.module is WeaponModuleBP))
        return;
      WeaponModuleBP module = this.module as WeaponModuleBP;
      this.NewText("Munitions", "x" + module.ResourceMax.ToString());
      ResupplyCycle resupplyCycle = module.ResupplyCycle;
      this.NewText("Resupply", $"{resupplyCycle.input.resource.resourceName} x{resupplyCycle.input.quantity.ToString()} -> {resupplyCycle.output.ToString()} munitions");
    }
  }

  private void SetMountData()
  {
    int count = this.module.Mounts.Count;
    if (count == 0)
    {
      this.mountsPanel.SetActive(false);
    }
    else
    {
      this.mountsPanel.SetActive(true);
      MountBP mount = this.module.Mounts[0];
      this.mountIcon.sprite = this.im.GetIconFor(mount.PartType);
      this.mountNameText.text = mount.PartNameFull;
      this.mountCountText.text = "x" + count.ToString();
      this.mountSizeText.text = mount.SizeClass.ToString();
      this.mountTypeText.text = mount.PartType.ToString();
      ResourceQuantity repairQuantity = this.module.RepairQuantity;
      this.mountRepairText.text = $"{repairQuantity.resource.resourceName} x{repairQuantity.quantity.ToString()}";
      this.mountMassText.text = mount.Mass.ToString() + "t";
      this.mountValueText.text = "cr";
      switch (mount)
      {
        case RadiatorMountBP _:
          RadiatorMountBP radiatorMountBp = mount as RadiatorMountBP;
          this.NewTextMount("Area", radiatorMountBp.Area.ToString() + " m^2");
          this.NewTextMount("Operating Temp", radiatorMountBp.OperatingTemp.ToString() + " K");
          this.NewTextMount("Heat Dissipation", radiatorMountBp.HeatDissipation.ToString() + " GW");
          break;
        case KineticMountBP _:
          break;
        default:
          LaserMountBP laserMountBp = mount as LaserMountBP;
          break;
      }
    }
  }
}
