// Decompiled with JetBrains decompiler
// Type: SkirmishSetup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SkirmishSetup : MonoBehaviour
{
  public static SkirmishSetup current;
  [SerializeField]
  private BlueprintLibrary bl;
  private SceneTransitionManager stm;
  private ShipSpawner shipSpawner;
  [SerializeField]
  private MainMenu mm;
  private AudioManager am;
  [SerializeField]
  private EndScreen es;
  public SkirmishMapName skirmishMapName;
  [SerializeField]
  private FleetManager playerFleet;
  [SerializeField]
  private FleetManager hostileFleet;
  [Header("UI")]
  [Header("FLEET LISTS ========================================")]
  [SerializeField]
  private Transform playerFleetList;
  [SerializeField]
  private Transform hostileFleetList;
  [SerializeField]
  private GameObject smallShipDataPF;
  [Header("UI")]
  [Header("SHIP AVAILABLE ========================================")]
  [SerializeField]
  private Transform allShipsList;
  private ToggleGroup allShipsToggleGroup;
  [SerializeField]
  private GameObject shipAvailablePF;
  private ShipBP selected;
  private List<ModuleBP> modules = new List<ModuleBP>();
  private FuelModuleBP fuel;
  private DriveModuleBP drive;
  private float dryMass;
  private float fullMass;
  private float cruiseAcc;
  private float combatAcc;
  private float dvMax;
  private float armorThickness;
  [Header("SHIP INFO ========================================")]
  [Header("HEADER")]
  [SerializeField]
  private GameObject shipInfoPanel;
  [SerializeField]
  private Image selectedImage;
  [SerializeField]
  private TMP_InputField selectedNameInput;
  [SerializeField]
  private TMP_Text selectedClassText;
  [SerializeField]
  private TMP_Text selectedDescriptionText;
  [Header("STATS")]
  [SerializeField]
  private TMP_Text dryMassText;
  [SerializeField]
  private TMP_Text fullMassText;
  [SerializeField]
  private TMP_Text cruiseAccText;
  [SerializeField]
  private TMP_Text combatAccText;
  [SerializeField]
  private TMP_Text dvText;
  [SerializeField]
  private TMP_Text armorText;
  [Header("MODULES")]
  [SerializeField]
  private ShipyardModuleCard moduleCardPF;
  [SerializeField]
  private Transform modulesList;

  private void Awake() => SkirmishSetup.current = this;

  private void Start()
  {
    this.stm = SceneTransitionManager.current;
    this.shipSpawner = ShipSpawner.current;
    this.am = AudioManager.current;
    this.allShipsToggleGroup = this.allShipsList.GetComponent<ToggleGroup>();
    foreach (ShipBP shipBlueprint in this.bl.shipBlueprints)
    {
      ShipAvailable component = UnityEngine.Object.Instantiate<GameObject>(this.shipAvailablePF, this.allShipsList).GetComponent<ShipAvailable>();
      component.GetComponent<Toggle>().group = this.allShipsToggleGroup;
      component.isSkirmish = true;
      component.Setup(shipBlueprint);
    }
    this.UpdateFleetLists();
  }

  private void UpdateFleetLists()
  {
    MonoBehaviour.print((object) ("updating fleet lists. ships in player fleet: " + this.playerFleet.ships.Count.ToString()));
    IEnumerator enumerator1 = (IEnumerator) this.playerFleetList.GetEnumerator();
    try
    {
      while (enumerator1.MoveNext())
        UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator1.Current).gameObject);
    }
    finally
    {
      if (enumerator1 is IDisposable disposable)
        disposable.Dispose();
    }
    IEnumerator enumerator2 = (IEnumerator) this.hostileFleetList.GetEnumerator();
    try
    {
      while (enumerator2.MoveNext())
        UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator2.Current).gameObject);
    }
    finally
    {
      if (enumerator2 is IDisposable disposable)
        disposable.Dispose();
    }
    foreach (S_Ship ship in this.playerFleet.ships)
    {
      MonoBehaviour.print((object) ("player ship: " + ship.name));
      UnityEngine.Object.Instantiate<GameObject>(this.smallShipDataPF, this.playerFleetList).GetComponent<S_ShipDataUI>().SetStructure(ship);
    }
    foreach (S_Ship ship in this.hostileFleet.ships)
    {
      MonoBehaviour.print((object) ("hostile ship: " + ship.name));
      UnityEngine.Object.Instantiate<GameObject>(this.smallShipDataPF, this.hostileFleetList).GetComponent<S_ShipDataUI>().SetStructure(ship);
    }
  }

  public void ClearFleets()
  {
    foreach (Component ship in this.playerFleet.ships)
      UnityEngine.Object.Destroy((UnityEngine.Object) ship.gameObject);
    this.playerFleet.ships.Clear();
    foreach (Component ship in this.hostileFleet.ships)
      UnityEngine.Object.Destroy((UnityEngine.Object) ship.gameObject);
    this.hostileFleet.ships.Clear();
    this.UpdateFleetLists();
  }

  private void GetShipData()
  {
    float fuelCapacity = this.fuel.FuelCapacity;
    this.armorThickness = this.selected.ArmorThickness;
    float num1 = (float) ((double) this.armorThickness * 60.0 / 1000.0);
    float num2 = 0.0f;
    float num3 = 0.0f;
    foreach (ModuleBP module in this.modules)
    {
      float num4 = num1 * 3.14159274f * module.Diameter * module.Length;
      num2 += num4;
      num3 += module.Mass + num4;
    }
    this.dryMass = num3 - fuelCapacity;
    this.fullMass = num3;
    this.dvMax = (float) ((double) this.drive.ExhaustVelocity * (double) Mathf.Log(this.fullMass / this.dryMass) / 1000.0);
    float cruiseThrustN = this.drive.CruiseThrustN;
    float num5 = this.fullMass * 1000f;
    this.cruiseAcc = cruiseThrustN / num5;
    this.combatAcc = cruiseThrustN * 100f / num5;
  }

  public void SelectShip(ShipBP newSelected)
  {
    MonoBehaviour.print((object) ("SELECT SHIP: " + newSelected.name));
    this.shipInfoPanel.SetActive(true);
    this.selected = newSelected;
    this.selectedImage.sprite = this.selected.Icon;
    this.selectedClassText.text = $"{this.selected.ClassName}-class {this.selected.TypeFull}";
    this.selectedDescriptionText.text = this.selected.Description;
    this.modules.Clear();
    IEnumerator enumerator = (IEnumerator) this.modulesList.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if (!((UnityEngine.Object) current == (UnityEngine.Object) null))
          UnityEngine.Object.Destroy((UnityEngine.Object) current.gameObject);
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    foreach (ModuleBP module in this.selected.Modules)
    {
      UnityEngine.Object.Instantiate<ShipyardModuleCard>(this.moduleCardPF, this.modulesList).Setup(module);
      this.modules.Add(module);
      if (module is FuelModuleBP fuelModuleBp)
        this.fuel = fuelModuleBp;
      else if (module is DriveModuleBP driveModuleBp)
        this.drive = driveModuleBp;
    }
    this.GetShipData();
    this.dryMassText.text = this.dryMass.ToString("N") + " t";
    this.fullMassText.text = this.fullMass.ToString("N") + " t";
    this.dvText.text = this.dvMax.ToString("N") + " km/s";
    this.cruiseAccText.text = this.cruiseAcc.ToString("N") + " m/s^2";
    this.combatAccText.text = this.combatAcc.ToString("N") + " m/s^2";
    this.armorText.text = this.armorThickness.ToString() + " cm";
  }

  public void BuySelectedShip(int factionID)
  {
    MonoBehaviour.print((object) "BUY SHIP");
    if (this.selectedNameInput.text == "" || string.IsNullOrEmpty(this.selectedNameInput.text))
      this.selectedNameInput.text = this.selected.ClassName;
    S_Ship sShip = this.shipSpawner.SpawnShip(this.selected.PrefabKey, factionID, this.selectedNameInput.text);
    if (!(bool) (UnityEngine.Object) sShip)
    {
      MonoBehaviour.print((object) "SHIP NOT SPAWNED");
    }
    else
    {
      if (factionID == 1)
        sShip.transform.SetParent(this.playerFleet.transform);
      else
        sShip.transform.SetParent(this.hostileFleet.transform);
      this.playerFleet.UpdateFleet();
      this.hostileFleet.UpdateFleet();
      this.selectedNameInput.text = "";
      this.UpdateFleetLists();
      this.shipInfoPanel.SetActive(false);
    }
  }

  public void StartSkirmish()
  {
    if (this.playerFleet.ships.Count == 0 || this.hostileFleet.ships.Count == 0)
    {
      this.am.PlaySFX(3);
    }
    else
    {
      this.mm.NewGameSkirmish();
      this.stm.gst = this.stm.skirmishGST;
      this.stm.skirmishMapName = this.skirmishMapName;
      this.stm.SetupEngagement(new List<TacticalGroupData>()
      {
        this.playerFleet.ToTacticalGroup(),
        this.hostileFleet.ToTacticalGroup()
      }, Vector3.zero);
      this.stm.NewEngagement();
    }
  }

  public void SetSkirmishMapName(SkirmishMapName newName)
  {
    MonoBehaviour.print((object) ("set skirmish map: " + newName.ToString()));
    this.skirmishMapName = newName;
  }
}
