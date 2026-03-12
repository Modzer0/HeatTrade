// Decompiled with JetBrains decompiler
// Type: TrackButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TrackButton : MonoBehaviour
{
  public Target target;

  private void Start()
  {
  }

  private void Update()
  {
  }

  public void OnClick() => Sensors.current.SetSelected(this.target.transform);

  public void OnHover()
  {
    MonoBehaviour.print((object) ("ON HOVER. TARGET: " + this.target.targetName));
  }

  public void OnScroll() => CamsManager.current.MoveCamTowards(this.target.transform);
}
