// Decompiled with JetBrains decompiler
// Type: OldGUIExamplesCS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class OldGUIExamplesCS : MonoBehaviour
{
  public Texture2D grumpy;
  public Texture2D beauty;
  private float w;
  private float h;
  private LTRect buttonRect1;
  private LTRect buttonRect2;
  private LTRect buttonRect3;
  private LTRect buttonRect4;
  private LTRect grumpyRect;
  private LTRect beautyTileRect;

  private void Start()
  {
    this.w = (float) Screen.width;
    this.h = (float) Screen.height;
    this.buttonRect1 = new LTRect(0.1f * this.w, 0.8f * this.h, 0.2f * this.w, 0.14f * this.h);
    this.buttonRect2 = new LTRect(1.2f * this.w, 0.8f * this.h, 0.2f * this.w, 0.14f * this.h);
    this.buttonRect3 = new LTRect(0.35f * this.w, 0.0f * this.h, 0.3f * this.w, 0.2f * this.h, 0.0f);
    this.buttonRect4 = new LTRect(0.0f * this.w, 0.4f * this.h, 0.3f * this.w, 0.2f * this.h, 1f, 15f);
    this.grumpyRect = new LTRect((float) (0.5 * (double) this.w - (double) this.grumpy.width * 0.5), (float) (0.5 * (double) this.h - (double) this.grumpy.height * 0.5), (float) this.grumpy.width, (float) this.grumpy.height);
    this.beautyTileRect = new LTRect(0.0f, 0.0f, 1f, 1f);
    LeanTween.move(this.buttonRect2, new Vector2(0.55f * this.w, this.buttonRect2.rect.y), 0.7f).setEase(LeanTweenType.easeOutQuad);
  }

  public void catMoved() => Debug.Log((object) "cat moved...");

  private void OnGUI()
  {
    GUI.DrawTexture(this.grumpyRect.rect, (Texture) this.grumpy);
    Rect rect;
    if (GUI.Button(new Rect(0.0f * this.w, 0.0f * this.h, 0.2f * this.w, 0.14f * this.h), "Move Cat") && !LeanTween.isTweening(this.grumpyRect))
    {
      Vector2 to;
      ref Vector2 local = ref to;
      rect = this.grumpyRect.rect;
      double x = (double) rect.x;
      rect = this.grumpyRect.rect;
      double y = (double) rect.y;
      local = new Vector2((float) x, (float) y);
      LeanTween.move(this.grumpyRect, new Vector2(1f * (float) Screen.width - (float) this.grumpy.width, 0.0f * (float) Screen.height), 1f).setEase(LeanTweenType.easeOutBounce).setOnComplete(new Action(this.catMoved));
      LeanTween.move(this.grumpyRect, to, 1f).setDelay(1f).setEase(LeanTweenType.easeOutBounce);
    }
    if (GUI.Button(this.buttonRect1.rect, "Scale Centered"))
    {
      LTRect buttonRect1_1 = this.buttonRect1;
      rect = this.buttonRect1.rect;
      double width = (double) rect.width;
      rect = this.buttonRect1.rect;
      double height = (double) rect.height;
      Vector2 to1 = new Vector2((float) width, (float) height) * 1.2f;
      LeanTween.scale(buttonRect1_1, to1, 0.25f).setEase(LeanTweenType.easeOutQuad);
      LTRect buttonRect1_2 = this.buttonRect1;
      rect = this.buttonRect1.rect;
      double x1 = (double) rect.x;
      rect = this.buttonRect1.rect;
      double num1 = (double) rect.width * 0.10000000149011612;
      double x2 = x1 - num1;
      rect = this.buttonRect1.rect;
      double y1 = (double) rect.y;
      rect = this.buttonRect1.rect;
      double num2 = (double) rect.height * 0.10000000149011612;
      double y2 = y1 - num2;
      Vector2 to2 = new Vector2((float) x2, (float) y2);
      LeanTween.move(buttonRect1_2, to2, 0.25f).setEase(LeanTweenType.easeOutQuad);
    }
    if (GUI.Button(this.buttonRect2.rect, "Scale"))
    {
      LTRect buttonRect2 = this.buttonRect2;
      rect = this.buttonRect2.rect;
      double width = (double) rect.width;
      rect = this.buttonRect2.rect;
      double height = (double) rect.height;
      Vector2 to = new Vector2((float) width, (float) height) * 1.2f;
      LeanTween.scale(buttonRect2, to, 0.25f).setEase(LeanTweenType.easeOutBounce);
    }
    if (GUI.Button(new Rect(0.76f * this.w, 0.53f * this.h, 0.2f * this.w, 0.14f * this.h), "Flip Tile"))
    {
      LTRect beautyTileRect = this.beautyTileRect;
      rect = this.beautyTileRect.rect;
      Vector2 to = new Vector2(0.0f, (float) ((double) rect.y + 1.0));
      LeanTween.move(beautyTileRect, to, 1f).setEase(LeanTweenType.easeOutBounce);
    }
    GUI.DrawTextureWithTexCoords(new Rect(0.8f * this.w, (float) (0.5 * (double) this.h - (double) this.beauty.height * 0.5), (float) this.beauty.width * 0.5f, (float) this.beauty.height * 0.5f), (Texture) this.beauty, this.beautyTileRect.rect);
    if (GUI.Button(this.buttonRect3.rect, "Alpha"))
    {
      LeanTween.alpha(this.buttonRect3, 0.0f, 1f).setEase(LeanTweenType.easeOutQuad);
      LeanTween.alpha(this.buttonRect3, 1f, 1f).setDelay(1f).setEase(LeanTweenType.easeInQuad);
      LeanTween.alpha(this.grumpyRect, 0.0f, 1f).setEase(LeanTweenType.easeOutQuad);
      LeanTween.alpha(this.grumpyRect, 1f, 1f).setDelay(1f).setEase(LeanTweenType.easeInQuad);
    }
    GUI.color = new Color(1f, 1f, 1f, 1f);
    if (GUI.Button(this.buttonRect4.rect, "Rotate"))
    {
      LeanTween.rotate(this.buttonRect4, 150f, 1f).setEase(LeanTweenType.easeOutElastic);
      LeanTween.rotate(this.buttonRect4, 0.0f, 1f).setDelay(1f).setEase(LeanTweenType.easeOutElastic);
    }
    GUI.matrix = Matrix4x4.identity;
  }
}
