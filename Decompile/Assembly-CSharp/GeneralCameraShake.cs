// Decompiled with JetBrains decompiler
// Type: GeneralCameraShake
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class GeneralCameraShake : MonoBehaviour
{
  private GameObject avatarBig;
  private float jumpIter = 9.5f;
  private AudioClip boomAudioClip;

  private void Start()
  {
    this.avatarBig = GameObject.Find("AvatarBig");
    this.boomAudioClip = LeanAudio.createAudio(new AnimationCurve(new Keyframe[4]
    {
      new Keyframe(8.130963E-06f, 0.06526042f, 0.0f, -1f),
      new Keyframe(0.0007692695f, 2.449077f, 9.078861f, 9.078861f),
      new Keyframe(0.01541314f, 0.9343268f, -40f, -40f),
      new Keyframe(0.05169491f, 0.03835937f, -0.08621139f, -0.08621139f)
    }), new AnimationCurve(new Keyframe[2]
    {
      new Keyframe(0.0f, 0.003005181f, 0.0f, 0.0f),
      new Keyframe(0.01507768f, 0.002227979f, 0.0f, 0.0f)
    }), LeanAudio.options().setVibrato(new Vector3[1]
    {
      new Vector3(0.1f, 0.0f, 0.0f)
    }));
    this.bigGuyJump();
  }

  private void bigGuyJump()
  {
    float height = Mathf.PerlinNoise(this.jumpIter, 0.0f) * 10f;
    height = (float) ((double) height * (double) height * 0.30000001192092896);
    LeanTween.moveY(this.avatarBig, height, 1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete((Action) (() => LeanTween.moveY(this.avatarBig, 0.0f, 0.27f).setEase(LeanTweenType.easeInQuad).setOnComplete((Action) (() =>
    {
      LeanTween.cancel(this.gameObject);
      float num = height * 0.2f;
      float time1 = 0.42f;
      float time2 = 1.6f;
      LTDescr shakeTween = LeanTween.rotateAroundLocal(this.gameObject, Vector3.right, num, time1).setEase(LeanTweenType.easeShake).setLoopClamp().setRepeat(-1);
      LeanTween.value(this.gameObject, num, 0.0f, time2).setOnUpdate((Action<float>) (val => shakeTween.setTo(Vector3.right * val))).setEase(LeanTweenType.easeOutQuad);
      foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Respawn"))
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 100f * height);
      foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("GameController"))
      {
        float z = gameObject.transform.eulerAngles.z;
        float x = (double) z <= 0.0 || (double) z >= 180.0 ? -1f : 1f;
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(x, 0.0f, 0.0f) * 15f * height);
      }
      LeanAudio.play(this.boomAudioClip, this.transform.position, height * 0.2f);
      LeanTween.delayedCall(2f, new Action(this.bigGuyJump));
    }))));
    this.jumpIter += 5.2f;
  }
}
