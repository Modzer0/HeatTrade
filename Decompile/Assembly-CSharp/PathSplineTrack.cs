// Decompiled with JetBrains decompiler
// Type: PathSplineTrack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PathSplineTrack : MonoBehaviour
{
  public GameObject car;
  public GameObject carInternal;
  public GameObject trackTrailRenderers;
  public Transform[] trackOnePoints;
  private LTSpline track;
  private int trackIter = 1;
  private float trackPosition;

  private void Start()
  {
    this.track = new LTSpline(new Vector3[7]
    {
      this.trackOnePoints[0].position,
      this.trackOnePoints[1].position,
      this.trackOnePoints[2].position,
      this.trackOnePoints[3].position,
      this.trackOnePoints[4].position,
      this.trackOnePoints[5].position,
      this.trackOnePoints[6].position
    });
    LeanTween.moveSpline(this.trackTrailRenderers, this.track, 2f).setOrientToPath(true).setRepeat(-1);
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
    this.trackPosition += Time.deltaTime * 0.03f;
    if ((double) this.trackPosition < 0.0)
    {
      this.trackPosition = 1f;
    }
    else
    {
      if ((double) this.trackPosition <= 1.0)
        return;
      this.trackPosition = 0.0f;
    }
  }

  private void OnDrawGizmos() => LTSpline.drawGizmo(this.trackOnePoints, Color.red);

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
