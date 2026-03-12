// Decompiled with JetBrains decompiler
// Type: MissileUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using TMPro;
using UnityEngine;

#nullable disable
public class MissileUI : WeaponUI
{
  private MissileSystem ms;
  private AudioManager am;
  [SerializeField]
  private TMP_Text nameText;
  [SerializeField]
  private TMP_Text targetText;
  [SerializeField]
  private TMP_Text deltaVText;
  [SerializeField]
  private TMP_Text salvoCountText;
  [SerializeField]
  private TMP_Dropdown missilesDropdown;
  [SerializeField]
  private TMP_Dropdown modeDropdown;
  private int modeInt;
  [SerializeField]
  private Transform barsHLG;
  [SerializeField]
  private GameObject barPF;
  [SerializeField]
  private GameObject barEmptyPF;
  [SerializeField]
  private GameObject noTargetCover;
  private int missilesRemaining;
  private int missilesMax;

  private void Start() => this.am = AudioManager.current;

  private void Update() => this.UpdateData();

  public void ModSalvoCount(int mod)
  {
    int missileCount = this.ms.GetMissileCount();
    MonoBehaviour.print((object) $"mod salvo count: {mod.ToString()}/{missileCount.ToString()}");
    if (missileCount == 0)
    {
      this.ms.salvoCount = 0;
      MonoBehaviour.print((object) "NO MISSILES!");
    }
    else
    {
      if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
      {
        if (mod > 0)
          mod = missileCount;
        else if (mod < 0)
          mod = 1;
      }
      else if (Input.GetKey(KeyCode.LeftShift))
        mod *= 5;
      else if (Input.GetKey(KeyCode.LeftControl))
        mod *= 2;
      if (mod < 0 && this.ms.salvoCount + mod <= 0)
        this.ms.salvoCount = missileCount;
      else if (mod > 0 && this.ms.salvoCount + mod > missileCount)
        this.ms.salvoCount = 1;
      else
        this.ms.salvoCount += mod;
      MonoBehaviour.print((object) ("MISSILE SALVO COUNT: " + this.ms.salvoCount.ToString()));
      this.am.PlaySFX(0);
    }
  }

  public void SetMode()
  {
    ++this.modeInt;
    if (this.modeInt > 2)
      this.modeInt = 0;
    MissileMode missileMode = MissileMode.INTERCEPT;
    if (this.modeInt == 0)
      missileMode = MissileMode.STRIKE;
    else if (this.modeInt == 1)
      missileMode = MissileMode.FLANK;
    else if (this.modeInt == 2)
      missileMode = MissileMode.MOVE;
    if (missileMode == this.ms.missileMode)
      return;
    this.ms.missileMode = missileMode;
    this.am.PlaySFX(0);
  }

  public void LaunchSalvo()
  {
    this.ms.LaunchSalvo();
    this.am.PlaySFX(3);
  }

  public override void AssignWeaponSystem(WeaponSystem nws)
  {
    this.ms = nws.GetComponent<MissileSystem>();
    this.nameText.text = this.ms.weaponName;
    this.deltaVText.text = this.ms.deltaV.ToString() + "m";
    this.missilesMax = (int) this.ms.resourceMax;
    this.UpdateBars();
  }

  public override void UpdateData()
  {
    if ((UnityEngine.Object) this.ms == (UnityEngine.Object) null || (UnityEngine.Object) this.ms.target == (UnityEngine.Object) null)
    {
      this.targetText.text = "NULL";
      this.targetText.color = Color.red;
      this.noTargetCover.SetActive(true);
    }
    else
    {
      this.noTargetCover.SetActive(false);
      this.targetText.text = this.ms.target.trackName;
      this.targetText.color = Color.white;
      this.salvoCountText.text = this.ms.salvoCount.ToString() ?? "";
      float resource = (float) (int) this.ms.resource;
      if ((double) resource == (double) this.missilesRemaining)
        return;
      this.missilesRemaining = (int) resource;
      this.UpdateBars();
    }
  }

  private void UpdateBars()
  {
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
    for (int index = 0; index < this.ms.GetMissileCount(); ++index)
      UnityEngine.Object.Instantiate<GameObject>(this.barPF, this.barsHLG).SetActive(true);
    for (int index = 0; (double) index < (double) this.ms.resourceMax - (double) this.ms.GetMissileCount(); ++index)
      UnityEngine.Object.Instantiate<GameObject>(this.barEmptyPF, this.barsHLG).SetActive(true);
  }
}
