// Decompiled with JetBrains decompiler
// Type: GeneralSimpleUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class GeneralSimpleUI : MonoBehaviour
{
  public RectTransform button;

  private void Start()
  {
    Debug.Log((object) "For better examples see the 4.6_Examples folder!");
    if ((UnityEngine.Object) this.button == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Button not assigned! Create a new button via Hierarchy->Create->UI->Button. Then assign it to the button variable");
    }
    else
    {
      LeanTween.value(this.button.gameObject, this.button.anchoredPosition, new Vector2(200f, 100f), 1f).setOnUpdate((Action<Vector2>) (val => this.button.anchoredPosition = val));
      LeanTween.value(this.gameObject, 1f, 0.5f, 1f).setOnUpdate((Action<float>) (volume => Debug.Log((object) ("volume:" + volume.ToString()))));
      LeanTween.value(this.gameObject, this.gameObject.transform.position, this.gameObject.transform.position + new Vector3(0.0f, 1f, 0.0f), 1f).setOnUpdate((Action<Vector3>) (val => this.gameObject.transform.position = val));
      LeanTween.value(this.gameObject, Color.red, Color.green, 1f).setOnUpdate((Action<Color>) (val => ((Graphic) this.button.gameObject.GetComponent((System.Type) typeof (Image))).color = val));
      LeanTween.move(this.button, new Vector3(200f, -100f, 0.0f), 1f).setDelay(1f);
      LeanTween.rotateAround(this.button, Vector3.forward, 90f, 1f).setDelay(2f);
      LeanTween.scale(this.button, this.button.localScale * 2f, 1f).setDelay(3f);
      LeanTween.rotateAround(this.button, Vector3.forward, -90f, 1f).setDelay(4f).setEase(LeanTweenType.easeInOutElastic);
    }
  }
}
