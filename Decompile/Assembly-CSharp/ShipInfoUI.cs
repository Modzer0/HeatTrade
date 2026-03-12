// Decompiled with JetBrains decompiler
// Type: ShipInfoUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ShipInfoUI : MonoBehaviour
{
  private ShipInfoManager sim;
  private S_Ship ship;
  private FleetPanels fps;
  private FactionsManager fm;
  [Header("= UI ====================")]
  [Header("Header")]
  [SerializeField]
  private Image factionImg;
  [SerializeField]
  private TMP_Text idText;
  [SerializeField]
  private TMP_InputField nameInput;
  [SerializeField]
  private TMP_Text classText;
  [Header("Navigation")]
  [SerializeField]
  private TMP_Text armorThicknessText;
  [SerializeField]
  private TMP_Text armorText;
  [SerializeField]
  private TMP_Text currentText;
  [SerializeField]
  private TMP_Text dryText;
  [SerializeField]
  private TMP_Text fullText;
  [SerializeField]
  private TMP_Text totalMassText;
  [SerializeField]
  private TMP_Text cruiseAccText;
  [SerializeField]
  private TMP_Text combatAccText;
  [SerializeField]
  private TMP_Text dvText;
  [SerializeField]
  private TMP_Text dvMaxText;
  [SerializeField]
  private TMP_Text fuelText;
  [SerializeField]
  private TMP_Text fuelMassText;
  [SerializeField]
  private uiBar fuelBar;
  private Image fuelBG;
  [Header("MODULES")]
  [SerializeField]
  private uiBar healthBar;
  [SerializeField]
  private TMP_Text healthText;
  [SerializeField]
  private Transform modulesList;
  [SerializeField]
  private RectTransform modulesPanel;
  [SerializeField]
  private S_PartHealth partHealthPF;
  [Header("MUNITIONS")]
  [SerializeField]
  private Transform munitionsList;
  [SerializeField]
  private RectTransform munitionsPanel;
  [SerializeField]
  private S_PartHealth partMunitionsPF;
  [Header("COLORS")]
  [SerializeField]
  private Color greenLight;
  [SerializeField]
  private Color greenMid;
  [SerializeField]
  private Color greenDark;
  [SerializeField]
  private Color redLight;
  [SerializeField]
  private Color redMid;
  [SerializeField]
  private Color redDark;
  [SerializeField]
  private Color violetLight;
  [SerializeField]
  private Color violetMid;
  [SerializeField]
  private Color violetDark;
  private bool isInit;

  private void Start() => this.Init();

  private void Init()
  {
    if (this.isInit)
      return;
    this.sim = ShipInfoManager.current;
    this.fps = FleetPanels.current;
    this.fm = FactionsManager.current;
    this.fuelBG = this.fuelBar.transform.parent.GetComponent<Image>();
    this.isInit = true;
  }

  private void Update()
  {
    if ((double) this.modulesPanel.sizeDelta.y != (double) this.modulesList.GetComponent<RectTransform>().sizeDelta.y)
      this.modulesPanel.sizeDelta = new Vector2(this.modulesPanel.sizeDelta.x, this.modulesList.GetComponent<RectTransform>().sizeDelta.y);
    if ((double) this.munitionsPanel.sizeDelta.y == (double) this.modulesList.GetComponent<RectTransform>().sizeDelta.y)
      return;
    this.munitionsPanel.sizeDelta = new Vector2(this.munitionsPanel.sizeDelta.x, this.munitionsList.GetComponent<RectTransform>().sizeDelta.y);
  }

  public void Setup(S_Ship newShip)
  {
    this.Init();
    this.ship = newShip;
    if (!(bool) (Object) this.ship)
      return;
    this.factionImg.sprite = this.fm.GetFactionFromID(this.ship.factionID).factionIcon;
    this.idText.text = this.ship.trackID;
    this.nameInput.text = this.ship.publicName;
    this.classText.text = this.ship.bp.GetFullClassName();
    TMP_Text armorThicknessText = this.armorThicknessText;
    int num = (int) this.ship.bp.ArmorThickness;
    string str1 = $"Thickness: {num.ToString()}cm";
    armorThicknessText.text = str1;
    TMP_Text armorText = this.armorText;
    num = (int) this.ship.armorMass;
    string str2 = num.ToString() + "t";
    armorText.text = str2;
    TMP_Text currentText = this.currentText;
    num = (int) this.ship.currentMass;
    string str3 = num.ToString() + "t";
    currentText.text = str3;
    TMP_Text dryText = this.dryText;
    num = (int) this.ship.dryMass;
    string str4 = num.ToString() + "t";
    dryText.text = str4;
    TMP_Text fullText = this.fullText;
    num = (int) this.ship.fullMass;
    string str5 = num.ToString() + "t";
    fullText.text = str5;
    TMP_Text totalMassText = this.totalMassText;
    num = (int) this.ship.fullMass;
    string str6 = num.ToString() + "t";
    totalMassText.text = str6;
    this.cruiseAccText.text = this.ship.maxAcceleration.ToString("N") + "m/s\u00B2";
    this.combatAccText.text = (this.ship.maxAcceleration * 10f).ToString("N") + "m/s\u00B2";
    float dv = 0.0f;
    float dvMax = 0.0f;
    this.ship.GetDvAndMax(out dv, out dvMax);
    this.dvText.text = dv.ToString("N") + "km/s";
    this.dvMaxText.text = dvMax.ToString("N") + "km/s";
    string resourceName = this.ship.GetFuelResource().resourceName;
    this.fuelText.text = $"FUEL ({resourceName})";
    this.fuelMassText.text = $"{this.ship.GetFuelMass().ToString("F2")}/{this.ship.GetFuelMax().ToString("F2")}t";
    this.fuelBar.SetBarSize(this.ship.GetFuelRatio());
    switch (resourceName)
    {
      case "DT Fuel":
        this.fuelBG.color = this.greenDark;
        this.fuelBar.GetComponent<Image>().color = this.greenMid;
        this.fuelText.color = this.greenLight;
        this.fuelMassText.color = this.greenLight;
        break;
      case "DH Fuel":
        this.fuelBG.color = this.redDark;
        this.fuelBar.GetComponent<Image>().color = this.redMid;
        this.fuelText.color = this.redLight;
        this.fuelMassText.color = this.redLight;
        break;
      case "Antimatter Fuel":
        this.fuelBG.color = this.violetDark;
        this.fuelBar.GetComponent<Image>().color = this.violetMid;
        this.fuelText.color = this.violetLight;
        this.fuelMassText.color = this.violetLight;
        break;
    }
    this.healthBar.SetBarSize(this.ship.GetHealthRatio());
    this.healthText.text = (this.ship.GetHealthRatio() * 100f).ToString("0.##") + "%";
    foreach (S_Module2 module in this.ship.modules)
    {
      S_PartHealth component = Object.Instantiate<S_PartHealth>(this.partHealthPF, this.modulesList).GetComponent<S_PartHealth>();
      component.Setup(module);
      component.gameObject.SetActive(true);
    }
    foreach (S_Module2 module in this.ship.modules)
    {
      if (module.bp is IResupplyable)
      {
        S_PartHealth component = Object.Instantiate<S_PartHealth>(this.partMunitionsPF, this.munitionsList).GetComponent<S_PartHealth>();
        component.Setup(module);
        component.gameObject.SetActive(true);
      }
    }
  }

  public void Close() => this.sim.DestroyMe(this);

  public void RenameShip()
  {
    this.ship.publicName = this.nameInput.text;
    this.fps.UpdateData();
  }
}
