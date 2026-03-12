// Decompiled with JetBrains decompiler
// Type: TestingPunch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class TestingPunch : MonoBehaviour
{
  public AnimationCurve exportCurve;
  public float overShootValue = 1f;
  private LTDescr descr;

  private void Start()
  {
    Debug.Log((object) ("exported curve:" + this.curveToString(this.exportCurve)));
  }

  private void Update()
  {
    LeanTween.dtManual = Time.deltaTime;
    if (Input.GetKeyDown(KeyCode.Q))
    {
      LeanTween.moveLocalX(this.gameObject, 5f, 1f).setOnComplete((Action) (() => Debug.Log((object) "on complete move local X"))).setOnCompleteOnStart(true);
      Light lt = GameObject.Find("DirectionalLight").GetComponent<Light>();
      LeanTween.value(lt.gameObject, lt.intensity, 0.0f, 1.5f).setEase(LeanTweenType.linear).setLoopPingPong().setRepeat(-1).setOnUpdate((Action<float>) (val => lt.intensity = val));
    }
    if (Input.GetKeyDown(KeyCode.S))
    {
      MonoBehaviour.print((object) "scale punch!");
      TestingPunch.tweenStatically(this.gameObject);
      LeanTween.scale(this.gameObject, new Vector3(1.15f, 1.15f, 1.15f), 0.6f);
      LeanTween.rotateAround(this.gameObject, Vector3.forward, -360f, 0.3f).setOnComplete((Action) (() => LeanTween.rotateAround(this.gameObject, Vector3.forward, -360f, 0.4f).setOnComplete((Action) (() =>
      {
        LeanTween.scale(this.gameObject, new Vector3(1f, 1f, 1f), 0.1f);
        LeanTween.value(this.gameObject, (Action<float>) (v => { }), 0.0f, 1f, 0.3f).setDelay(1f);
      }))));
    }
    if (Input.GetKeyDown(KeyCode.T))
      this.descr = LeanTween.move(this.gameObject, new Vector3[4]
      {
        new Vector3(-1f, 0.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(4f, 0.0f, 0.0f),
        new Vector3(20f, 0.0f, 0.0f)
      }, 15f).setOrientToPath(true).setDirection(1f).setOnComplete((Action) (() => Debug.Log((object) "move path finished")));
    if (Input.GetKeyDown(KeyCode.Y))
      this.descr.setDirection(-this.descr.direction);
    if (Input.GetKeyDown(KeyCode.R))
    {
      LeanTween.rotateAroundLocal(this.gameObject, this.transform.forward, -80f, 5f).setPoint(new Vector3(1.25f, 0.0f, 0.0f));
      MonoBehaviour.print((object) "rotate punch!");
    }
    if (Input.GetKeyDown(KeyCode.M))
    {
      MonoBehaviour.print((object) "move punch!");
      Time.timeScale = 0.25f;
      float start = Time.realtimeSinceStartup;
      LeanTween.moveX(this.gameObject, 1f, 1f).setOnComplete(new Action<object>(this.destroyOnComp)).setOnCompleteParam((object) this.gameObject).setOnComplete((Action) (() =>
      {
        float realtimeSinceStartup = Time.realtimeSinceStartup;
        float num = realtimeSinceStartup - start;
        Debug.Log((object) $"start:{start.ToString()} end:{realtimeSinceStartup.ToString()} diff:{num.ToString()} x:{this.gameObject.transform.position.x.ToString()}");
      })).setEase(LeanTweenType.easeInBack).setOvershoot(this.overShootValue).setPeriod(0.3f);
    }
    if (Input.GetKeyDown(KeyCode.C))
    {
      LeanTween.color(this.gameObject, new Color(1f, 0.0f, 0.0f, 0.5f), 1f);
      Color to = new Color(UnityEngine.Random.Range(0.0f, 1f), 0.0f, UnityEngine.Random.Range(0.0f, 1f), 0.0f);
      LeanTween.color(GameObject.Find("LCharacter"), to, 4f).setLoopPingPong(1).setEase(LeanTweenType.easeOutBounce);
    }
    if (Input.GetKeyDown(KeyCode.E))
      LeanTween.delayedCall(this.gameObject, 0.3f, new Action<object>(this.delayedMethod)).setRepeat(4).setOnCompleteOnRepeat(true).setOnCompleteParam((object) "hi");
    if (Input.GetKeyDown(KeyCode.V))
      LeanTween.value(this.gameObject, new Action<Color>(this.updateColor), new Color(1f, 0.0f, 0.0f, 1f), Color.blue, 4f);
    if (Input.GetKeyDown(KeyCode.P))
      LeanTween.delayedCall(0.05f, new Action<object>(this.enterMiniGameStart)).setOnCompleteParam((object) new object[1]
      {
        (object) (5.ToString() ?? "")
      });
    if (!Input.GetKeyDown(KeyCode.U))
      return;
    LeanTween.value(this.gameObject, (Action<Vector2>) (val => this.transform.position = new Vector3(val.x, this.transform.position.y, this.transform.position.z)), new Vector2(0.0f, 0.0f), new Vector2(5f, 100f), 1f).setEase(LeanTweenType.easeOutBounce);
    GameObject l = GameObject.Find("LCharacter");
    Vector3 position = l.transform.position;
    string str1 = position.x.ToString();
    position = l.transform.position;
    string str2 = position.y.ToString();
    Debug.Log((object) $"x:{str1} y:{str2}");
    LeanTween.value(l, new Vector2(l.transform.position.x, l.transform.position.y), new Vector2(l.transform.position.x, l.transform.position.y + 5f), 1f).setOnUpdate((Action<Vector2>) (val =>
    {
      Debug.Log((object) ("tweening vec2 val:" + val.ToString()));
      l.transform.position = new Vector3(val.x, val.y, this.transform.position.z);
    }));
  }

  private static void tweenStatically(GameObject gameObject)
  {
    Debug.Log((object) "Starting to tween...");
    LeanTween.value(gameObject, (Action<float>) (val => Debug.Log((object) ("tweening val:" + val.ToString()))), 0.0f, 1f, 1f);
  }

  private void enterMiniGameStart(object val)
  {
    Debug.Log((object) ("level:" + int.Parse((string) ((object[]) val)[0]).ToString()));
  }

  private void updateColor(Color c)
  {
    GameObject.Find("LCharacter").GetComponent<Renderer>().material.color = c;
  }

  private void delayedMethod(object myVal)
  {
    string str = myVal as string;
    Debug.Log((object) $"delayed call:{Time.time.ToString()} myVal:{str}");
  }

  private void destroyOnComp(object p) => UnityEngine.Object.Destroy((UnityEngine.Object) p);

  private string curveToString(AnimationCurve curve)
  {
    string str = "";
    for (int index = 0; index < curve.length; ++index)
    {
      str = $"{str}new Keyframe({curve[index].time.ToString()}f, {curve[index].value.ToString()}f)";
      if (index < curve.length - 1)
        str += ", ";
    }
    return $"new AnimationCurve( {str} )";
  }
}
