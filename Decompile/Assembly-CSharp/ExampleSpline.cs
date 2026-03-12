// Decompiled with JetBrains decompiler
// Type: ExampleSpline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ExampleSpline : MonoBehaviour
{
  public Transform[] trans;
  private LTSpline spline;
  private GameObject ltLogo;
  private GameObject ltLogo2;
  private float iter;

  private void Start()
  {
    this.spline = new LTSpline(new Vector3[5]
    {
      this.trans[0].position,
      this.trans[1].position,
      this.trans[2].position,
      this.trans[3].position,
      this.trans[4].position
    });
    this.ltLogo = GameObject.Find("LeanTweenLogo1");
    this.ltLogo2 = GameObject.Find("LeanTweenLogo2");
    LeanTween.moveSpline(this.ltLogo2, this.spline.pts, 1f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong().setOrientToPath(true);
    LeanTween.moveSpline(this.ltLogo2, new Vector3[5]
    {
      Vector3.zero,
      Vector3.zero,
      new Vector3(1f, 1f, 1f),
      new Vector3(2f, 1f, 1f),
      new Vector3(2f, 1f, 1f)
    }, 1.5f).setUseEstimatedTime(true);
  }

  private void Update()
  {
    this.ltLogo.transform.position = this.spline.point(this.iter);
    this.iter += Time.deltaTime * 0.1f;
    if ((double) this.iter <= 1.0)
      return;
    this.iter = 0.0f;
  }

  private void OnDrawGizmos()
  {
    if (this.spline == null)
      return;
    this.spline.gizmoDraw();
  }
}
