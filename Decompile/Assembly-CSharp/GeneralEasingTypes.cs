// Decompiled with JetBrains decompiler
// Type: GeneralEasingTypes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class GeneralEasingTypes : MonoBehaviour
{
  public float lineDrawScale = 10f;
  public AnimationCurve animationCurve;
  private string[] easeTypes = new string[35]
  {
    "EaseLinear",
    "EaseAnimationCurve",
    "EaseSpring",
    "EaseInQuad",
    "EaseOutQuad",
    "EaseInOutQuad",
    "EaseInCubic",
    "EaseOutCubic",
    "EaseInOutCubic",
    "EaseInQuart",
    "EaseOutQuart",
    "EaseInOutQuart",
    "EaseInQuint",
    "EaseOutQuint",
    "EaseInOutQuint",
    "EaseInSine",
    "EaseOutSine",
    "EaseInOutSine",
    "EaseInExpo",
    "EaseOutExpo",
    "EaseInOutExpo",
    "EaseInCirc",
    "EaseOutCirc",
    "EaseInOutCirc",
    "EaseInBounce",
    "EaseOutBounce",
    "EaseInOutBounce",
    "EaseInBack",
    "EaseOutBack",
    "EaseInOutBack",
    "EaseInElastic",
    "EaseOutElastic",
    "EaseInOutElastic",
    "EasePunch",
    "EaseShake"
  };

  private void Start() => this.demoEaseTypes();

  private void demoEaseTypes()
  {
    for (int index = 0; index < this.easeTypes.Length; ++index)
    {
      string easeType = this.easeTypes[index];
      Transform obj1 = GameObject.Find(easeType).transform.Find("Line");
      float obj1val = 0.0f;
      LTDescr ltDescr = LeanTween.value(obj1.gameObject, 0.0f, 1f, 5f).setOnUpdate((Action<float>) (val =>
      {
        Vector3 localPosition = obj1.localPosition with
        {
          x = obj1val * this.lineDrawScale,
          y = val * this.lineDrawScale
        };
        obj1.localPosition = localPosition;
        obj1val += Time.deltaTime / 5f;
        if ((double) obj1val <= 1.0)
          return;
        obj1val = 0.0f;
      }));
      if (easeType.IndexOf("AnimationCurve") >= 0)
        ltDescr.setEase(this.animationCurve);
      else
        ltDescr.GetType().GetMethod("set" + easeType).Invoke((object) ltDescr, (object[]) null);
      if (easeType.IndexOf("EasePunch") >= 0)
        ltDescr.setScale(1f);
      else if (easeType.IndexOf("EaseOutBounce") >= 0)
        ltDescr.setOvershoot(2f);
    }
    LeanTween.delayedCall(this.gameObject, 10f, new Action(this.resetLines));
    LeanTween.delayedCall(this.gameObject, 10.1f, new Action(this.demoEaseTypes));
  }

  private void resetLines()
  {
    for (int index = 0; index < this.easeTypes.Length; ++index)
      GameObject.Find(this.easeTypes[index]).transform.Find("Line").localPosition = new Vector3(0.0f, 0.0f, 0.0f);
  }
}
