// Decompiled with JetBrains decompiler
// Type: Borderline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Borderline : MonoBehaviour
{
  private LineRenderer lr;
  private float maxDist = 5000f;
  private int segments = 128 /*0x80*/;
  private float currentRadius = 5000f;
  [SerializeField]
  private float height;

  private void Start()
  {
    this.lr = this.GetComponent<LineRenderer>();
    this.lr.useWorldSpace = true;
    this.lr.loop = true;
    this.height = this.transform.position.y;
    this.currentRadius = Mathf.Sqrt((float) ((double) this.maxDist * (double) this.maxDist - (double) this.height * (double) this.height));
    this.DrawBorder();
  }

  private void DrawBorder()
  {
    if (this.segments < 3)
      this.segments = 3;
    this.lr.positionCount = this.segments;
    float num = 6.28318548f / (float) this.segments;
    for (int index = 0; index < this.segments; ++index)
    {
      double f = (double) index * (double) num;
      float x = Mathf.Cos((float) f) * this.currentRadius;
      float z = Mathf.Sin((float) f) * this.currentRadius;
      this.lr.SetPosition(index, new Vector3(x, this.height, z));
    }
  }

  public void UpdateHeight(float heightPoint)
  {
    heightPoint = Mathf.Clamp(heightPoint, -this.maxDist, this.maxDist);
    this.height = heightPoint;
    this.currentRadius = Mathf.Sqrt((float) ((double) this.maxDist * (double) this.maxDist - (double) this.height * (double) this.height));
    this.DrawBorder();
  }
}
