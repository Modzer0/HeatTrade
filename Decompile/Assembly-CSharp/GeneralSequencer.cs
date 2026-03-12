// Decompiled with JetBrains decompiler
// Type: GeneralSequencer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class GeneralSequencer : MonoBehaviour
{
  public GameObject avatar1;
  public GameObject star;
  public GameObject dustCloudPrefab;
  public float speedScale = 1f;

  public void Start()
  {
    LTSeq ltSeq = LeanTween.sequence();
    ltSeq.append(LeanTween.moveY(this.avatar1, this.avatar1.transform.localPosition.y + 6f, 1f).setEaseOutQuad());
    ltSeq.insert(LeanTween.alpha(this.star, 0.0f, 1f));
    ltSeq.insert(LeanTween.scale(this.star, Vector3.one * 3f, 1f));
    ltSeq.append(LeanTween.rotateAround(this.avatar1, Vector3.forward, 360f, 0.6f).setEaseInBack());
    ltSeq.append(LeanTween.moveY(this.avatar1, this.avatar1.transform.localPosition.y, 1f).setEaseInQuad());
    ltSeq.append((Action) (() =>
    {
      for (int index = 0; (double) index < 50.0; ++index)
      {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.dustCloudPrefab);
        gameObject.transform.parent = this.avatar1.transform;
        gameObject.transform.localPosition = new Vector3(UnityEngine.Random.Range(-2f, 2f), 0.0f, 0.0f);
        gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360f));
        Vector3 to = new Vector3(gameObject.transform.localPosition.x, UnityEngine.Random.Range(2f, 4f), UnityEngine.Random.Range(-10f, 10f));
        LeanTween.moveLocal(gameObject, to, 3f * this.speedScale).setEaseOutCirc();
        LeanTween.rotateAround(gameObject, Vector3.forward, 720f, 3f * this.speedScale).setEaseOutCirc();
        LeanTween.alpha(gameObject, 0.0f, 3f * this.speedScale).setEaseOutCirc().setDestroyOnComplete(true);
      }
    }));
    ltSeq.setScale(this.speedScale);
  }
}
