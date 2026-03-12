// Decompiled with JetBrains decompiler
// Type: StationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class StationManager : MonoBehaviour
{
  public Track track;
  public List<Structure> structures = new List<Structure>();
  public List<S_Module> modules = new List<S_Module>();
  public S_FuelStorage fuel;
  public StationPort port;
  [Header("INVENTORY")]
  [SerializeField]
  private AllResources allResources;
  private TimeManager tm;
  private ResourceInventory inventory;
  public int personnelCount;

  private void Start()
  {
    this.track = this.GetComponent<Track>();
    IEnumerator enumerator = (IEnumerator) this.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if ((bool) (UnityEngine.Object) current.GetComponent<S_Module>())
        {
          this.modules.Add(current.GetComponent<S_Module>());
          if ((bool) (UnityEngine.Object) current.GetComponent<S_FuelStorage>())
            this.fuel = current.GetComponent<S_FuelStorage>();
        }
        if ((bool) (UnityEngine.Object) current.GetComponent<StationPort>())
        {
          this.port = current.GetComponent<StationPort>();
          this.port.fuel = this.fuel;
          this.port.station = this;
          break;
        }
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    this.tm = TimeManager.current;
    this.SetInventory();
  }

  private void SetInventory()
  {
    IEnumerator enumerator = (IEnumerator) this.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if ((bool) (UnityEngine.Object) current.GetComponent<ResourceInventory>())
          this.inventory = current.GetComponent<ResourceInventory>();
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
  }

  private void LifeCycle()
  {
    if (this.personnelCount == 0)
      return;
    float quantity = (float) ((double) this.personnelCount * 2.5 / 1000.0);
    float num1 = this.inventory.RemoveResource(this.allResources.GetResource(ResourceType.ORGANICS), quantity);
    if ((double) num1 >= (double) quantity)
      return;
    int num2 = (int) ((double) this.personnelCount - (double) num1 * 1000.0);
    MonoBehaviour.print((object) $"{this.name} Num of personnel who starved to death: {num2.ToString()} organics to consume: {quantity.ToString()} organics removed: {num1.ToString()}");
    this.personnelCount -= num2;
  }
}
