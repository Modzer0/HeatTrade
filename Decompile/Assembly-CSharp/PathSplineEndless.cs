// Decompiled with JetBrains decompiler
// Type: PathSplineEndless
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PathSplineEndless : MonoBehaviour
{
  public GameObject trackTrailRenderers;
  public GameObject car;
  public GameObject carInternal;
  public GameObject[] cubes;
  private int cubesIter;
  public GameObject[] trees;
  private int treesIter;
  public float randomIterWidth = 0.1f;
  private LTSpline track;
  private List<Vector3> trackPts = new List<Vector3>();
  private int zIter;
  private float carIter;
  private float carAdd;
  private int trackMaxItems = 15;
  private int trackIter = 1;
  private float pushTrackAhead;
  private float randomIter;

  private void Start()
  {
    for (int index = 0; index < 4; ++index)
      this.addRandomTrackPoint();
    this.refreshSpline();
    LeanTween.value(this.gameObject, 0.0f, 0.3f, 2f).setOnUpdate((Action<float>) (val => this.pushTrackAhead = val));
  }

  private void Update()
  {
    if ((double) this.trackPts[this.trackPts.Count - 1].z - (double) this.transform.position.z < 200.0)
    {
      this.addRandomTrackPoint();
      this.refreshSpline();
    }
    this.track.place(this.car.transform, this.carIter);
    this.carIter += this.carAdd * Time.deltaTime;
    this.track.place(this.trackTrailRenderers.transform, this.carIter + this.pushTrackAhead);
    float axis = Input.GetAxis("Horizontal");
    if (!Input.anyKeyDown)
      return;
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

  private GameObject objectQueue(GameObject[] arr, ref int lastIter)
  {
    lastIter = lastIter >= arr.Length - 1 ? 0 : lastIter + 1;
    arr[lastIter].transform.localScale = Vector3.one;
    arr[lastIter].transform.rotation = Quaternion.identity;
    return arr[lastIter];
  }

  private void addRandomTrackPoint()
  {
    float num1 = Mathf.PerlinNoise(0.0f, this.randomIter);
    this.randomIter += this.randomIterWidth;
    Vector3 vector3 = new Vector3((float) (((double) num1 - 0.5) * 20.0), 0.0f, (float) this.zIter * 40f);
    this.objectQueue(this.cubes, ref this.cubesIter).transform.position = vector3;
    GameObject gameObject = this.objectQueue(this.trees, ref this.treesIter);
    float num2 = this.zIter % 2 == 0 ? -15f : 15f;
    gameObject.transform.position = new Vector3(vector3.x + num2, 0.0f, (float) this.zIter * 40f);
    LeanTween.rotateAround(gameObject, Vector3.forward, 0.0f, 1f).setFrom(this.zIter % 2 == 0 ? 180f : -180f).setEase(LeanTweenType.easeOutBack);
    this.trackPts.Add(vector3);
    if (this.trackPts.Count > this.trackMaxItems)
      this.trackPts.RemoveAt(0);
    ++this.zIter;
  }

  private void refreshSpline()
  {
    this.track = new LTSpline(this.trackPts.ToArray());
    this.carIter = this.track.ratioAtPoint(this.car.transform.position);
    this.carAdd = 40f / this.track.distance;
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
