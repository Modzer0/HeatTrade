// Decompiled with JetBrains decompiler
// Type: GeneralBasics2d
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class GeneralBasics2d : MonoBehaviour
{
  public Texture2D dudeTexture;
  public GameObject prefabParticles;

  private void Start()
  {
    GameObject spriteDude1 = this.createSpriteDude("avatarRotate", new Vector3(-2.51208f, 10.7119f, -14.37754f));
    GameObject spriteDude2 = this.createSpriteDude("avatarScale", new Vector3(2.51208f, 10.2119f, -14.37754f));
    GameObject spriteDude3 = this.createSpriteDude("avatarMove", new Vector3(-3.1208f, 7.100643f, -14.37754f));
    LeanTween.rotateAround(spriteDude1, Vector3.forward, -360f, 5f);
    LeanTween.scale(spriteDude2, new Vector3(1.7f, 1.7f, 1.7f), 5f).setEase(LeanTweenType.easeOutBounce);
    LeanTween.moveX(spriteDude2, spriteDude2.transform.position.x + 1f, 5f).setEase(LeanTweenType.easeOutBounce);
    LeanTween.move(spriteDude3, spriteDude3.transform.position + new Vector3(1.7f, 0.0f, 0.0f), 2f).setEase(LeanTweenType.easeInQuad);
    LeanTween.move(spriteDude3, spriteDude3.transform.position + new Vector3(2f, -1f, 0.0f), 2f).setDelay(3f);
    LeanTween.scale(spriteDude2, new Vector3(0.2f, 0.2f, 0.2f), 1f).setDelay(7f).setEase(LeanTweenType.easeInOutCirc).setLoopPingPong(3);
    LeanTween.delayedCall(this.gameObject, 0.2f, new Action(this.advancedExamples));
  }

  private GameObject createSpriteDude(string name, Vector3 pos, bool hasParticles = true)
  {
    GameObject spriteDude = new GameObject(name);
    SpriteRenderer spriteRenderer = spriteDude.AddComponent<SpriteRenderer>();
    spriteDude.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.709803939f, 1f);
    Sprite sprite = Sprite.Create(this.dudeTexture, new Rect(0.0f, 0.0f, 256f, 256f), new Vector2(0.5f, 0.0f), 256f);
    spriteRenderer.sprite = sprite;
    spriteDude.transform.position = pos;
    if (hasParticles)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefabParticles, Vector3.zero, this.prefabParticles.transform.rotation);
      gameObject.transform.parent = spriteDude.transform;
      gameObject.transform.localPosition = this.prefabParticles.transform.position;
    }
    return spriteDude;
  }

  private void advancedExamples()
  {
    LeanTween.delayedCall(this.gameObject, 14f, (Action) (() =>
    {
      for (int index = 0; index < 10; ++index)
      {
        GameObject rotator = new GameObject("rotator" + index.ToString());
        rotator.transform.position = new Vector3(2.71208f, 7.100643f, -12.37754f);
        GameObject spriteDude = this.createSpriteDude("dude" + index.ToString(), new Vector3(-2.51208f, 7.100643f, -14.37754f), false);
        spriteDude.transform.parent = rotator.transform;
        spriteDude.transform.localPosition = new Vector3(0.0f, 0.5f, 0.5f * (float) index);
        spriteDude.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        LeanTween.scale(spriteDude, new Vector3(0.65f, 0.65f, 0.65f), 1f).setDelay((float) index * 0.2f).setEase(LeanTweenType.easeOutBack);
        double num = (double) LeanTween.tau / 10.0 * (double) index;
        LeanTween.color(spriteDude, new Color((float) ((double) Mathf.Sin((float) (num + (double) LeanTween.tau * 0.0 / 3.0)) * 0.5 + 0.5), (float) ((double) Mathf.Sin((float) (num + (double) LeanTween.tau * 1.0 / 3.0)) * 0.5 + 0.5), (float) ((double) Mathf.Sin((float) (num + (double) LeanTween.tau * 2.0 / 3.0)) * 0.5 + 0.5)), 0.3f).setDelay((float) (1.2000000476837158 + (double) index * 0.40000000596046448));
        LeanTween.moveLocalZ(spriteDude, -2f, 0.3f).setDelay((float) (1.2000000476837158 + (double) index * 0.40000000596046448)).setEase(LeanTweenType.easeSpring).setOnComplete((Action) (() => LeanTween.rotateAround(rotator, Vector3.forward, -1080f, 12f)));
        LeanTween.moveLocalY(spriteDude, 1.17f, 1.2f).setDelay((float) (5.0 + (double) index * 0.20000000298023224)).setLoopPingPong(1).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.alpha(spriteDude, 0.0f, 0.6f).setDelay((float) (9.1999998092651367 + (double) index * 0.40000000596046448)).setDestroyOnComplete(true).setOnComplete((Action) (() => UnityEngine.Object.Destroy((UnityEngine.Object) rotator)));
      }
    })).setOnCompleteOnStart(true).setRepeat(-1);
  }
}
