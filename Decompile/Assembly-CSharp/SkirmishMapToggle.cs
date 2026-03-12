// Decompiled with JetBrains decompiler
// Type: SkirmishMapToggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SkirmishMapToggle : MonoBehaviour
{
  [SerializeField]
  private SkirmishSetup ss;
  [SerializeField]
  private SkirmishMapName mapName;

  public void SetOn(bool isOn)
  {
    MonoBehaviour.print((object) $"set on: {isOn.ToString()} {this.mapName.ToString()}");
    if (!isOn)
      return;
    this.ss.SetSkirmishMapName(this.mapName);
  }
}
