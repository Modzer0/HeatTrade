// Decompiled with JetBrains decompiler
// Type: PartHealthUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PartHealthUI : MonoBehaviour
{
  private PartHealthUI.PartHealthState state;
  private T_Module module;
  private List<T_Mount> mounts;
  private ShipController ship;
  private float healthRatio;
  [SerializeField]
  private Gradient healthGradient;
  [SerializeField]
  private Image damageIcon;
  [SerializeField]
  private GameObject dcButton;
  [SerializeField]
  private GameObject dcIcon;
  [SerializeField]
  private Image moduleIcon;
  [SerializeField]
  private Image armorIcon;
  [SerializeField]
  private TMP_Text armorText;
  [SerializeField]
  private Transform mountList;
  [SerializeField]
  private Image mountIconPF;
  private List<Image> mountIcons = new List<Image>();
  [Header("DAMAGE BLINKER, COLORS")]
  private bool isBlinking;
  private float armorHealth;
  [SerializeField]
  private Color grayColor;
  [SerializeField]
  private Color redColor;
  [SerializeField]
  private Color greenColor;

  private void Update()
  {
    if (this.state == PartHealthUI.PartHealthState.OFF)
      return;
    this.UpdateUI();
  }

  private void UpdateUI()
  {
    this.armorText.text = Mathf.CeilToInt(this.module.armorThickness).ToString();
    float num = 0.0f;
    if ((double) this.module.armorHealthMax > 0.0)
      num = this.module.armorHealth / this.module.armorHealthMax;
    Color color1 = this.healthGradient.Evaluate(Mathf.Clamp01(num));
    this.armorIcon.color = color1;
    this.armorText.color = color1;
    if ((double) this.module.armorHealth > 0.0)
    {
      float armorHealth = this.module.armorHealth;
      if ((double) this.armorHealth > (double) armorHealth)
      {
        if (!this.isBlinking)
          this.StartCoroutine((IEnumerator) this.BlinkRed());
      }
      else if ((double) this.armorHealth < (double) armorHealth && !this.isBlinking)
        this.StartCoroutine((IEnumerator) this.BlinkGreen());
      this.armorHealth = armorHealth;
    }
    if ((double) this.module.health > 0.0)
    {
      float healthRatio = this.module.healthRatio;
      if ((double) this.healthRatio > (double) healthRatio)
      {
        if (!this.isBlinking)
          this.StartCoroutine((IEnumerator) this.BlinkRed());
      }
      else if ((double) this.healthRatio < (double) healthRatio && !this.isBlinking)
        this.StartCoroutine((IEnumerator) this.BlinkGreen());
      this.healthRatio = healthRatio;
    }
    else
      this.healthRatio = 0.0f;
    Color color2 = this.healthGradient.Evaluate(this.healthRatio);
    if (!this.module.isOn)
      color2.a = 0.5f;
    this.moduleIcon.color = color2;
    if (this.module.isBeingFixed)
      this.dcIcon.SetActive(true);
    else
      this.dcIcon.SetActive(false);
    if (this.mountIcons.Count <= 0)
      return;
    for (int index = 0; index < this.mountIcons.Count; ++index)
    {
      Color color3 = this.healthGradient.Evaluate(this.mounts[index].healthRatio);
      if (!this.module.isOn)
        color3.a = 0.5f;
      this.mountIcons[index].color = color3;
    }
  }

  public void Init(T_Module newModule, Sprite moduleIcon, Sprite mountIcon)
  {
    this.module = newModule;
    this.ship = this.module.transform.root.GetComponent<ShipController>();
    this.healthRatio = this.module.healthRatio;
    this.armorHealth = this.module.armorHealth;
    this.state = PartHealthUI.PartHealthState.MODULE;
    this.moduleIcon.sprite = moduleIcon;
    this.mounts = this.module.mounts;
    if (this.mounts.Count > 0)
    {
      foreach (T_Mount mount in this.mounts)
      {
        Image image = Object.Instantiate<Image>(this.mountIconPF, this.mountList);
        image.sprite = mountIcon;
        this.mountIcons.Add(image);
      }
      Object.Destroy((Object) this.mountIconPF.gameObject);
    }
    else
      Object.Destroy((Object) this.mountIconPF.gameObject);
    this.UpdateUI();
  }

  private IEnumerator BlinkRed()
  {
    float blinkTimer = 0.25f;
    this.isBlinking = true;
    this.damageIcon.color = this.redColor;
    yield return (object) new WaitForSeconds(blinkTimer);
    this.damageIcon.color = this.grayColor;
    yield return (object) new WaitForSeconds(blinkTimer);
    this.damageIcon.color = this.redColor;
    yield return (object) new WaitForSeconds(blinkTimer);
    this.damageIcon.color = this.grayColor;
    yield return (object) new WaitForSeconds(blinkTimer);
    this.isBlinking = false;
  }

  private IEnumerator BlinkGreen()
  {
    float blinkTimer = 0.25f;
    this.isBlinking = true;
    this.damageIcon.color = this.greenColor;
    yield return (object) new WaitForSeconds(blinkTimer);
    this.damageIcon.color = this.grayColor;
    yield return (object) new WaitForSeconds(blinkTimer);
    this.damageIcon.color = this.greenColor;
    yield return (object) new WaitForSeconds(blinkTimer);
    this.damageIcon.color = this.grayColor;
    yield return (object) new WaitForSeconds(blinkTimer);
    this.isBlinking = false;
  }

  public void TryAddDC() => this.ship.TryAddDCTo(this.module);

  private enum PartHealthState
  {
    OFF,
    MODULE,
  }
}
