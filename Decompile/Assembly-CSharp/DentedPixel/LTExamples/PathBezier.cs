// Decompiled with JetBrains decompiler
// Type: DentedPixel.LTExamples.PathBezier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace DentedPixel.LTExamples;

public class PathBezier : MonoBehaviour
{
  public Transform[] trans;
  private LTBezierPath cr;
  private GameObject avatar1;
  private float iter;

  private void OnEnable()
  {
    this.cr = new LTBezierPath(new Vector3[8]
    {
      this.trans[0].position,
      this.trans[2].position,
      this.trans[1].position,
      this.trans[3].position,
      this.trans[3].position,
      this.trans[5].position,
      this.trans[4].position,
      this.trans[6].position
    });
  }

  private void Start()
  {
    this.avatar1 = GameObject.Find("Avatar1");
    LTDescr ltDescr = LeanTween.move(this.avatar1, this.cr.pts, 6.5f).setOrientToPath(true).setRepeat(-1);
    Debug.Log((object) ("length of path 1:" + this.cr.length.ToString()));
    Debug.Log((object) ("length of path 2:" + ltDescr.optional.path.length.ToString()));
  }

  private void Update()
  {
    this.iter += Time.deltaTime * 0.07f;
    if ((double) this.iter <= 1.0)
      return;
    this.iter = 0.0f;
  }

  private void OnDrawGizmos()
  {
    if (this.cr != null)
      this.OnEnable();
    Gizmos.color = Color.red;
    if (this.cr == null)
      return;
    this.cr.gizmoDraw();
  }
}
