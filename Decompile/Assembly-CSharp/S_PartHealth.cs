// Decompiled with JetBrains decompiler
// Type: S_PartHealth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class S_PartHealth : MonoBehaviour
{
  public bool isHealth;
  private S_Module2 module;
  [Header("UI")]
  [SerializeField]
  private Image moduleImg;
  [SerializeField]
  private Image mountPF;
  [SerializeField]
  private Transform mountsList;
  [SerializeField]
  private Gradient gradient;
  [Header("UI: Munitions")]
  [SerializeField]
  private TMP_Text moduleText;
  [SerializeField]
  private TMP_Text moduleResourcesText;
  [SerializeField]
  private Image mountImg;
  [SerializeField]
  private TMP_Text mountsText;
  [SerializeField]
  private TMP_Text mountsResourcesText;
  [Header("ICONS")]
  [Header("Modules")]
  [SerializeField]
  private Sprite cargo;
  [SerializeField]
  private Sprite crew;
  [SerializeField]
  private Sprite drive;
  [SerializeField]
  private Sprite ewar;
  [SerializeField]
  private Sprite fuel;
  [SerializeField]
  private Sprite heatsink;
  [SerializeField]
  private Sprite missiles;
  [SerializeField]
  private Sprite nose;
  [SerializeField]
  private Sprite nozzle;
  [SerializeField]
  private Sprite sensors;
  [SerializeField]
  private Sprite weapon;
  [Header("Mounts")]
  [SerializeField]
  private Sprite kinetics;
  [SerializeField]
  private Sprite laser;
  [SerializeField]
  private Sprite missile;
  [SerializeField]
  private Sprite pd;
  [SerializeField]
  private Sprite radiator;

  public void Setup(S_Module2 newModule)
  {
    this.module = newModule;
    this.moduleImg.sprite = this.GetIcon(this.module.bp.PartType);
    if (this.isHealth)
      this.SetupHealth();
    else
      this.SetupMunitions();
  }

  private void SetupHealth()
  {
    this.moduleImg.color = this.gradient.Evaluate(this.module.GetHealthRatio());
    this.moduleText.text = this.module.bp.PartNameShort;
    this.moduleResourcesText.text = $"{this.module.health.ToString("0.##")}/{100.ToString()}";
    this.ClearMountsList();
    if (this.module.mounts.Count == 0)
    {
      this.mountImg.gameObject.SetActive(false);
    }
    else
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      S_Mount2 mount1 = this.module.mounts[0];
      int num3 = 0;
      string partNameShort = mount1.bp.PartNameShort;
      foreach (S_Mount2 mount2 in this.module.mounts)
      {
        num1 += mount2.health;
        num2 += 100f;
        ++num3;
      }
      float time = (double) num2 != 0.0 ? Mathf.Clamp01(num1 / num2) : 0.0f;
      this.mountImg.sprite = this.GetIcon(mount1.bp.PartType);
      this.mountImg.color = this.gradient.Evaluate(time);
      this.mountsText.text = $"x{num3.ToString()} {partNameShort}";
      this.mountsResourcesText.text = $"{num1.ToString()}/{num2.ToString()}";
    }
  }

  private void SetupMunitions()
  {
    if ((UnityEngine.Object) this.module == (UnityEngine.Object) null || !(this.module.bp is IResupplyable))
      return;
    this.moduleImg.color = this.gradient.Evaluate(this.module.GetSupplyRatio());
    this.moduleText.text = this.module.bp.PartNameShort;
    this.moduleResourcesText.text = $"{this.module.supplies.ToString()}/{(this.module.bp as IResupplyable).GetResourceMax().ToString()}";
    this.ClearMountsList();
    if (this.module.mounts.Count == 0)
    {
      this.mountImg.gameObject.SetActive(false);
    }
    else
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      S_Mount2 mount1 = this.module.mounts[0];
      int num3 = 0;
      string partNameShort = mount1.bp.PartNameShort;
      foreach (S_Mount2 mount2 in this.module.mounts)
      {
        num1 += mount2.resource;
        if (mount2.bp is IResupplyable)
          num2 += (mount2.bp as IResupplyable).GetResourceMax();
        ++num3;
      }
      float time = (double) num2 != 0.0 ? Mathf.Clamp01(num1 / num2) : 0.0f;
      this.mountImg.sprite = this.GetIcon(mount1.bp.PartType);
      this.mountImg.color = this.gradient.Evaluate(time);
      this.mountsText.text = $"x{num3.ToString()} {partNameShort}";
      this.mountsResourcesText.text = $"{num1.ToString()}/{num2.ToString()}";
    }
  }

  private void ClearMountsList()
  {
    IEnumerator enumerator = (IEnumerator) this.mountsList.GetEnumerator();
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
  }

  private Sprite GetIcon(PartType type)
  {
    switch (type)
    {
      case PartType.DRIVE:
        return this.drive;
      case PartType.HEATSINK:
        return this.heatsink;
      case PartType.CARGO:
        return this.cargo;
      case PartType.CREW:
        return this.crew;
      case PartType.SENSORS:
        return this.sensors;
      case PartType.WEAPON:
        return this.weapon;
      case PartType.NOSE:
        return this.nose;
      case PartType.RADIATORS:
        return this.radiator;
      case PartType.KINETIC:
        return this.kinetics;
      case PartType.BEAM:
        return this.laser;
      case PartType.MISSILE:
        return this.missile;
      case PartType.EWAR:
        return this.ewar;
      case PartType.PD:
        return this.pd;
      case PartType.FUEL:
        return this.fuel;
      case PartType.NOZZLE:
        return this.nozzle;
      default:
        return (Sprite) null;
    }
  }
}
