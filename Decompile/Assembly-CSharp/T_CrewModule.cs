// Decompiled with JetBrains decompiler
// Type: T_CrewModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class T_CrewModule : T_Module
{
  [Header("CREW MODULE")]
  public int dcTeams;
  public int materials;

  public override void InitBP()
  {
    base.InitBP();
    if (!(this.BP is CrewModuleBP))
      return;
    CrewModuleBP bp = this.BP as CrewModuleBP;
    this.dcTeams = bp.DCTeams;
    this.materials = bp.Materials;
  }
}
