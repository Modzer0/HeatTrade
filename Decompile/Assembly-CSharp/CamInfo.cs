// Decompiled with JetBrains decompiler
// Type: CamInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CamInfo : MonoBehaviour, IClickable
{
  private MapInputs mi;
  public CamInfo zoomOutCI;
  public Vector2 zoomRange;
  public LayerMask lm;

  private void Start() => this.mi = MapInputs.current;

  public void OnEnter() => this.mi.EnterTarget(this);

  public void OnExit() => this.mi.ExitTarget(this);
}
