// Decompiled with JetBrains decompiler
// Type: Shipyard
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
public class Shipyard : MonoBehaviour
{
  public static Shipyard current;
  private FactionsManager fm;
  private StationHarborUI shui;
  private ShipSpawner shipSpawner;
  private AudioManager am;
  private bool isOn;
  private StationManager station;
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
  [Header("SHIP AVAILABLE ========================================")]
  [SerializeField]
  private List<ShipBP> shipsAvailable;
  [SerializeField]
  private Transform shipsList;
  [SerializeField]
  private ShipAvailable availableShipButtonPF;
  [SerializeField]
  private ToggleGroup availableToggleGroup;
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
  [SerializeField]
  private TMP_Text selectedPriceText;
  [SerializeField]
  private Button buyButton;
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
  [Header("SHIP BUYING ========================================")]
  [SerializeField]
  private GameObject shipPF;
  public Action onPlayerBuyShip;

  private void Awake() => Shipyard.current = this;

  private void Start()
  {
    this.fm = FactionsManager.current;
    this.shui = StationHarborUI.current;
    this.shipSpawner = ShipSpawner.current;
    this.shipsAvailable = this.shipSpawner.ShipBPs;
    this.am = AudioManager.current;
    IEnumerator enumerator = (IEnumerator) this.shipsList.GetEnumerator();
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
    foreach (ShipBP newShip in this.shipsAvailable)
    {
      ShipAvailable shipAvailable = UnityEngine.Object.Instantiate<ShipAvailable>(this.availableShipButtonPF, this.shipsList);
      shipAvailable.GetComponent<Toggle>().group = this.availableToggleGroup;
      shipAvailable.Setup(newShip);
    }
    this.shipInfoPanel.SetActive(false);
  }

  private void Update()
  {
    if ((bool) (UnityEngine.Object) this.selected && this.fm.playerFaction.credits >= this.selected.Value)
      this.buyButton.interactable = true;
    else
      this.buyButton.interactable = false;
  }

  public void SelectShip(ShipBP newSelected)
  {
    this.shipInfoPanel.SetActive(true);
    this.selected = newSelected;
    this.selectedImage.sprite = this.selected.Icon;
    this.selectedClassText.text = $"{this.selected.ClassName}-class {this.selected.TypeFull}";
    this.selectedDescriptionText.text = this.selected.Description;
    this.selectedPriceText.text = this.selected.Value.ToString("N") + " cr";
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

  public void SetOn(bool newIsOn)
  {
    this.isOn = newIsOn;
    if (!this.isOn)
      return;
    this.SelectShip(this.shipsAvailable[0]);
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

  public void BuySelectedShip()
  {
    MonoBehaviour.print((object) "BUY SHIP");
    if (this.selectedNameInput.text == "" || string.IsNullOrEmpty(this.selectedNameInput.text))
      this.am.PlaySFX(6);
    else if (this.fm.playerFaction.credits < this.selected.Value)
    {
      this.am.PlaySFX(6);
    }
    else
    {
      this.fm.ModPlayerCredits("SHIP BOUGHT", -this.selected.Value);
      this.am.PlaySFX(2);
      S_Ship sShip = this.shipSpawner.SpawnShip(this.selected.PrefabKey, this.fm.playerFaction.factionID, this.selectedNameInput.text);
      if (!(bool) (UnityEngine.Object) sShip)
        return;
      this.shui.TryTransferShip(sShip.transform);
      this.StartCoroutine((IEnumerator) this.DelayTransfer());
      this.selectedNameInput.text = "";
      Action onPlayerBuyShip = this.onPlayerBuyShip;
      if (onPlayerBuyShip == null)
        return;
      onPlayerBuyShip();
    }
  }

  private IEnumerator DelayTransfer()
  {
    yield return (object) new WaitForSeconds(0.1f);
    if (this.shui.HasPlayerFleet())
    {
      if (this.shui.GetPlayerFleet().ships.Count >= 10)
        this.TransferToNewFleet();
      else
        this.TransferToOldFleet();
    }
    else
      this.TransferToNewFleet();
  }

  private void TransferToOldFleet() => this.shui.TransferToThisFleet(this.shui.GetPlayerFleet());

  private void TransferToNewFleet()
  {
    this.shui.TransferToThisFleet(this.shui.NewFleetCustom("New Fleet"));
  }
}
