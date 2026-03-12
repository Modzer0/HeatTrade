// Decompiled with JetBrains decompiler
// Type: Structure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structure : MonoBehaviour
{
  private AllStructures allStructures;
  public TacticalObjectSource tacticalObjectSource;
  private bool isInitialized;
  public List<S_Module> modules = new List<S_Module>();
  private S_Propulsion propulsion;
  public S_FuelStorage fuelStorage = new S_FuelStorage();
  [Header("ARMOR")]
  public float armorThickness;
  [SerializeField]
  private float armorDensity;
  public float armorMass;
  [Header("MASS AND ACCELERATION")]
  public float currentMass;
  public float dryMass;
  public float fullMass;
  public float maxAcceleration;
  public float dV;
  public float dVMax;

  private void Start() => this.Init();

  private void Update()
  {
    if ((double) this.maxAcceleration != 0.0 || !(bool) (UnityEngine.Object) this.propulsion || (double) this.propulsion.maxThrust <= 0.0)
      return;
    this.SetMaxAcc();
  }

  public float GetHealthRatio()
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    foreach (S_Module module in this.modules)
    {
      num1 += module.health;
      num2 += module.healthMax;
    }
    return (double) num2 == 0.0 ? 0.0f : Mathf.Clamp01(num1 / num2);
  }

  private void SetMaxAcc()
  {
    this.maxAcceleration = (float) ((double) this.propulsion.maxThrust * 1000000.0 / ((double) this.currentMass * 1000.0));
    this.tacticalObjectSource.maxAcceleration = this.maxAcceleration;
  }

  public void Init()
  {
    if (this.isInitialized)
      return;
    MonoBehaviour.print((object) ("Init: " + this.name));
    this.allStructures = AllStructures.current;
    this.tacticalObjectSource = this.GetComponent<TacticalObjectSource>();
    IEnumerator enumerator = (IEnumerator) this.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if ((bool) (UnityEngine.Object) current.GetComponent<S_Module>())
        {
          S_Module component = current.GetComponent<S_Module>();
          if (!this.modules.Contains(component))
            this.modules.Add(component);
          if (component is S_Propulsion)
          {
            this.propulsion = component.GetComponent<S_Propulsion>();
            this.propulsion.Init();
          }
          else if (component is S_FuelStorage)
          {
            this.fuelStorage = component.GetComponent<S_FuelStorage>();
            this.propulsion.TryAddFuel(this.fuelStorage);
          }
        }
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    this.UpdateMass();
    this.isInitialized = true;
  }

  public void UpdateMass()
  {
    float fuelMass = this.fuelStorage.GetFuelMass();
    float fuelMax = this.fuelStorage.GetFuelMax();
    this.armorMass = 0.0f;
    this.armorDensity = (float) ((double) this.armorThickness * 60.0 / 1000.0);
    float num1 = 0.0f;
    foreach (S_Module module in this.modules)
    {
      float num2 = this.armorDensity * 3.14159274f * module.diameter * module.length;
      this.armorMass += num2;
      num1 += module.mass + num2;
    }
    this.dryMass = num1;
    this.currentMass = num1 + fuelMass;
    this.fullMass = num1 + fuelMax;
  }

  public void ConsumeDv(float acc) => this.propulsion.ConsumeDv(acc);

  public float GetDv()
  {
    if (!(bool) (UnityEngine.Object) this.propulsion || !(bool) (UnityEngine.Object) this.fuelStorage)
      return 0.0f;
    this.UpdateMass();
    this.dV = (float) ((double) (9.81f * this.propulsion.isp) * (double) Mathf.Log(this.currentMass / this.dryMass) / 1000.0);
    return this.dV;
  }

  public float GetDvMax()
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || !(bool) (UnityEngine.Object) this.propulsion || !(bool) (UnityEngine.Object) this.fuelStorage)
      return 0.0f;
    this.UpdateMass();
    this.dVMax = (float) ((double) (9.81f * this.propulsion.isp) * (double) Mathf.Log(this.fullMass / this.dryMass) / 1000.0);
    return this.dVMax;
  }

  public float GetMaxAcceleration()
  {
    if ((double) this.maxAcceleration == 0.0 && (bool) (UnityEngine.Object) this.propulsion && (double) this.propulsion.maxThrust > 0.0)
      this.SetMaxAcc();
    return this.maxAcceleration;
  }

  public List<S_Module> GetModules(PartType type)
  {
    List<S_Module> modules = new List<S_Module>();
    foreach (S_Module module in this.modules)
    {
      if (module.partType == type)
        modules.Add(module);
    }
    return modules;
  }
}
