// Decompiled with JetBrains decompiler
// Type: DentedPixel.LTExamples.TestingUnitTests
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace DentedPixel.LTExamples;

public class TestingUnitTests : MonoBehaviour
{
  public GameObject cube1;
  public GameObject cube2;
  public GameObject cube3;
  public GameObject cube4;
  public GameObject cubeAlpha1;
  public GameObject cubeAlpha2;
  private bool eventGameObjectWasCalled;
  private bool eventGeneralWasCalled;
  private int lt1Id;
  private LTDescr lt2;
  private LTDescr lt3;
  private LTDescr lt4;
  private LTDescr[] groupTweens;
  private GameObject[] groupGOs;
  private int groupTweensCnt;
  private int rotateRepeat;
  private int rotateRepeatAngle;
  private GameObject boxNoCollider;
  private float timeElapsedNormalTimeScale;
  private float timeElapsedIgnoreTimeScale;
  private bool pauseTweenDidFinish;

  private void Awake()
  {
    this.boxNoCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.boxNoCollider.GetComponent((System.Type) typeof (BoxCollider)));
  }

  private void Start()
  {
    LeanTest.timeout = 46f;
    LeanTest.expected = 62;
    LeanTween.init(1300);
    LeanTween.addListener(this.cube1, 0, new Action<LTEvent>(this.eventGameObjectCalled));
    LeanTest.expect(!LeanTween.isTweening(), "NOTHING TWEEENING AT BEGINNING");
    LeanTest.expect(!LeanTween.isTweening(this.cube1), "OBJECT NOT TWEEENING AT BEGINNING");
    LeanTween.scaleX(this.cube4, 2f, 0.0f).setOnComplete((Action) (() => LeanTest.expect((double) this.cube4.transform.localScale.x == 2.0, "TWEENED WITH ZERO TIME")));
    LeanTween.dispatchEvent(0);
    LeanTest.expect(this.eventGameObjectWasCalled, "EVENT GAMEOBJECT RECEIVED");
    LeanTest.expect(!LeanTween.removeListener(this.cube2, 0, new Action<LTEvent>(this.eventGameObjectCalled)), "EVENT GAMEOBJECT NOT REMOVED");
    LeanTest.expect(LeanTween.removeListener(this.cube1, 0, new Action<LTEvent>(this.eventGameObjectCalled)), "EVENT GAMEOBJECT REMOVED");
    LeanTween.addListener(1, new Action<LTEvent>(this.eventGeneralCalled));
    LeanTween.dispatchEvent(1);
    LeanTest.expect(this.eventGeneralWasCalled, "EVENT ALL RECEIVED");
    LeanTest.expect(LeanTween.removeListener(1, new Action<LTEvent>(this.eventGeneralCalled)), "EVENT ALL REMOVED");
    this.lt1Id = LeanTween.move(this.cube1, new Vector3(3f, 2f, 0.5f), 1.1f).id;
    LeanTween.move(this.cube2, new Vector3(-3f, -2f, -0.5f), 1.1f);
    LeanTween.reset();
    GameObject[] cubes = new GameObject[99];
    int[] tweenIds = new int[cubes.Length];
    for (int index = 0; index < cubes.Length; ++index)
    {
      GameObject gameObject = this.cubeNamed("cancel" + index.ToString());
      tweenIds[index] = LeanTween.moveX(gameObject, 100f, 1f).id;
      cubes[index] = gameObject;
    }
    int onCompleteCount = 0;
    LeanTween.delayedCall(cubes[0], 0.2f, (Action) (() =>
    {
      for (int index = 0; index < cubes.Length; ++index)
      {
        if (index % 3 == 0)
          LeanTween.cancel(cubes[index]);
        else if (index % 3 == 1)
          LeanTween.cancel(tweenIds[index]);
        else if (index % 3 == 2)
          LeanTween.descr(tweenIds[index]).setOnComplete((Action) (() =>
          {
            ++onCompleteCount;
            if (onCompleteCount < 33)
              return;
            LeanTest.expect(true, "CANCELS DO NOT EFFECT FINISHING");
          }));
      }
    }));
    new LTSpline(new Vector3[5]
    {
      new Vector3(-1f, 0.0f, 0.0f),
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(4f, 0.0f, 0.0f),
      new Vector3(20f, 0.0f, 0.0f),
      new Vector3(30f, 0.0f, 0.0f)
    }).place(this.cube4.transform, 0.5f);
    LeanTest.expect((double) Vector3.Distance(this.cube4.transform.position, new Vector3(10f, 0.0f, 0.0f)) <= 0.699999988079071, "SPLINE POSITIONING AT HALFWAY", $"position is:{this.cube4.transform.position.ToString()} but should be:(10f,0f,0f)");
    LeanTween.color(this.cube4, Color.green, 0.01f);
    GameObject gameObject1 = this.cubeNamed("cubeDest");
    Vector3 cubeDestEnd = new Vector3(100f, 20f, 0.0f);
    LeanTween.move(gameObject1, cubeDestEnd, 0.7f);
    GameObject cubeToTrans = this.cubeNamed("cubeToTrans");
    LeanTween.move(cubeToTrans, gameObject1.transform, 1.2f).setEase(LeanTweenType.easeOutQuad).setOnComplete((Action) (() => LeanTest.expect(cubeToTrans.transform.position == cubeDestEnd, "MOVE TO TRANSFORM WORKS")));
    GameObject gameObject2 = this.cubeNamed("cubeDestroy");
    LeanTween.moveX(gameObject2, 200f, 0.05f).setDelay(0.02f).setDestroyOnComplete(true);
    LeanTween.moveX(gameObject2, 200f, 0.1f).setDestroyOnComplete(true).setOnComplete((Action) (() => LeanTest.expect(true, "TWO DESTROY ON COMPLETE'S SUCCEED")));
    GameObject cubeSpline = this.cubeNamed("cubeSpline");
    LeanTween.moveSpline(cubeSpline, new Vector3[4]
    {
      new Vector3(0.5f, 0.0f, 0.5f),
      new Vector3(0.75f, 0.0f, 0.75f),
      new Vector3(1f, 0.0f, 1f),
      new Vector3(1f, 0.0f, 1f)
    }, 0.1f).setOnComplete((Action) (() => LeanTest.expect((double) Vector3.Distance(new Vector3(1f, 0.0f, 1f), cubeSpline.transform.position) < 0.0099999997764825821, "SPLINE WITH TWO POINTS SUCCEEDS")));
    GameObject jumpCube = this.cubeNamed("jumpTime");
    jumpCube.transform.position = new Vector3(100f, 0.0f, 0.0f);
    jumpCube.transform.localScale *= 100f;
    int jumpTimeId = LeanTween.moveX(jumpCube, 200f, 1f).id;
    LeanTween.delayedCall(this.gameObject, 0.2f, (Action) (() =>
    {
      LTDescr ltDescr = LeanTween.descr(jumpTimeId);
      float beforeX = jumpCube.transform.position.x;
      ltDescr.setTime(0.5f);
      LeanTween.delayedCall(0.0f, (Action) (() => { })).setOnStart((Action) (() =>
      {
        float num = 1f;
        beforeX += (float) ((double) Time.deltaTime * 100.0 * 2.0);
        LeanTest.expect(((double) Mathf.Abs(jumpCube.transform.position.x - beforeX) < (double) num ? 1 : 0) != 0, "CHANGING TIME DOESN'T JUMP AHEAD", $"Difference:{Mathf.Abs(jumpCube.transform.position.x - beforeX).ToString()} beforeX:{beforeX.ToString()} now:{jumpCube.transform.position.x.ToString()} dt:{Time.deltaTime.ToString()}");
      }));
    }));
    GameObject zeroCube = this.cubeNamed("zeroCube");
    LeanTween.moveX(zeroCube, 10f, 0.0f).setOnComplete((Action) (() => LeanTest.expect((double) zeroCube.transform.position.x == 10.0, "ZERO TIME FINSHES CORRECTLY", "final x:" + zeroCube.transform.position.x.ToString())));
    GameObject cubeScale = this.cubeNamed("cubeScale");
    LeanTween.scale(cubeScale, new Vector3(5f, 5f, 5f), 0.01f).setOnStart((Action) (() => LeanTest.expect(true, "ON START WAS CALLED"))).setOnComplete((Action) (() => LeanTest.expect((double) cubeScale.transform.localScale.z == 5.0, "SCALE", $"expected scale z:{5f.ToString()} returned:{cubeScale.transform.localScale.z.ToString()}")));
    GameObject cubeRotate = this.cubeNamed("cubeRotate");
    LeanTween.rotate(cubeRotate, new Vector3(0.0f, 180f, 0.0f), 0.02f).setOnComplete((Action) (() => LeanTest.expect((double) cubeRotate.transform.eulerAngles.y == 180.0, "ROTATE", $"expected rotate y:{180f.ToString()} returned:{cubeRotate.transform.eulerAngles.y.ToString()}")));
    GameObject cubeRotateA = this.cubeNamed("cubeRotateA");
    LeanTween.rotateAround(cubeRotateA, Vector3.forward, 90f, 0.3f).setOnComplete((Action) (() => LeanTest.expect((double) cubeRotateA.transform.eulerAngles.z == 90.0, "ROTATE AROUND", $"expected rotate z:{90f.ToString()} returned:{cubeRotateA.transform.eulerAngles.z.ToString()}")));
    GameObject cubeRotateB = this.cubeNamed("cubeRotateB");
    cubeRotateB.transform.position = new Vector3(200f, 10f, 8f);
    LeanTween.rotateAround(cubeRotateB, Vector3.forward, 360f, 0.3f).setPoint(new Vector3(5f, 3f, 2f)).setOnComplete((Action) (() =>
    {
      Vector3 vector3 = cubeRotateB.transform.position;
      string str1 = vector3.ToString();
      vector3 = new Vector3(200f, 10f, 8f);
      string str2 = vector3.ToString();
      int num = str1 == str2 ? 1 : 0;
      vector3 = new Vector3(200f, 10f, 8f);
      string str3 = vector3.ToString();
      vector3 = cubeRotateB.transform.position;
      string str4 = vector3.ToString();
      string failExplaination = $"expected rotate pos:{str3} returned:{str4}";
      LeanTest.expect(num != 0, "ROTATE AROUND 360", failExplaination);
    }));
    LeanTween.alpha(this.cubeAlpha1, 0.5f, 0.1f).setOnUpdate((Action<float>) (val => LeanTest.expect((double) val != 0.0, "ON UPDATE VAL"))).setOnCompleteParam((object) "Hi!").setOnComplete((Action<object>) (completeObj =>
    {
      LeanTest.expect((string) completeObj == "Hi!", "ONCOMPLETE OBJECT");
      LeanTest.expect((double) this.cubeAlpha1.GetComponent<Renderer>().material.color.a == 0.5, "ALPHA");
    }));
    float onStartTime = -1f;
    LeanTween.color(this.cubeAlpha2, Color.cyan, 0.3f).setOnComplete((Action) (() =>
    {
      LeanTest.expect(this.cubeAlpha2.GetComponent<Renderer>().material.color == Color.cyan, "COLOR");
      LeanTest.expect((double) onStartTime >= 0.0 && (double) onStartTime < (double) Time.time, "ON START", $"onStartTime:{onStartTime.ToString()} time:{Time.time.ToString()}");
    })).setOnStart((Action) (() => onStartTime = Time.time));
    Vector3 beforePos = this.cubeAlpha1.transform.position;
    LeanTween.moveY(this.cubeAlpha1, 3f, 0.2f).setOnComplete((Action) (() => LeanTest.expect((double) this.cubeAlpha1.transform.position.x == (double) beforePos.x && (double) this.cubeAlpha1.transform.position.z == (double) beforePos.z, "MOVE Y")));
    Vector3 beforePos2 = this.cubeAlpha2.transform.localPosition;
    LeanTween.moveLocalZ(this.cubeAlpha2, 12f, 0.2f).setOnComplete((Action) (() =>
    {
      int num = (double) this.cubeAlpha2.transform.localPosition.x != (double) beforePos2.x ? 0 : ((double) this.cubeAlpha2.transform.localPosition.y == (double) beforePos2.y ? 1 : 0);
      string[] strArray = new string[8];
      strArray[0] = "ax:";
      Vector3 localPosition = this.cubeAlpha2.transform.localPosition;
      strArray[1] = localPosition.x.ToString();
      strArray[2] = " bx:";
      strArray[3] = beforePos.x.ToString();
      strArray[4] = " ay:";
      localPosition = this.cubeAlpha2.transform.localPosition;
      strArray[5] = localPosition.y.ToString();
      strArray[6] = " by:";
      strArray[7] = beforePos2.y.ToString();
      string failExplaination = string.Concat(strArray);
      LeanTest.expect(num != 0, "MOVE LOCAL Z", failExplaination);
    }));
    LeanTween.delayedSound(this.gameObject, LeanAudio.createAudio(new AnimationCurve(new Keyframe[2]
    {
      new Keyframe(0.0f, 1f, 0.0f, -1f),
      new Keyframe(1f, 0.0f, -1f, 0.0f)
    }), new AnimationCurve(new Keyframe[2]
    {
      new Keyframe(0.0f, 1f / 1000f, 0.0f, 0.0f),
      new Keyframe(1f, 1f / 1000f, 0.0f, 0.0f)
    }), LeanAudio.options()), new Vector3(0.0f, 0.0f, 0.0f), 0.1f).setDelay(0.2f).setOnComplete((Action) (() => LeanTest.expect((double) Time.time > 0.0, "DELAYED SOUND")));
    int totalEasingCheck = 0;
    int totalEasingCheckSuccess = 0;
    for (int index1 = 0; index1 < 2; ++index1)
    {
      bool flag = index1 == 1;
      int totalTweenTypeLength = 33;
      for (int index2 = 0; index2 < totalTweenTypeLength; ++index2)
      {
        GameObject gameObject3 = this.cubeNamed("cube" + ((LeanTweenType) index2).ToString());
        LTDescr ltDescr = LeanTween.moveLocalX(gameObject3, 5f, 0.1f).setOnComplete((Action<object>) (obj =>
        {
          GameObject gameObject4 = obj as GameObject;
          ++totalEasingCheck;
          if ((double) gameObject4.transform.position.x == 5.0)
            ++totalEasingCheckSuccess;
          if (totalEasingCheck != 2 * totalTweenTypeLength)
            return;
          LeanTest.expect(totalEasingCheck == totalEasingCheckSuccess, "EASING TYPES");
        })).setOnCompleteParam((object) gameObject3);
        if (flag)
          ltDescr.setFrom(-5f);
      }
    }
    bool value2UpdateCalled = false;
    LeanTween.value(this.gameObject, new Vector2(0.0f, 0.0f), new Vector2(256f, 96f), 0.1f).setOnUpdate((Action<Vector2>) (value => value2UpdateCalled = true));
    LeanTween.delayedCall(0.2f, (Action) (() => LeanTest.expect(value2UpdateCalled, "VALUE2 UPDATE")));
    this.StartCoroutine((IEnumerator) this.timeBasedTesting());
  }

  private GameObject cubeNamed(string name)
  {
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.boxNoCollider);
    gameObject.name = name;
    return gameObject;
  }

  private IEnumerator timeBasedTesting()
  {
    TestingUnitTests testingUnitTests1 = this;
    yield return (object) new WaitForEndOfFrame();
    GameObject gameObject1 = testingUnitTests1.cubeNamed("normalTimeScale");
    LeanTween.moveX(gameObject1, 12f, 1.5f).setIgnoreTimeScale(false).setOnComplete((Action) (() => this.timeElapsedNormalTimeScale = Time.time));
    LTDescr[] ltDescrArray = LeanTween.descriptions(gameObject1);
    LeanTest.expect(ltDescrArray.Length >= 0 && (double) ltDescrArray[0].to.x == 12.0, "WE CAN RETRIEVE A DESCRIPTION");
    LeanTween.moveX(testingUnitTests1.cubeNamed("ignoreTimeScale"), 5f, 1.5f).setIgnoreTimeScale(true).setOnComplete((Action) (() => this.timeElapsedIgnoreTimeScale = Time.time));
    yield return (object) new WaitForSeconds(1.5f);
    LeanTest.expect((double) Mathf.Abs(testingUnitTests1.timeElapsedNormalTimeScale - testingUnitTests1.timeElapsedIgnoreTimeScale) < 0.699999988079071, "START IGNORE TIMING", $"timeElapsedIgnoreTimeScale:{testingUnitTests1.timeElapsedIgnoreTimeScale.ToString()} timeElapsedNormalTimeScale:{testingUnitTests1.timeElapsedNormalTimeScale.ToString()}");
    Time.timeScale = 4f;
    int pauseCount = 0;
    LeanTween.value(testingUnitTests1.gameObject, 0.0f, 1f, 1f).setOnUpdate((Action<float>) (val => ++pauseCount)).pause();
    Vector3[] vector3Array = new Vector3[16 /*0x10*/]
    {
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(-9.1f, 25.1f, 0.0f),
      new Vector3(-1.2f, 15.9f, 0.0f),
      new Vector3(-25f, 25f, 0.0f),
      new Vector3(-25f, 25f, 0.0f),
      new Vector3(-50.1f, 15.9f, 0.0f),
      new Vector3(-40.9f, 25.1f, 0.0f),
      new Vector3(-50f, 0.0f, 0.0f),
      new Vector3(-50f, 0.0f, 0.0f),
      new Vector3(-40.9f, -25.1f, 0.0f),
      new Vector3(-50.1f, -15.9f, 0.0f),
      new Vector3(-25f, -25f, 0.0f),
      new Vector3(-25f, -25f, 0.0f),
      new Vector3(0.0f, -15.9f, 0.0f),
      new Vector3(-9.1f, -25.1f, 0.0f),
      new Vector3(0.0f, 0.0f, 0.0f)
    };
    GameObject cubeRound = testingUnitTests1.cubeNamed("bRound");
    Vector3 onStartPos = cubeRound.transform.position;
    LeanTween.moveLocal(cubeRound, vector3Array, 0.5f).setOnComplete((Action) (() =>
    {
      int num = cubeRound.transform.position == onStartPos ? 1 : 0;
      Vector3 vector3 = onStartPos;
      string str1 = vector3.ToString();
      vector3 = cubeRound.transform.position;
      string str2 = vector3.ToString();
      string failExplaination = $"onStartPos:{str1} onEnd:{str2}";
      LeanTest.expect(num != 0, "BEZIER CLOSED LOOP SHOULD END AT START", failExplaination);
    }));
    LeanTest.expect(object.Equals((object) new LTBezierPath(vector3Array).ratioAtPoint(new Vector3(-25f, 25f, 0.0f)), (object) 0.25f), "BEZIER RATIO POINT");
    Vector3[] to1 = new Vector3[6]
    {
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(2f, 0.0f, 0.0f),
      new Vector3(0.9f, 2f, 0.0f),
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(0.0f, 0.0f, 0.0f)
    };
    GameObject cubeSpline = testingUnitTests1.cubeNamed("bSpline");
    Vector3 onStartPosSpline = cubeSpline.transform.position;
    LeanTween.moveSplineLocal(cubeSpline, to1, 0.5f).setOnComplete((Action) (() => LeanTest.expect(((double) Vector3.Distance(onStartPosSpline, cubeSpline.transform.position) <= 0.0099999997764825821 ? 1 : 0) != 0, "SPLINE CLOSED LOOP SHOULD END AT START", $"onStartPos:{onStartPosSpline.ToString()} onEnd:{cubeSpline.transform.position.ToString()} dist:{Vector3.Distance(onStartPosSpline, cubeSpline.transform.position).ToString()}")));
    GameObject cubeSeq = testingUnitTests1.cubeNamed("cSeq");
    LTSeq ltSeq = LeanTween.sequence().append(LeanTween.moveX(cubeSeq, 100f, 0.2f));
    ltSeq.append(0.1f).append(LeanTween.scaleX(cubeSeq, 2f, 0.1f));
    ltSeq.append((Action) (() =>
    {
      LeanTest.expect((double) cubeSeq.transform.position.x == 100.0, "SEQ MOVE X FINISHED", "move x:" + cubeSeq.transform.position.x.ToString());
      LeanTest.expect((double) cubeSeq.transform.localScale.x == 2.0, "SEQ SCALE X FINISHED", "scale x:" + cubeSeq.transform.localScale.x.ToString());
    })).setScale(0.2f);
    GameObject cubeBounds = testingUnitTests1.cubeNamed("cBounds");
    bool didPassBounds = true;
    Vector3 failPoint = Vector3.zero;
    LeanTween.move(cubeBounds, new Vector3(10f, 10f, 10f), 0.1f).setOnUpdate((Action<float>) (val =>
    {
      if ((double) cubeBounds.transform.position.x >= 0.0 && (double) cubeBounds.transform.position.x <= 10.0 && (double) cubeBounds.transform.position.y >= 0.0 && (double) cubeBounds.transform.position.y <= 10.0 && (double) cubeBounds.transform.position.z >= 0.0 && (double) cubeBounds.transform.position.z <= 10.0)
        return;
      didPassBounds = false;
      failPoint = cubeBounds.transform.position;
    })).setLoopPingPong().setRepeat(8).setOnComplete((Action) (() => LeanTest.expect((didPassBounds ? 1 : 0) != 0, "OUT OF BOUNDS", $"pos x:{failPoint.x.ToString()} y:{failPoint.y.ToString()} z:{failPoint.z.ToString()}")));
    testingUnitTests1.groupTweens = new LTDescr[1200];
    testingUnitTests1.groupGOs = new GameObject[testingUnitTests1.groupTweens.Length];
    testingUnitTests1.groupTweensCnt = 0;
    int descriptionMatchCount = 0;
    for (int index = 0; index < testingUnitTests1.groupTweens.Length; ++index)
    {
      GameObject gameObject2 = testingUnitTests1.cubeNamed("c" + index.ToString());
      gameObject2.transform.position = new Vector3(0.0f, 0.0f, (float) (index * 3));
      testingUnitTests1.groupGOs[index] = gameObject2;
    }
    yield return (object) new WaitForEndOfFrame();
    bool hasGroupTweensCheckStarted = false;
    int setOnStartNum = 0;
    int setPosNum = 0;
    bool setPosOnUpdate = true;
    for (int index = 0; index < testingUnitTests1.groupTweens.Length; ++index)
    {
      Vector3 to2 = testingUnitTests1.transform.position + Vector3.one * 3f;
      Dictionary<string, object> onCompleteParam = new Dictionary<string, object>()
      {
        {
          "final",
          (object) to2
        },
        {
          "go",
          (object) testingUnitTests1.groupGOs[index]
        }
      };
      testingUnitTests1.groupTweens[index] = LeanTween.move(testingUnitTests1.groupGOs[index], to2, 3f).setOnStart((Action) (() => ++setOnStartNum)).setOnUpdate((Action<Vector3>) (newPosition =>
      {
        if ((double) this.transform.position.z <= (double) newPosition.z)
          return;
        setPosOnUpdate = false;
      })).setOnCompleteParam((object) onCompleteParam).setOnComplete((Action<object>) (param =>
      {
        Dictionary<string, object> dictionary = param as Dictionary<string, object>;
        Vector3 vector3 = (Vector3) dictionary["final"];
        GameObject gameObject3 = dictionary["go"] as GameObject;
        if (vector3.ToString() == gameObject3.transform.position.ToString())
          ++setPosNum;
        if (!hasGroupTweensCheckStarted)
        {
          hasGroupTweensCheckStarted = true;
          LeanTween.delayedCall(this.gameObject, 0.1f, (Action) (() =>
          {
            LeanTest.expect(setOnStartNum == this.groupTweens.Length, "SETONSTART CALLS", $"expected:{this.groupTweens.Length.ToString()} was:{setOnStartNum.ToString()}");
            LeanTest.expect(this.groupTweensCnt == this.groupTweens.Length, "GROUP FINISH", $"expected {this.groupTweens.Length.ToString()} tweens but got {this.groupTweensCnt.ToString()}");
            LeanTest.expect(setPosNum == this.groupTweens.Length, "GROUP POSITION FINISH", $"expected {this.groupTweens.Length.ToString()} tweens but got {setPosNum.ToString()}");
            LeanTest.expect(setPosOnUpdate, "GROUP POSITION ON UPDATE");
          }));
        }
        ++this.groupTweensCnt;
      }));
      if ((UnityEngine.Object) LeanTween.description(testingUnitTests1.groupTweens[index].id).trans == (UnityEngine.Object) testingUnitTests1.groupTweens[index].trans)
        ++descriptionMatchCount;
    }
    while (LeanTween.tweensRunning < testingUnitTests1.groupTweens.Length)
      yield return (object) null;
    LeanTest.expect(descriptionMatchCount == testingUnitTests1.groupTweens.Length, "GROUP IDS MATCH");
    int num1 = testingUnitTests1.groupTweens.Length + 7;
    int num2 = LeanTween.maxSearch <= num1 ? 1 : 0;
    int num3 = LeanTween.maxSearch;
    string failExplaination1 = $"maxSearch:{num3.ToString()} should be:{num1.ToString()}";
    LeanTest.expect(num2 != 0, "MAX SEARCH OPTIMIZED", failExplaination1);
    LeanTest.expect(LeanTween.isTweening(), "SOMETHING IS TWEENING");
    float previousXlt4 = testingUnitTests1.cube4.transform.position.x;
    testingUnitTests1.lt4 = LeanTween.moveX(testingUnitTests1.cube4, 5f, 1.1f).setOnComplete((Action) (() => LeanTest.expect((!((UnityEngine.Object) this.cube4 != (UnityEngine.Object) null) ? 0 : ((double) previousXlt4 != (double) this.cube4.transform.position.x ? 1 : 0)) != 0, "RESUME OUT OF ORDER", $"cube4:{((object) this.cube4)?.ToString()} previousXlt4:{previousXlt4.ToString()} cube4.transform.position.x:{((UnityEngine.Object) this.cube4 != (UnityEngine.Object) null ? this.cube4.transform.position.x : 0.0f).ToString()}"))).setDestroyOnComplete(true);
    testingUnitTests1.lt4.resume();
    TestingUnitTests testingUnitTests2 = testingUnitTests1;
    testingUnitTests1.rotateRepeatAngle = num3 = 0;
    int num4 = num3;
    testingUnitTests2.rotateRepeat = num4;
    LeanTween.rotateAround(testingUnitTests1.cube3, Vector3.forward, 360f, 0.1f).setRepeat(3).setOnComplete(new Action(testingUnitTests1.rotateRepeatFinished)).setOnCompleteOnRepeat(true).setDestroyOnComplete(true);
    yield return (object) new WaitForEndOfFrame();
    LeanTween.delayedCall(1.8f, new Action(testingUnitTests1.rotateRepeatAllFinished));
    int tweensRunning = LeanTween.tweensRunning;
    LeanTween.cancel(testingUnitTests1.lt1Id);
    LeanTest.expect(tweensRunning == LeanTween.tweensRunning, "CANCEL AFTER RESET SHOULD FAIL", $"expected {tweensRunning.ToString()} but got {LeanTween.tweensRunning.ToString()}");
    LeanTween.cancel(testingUnitTests1.cube2);
    int num5 = 0;
    for (int index = 0; index < testingUnitTests1.groupTweens.Length; ++index)
    {
      if (LeanTween.isTweening(testingUnitTests1.groupGOs[index]))
        ++num5;
      if (index % 3 == 0)
        LeanTween.pause(testingUnitTests1.groupGOs[index]);
      else if (index % 3 == 1)
        testingUnitTests1.groupTweens[index].pause();
      else
        LeanTween.pause(testingUnitTests1.groupTweens[index].id);
    }
    LeanTest.expect(num5 == testingUnitTests1.groupTweens.Length, "GROUP ISTWEENING", $"expected {testingUnitTests1.groupTweens.Length.ToString()} tweens but got {num5.ToString()}");
    yield return (object) new WaitForEndOfFrame();
    int num6 = 0;
    for (int index = 0; index < testingUnitTests1.groupTweens.Length; ++index)
    {
      if (index % 3 == 0)
        LeanTween.resume(testingUnitTests1.groupGOs[index]);
      else if (index % 3 == 1)
        testingUnitTests1.groupTweens[index].resume();
      else
        LeanTween.resume(testingUnitTests1.groupTweens[index].id);
      if ((index % 2 == 0 ? (LeanTween.isTweening(testingUnitTests1.groupTweens[index].id) ? 1 : 0) : (LeanTween.isTweening(testingUnitTests1.groupGOs[index]) ? 1 : 0)) != 0)
        ++num6;
    }
    LeanTest.expect(num6 == testingUnitTests1.groupTweens.Length, "GROUP RESUME");
    LeanTest.expect(!LeanTween.isTweening(testingUnitTests1.cube1), "CANCEL TWEEN LTDESCR");
    LeanTest.expect(!LeanTween.isTweening(testingUnitTests1.cube2), "CANCEL TWEEN LEANTWEEN");
    LeanTest.expect(pauseCount == 0, "ON UPDATE NOT CALLED DURING PAUSE", "expect pause count of 0, but got " + pauseCount.ToString());
    yield return (object) new WaitForEndOfFrame();
    Time.timeScale = 0.25f;
    float time = 0.2f;
    float expectedTime = time * (1f / Time.timeScale);
    float start = Time.realtimeSinceStartup;
    bool onUpdateWasCalled = false;
    LeanTween.moveX(testingUnitTests1.cube1, -5f, time).setOnUpdate((Action<float>) (val => onUpdateWasCalled = true)).setOnComplete((Action) (() =>
    {
      float num7 = Time.realtimeSinceStartup - start;
      LeanTest.expect((double) Mathf.Abs(expectedTime - num7) < 0.059999998658895493, "SCALED TIMING DIFFERENCE", $"expected to complete in roughly {expectedTime.ToString()} but completed in {num7.ToString()}");
      LeanTest.expect(Mathf.Approximately(this.cube1.transform.position.x, -5f), "SCALED ENDING POSITION", "expected to end at -5f, but it ended at " + this.cube1.transform.position.x.ToString());
      LeanTest.expect(onUpdateWasCalled, "ON UPDATE FIRED");
    }));
    bool didGetCorrectOnUpdate = false;
    LeanTween.value(testingUnitTests1.gameObject, new Vector3(1f, 1f, 1f), new Vector3(10f, 10f, 10f), 1f).setOnUpdate((Action<Vector3>) (val => didGetCorrectOnUpdate = (double) val.x >= 1.0 && (double) val.y >= 1.0 && (double) val.z >= 1.0)).setOnComplete((Action) (() => LeanTest.expect(didGetCorrectOnUpdate, "VECTOR3 CALLBACK CALLED")));
    yield return (object) new WaitForSeconds(expectedTime);
    Time.timeScale = 1f;
    int num8 = 0;
    foreach (UnityEngine.Object @object in UnityEngine.Object.FindObjectsOfType((System.Type) typeof (GameObject)) as GameObject[])
    {
      if (@object.name == "~LeanTween")
        ++num8;
    }
    LeanTest.expect(num8 == 1, "RESET CORRECTLY CLEANS UP");
    testingUnitTests1.StartCoroutine((IEnumerator) testingUnitTests1.lotsOfCancels());
  }

  private IEnumerator lotsOfCancels()
  {
    TestingUnitTests testingUnitTests = this;
    yield return (object) new WaitForEndOfFrame();
    Time.timeScale = 4f;
    int cubeCount = 10;
    int[] tweensA = new int[cubeCount];
    GameObject[] aGOs = new GameObject[cubeCount];
    for (int index = 0; index < aGOs.Length; ++index)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(testingUnitTests.boxNoCollider);
      gameObject.transform.position = new Vector3(0.0f, 0.0f, (float) index * 2f);
      gameObject.name = "a" + index.ToString();
      aGOs[index] = gameObject;
      tweensA[index] = LeanTween.move(gameObject, gameObject.transform.position + new Vector3(10f, 0.0f, 0.0f), (float) (0.5 + 1.0 * (1.0 / (double) aGOs.Length))).id;
      LeanTween.color(gameObject, Color.red, 0.01f);
    }
    yield return (object) new WaitForSeconds(1f);
    int[] tweensB = new int[cubeCount];
    GameObject[] bGOs = new GameObject[cubeCount];
    for (int index = 0; index < bGOs.Length; ++index)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(testingUnitTests.boxNoCollider);
      gameObject.transform.position = new Vector3(0.0f, 0.0f, (float) index * 2f);
      gameObject.name = "b" + index.ToString();
      bGOs[index] = gameObject;
      tweensB[index] = LeanTween.move(gameObject, gameObject.transform.position + new Vector3(10f, 0.0f, 0.0f), 2f).id;
    }
    for (int index = 0; index < aGOs.Length; ++index)
    {
      LeanTween.cancel(aGOs[index]);
      GameObject gameObject = aGOs[index];
      tweensA[index] = LeanTween.move(gameObject, new Vector3(0.0f, 0.0f, (float) index * 2f), 2f).id;
    }
    yield return (object) new WaitForSeconds(0.5f);
    for (int index = 0; index < aGOs.Length; ++index)
    {
      LeanTween.cancel(aGOs[index]);
      GameObject gameObject = aGOs[index];
      tweensA[index] = LeanTween.move(gameObject, new Vector3(0.0f, 0.0f, (float) index * 2f) + new Vector3(10f, 0.0f, 0.0f), 2f).id;
    }
    for (int index = 0; index < bGOs.Length; ++index)
    {
      LeanTween.cancel(bGOs[index]);
      GameObject gameObject = bGOs[index];
      tweensB[index] = LeanTween.move(gameObject, new Vector3(0.0f, 0.0f, (float) index * 2f), 2f).id;
    }
    yield return (object) new WaitForSeconds(2.1f);
    bool didPass = true;
    for (int index = 0; index < aGOs.Length; ++index)
    {
      if ((double) Vector3.Distance(aGOs[index].transform.position, new Vector3(0.0f, 0.0f, (float) index * 2f) + new Vector3(10f, 0.0f, 0.0f)) > 0.10000000149011612)
        didPass = false;
    }
    for (int index = 0; index < bGOs.Length; ++index)
    {
      if ((double) Vector3.Distance(bGOs[index].transform.position, new Vector3(0.0f, 0.0f, (float) index * 2f)) > 0.10000000149011612)
        didPass = false;
    }
    LeanTest.expect(didPass, "AFTER LOTS OF CANCELS");
    // ISSUE: reference to a compiler-generated method
    testingUnitTests.cubeNamed("cPaused").LeanMoveX(10f, 1f).setOnComplete(new Action(testingUnitTests.\u003ClotsOfCancels\u003Eb__25_0));
    testingUnitTests.StartCoroutine((IEnumerator) testingUnitTests.pauseTimeNow());
  }

  private IEnumerator pauseTimeNow()
  {
    TestingUnitTests testingUnitTests = this;
    yield return (object) new WaitForSeconds(0.5f);
    Time.timeScale = 0.0f;
    LeanTween.delayedCall(0.5f, (Action) (() => Time.timeScale = 1f)).setUseEstimatedTime(true);
    // ISSUE: reference to a compiler-generated method
    LeanTween.delayedCall(1.5f, new Action(testingUnitTests.\u003CpauseTimeNow\u003Eb__26_1)).setUseEstimatedTime(true);
  }

  private void rotateRepeatFinished()
  {
    if ((double) Mathf.Abs(this.cube3.transform.eulerAngles.z) < 9.9999997473787516E-05)
      ++this.rotateRepeatAngle;
    ++this.rotateRepeat;
  }

  private void rotateRepeatAllFinished()
  {
    LeanTest.expect(this.rotateRepeatAngle == 3, "ROTATE AROUND MULTIPLE", $"expected 3 times received {this.rotateRepeatAngle.ToString()} times");
    LeanTest.expect(this.rotateRepeat == 3, "ROTATE REPEAT", $"expected 3 times received {this.rotateRepeat.ToString()} times");
    LeanTest.expect((UnityEngine.Object) this.cube3 == (UnityEngine.Object) null, "DESTROY ON COMPLETE", "cube3:" + ((object) this.cube3)?.ToString());
  }

  private void eventGameObjectCalled(LTEvent e) => this.eventGameObjectWasCalled = true;

  private void eventGeneralCalled(LTEvent e) => this.eventGeneralWasCalled = true;
}
