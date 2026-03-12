// Decompiled with JetBrains decompiler
// Type: TraderPanelManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class TraderPanelManager : MonoBehaviour
{
  public static TraderPanelManager current;
  [SerializeField]
  private AllResources ar;
  private Trader trader;
  [Header("UI - TRADER")]
  [SerializeField]
  private Toggle tradeToggle;
  public TMP_Text currentMarketText;
  public TMP_Text targetMarketText;
  public TMP_Text targetResourceText;
  public TMP_Text targetQuantityText;
  public GameObject isBuyingImg;
  public TMP_Text buyPriceText;
  public TMP_Text sellPriceText;
  public TMP_Text profitMarginText;
  [Header("UI - CARGO")]
  [SerializeField]
  private Transform resourcesList;
  [SerializeField]
  private ResourceCargoData rcdPF;
  private List<ResourceCargoData> resourceDatas = new List<ResourceCargoData>();
  private List<ResourceQuantity> resourceQs = new List<ResourceQuantity>();
  private List<ResourceInventory> inventories = new List<ResourceInventory>();
  private float totalStorageMax;

  private void Awake() => TraderPanelManager.current = this;

  private void Start()
  {
    foreach (ResourceDefinition resource in this.ar.resources)
      this.resourceQs.Add(new ResourceQuantity(resource, 0.0f));
  }

  private void Update()
  {
    if (!(bool) (UnityEngine.Object) this.trader)
      return;
    if (this.inventories.Count == 0)
    {
      if (this.trader.cargoStorages.Count <= 0)
        return;
      this.NewSelected(this.trader);
    }
    this.UpdateData();
  }

  public void NewSelected(Trader newTrader)
  {
    this.trader = newTrader;
    this.inventories = this.trader.cargoStorages;
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
    this.UpdateData();
  }

  private void UpdateData()
  {
    this.tradeToggle.isOn = this.trader.isAutoTrading;
    string str1 = "NULL";
    if ((bool) (UnityEngine.Object) this.trader.currentMarket)
      str1 = this.trader.currentMarket.GetComponent<Track>().GetFullName();
    this.currentMarketText.text = str1;
    string str2 = "NULL";
    if ((bool) (UnityEngine.Object) this.trader.targetMarket)
      str2 = this.trader.targetMarket.GetComponent<Track>().GetFullName();
    this.targetMarketText.text = str2;
    string str3 = "NULL";
    if ((bool) (UnityEngine.Object) this.trader.targetResource)
      str3 = this.trader.targetResource.resourceName;
    this.targetResourceText.text = str3;
    this.targetQuantityText.text = $"{Mathf.RoundToInt(this.trader.targetQuantity).ToString()}/{Mathf.RoundToInt(this.trader.totalStorageSpace).ToString()}t";
    this.isBuyingImg.SetActive(this.trader.isBuying);
    this.buyPriceText.text = this.trader.buyPrice.ToString() + "cr";
    this.sellPriceText.text = this.trader.bestSellPrice.ToString() + "cr";
    TMP_Text profitMarginText = this.profitMarginText;
    int num = Mathf.RoundToInt(this.trader.profitMargin * 100f);
    string str4 = num.ToString();
    num = Mathf.RoundToInt(this.trader.currentMinProfitMargin * 100f);
    string str5 = num.ToString();
    string str6 = $"{str4}/{str5}%";
    profitMarginText.text = str6;
    this.ClearQuantities();
    this.totalStorageMax = 0.0f;
    foreach (ResourceInventory inventory in this.inventories)
    {
      foreach (ResourceQuantity resourceQuantity in inventory.GetAllStock())
      {
        foreach (ResourceQuantity resourceQ in this.resourceQs)
        {
          if ((UnityEngine.Object) resourceQ.resource == (UnityEngine.Object) resourceQuantity.resource)
            resourceQ.quantity += resourceQuantity.quantity;
        }
      }
      this.totalStorageMax += inventory.storageMax;
    }
    foreach (ResourceQuantity resourceQ in this.resourceQs)
    {
      ResourceQuantity rq = resourceQ;
      if ((double) rq.quantity > 0.0)
      {
        if (!this.resourceDatas.Exists((Predicate<ResourceCargoData>) (x => (UnityEngine.Object) x.resource == (UnityEngine.Object) rq.resource)))
        {
          ResourceCargoData component = UnityEngine.Object.Instantiate<ResourceCargoData>(this.rcdPF, this.resourcesList).GetComponent<ResourceCargoData>();
          component.Setup(rq.resource, rq.resource.icon, rq.resource.resourceName, Mathf.RoundToInt(rq.quantity), Mathf.RoundToInt(this.totalStorageMax));
          this.resourceDatas.Add(component);
        }
        else
          this.resourceDatas.Find((Predicate<ResourceCargoData>) (x => (UnityEngine.Object) x.resource == (UnityEngine.Object) rq.resource)).UpdateData(Mathf.RoundToInt(rq.quantity), Mathf.RoundToInt(this.totalStorageMax));
      }
      else if (this.resourceDatas.Exists((Predicate<ResourceCargoData>) (x => (UnityEngine.Object) x.resource == (UnityEngine.Object) rq.resource)))
      {
        ResourceCargoData resourceCargoData = this.resourceDatas.Find((Predicate<ResourceCargoData>) (x => (UnityEngine.Object) x.resource == (UnityEngine.Object) rq.resource));
        this.resourceDatas.Remove(resourceCargoData);
        if ((bool) (UnityEngine.Object) resourceCargoData)
          UnityEngine.Object.Destroy((UnityEngine.Object) resourceCargoData.gameObject);
      }
    }
  }

  private void ClearQuantities()
  {
    foreach (ResourceQuantity resourceQ in this.resourceQs)
      resourceQ.quantity = 0.0f;
  }

  public void ToggleTrade(bool isOn) => this.trader.SetOn(isOn);
}
