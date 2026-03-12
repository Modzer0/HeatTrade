// Decompiled with JetBrains decompiler
// Type: ShipBP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "ShipBP", menuName = "ScriptableObjects/ShipBP")]
public class ShipBP : ScriptableObject
{
  [SerializeField]
  private Sprite icon;
  [SerializeField]
  private int factionID;
  [SerializeField]
  private string className;
  [SerializeField]
  private string typeFull;
  [SerializeField]
  private string typeShort;
  [SerializeField]
  [TextArea]
  private string description;
  [SerializeField]
  private string prefabKey;
  [SerializeField]
  private float massFull;
  [SerializeField]
  private float cruiseAccMps;
  [SerializeField]
  private float armorThickness;
  [SerializeField]
  private List<ModuleBP> modules;
  [SerializeField]
  private int value;

  public Sprite Icon => this.icon;

  public int FactionID => this.factionID;

  public string ClassName => this.className;

  public string TypeFull => this.typeFull;

  public string TypeShort => this.typeShort;

  public string Description => this.description;

  public string PrefabKey => this.prefabKey;

  public float MassFull => this.massFull;

  public float CruiseAccMps => this.cruiseAccMps;

  public float ArmorThickness => this.armorThickness;

  public List<ModuleBP> Modules => this.modules;

  public int Value => this.value;

  public string GetFullClassName() => $"{this.className}-class {this.typeFull}";

  public string GetShortClassName() => $"{this.className} {this.typeShort}";

  public float SetMass()
  {
    Debug.Log((object) (this.className + " GetSetMass"));
    float num1 = 0.0f;
    float num2 = (float) ((double) this.armorThickness * 60.0 / 1000.0);
    foreach (ModuleBP module in this.modules)
    {
      Debug.Log((object) $"{module.name} mass: {module.Mass.ToString()}");
      float num3 = num2 * 3.14159274f * module.Diameter * module.Length;
      num1 += module.Mass + num3;
    }
    Debug.Log((object) ("mass full: " + num1.ToString()));
    this.massFull = num1;
    return num1;
  }

  public float SetCruiseAcc()
  {
    Debug.Log((object) (this.className + " get set cruise acc"));
    double num1 = (double) this.SetMass();
    DriveModuleBP driveModuleBp = this.modules.Find((Predicate<ModuleBP>) (x => x is DriveModuleBP)) as DriveModuleBP;
    float num2 = driveModuleBp.CruiseThrustN * (1f / 1000f);
    if ((bool) (UnityEngine.Object) driveModuleBp)
      this.cruiseAccMps = num2 / this.massFull;
    Debug.Log((object) $"{this.className} cruise acc: {this.cruiseAccMps.ToString()}");
    return this.cruiseAccMps;
  }

  [ContextMenu("Set Values")]
  public void SetValues()
  {
    Debug.Log((object) ("Setting values of: " + this.className));
    double num = (double) this.SetCruiseAcc();
  }
}
