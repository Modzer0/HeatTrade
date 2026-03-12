// Decompiled with JetBrains decompiler
// Type: GlobalMarketPanelManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GlobalMarketPanelManager : MonoBehaviour
{
  private TimeManager tm;
  [SerializeField]
  private GlobalMarket gm;
  [SerializeField]
  private AllResources allResources;
  public bool isOn;
  private List<ResourceGlobalData> rgdList = new List<ResourceGlobalData>();
  [Header("UI")]
  [SerializeField]
  private GameObject panel;
  [SerializeField]
  private ResourceGlobalData rgdPF;
  [SerializeField]
  private Transform dataList;

  private void Start()
  {
    this.tm = TimeManager.current;
    this.tm.NewHour += new Action(this.NewHour);
    this.StartCoroutine((IEnumerator) this.DelayedStart());
  }

  private IEnumerator DelayedStart()
  {
    yield return (object) new WaitForSeconds(1f);
    IEnumerator enumerator = (IEnumerator) this.dataList.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if (!(current.name == "COLUMNS"))
          UnityEngine.Object.Destroy((UnityEngine.Object) current.gameObject);
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    foreach (ResourceDefinition resource in this.allResources.resources)
    {
      ResourceGlobalData component = UnityEngine.Object.Instantiate<ResourceGlobalData>(this.rgdPF, this.dataList).GetComponent<ResourceGlobalData>();
      this.rgdList.Add(component);
      component.SetResource(resource, this.gm.GetAverageBuyPriceOf(component.resource), this.gm.GetAverageSellPriceOf(component.resource), this.gm.GetSupplyOf(component.resource), this.gm.GetDemandOf(component.resource));
    }
    this.NewHour();
  }

  public void Toggle(bool newIsOn)
  {
    this.isOn = newIsOn;
    this.panel.SetActive(this.isOn);
    this.NewHour();
  }

  private void NewHour()
  {
    if (!this.isOn)
      return;
    foreach (ResourceGlobalData rgd in this.rgdList)
      rgd.UpdateData(this.gm.GetAverageBuyPriceOf(rgd.resource), this.gm.GetAverageSellPriceOf(rgd.resource), this.gm.GetSupplyOf(rgd.resource), this.gm.GetDemandOf(rgd.resource));
  }
}
