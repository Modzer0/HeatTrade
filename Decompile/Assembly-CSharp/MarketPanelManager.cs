// Decompiled with JetBrains decompiler
// Type: MarketPanelManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class MarketPanelManager : MonoBehaviour
{
  [SerializeField]
  private AllResources ar;
  private Market currentMarket;
  private ResourceInventory currentInventory;
  [SerializeField]
  private Transform resourcesList;
  [SerializeField]
  private ResourceMarketData rmdPF;
  private List<ResourceMarketData> resourceDatas = new List<ResourceMarketData>();
  [SerializeField]
  private GameObject marketFullImg;
  [Header("Capacity bar")]
  [SerializeField]
  private uiBar capacityBar;
  [SerializeField]
  private TMP_Text capacityText;
  [SerializeField]
  private TMP_Text percentText;

  private void Update()
  {
    if (!(bool) (UnityEngine.Object) this.currentMarket)
      return;
    foreach (ResourceMarketData resourceData in this.resourceDatas)
    {
      foreach (ResourceDefinition resource in this.ar.resources)
      {
        if ((UnityEngine.Object) resourceData.resource == (UnityEngine.Object) resource)
          resourceData.UpdateData(this.currentMarket.GetCurrentPrice(resource, true), this.currentMarket.GetCurrentPrice(resource, false), (int) this.currentInventory.GetQuantityOf(resource), (int) this.currentInventory.storageMax);
      }
    }
    if ((bool) (UnityEngine.Object) this.capacityBar)
    {
      float capacityRatio = this.currentInventory.GetCapacityRatio();
      this.capacityBar.SetBarSize(capacityRatio);
      this.percentText.text = Mathf.RoundToInt(capacityRatio * 100f).ToString() + "%";
      float storageUsed = this.currentInventory.storageUsed;
      float storageMax = this.currentInventory.storageMax;
      this.capacityText.text = $"{storageUsed.ToString("N0")} / {storageMax.ToString("N0")}t";
    }
    if (this.currentInventory.IsFull())
      this.marketFullImg.SetActive(true);
    else
      this.marketFullImg.SetActive(false);
  }

  public void Setup(Market newMarket)
  {
    this.currentMarket = newMarket;
    this.currentInventory = this.currentMarket.inventory;
    this.resourceDatas.Clear();
    IEnumerator enumerator = (IEnumerator) this.resourcesList.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if (current.name != "COLUMNS")
          UnityEngine.Object.Destroy((UnityEngine.Object) current.gameObject);
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    foreach (ResourceDefinition resource in this.ar.resources)
    {
      ResourceMarketData component = UnityEngine.Object.Instantiate<ResourceMarketData>(this.rmdPF, this.resourcesList).GetComponent<ResourceMarketData>();
      component.Setup(resource, resource.icon, resource.resourceName, this.currentMarket.GetCurrentPrice(resource, true), this.currentMarket.GetCurrentPrice(resource, false), (int) this.currentInventory.GetQuantityOf(resource), (int) this.currentInventory.storageMax);
      this.resourceDatas.Add(component);
      component.gameObject.SetActive(true);
    }
  }
}
