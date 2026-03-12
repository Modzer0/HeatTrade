// Decompiled with JetBrains decompiler
// Type: GeneralUISpace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class GeneralUISpace : MonoBehaviour
{
  public RectTransform mainWindow;
  public RectTransform mainParagraphText;
  public RectTransform mainTitleText;
  public RectTransform mainButton1;
  public RectTransform mainButton2;
  public RectTransform pauseRing1;
  public RectTransform pauseRing2;
  public RectTransform pauseWindow;
  public RectTransform chatWindow;
  public RectTransform chatRect;
  public Sprite[] chatSprites;
  public RectTransform chatBar1;
  public RectTransform chatBar2;
  public Text chatText;
  public RectTransform rawImageRect;

  private void Start()
  {
    this.mainWindow.localScale = Vector3.zero;
    LeanTween.scale(this.mainWindow, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);
    LeanTween.alphaCanvas(this.mainWindow.GetComponent<CanvasGroup>(), 0.0f, 1f).setDelay(2f).setLoopPingPong().setRepeat(2);
    this.mainParagraphText.anchoredPosition3D += new Vector3(0.0f, -10f, 0.0f);
    LeanTween.textAlpha(this.mainParagraphText, 0.0f, 0.6f).setFrom(0.0f).setDelay(0.0f);
    LeanTween.textAlpha(this.mainParagraphText, 1f, 0.6f).setEase(LeanTweenType.easeOutQuad).setDelay(0.6f);
    LeanTween.move(this.mainParagraphText, this.mainParagraphText.anchoredPosition3D + new Vector3(0.0f, 10f, 0.0f), 0.6f).setEase(LeanTweenType.easeOutQuad).setDelay(0.6f);
    LeanTween.textColor(this.mainTitleText, new Color(0.521568656f, 0.5686275f, 0.8745098f), 0.6f).setEase(LeanTweenType.easeOutQuad).setDelay(0.6f).setLoopPingPong().setRepeat(-1);
    LeanTween.textAlpha(this.mainButton2, 1f, 2f).setFrom(0.0f).setDelay(0.0f).setEase(LeanTweenType.easeOutQuad);
    LeanTween.alpha(this.mainButton2, 1f, 2f).setFrom(0.0f).setDelay(0.0f).setEase(LeanTweenType.easeOutQuad);
    LeanTween.size(this.mainButton1, this.mainButton1.sizeDelta * 1.1f, 0.5f).setDelay(3f).setEaseInOutCirc().setRepeat(6).setLoopPingPong();
    this.pauseWindow.anchoredPosition3D += new Vector3(0.0f, 200f, 0.0f);
    LeanTween.moveY(this.pauseWindow, this.pauseWindow.anchoredPosition3D.y - 200f, 0.6f).setEase(LeanTweenType.easeOutSine).setDelay(0.6f);
    RectTransform component = this.pauseWindow.Find("PauseText").GetComponent<RectTransform>();
    LeanTween.moveZ(component, component.anchoredPosition3D.z - 80f, 1.5f).setEase(LeanTweenType.punch).setDelay(2f);
    LeanTween.rotateAroundLocal(this.pauseRing1, Vector3.forward, 360f, 12f).setRepeat(-1);
    LeanTween.rotateAroundLocal(this.pauseRing2, Vector3.forward, -360f, 22f).setRepeat(-1);
    this.chatWindow.RotateAround(this.chatWindow.position, Vector3.up, -180f);
    LeanTween.rotateAround(this.chatWindow, Vector3.up, 180f, 2f).setEase(LeanTweenType.easeOutElastic).setDelay(1.2f);
    LeanTween.play(this.chatRect, this.chatSprites).setLoopPingPong();
    LeanTween.color(this.chatBar2, new Color(0.972549f, 0.2627451f, 0.423529416f, 0.5f), 1.2f).setEase(LeanTweenType.easeInQuad).setLoopPingPong().setDelay(1.2f);
    LeanTween.scale(this.chatBar2, (Vector3) new Vector2(1f, 0.7f), 1.2f).setEase(LeanTweenType.easeInQuad).setLoopPingPong();
    string origText = this.chatText.text;
    this.chatText.text = "";
    LeanTween.value(this.gameObject, 0.0f, (float) origText.Length, 6f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((Action<float>) (val => this.chatText.text = origText.Substring(0, Mathf.RoundToInt(val)))).setLoopClamp().setDelay(2f);
    LeanTween.alpha(this.rawImageRect, 0.0f, 1f).setLoopPingPong();
  }
}
