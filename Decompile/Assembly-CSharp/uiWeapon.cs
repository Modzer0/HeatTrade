// Decompiled with JetBrains decompiler
// Type: uiWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class uiWeapon : MonoBehaviour
{
  [SerializeField]
  private TMP_Text uiName;
  [SerializeField]
  private TMP_Text uiPosition;
  [SerializeField]
  private TMP_Text uiStatus;
  [SerializeField]
  private TMP_Text uiTarget;
  [SerializeField]
  private TMP_Text uiAmmo;
  private Weapon weapon;

  private void Start()
  {
  }

  private void Update()
  {
    this.UpdateUIData();
    this.UpdateUIAmmo();
  }

  public void SetWeapon(Weapon newWeapon)
  {
    this.weapon = newWeapon;
    this.UpdateUIData();
    this.UpdateUIAmmo();
  }

  public void UpdateUIData()
  {
    this.uiName.text = this.weapon.weaponName;
    this.uiPosition.text = this.weapon.position;
    this.uiStatus.text = this.weapon.status;
    this.uiTarget.text = this.weapon.targetName;
  }

  public void UpdateUIAmmo() => this.uiAmmo.text = this.weapon.ammoCount.ToString() ?? "";

  public void SetTarget()
  {
    if (!(bool) (Object) Sensors.current.selected)
      return;
    this.weapon.SetTarget(Sensors.current.selected.GetComponent<Target>());
  }

  public void CeaseFire() => this.weapon.CeaseFire();
}
