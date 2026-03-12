// Decompiled with JetBrains decompiler
// Type: LaserToggler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LaserToggler : MonoBehaviour
{
  public static LaserToggler current;
  private bool isVisible = true;
  private List<LineRenderer> lasers = new List<LineRenderer>();

  public bool IsVisible => this.isVisible;

  private void Awake() => LaserToggler.current = this;

  public void AddThisLaser(LineRenderer newLaser)
  {
    newLaser.enabled = this.isVisible;
    this.lasers.Add(newLaser);
  }

  public void ToggleLaserVisibility(bool? mod)
  {
    MonoBehaviour.print((object) ("LT: TOGGLE LASER VISIBILITY: " + mod.ToString()));
    this.isVisible = !mod.HasValue ? !this.isVisible : mod.Value;
    foreach (Renderer laser in this.lasers)
      laser.enabled = this.isVisible;
  }
}
