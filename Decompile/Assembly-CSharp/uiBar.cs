// Decompiled with JetBrains decompiler
// Type: uiBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class uiBar : MonoBehaviour
{
  [SerializeField]
  private Transform bar;
  [SerializeField]
  private Image barImage;
  [SerializeField]
  private uiBar.Axis axis;
  [SerializeField]
  private float duration = 0.1f;
  private LTDescr currentTween = new LTDescr();

  private void Start()
  {
    if (!((UnityEngine.Object) this.bar == (UnityEngine.Object) null) || !((UnityEngine.Object) this.barImage == (UnityEngine.Object) null) || this.transform.childCount <= 0)
      return;
    this.bar = this.transform.GetChild(0);
    this.barImage = this.bar.GetComponent<Image>();
  }

  public void SetBarSize(float percentage)
  {
    if (!(bool) (UnityEngine.Object) this.bar || !(bool) (UnityEngine.Object) this.barImage)
      return;
    if (float.IsNaN(percentage) || float.IsInfinity(percentage))
      percentage = 0.0f;
    percentage = Mathf.Clamp01(percentage);
    if (this.currentTween != null)
      LeanTween.cancel(this.bar.gameObject, this.currentTween.id);
    if (this.axis == uiBar.Axis.X)
    {
      this.currentTween = LeanTween.value(this.bar.gameObject, this.bar.localScale.x, percentage, this.duration).setEase(LeanTweenType.linear).setOnUpdate((Action<float>) (val => this.bar.localScale = new Vector3(val, 1f, 1f)));
    }
    else
    {
      if (this.axis != uiBar.Axis.Y)
        return;
      this.currentTween = LeanTween.value(this.bar.gameObject, this.bar.localScale.y, percentage, this.duration).setEase(LeanTweenType.linear).setOnUpdate((Action<float>) (val => this.bar.localScale = new Vector3(1f, val, 1f)));
    }
  }

  public void SetBarColor(Color color)
  {
    if (!(bool) (UnityEngine.Object) this.barImage)
      return;
    this.barImage.color = color;
  }

  private enum Axis
  {
    X,
    Y,
  }
}
