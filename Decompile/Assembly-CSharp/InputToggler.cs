// Decompiled with JetBrains decompiler
// Type: InputToggler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class InputToggler : MonoBehaviour
{
  private MapInputs mi;
  private TacticalInputs ti;
  private bool isInit;
  private bool isOn = true;

  private void Start() => this.Init();

  private void Init()
  {
    if (this.isInit)
      return;
    this.mi = MapInputs.current;
    this.ti = TacticalInputs.current;
    this.isInit = true;
  }

  public void Toggle(bool newIsOn)
  {
    this.Init();
    this.isOn = newIsOn;
    if ((bool) (Object) this.mi)
    {
      this.mi.isInputOn = this.isOn;
    }
    else
    {
      if (!(bool) (Object) this.ti)
        return;
      this.ti.isInputOn = this.isOn;
    }
  }
}
