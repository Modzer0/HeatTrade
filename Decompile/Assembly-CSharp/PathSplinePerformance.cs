// Decompiled with JetBrains decompiler
// Type: PathSplinePerformance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PathSplinePerformance : MonoBehaviour
{
  public GameObject trackTrailRenderers;
  public GameObject car;
  public GameObject carInternal;
  public float circleLength = 10f;
  public float randomRange = 1f;
  public int trackNodes = 30;
  public float carSpeed = 30f;
  public float tracerSpeed = 2f;
  private LTSpline track;
  private int trackIter = 1;
  private float carAdd;
  private float trackPosition;

  private void Start()
  {
    Application.targetFrameRate = 240 /*0xF0*/;
    List<Vector3> vector3List = new List<Vector3>();
    float num1 = 0.0f;
    int num2 = this.trackNodes + 1;
    for (int index = 0; index < num2; ++index)
    {
      float x = Mathf.Cos(num1 * ((float) Math.PI / 180f)) * this.circleLength + UnityEngine.Random.Range(0.0f, this.randomRange);
      float z = Mathf.Sin(num1 * ((float) Math.PI / 180f)) * this.circleLength + UnityEngine.Random.Range(0.0f, this.randomRange);
      vector3List.Add(new Vector3(x, 1f, z));
      num1 += 360f / (float) this.trackNodes;
    }
    vector3List[0] = vector3List[vector3List.Count - 1];
    vector3List.Add(vector3List[1]);
    vector3List.Add(vector3List[2]);
    this.track = new LTSpline(vector3List.ToArray());
    this.carAdd = this.carSpeed / this.track.distance;
    this.tracerSpeed = this.track.distance / (this.carSpeed * 1.2f);
    LeanTween.moveSpline(this.trackTrailRenderers, this.track, this.tracerSpeed).setOrientToPath(true).setRepeat(-1);
  }

  private void Update()
  {
    float axis = Input.GetAxis("Horizontal");
    if (Input.anyKeyDown)
    {
      if ((double) axis < 0.0 && this.trackIter > 0)
      {
        --this.trackIter;
        this.playSwish();
      }
      else if ((double) axis > 0.0 && this.trackIter < 2)
      {
        ++this.trackIter;
        this.playSwish();
      }
      LeanTween.moveLocalX(this.carInternal, (float) (this.trackIter - 1) * 6f, 0.3f).setEase(LeanTweenType.easeOutBack);
    }
    this.track.place(this.car.transform, this.trackPosition);
    this.trackPosition += Time.deltaTime * this.carAdd;
    if ((double) this.trackPosition <= 1.0)
      return;
    this.trackPosition = 0.0f;
  }

  private void OnDrawGizmos()
  {
    if (this.track == null)
      return;
    this.track.drawGizmo(Color.red);
  }

  private void playSwish()
  {
    LeanAudio.play(LeanAudio.createAudio(new AnimationCurve(new Keyframe[4]
    {
      new Keyframe(0.0f, 0.005464481f, 1.83897f, 0.0f),
      new Keyframe(0.1114856f, 2.281785f, 0.0f, 0.0f),
      new Keyframe(0.2482903f, 2.271654f, 0.0f, 0.0f),
      new Keyframe(0.3f, 0.01670286f, 0.0f, 0.0f)
    }), new AnimationCurve(new Keyframe[3]
    {
      new Keyframe(0.0f, 0.00136725f, 0.0f, 0.0f),
      new Keyframe(0.1482391f, 0.005405405f, 0.0f, 0.0f),
      new Keyframe(0.2650336f, 0.002480127f, 0.0f, 0.0f)
    }), LeanAudio.options().setVibrato(new Vector3[1]
    {
      new Vector3(0.2f, 0.5f, 0.0f)
    }).setWaveNoise().setWaveNoiseScale(1000f)));
  }
}
