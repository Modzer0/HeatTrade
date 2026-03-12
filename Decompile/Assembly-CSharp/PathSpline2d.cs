// Decompiled with JetBrains decompiler
// Type: PathSpline2d
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PathSpline2d : MonoBehaviour
{
  public Transform[] cubes;
  public GameObject dude1;
  public GameObject dude2;
  private LTSpline visualizePath;

  private void Start()
  {
    Vector3[] vector3Array = new Vector3[5]
    {
      this.cubes[0].position,
      this.cubes[1].position,
      this.cubes[2].position,
      this.cubes[3].position,
      this.cubes[4].position
    };
    this.visualizePath = new LTSpline(vector3Array);
    LeanTween.moveSpline(this.dude1, vector3Array, 10f).setOrientToPath2d(true).setSpeed(2f);
    LeanTween.moveSplineLocal(this.dude2, vector3Array, 10f).setOrientToPath2d(true).setSpeed(2f);
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    if (this.visualizePath == null)
      return;
    this.visualizePath.gizmoDraw();
  }
}
