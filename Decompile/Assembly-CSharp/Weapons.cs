// Decompiled with JetBrains decompiler
// Type: Weapons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Weapons : MonoBehaviour
{
  [SerializeField]
  private List<Weapon> weapons;
  [SerializeField]
  private GameObject uiWeaponPF;
  [SerializeField]
  private Transform uiWeapons;

  private void Start() => this.UpdateWeaponsList();

  private void Update()
  {
  }

  private void UpdateWeaponsList()
  {
    foreach (Weapon weapon in this.weapons)
    {
      uiWeapon component = Object.Instantiate<GameObject>(this.uiWeaponPF, this.uiWeapons).GetComponent<uiWeapon>();
      component.SetWeapon(weapon);
      component.gameObject.SetActive(true);
    }
  }

  public void SetAllTarget()
  {
    foreach (Weapon weapon in this.weapons)
      weapon.SetTarget(Sensors.current.selected.GetComponent<Target>());
  }

  public void CeaseAll()
  {
    foreach (Weapon weapon in this.weapons)
      weapon.CeaseFire();
  }
}
