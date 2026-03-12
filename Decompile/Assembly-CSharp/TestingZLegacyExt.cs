// Decompiled with JetBrains decompiler
// Type: TestingZLegacyExt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class TestingZLegacyExt : MonoBehaviour
{
  public AnimationCurve customAnimationCurve;
  public Transform pt1;
  public Transform pt2;
  public Transform pt3;
  public Transform pt4;
  public Transform pt5;
  private int exampleIter;
  private string[] exampleFunctions = new string[14]
  {
    "updateValue3Example",
    "loopTestClamp",
    "loopTestPingPong",
    "moveOnACurveExample",
    "customTweenExample",
    "moveExample",
    "rotateExample",
    "scaleExample",
    "updateValueExample",
    "delayedCallExample",
    "alphaExample",
    "moveLocalExample",
    "rotateAroundExample",
    "colorExample"
  };
  public bool useEstimatedTime = true;
  private Transform ltLogo;
  private TestingZLegacyExt.TimingType timingType;
  private int descrTimeScaleChangeId;
  private Vector3 origin;

  private void Awake()
  {
  }

  private void Start()
  {
    this.ltLogo = GameObject.Find("LeanTweenLogo").transform;
    LeanTween.delayedCall(1f, new Action(this.cycleThroughExamples));
    this.origin = this.ltLogo.position;
  }

  private void pauseNow()
  {
    Time.timeScale = 0.0f;
    Debug.Log((object) "pausing");
  }

  private void OnGUI()
  {
    GUI.Label(new Rect(0.03f * (float) Screen.width, 0.03f * (float) Screen.height, 0.5f * (float) Screen.width, 0.3f * (float) Screen.height), this.useEstimatedTime ? "useEstimatedTime" : "timeScale:" + Time.timeScale.ToString());
  }

  private void endlessCallback() => Debug.Log((object) "endless");

  private void cycleThroughExamples()
  {
    if (this.exampleIter == 0)
    {
      int num = (int) (this.timingType + 1);
      if (num > 4)
        num = 0;
      this.timingType = (TestingZLegacyExt.TimingType) num;
      this.useEstimatedTime = this.timingType == TestingZLegacyExt.TimingType.IgnoreTimeScale;
      Time.timeScale = this.useEstimatedTime ? 0.0f : 1f;
      if (this.timingType == TestingZLegacyExt.TimingType.HalfTimeScale)
        Time.timeScale = 0.5f;
      if (this.timingType == TestingZLegacyExt.TimingType.VariableTimeScale)
      {
        this.descrTimeScaleChangeId = this.gameObject.LeanValue(0.01f, 10f, 3f).setOnUpdate((Action<float>) (val => Time.timeScale = val)).setEase(LeanTweenType.easeInQuad).setUseEstimatedTime(true).setRepeat(-1).id;
      }
      else
      {
        Debug.Log((object) "cancel variable time");
        LeanTween.cancel(this.descrTimeScaleChangeId);
      }
    }
    this.gameObject.BroadcastMessage(this.exampleFunctions[this.exampleIter]);
    this.gameObject.LeanDelayedCall(1.1f, new Action(this.cycleThroughExamples)).setUseEstimatedTime(this.useEstimatedTime);
    this.exampleIter = this.exampleIter + 1 >= this.exampleFunctions.Length ? 0 : this.exampleIter + 1;
  }

  public void updateValue3Example()
  {
    Debug.Log((object) ("updateValue3Example Time:" + Time.time.ToString()));
    this.gameObject.LeanValue(new Action<Vector3>(this.updateValue3ExampleCallback), new Vector3(0.0f, 270f, 0.0f), new Vector3(30f, 270f, 180f), 0.5f).setEase(LeanTweenType.easeInBounce).setRepeat(2).setLoopPingPong().setOnUpdateVector3(new Action<Vector3>(this.updateValue3ExampleUpdate)).setUseEstimatedTime(this.useEstimatedTime);
  }

  public void updateValue3ExampleUpdate(Vector3 val)
  {
  }

  public void updateValue3ExampleCallback(Vector3 val) => this.ltLogo.transform.eulerAngles = val;

  public void loopTestClamp()
  {
    Debug.Log((object) ("loopTestClamp Time:" + Time.time.ToString()));
    Transform transform = GameObject.Find("Cube1").transform;
    transform.localScale = new Vector3(1f, 1f, 1f);
    transform.LeanScaleZ(4f, 1f).setEase(LeanTweenType.easeOutElastic).setRepeat(7).setLoopClamp().setUseEstimatedTime(this.useEstimatedTime);
  }

  public void loopTestPingPong()
  {
    Debug.Log((object) ("loopTestPingPong Time:" + Time.time.ToString()));
    Transform transform = GameObject.Find("Cube2").transform;
    transform.localScale = new Vector3(1f, 1f, 1f);
    transform.LeanScaleY(4f, 1f).setEase(LeanTweenType.easeOutQuad).setLoopPingPong(4).setUseEstimatedTime(this.useEstimatedTime);
  }

  public void colorExample()
  {
    GameObject.Find("LCharacter").LeanColor(new Color(1f, 0.0f, 0.0f, 0.5f), 0.5f).setEase(LeanTweenType.easeOutBounce).setRepeat(2).setLoopPingPong().setUseEstimatedTime(this.useEstimatedTime);
  }

  public void moveOnACurveExample()
  {
    Debug.Log((object) ("moveOnACurveExample Time:" + Time.time.ToString()));
    this.ltLogo.LeanMove(new Vector3[8]
    {
      this.origin,
      this.pt1.position,
      this.pt2.position,
      this.pt3.position,
      this.pt3.position,
      this.pt4.position,
      this.pt5.position,
      this.origin
    }, 1f).setEase(LeanTweenType.easeOutQuad).setOrientToPath(true).setUseEstimatedTime(this.useEstimatedTime);
  }

  public void customTweenExample()
  {
    Vector3 vector3 = this.ltLogo.position;
    string str1 = vector3.ToString();
    vector3 = this.origin;
    string str2 = vector3.ToString();
    Debug.Log((object) $"customTweenExample starting pos:{str1} origin:{str2}");
    this.ltLogo.LeanMoveX(-10f, 0.5f).setEase(this.customAnimationCurve).setUseEstimatedTime(this.useEstimatedTime);
    this.ltLogo.LeanMoveX(0.0f, 0.5f).setEase(this.customAnimationCurve).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
  }

  public void moveExample()
  {
    Debug.Log((object) nameof (moveExample));
    this.ltLogo.LeanMove(new Vector3(-2f, -1f, 0.0f), 0.5f).setUseEstimatedTime(this.useEstimatedTime);
    this.ltLogo.LeanMove(this.origin, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
  }

  public void rotateExample()
  {
    Debug.Log((object) nameof (rotateExample));
    this.ltLogo.LeanRotate(new Vector3(0.0f, 360f, 0.0f), 1f).setEase(LeanTweenType.easeOutQuad).setOnComplete(new Action<object>(this.rotateFinished)).setOnCompleteParam((object) new Hashtable()
    {
      {
        (object) "yo",
        (object) 5.0
      }
    }).setOnUpdate(new Action<float>(this.rotateOnUpdate)).setUseEstimatedTime(this.useEstimatedTime);
  }

  public void rotateOnUpdate(float val)
  {
  }

  public void rotateFinished(object hash)
  {
    Debug.Log((object) ("rotateFinished hash:" + (hash as Hashtable)[(object) "yo"]?.ToString()));
  }

  public void scaleExample()
  {
    Debug.Log((object) nameof (scaleExample));
    Vector3 localScale = this.ltLogo.localScale;
    this.ltLogo.LeanScale(new Vector3(localScale.x + 0.2f, localScale.y + 0.2f, localScale.z + 0.2f), 1f).setEase(LeanTweenType.easeOutBounce).setUseEstimatedTime(this.useEstimatedTime);
  }

  public void updateValueExample()
  {
    Debug.Log((object) nameof (updateValueExample));
    this.gameObject.LeanValue(new Action<float, object>(this.updateValueExampleCallback), this.ltLogo.eulerAngles.y, 270f, 1f).setEase(LeanTweenType.easeOutElastic).setOnUpdateParam((object) new Hashtable()
    {
      {
        (object) "message",
        (object) "hi"
      }
    }).setUseEstimatedTime(this.useEstimatedTime);
  }

  public void updateValueExampleCallback(float val, object hash)
  {
    this.ltLogo.transform.eulerAngles = this.ltLogo.eulerAngles with
    {
      y = val
    };
  }

  public void delayedCallExample()
  {
    Debug.Log((object) nameof (delayedCallExample));
    LeanTween.delayedCall(0.5f, new Action(this.delayedCallExampleCallback)).setUseEstimatedTime(this.useEstimatedTime);
  }

  public void delayedCallExampleCallback()
  {
    Debug.Log((object) "Delayed function was called");
    Vector3 localScale = this.ltLogo.localScale;
    this.ltLogo.LeanScale(new Vector3(localScale.x - 0.2f, localScale.y - 0.2f, localScale.z - 0.2f), 0.5f).setEase(LeanTweenType.easeInOutCirc).setUseEstimatedTime(this.useEstimatedTime);
  }

  public void alphaExample()
  {
    Debug.Log((object) nameof (alphaExample));
    GameObject gameObject = GameObject.Find("LCharacter");
    gameObject.LeanAlpha(0.0f, 0.5f).setUseEstimatedTime(this.useEstimatedTime);
    gameObject.LeanAlpha(1f, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
  }

  public void moveLocalExample()
  {
    Debug.Log((object) nameof (moveLocalExample));
    GameObject gameObject = GameObject.Find("LCharacter");
    Vector3 localPosition = gameObject.transform.localPosition;
    gameObject.LeanMoveLocal(new Vector3(0.0f, 2f, 0.0f), 0.5f).setUseEstimatedTime(this.useEstimatedTime);
    gameObject.LeanMoveLocal(localPosition, 0.5f).setDelay(0.5f).setUseEstimatedTime(this.useEstimatedTime);
  }

  public void rotateAroundExample()
  {
    Debug.Log((object) nameof (rotateAroundExample));
    GameObject.Find("LCharacter").LeanRotateAround(Vector3.up, 360f, 1f).setUseEstimatedTime(this.useEstimatedTime);
  }

  public void loopPause() => GameObject.Find("Cube1").LeanPause();

  public void loopResume() => GameObject.Find("Cube1").LeanResume();

  public void punchTest()
  {
    this.ltLogo.LeanMoveX(7f, 1f).setEase(LeanTweenType.punch).setUseEstimatedTime(this.useEstimatedTime);
  }

  public delegate void NextFunc();

  public enum TimingType
  {
    SteadyNormalTime,
    IgnoreTimeScale,
    HalfTimeScale,
    VariableTimeScale,
    Length,
  }
}
