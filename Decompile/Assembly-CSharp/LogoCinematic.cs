// Decompiled with JetBrains decompiler
// Type: LogoCinematic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class LogoCinematic : MonoBehaviour
{
  public GameObject lean;
  public GameObject tween;

  private void Awake()
  {
  }

  private void Start()
  {
    this.tween.transform.localPosition += -Vector3.right * 15f;
    LeanTween.moveLocalX(this.tween, this.tween.transform.localPosition.x + 15f, 0.4f).setEase(LeanTweenType.linear).setDelay(0.0f).setOnComplete(new Action(this.playBoom));
    this.tween.transform.RotateAround(this.tween.transform.position, Vector3.forward, -30f);
    LeanTween.rotateAround(this.tween, Vector3.forward, 30f, 0.4f).setEase(LeanTweenType.easeInQuad).setDelay(0.4f).setOnComplete(new Action(this.playBoom));
    this.lean.transform.position += Vector3.up * 5.1f;
    LeanTween.moveY(this.lean, this.lean.transform.position.y - 5.1f, 0.6f).setEase(LeanTweenType.easeInQuad).setDelay(0.6f).setOnComplete(new Action(this.playBoom));
  }

  private void playBoom()
  {
    LeanAudio.play(LeanAudio.createAudio(new AnimationCurve(new Keyframe[3]
    {
      new Keyframe(0.0f, 1.163155f, 0.0f, -1f),
      new Keyframe(0.3098361f, 0.0f, 0.0f, 0.0f),
      new Keyframe(0.5f, 0.003524712f, 0.0f, 0.0f)
    }), new AnimationCurve(new Keyframe[11]
    {
      new Keyframe(0.000819672f, 0.007666667f, 0.0f, 0.0f),
      new Keyframe(0.01065573f, 0.002424242f, 0.0f, 0.0f),
      new Keyframe(0.02704918f, 0.007454545f, 0.0f, 0.0f),
      new Keyframe(0.03770492f, 0.002575758f, 0.0f, 0.0f),
      new Keyframe(0.052459f, 0.007090909f, 0.0f, 0.0f),
      new Keyframe(0.06885245f, 0.002939394f, 0.0f, 0.0f),
      new Keyframe(0.0819672f, 0.006727273f, 0.0f, 0.0f),
      new Keyframe(0.1040983f, 0.003181818f, 0.0f, 0.0f),
      new Keyframe(0.1188525f, 0.006212121f, 0.0f, 0.0f),
      new Keyframe(0.145082f, 0.004151515f, 0.0f, 0.0f),
      new Keyframe(0.1893443f, 0.005636364f, 0.0f, 0.0f)
    }), LeanAudio.options().setVibrato(new Vector3[1]
    {
      new Vector3(0.1f, 0.0f, 0.0f)
    }).setFrequency(11025)));
  }
}
