// Decompiled with JetBrains decompiler
// Type: GeneralBasic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class GeneralBasic : MonoBehaviour
{
  public GameObject prefabAvatar;

  private void Start()
  {
    GameObject gameObject1 = GameObject.Find("AvatarRotate");
    GameObject gameObject2 = GameObject.Find("AvatarScale");
    GameObject gameObject3 = GameObject.Find("AvatarMove");
    Vector3 forward = Vector3.forward;
    LeanTween.rotateAround(gameObject1, forward, 360f, 5f);
    LeanTween.scale(gameObject2, new Vector3(1.7f, 1.7f, 1.7f), 5f).setEase(LeanTweenType.easeOutBounce);
    LeanTween.moveX(gameObject2, gameObject2.transform.position.x + 5f, 5f).setEase(LeanTweenType.easeOutBounce);
    LeanTween.move(gameObject3, gameObject3.transform.position + new Vector3(-9f, 0.0f, 1f), 2f).setEase(LeanTweenType.easeInQuad);
    LeanTween.move(gameObject3, gameObject3.transform.position + new Vector3(-6f, 0.0f, 1f), 2f).setDelay(3f);
    LeanTween.scale(gameObject2, new Vector3(0.2f, 0.2f, 0.2f), 1f).setDelay(7f).setEase(LeanTweenType.easeInOutCirc).setLoopPingPong(3);
    LeanTween.delayedCall(this.gameObject, 0.2f, new Action(this.advancedExamples));
  }

  private void advancedExamples()
  {
    LeanTween.delayedCall(this.gameObject, 14f, (Action) (() =>
    {
      for (int index = 0; index < 10; ++index)
      {
        GameObject rotator = new GameObject("rotator" + index.ToString());
        rotator.transform.position = new Vector3(10.2f, 2.85f, 0.0f);
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefabAvatar, Vector3.zero, this.prefabAvatar.transform.rotation);
        gameObject.transform.parent = rotator.transform;
        gameObject.transform.localPosition = new Vector3(0.0f, 1.5f, 2.5f * (float) index);
        gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        LeanTween.scale(gameObject, new Vector3(0.65f, 0.65f, 0.65f), 1f).setDelay((float) index * 0.2f).setEase(LeanTweenType.easeOutBack);
        double num = (double) LeanTween.tau / 10.0 * (double) index;
        LeanTween.color(gameObject, new Color((float) ((double) Mathf.Sin((float) (num + (double) LeanTween.tau * 0.0 / 3.0)) * 0.5 + 0.5), (float) ((double) Mathf.Sin((float) (num + (double) LeanTween.tau * 1.0 / 3.0)) * 0.5 + 0.5), (float) ((double) Mathf.Sin((float) (num + (double) LeanTween.tau * 2.0 / 3.0)) * 0.5 + 0.5)), 0.3f).setDelay((float) (1.2000000476837158 + (double) index * 0.40000000596046448));
        LeanTween.moveZ(gameObject, 0.0f, 0.3f).setDelay((float) (1.2000000476837158 + (double) index * 0.40000000596046448)).setEase(LeanTweenType.easeSpring).setOnComplete((Action) (() => LeanTween.rotateAround(rotator, Vector3.forward, -1080f, 12f)));
        LeanTween.moveLocalY(gameObject, 4f, 1.2f).setDelay((float) (5.0 + (double) index * 0.20000000298023224)).setLoopPingPong(1).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.alpha(gameObject, 0.0f, 0.6f).setDelay((float) (9.1999998092651367 + (double) index * 0.40000000596046448)).setDestroyOnComplete(true).setOnComplete((Action) (() => UnityEngine.Object.Destroy((UnityEngine.Object) rotator)));
      }
    })).setOnCompleteOnStart(true).setRepeat(-1);
  }
}
