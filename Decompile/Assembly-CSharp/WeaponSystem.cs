// Decompiled with JetBrains decompiler
// Type: WeaponSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WeaponSystem : T_Module, IInitable
{
  [Header("Weapon System")]
  public string weaponName;
  public WeaponType type;
  public Rigidbody parentRB;
  public Track parentTrack;
  public WeaponMode standardMode = WeaponMode.DEFENSE;
  public WeaponMode currentMode = WeaponMode.DEFENSE;
  public List<TurretWeapon> weapons = new List<TurretWeapon>();
  public int range;
  public bool isCeaseFire;

  public override void Start()
  {
    base.Start();
    this.currentMode = this.standardMode;
  }

  public override void SetIsDead()
  {
    base.SetIsDead();
    foreach (TurretWeapon weapon in this.weapons)
      this.WeaponSetTarget(weapon, (Track) null);
  }

  public override void SetOn(bool isOn)
  {
    base.SetOn(isOn);
    foreach (T_Mount weapon in this.weapons)
      weapon.isOn = isOn;
  }

  public virtual void Init(Track newTrack, Rigidbody newRB)
  {
    this.parentTrack = newTrack;
    this.parentRB = newRB;
    foreach (TurretWeapon componentsInChild in this.transform.GetComponentsInChildren<TurretWeapon>())
    {
      if (!this.weapons.Contains(componentsInChild))
        this.weapons.Add(componentsInChild);
      componentsInChild.parentRB = this.parentRB;
    }
    if (this.weapons.Count <= 0)
      return;
    this.range = this.weapons[0].range;
  }

  public override void InitBP()
  {
    MonoBehaviour.print((object) ("override weapon system initbp: " + this.name));
    base.InitBP();
    if (!(this.BP is WeaponModuleBP))
      return;
    this.resourceMax = (this.BP as WeaponModuleBP).ResourceMax;
  }

  public void AllTarget(Track target)
  {
    if (this.isDead || !this.isOn || this.isCeaseFire || (double) this.healthRatio <= 0.0)
      return;
    foreach (TurretWeapon weapon in this.weapons)
    {
      if (weapon.CanSee(target))
        this.WeaponSetTarget(weapon, target);
    }
  }

  public bool TryTarget(Track newTarget)
  {
    if (this.isDead || !this.isOn || this.isCeaseFire || (double) this.healthRatio <= 0.0)
      return false;
    if (this.type == WeaponType.MISSILE)
      return true;
    bool flag = false;
    foreach (TurretWeapon weapon in this.weapons)
    {
      if (!(bool) (Object) weapon.target)
      {
        if (flag = weapon.CanSee(newTarget))
          this.WeaponSetTarget(weapon, newTarget);
        else
          this.WeaponSetTarget(weapon, (Track) null);
      }
    }
    return flag;
  }

  private void WeaponSetTarget(TurretWeapon weapon, Track target)
  {
    if ((double) weapon.resource <= 0.0 && !weapon.hasInfiniteAmmo)
    {
      if ((double) this.resource >= (double) weapon.resourceMax)
      {
        this.resource -= (float) weapon.resourceMax;
        weapon.resource = (float) weapon.resourceMax;
        double num = (double) Mathf.Clamp(weapon.resource, 0.0f, (float) weapon.resourceMax);
      }
      else
      {
        weapon.resource = this.resource;
        this.resource = 0.0f;
      }
    }
    if ((double) weapon.resource <= 0.0 && !weapon.hasInfiniteAmmo || !((Object) weapon.target == (Object) null))
      return;
    weapon.SetTarget(target);
  }

  public void SetMode(WeaponMode newMode)
  {
    this.currentMode = newMode;
    foreach (TurretWeapon weapon in this.weapons)
      weapon.SetTarget((Track) null);
  }

  public void SetCeaseFire(bool isOn)
  {
    this.isCeaseFire = isOn;
    foreach (TurretWeapon weapon in this.weapons)
      weapon.SetTarget((Track) null);
  }
}
