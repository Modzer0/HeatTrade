// Decompiled with JetBrains decompiler
// Type: MoveLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MoveLine : MonoBehaviour
{
  private LineRenderer lr;
  public Vector3 movePoint;
  [SerializeField]
  private Color color;
  private WorldWidth ww;

  private void Start()
  {
    this.lr = this.GetComponent<LineRenderer>();
    this.ww = WorldWidth.current;
    this.transform.localPosition = Vector3.zero;
    this.movePoint = this.transform.position;
    this.color = this.transform.root.GetComponent<Track>().factionID != 1 ? Color.red : Color.green;
    this.color.a = 0.5f;
    this.lr.startColor = this.color;
    this.lr.endColor = this.color;
  }

  public void Off() => this.UpdateLine(this.transform.position);

  public void UpdateLine(Vector3 targetPos)
  {
    if (!(bool) (Object) this.ww)
      this.ww = WorldWidth.current;
    if ((double) Vector3.Distance(this.transform.position, targetPos) < 10.0)
    {
      this.lr.enabled = false;
    }
    else
    {
      this.lr.enabled = true;
      this.lr.SetPosition(0, this.transform.position);
      this.lr.SetPosition(1, targetPos);
      this.lr.startWidth = this.ww.GetWorldWidth(this.transform.position);
      this.lr.endWidth = this.ww.GetWorldWidth(targetPos);
    }
  }
}
