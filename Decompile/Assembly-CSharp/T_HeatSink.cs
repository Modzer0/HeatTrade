// Decompiled with JetBrains decompiler
// Type: T_HeatSink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class T_HeatSink : T_Module
{
  public override void InitBP()
  {
    base.InitBP();
    if (!(this.BP is HeatsinkModuleBP))
      return;
    this.heatCapacityMJ = (this.BP as HeatsinkModuleBP).HeatCapacity;
    this.heatCapacityMaxMJ = this.heatCapacityMJ;
  }
}
