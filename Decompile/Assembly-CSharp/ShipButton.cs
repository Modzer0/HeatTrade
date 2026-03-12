// Decompiled with JetBrains decompiler
// Type: ShipButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ShipButton : MonoBehaviour
{
  private TacticalInputs ti;
  public int index;

  private void Start() => this.ti = TacticalInputs.current;

  public void OnClick()
  {
    if (this.index - 1 < 0)
      return;
    this.ti.SetSelectedShipIndex(this.index - 1);
  }

  public void Toggle(bool isOn)
  {
    if (!(bool) (Object) this.ti)
      this.ti = TacticalInputs.current;
    if (this.index - 1 < 0 || !isOn)
      return;
    this.ti.SetSelectedShipIndex(this.index - 1);
  }
}
