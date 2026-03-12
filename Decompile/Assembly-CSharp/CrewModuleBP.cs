// Decompiled with JetBrains decompiler
// Type: CrewModuleBP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "CrewModuleBP", menuName = "ScriptableObjects/CrewModuleBP")]
public class CrewModuleBP : ModuleBP
{
  [Header("= CREW MODULE ====================")]
  [SerializeField]
  private int crewCount;
  [SerializeField]
  private int dcTeams;
  [SerializeField]
  private int materials;
  [SerializeField]
  private ResupplyCycle resupplyCycle;

  public int CrewCount => this.crewCount;

  public int DCTeams => this.dcTeams;

  public int Materials => this.materials;

  public ResupplyCycle ResupplyCycle => this.resupplyCycle;

  public float GetResourceMax() => (float) this.materials;

  public ResupplyCycle GetResupplyCycle() => this.resupplyCycle;
}
