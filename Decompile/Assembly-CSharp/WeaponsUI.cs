// Decompiled with JetBrains decompiler
// Type: WeaponsUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class WeaponsUI : MonoBehaviour
{
  private TacticalInputs ti;
  [SerializeField]
  private GameObject weaponsPanel;
  [SerializeField]
  private WeaponUI pdcPF;
  [SerializeField]
  private WeaponUI railgunPF;
  [SerializeField]
  private WeaponUI coilgunPF;
  [SerializeField]
  private WeaponUI missilePF;
  [SerializeField]
  private Transform weaponsList;

  private void Start()
  {
    this.ti = TacticalInputs.current;
    this.ti.newSelection += new Action(this.NewShipSelected);
  }

  private void Update()
  {
    int num = this.weaponsPanel.activeSelf ? 1 : 0;
  }

  private void NewShipSelected()
  {
    IEnumerator enumerator = (IEnumerator) this.weaponsList.GetEnumerator();
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
    if ((UnityEngine.Object) this.ti.selectedShip == (UnityEngine.Object) null)
      return;
    foreach (WeaponSystem weapon in this.ti.selectedShip.weapons)
    {
      if (weapon.type == WeaponType.PDC || weapon.type == WeaponType.RAILGUN || weapon.type == WeaponType.COILGUN || weapon.type == WeaponType.LASER)
      {
        WeaponUI weaponUi = UnityEngine.Object.Instantiate<WeaponUI>(this.pdcPF, this.weaponsList);
        weaponUi.AssignWeaponSystem(weapon);
        weaponUi.gameObject.SetActive(true);
      }
      else if (weapon.type == WeaponType.MISSILE)
      {
        WeaponUI weaponUi = UnityEngine.Object.Instantiate<WeaponUI>(this.missilePF, this.weaponsList);
        weaponUi.AssignWeaponSystem(weapon);
        weaponUi.gameObject.SetActive(true);
      }
    }
  }
}
