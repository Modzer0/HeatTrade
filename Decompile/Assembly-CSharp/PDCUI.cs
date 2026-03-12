// Decompiled with JetBrains decompiler
// Type: PDCUI
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
public class PDCUI : WeaponUI
{
  private AudioManager am;
  private WeaponSystem weaponSystem;
  [SerializeField]
  private TMP_Text nameText;
  [SerializeField]
  private TMP_Text targetText;
  [SerializeField]
  private TMP_Text rangeText;
  [SerializeField]
  private Toggle onToggle;
  [SerializeField]
  private Toggle ceaseFireToggle;
  [SerializeField]
  private Toggle defenseToggle;
  [SerializeField]
  private Toggle offenseToggle;
  [SerializeField]
  private Toggle targetToggle;
  [SerializeField]
  private Toggle nearestToggle;
  [SerializeField]
  private Transform barsHLG;
  [SerializeField]
  private GameObject barPF;
  [SerializeField]
  private GameObject barEmptyPF;
  [SerializeField]
  private List<T_Mount> mounts = new List<T_Mount>();

  private void Start() => this.am = AudioManager.current;

  private void Update()
  {
    if (!(bool) (UnityEngine.Object) this.am)
      this.am = AudioManager.current;
    this.UpdateData();
  }

  public override void AssignWeaponSystem(WeaponSystem nws)
  {
    this.weaponSystem = nws;
    this.nameText.text = this.weaponSystem.weaponName;
    this.rangeText.text = (this.weaponSystem.range * 10 / 1000).ToString() + "km";
    this.SetModeToggle(this.weaponSystem.currentMode);
    this.SetCeaseFire(this.weaponSystem.isCeaseFire);
    this.SetOn(this.weaponSystem.isOn);
    IEnumerator enumerator = (IEnumerator) this.barsHLG.GetEnumerator();
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
    foreach (TurretWeapon weapon in this.weaponSystem.weapons)
    {
      UnityEngine.Object.Instantiate<GameObject>(this.barPF, this.barsHLG).SetActive(true);
      if ((bool) (UnityEngine.Object) weapon.GetComponent<T_Mount>())
        this.mounts.Add(weapon.GetComponent<T_Mount>());
    }
  }

  public override void UpdateData()
  {
    this.targetText.text = "";
    int count = this.mounts.Count;
    if (this.barsHLG.childCount == 0)
      return;
    for (int index = 0; index < count; ++index)
    {
      float num = this.mounts[index].resource / (float) this.mounts[index].resourceMax;
      if (float.IsNaN(num))
        num = 0.0f;
      if ((double) this.mounts[index].resource == 0.0)
        this.barsHLG.GetChild(index).transform.localScale = new Vector3(num, 1f, 1f);
      else
        this.barsHLG.GetChild(index).transform.localScale = new Vector3(num, 1f, 1f);
    }
    this.ceaseFireToggle.isOn = this.weaponSystem.isCeaseFire;
    this.onToggle.isOn = this.weaponSystem.isOn;
    this.SetModeToggle(this.weaponSystem.currentMode);
  }

  public void SetMode(string newModeString)
  {
    WeaponMode newMode = WeaponMode.DEFENSE;
    switch (newModeString)
    {
      case "Offense":
        newMode = WeaponMode.OFFENSE;
        break;
      case "Target":
        newMode = WeaponMode.TARGET;
        break;
      case "Nearest":
        newMode = WeaponMode.NEAREST;
        break;
    }
    if (this.weaponSystem.currentMode != newMode)
      this.weaponSystem.SetMode(newMode);
    if (!(bool) (UnityEngine.Object) this.am)
      return;
    this.am.PlaySFX(0);
  }

  private void SetModeToggle(WeaponMode newMode)
  {
    switch (newMode)
    {
      case WeaponMode.OFFENSE:
        this.offenseToggle.isOn = true;
        break;
      case WeaponMode.TARGET:
        this.targetToggle.isOn = true;
        break;
      case WeaponMode.NEAREST:
        this.nearestToggle.isOn = true;
        break;
      default:
        this.defenseToggle.isOn = true;
        break;
    }
  }

  public void SetCeaseFire(bool isOn)
  {
    this.ceaseFireToggle.isOn = isOn;
    this.weaponSystem.SetCeaseFire(isOn);
    if (!(bool) (UnityEngine.Object) this.am)
      return;
    this.am.PlaySFX(0);
  }

  public void SetOn(bool isOn)
  {
    this.onToggle.isOn = isOn;
    this.weaponSystem.SetOn(isOn);
    if (!(bool) (UnityEngine.Object) this.am)
      return;
    this.am.PlaySFX(0);
  }
}
