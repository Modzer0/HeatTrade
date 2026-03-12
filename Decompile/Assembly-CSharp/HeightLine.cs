// Decompiled with JetBrains decompiler
// Type: HeightLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (LineRenderer))]
public class HeightLine : MonoBehaviour
{
  private LineRenderer lr;
  public float heightPoint;
  private Transform circle;
  private WorldWidth ww;
  private float worldWidth;
  [SerializeField]
  private Color color;
  public bool onlyShowWhenMove;
  public bool isShow = true;
  private Vector3 endPos;
  private Material lineMaterial;
  private static readonly int LineLengthID = Shader.PropertyToID("_LineLength");

  private void Start()
  {
    this.transform.localPosition = Vector3.zero;
    this.lr = this.GetComponent<LineRenderer>();
    this.ww = WorldWidth.current;
    this.circle = this.transform.GetChild(0);
    if (this.onlyShowWhenMove)
      this.isShow = false;
    this.color = new Color(0.1f, 0.1f, 0.1f, 1f);
    this.lr.startColor = this.color;
    this.lr.endColor = this.color;
    this.lineMaterial = this.lr.material;
    if (!((Object) this.lineMaterial != (Object) null))
      return;
    this.lineMaterial = new Material(this.lineMaterial);
    this.lr.material = this.lineMaterial;
  }

  private void Update()
  {
    if (this.lr.positionCount < 2)
      return;
    float num = 0.0f;
    Vector3 a = this.lr.GetPosition(0);
    for (int index = 1; index < this.lr.positionCount; ++index)
    {
      Vector3 position = this.lr.GetPosition(index);
      num += Vector3.Distance(a, position);
      a = position;
    }
    if (!(bool) (Object) this.lineMaterial)
      return;
    this.lineMaterial.SetFloat(HeightLine.LineLengthID, num);
  }

  public void LateUpdate()
  {
    if (!this.isShow)
    {
      this.lr.SetPosition(0, Vector3.zero);
      this.lr.SetPosition(1, Vector3.zero);
    }
    else
    {
      this.endPos = new Vector3(this.transform.position.x, this.heightPoint, this.transform.position.z);
      if ((bool) (Object) this.circle)
      {
        this.circle.position = this.endPos;
        this.circle.rotation = Quaternion.Euler(90f, 0.0f, 0.0f);
      }
      this.lr.SetPosition(0, this.endPos);
      this.lr.SetPosition(1, this.transform.position);
      if (!(bool) (Object) this.ww)
        this.ww = WorldWidth.current;
      if (!(bool) (Object) this.ww)
        return;
      this.worldWidth = this.ww.GetWorldWidth(this.endPos);
      this.lr.startWidth = this.worldWidth;
      this.worldWidth *= 20f;
      if ((bool) (Object) this.circle)
        this.circle.localScale = new Vector3(this.worldWidth, this.worldWidth, this.worldWidth);
      this.worldWidth = this.ww.GetWorldWidth(this.transform.position);
      this.lr.endWidth = this.worldWidth;
    }
  }
}
