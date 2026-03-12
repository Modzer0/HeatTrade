// Decompiled with JetBrains decompiler
// Type: T_Drive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class T_Drive : T_Module
{
  [Header("DRIVE")]
  public float throttle;

  public override void InitBP()
  {
    MonoBehaviour.print((object) ("override drive initbp: " + this.name));
    base.InitBP();
    if (!(this.BP is DriveModuleBP))
      return;
    this.heat = (this.BP as DriveModuleBP).WasteOutput * 1000f;
  }
}
