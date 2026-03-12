// Decompiled with JetBrains decompiler
// Type: PartBP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "PartBP", menuName = "ScriptableObjects/PartBP")]
public class PartBP : ScriptableObject
{
  [Header("= PART ====================")]
  [SerializeField]
  private string prefabKey;
  [SerializeField]
  private string partNameFull;
  [SerializeField]
  private string partNameShort;
  [SerializeField]
  private string description;
  [SerializeField]
  private PartType partType;
  [SerializeField]
  private SizeClass sizeClass;
  [SerializeField]
  private RepairCycle repairCycle;
  [SerializeField]
  private float density;
  [SerializeField]
  private float mass;
  [SerializeField]
  private float armorHealthMax;
  [SerializeField]
  private float armorThickness;

  public string PrefabKey => this.prefabKey;

  public string PartNameFull => this.partNameFull;

  public string PartNameShort => this.partNameShort;

  public string Description => this.description;

  public PartType PartType => this.partType;

  public SizeClass SizeClass => this.sizeClass;

  public ResourceQuantity RepairQuantity => this.repairCycle.input;

  public float Density => this.density;

  public float Mass => this.mass;

  public float ArmorHealthMax => this.armorHealthMax;

  public float ArmorThickness => this.armorThickness;
}
