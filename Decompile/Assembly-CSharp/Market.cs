// Decompiled with JetBrains decompiler
// Type: Market
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Market : MonoBehaviour
{
  private FactionsManager fm;
  public Faction thisFaction;
  public ResourceInventory inventory;
  [SerializeField]
  private List<ResourceQuantity> stockRatios = new List<ResourceQuantity>();
  private List<ResourcePrice> buyPrices = new List<ResourcePrice>();
  private const float buySensitivity = 0.09f;
  private const float sellSensitivity = 0.11f;
  private const float MaxFluctuationRatio = 0.8f;
  [Tooltip("The percentage of Max Storage where the market price equals the base price.")]
  [Range(0.1f, 0.9f)]
  public float idealStockRatio = 0.5f;
  [Header("Transaction Spread")]
  [Tooltip("The percentage difference between the market's BUY and SELL price. (e.g., 0.1 is 10% spread)")]
  public float transactionSpread = 0.1f;
  public float resupplyRatePerHour;

  private void Awake()
  {
    this.SetInventory();
    if (!((UnityEngine.Object) this.inventory == (UnityEngine.Object) null))
      return;
    Debug.LogError((object) "StationMarket requires a ResourceInventory component.");
  }

  private void Start()
  {
    this.fm = FactionsManager.current;
    this.thisFaction = this.fm.GetFactionFromID(this.GetComponent<Track>().factionID);
    GlobalMarket.current.RegisterMarket(this);
    this.UpdatePrices();
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
        {
          this.inventory = current.GetComponent<ResourceInventory>();
          break;
        }
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
  }

  private void UpdatePrices()
  {
    this.buyPrices = new List<ResourcePrice>();
    foreach (ResourceQuantity resourceQuantity in this.inventory.GetAllStock())
      this.buyPrices.Add(new ResourcePrice(resourceQuantity.resource, this.GetCurrentPrice(resourceQuantity.resource, true)));
  }

  public float GetQuantityOf(ResourceDefinition resource) => this.inventory.GetQuantityOf(resource);

  public float GetTotalQuantity() => this.inventory.GetTotalQuantity();

  public int GetCurrentPrice(ResourceDefinition resource, bool isBuying)
  {
    int basePrice = resource.basePrice;
    float quantityOf = this.inventory.GetQuantityOf(resource);
    float minStockQuantity = this.GetMinStockQuantity(resource);
    float b = (double) minStockQuantity > 0.0 ? quantityOf / minStockQuantity : this.idealStockRatio;
    double f = (double) b - (double) this.idealStockRatio;
    float p = isBuying ? 0.09f : 0.11f;
    double num1 = -f * (double) p;
    float num2 = 0.8f * p;
    double min = -(double) num2;
    double max = (double) num2;
    double num3 = (double) Mathf.Clamp((float) num1, (float) min, (float) max);
    float num4 = Mathf.Pow(this.idealStockRatio / Mathf.Max(0.01f, b), p);
    int num5 = Mathf.RoundToInt((float) basePrice * num4);
    float num6 = this.transactionSpread * (1f - Mathf.Clamp01(Mathf.Abs((float) f) / this.idealStockRatio));
    return !isBuying ? Mathf.RoundToInt((float) num5 * (float) (1.0 - (double) num6 * 0.5)) : Mathf.RoundToInt((float) num5 * (1f + num6));
  }

  private float GetStockRatio(ResourceDefinition resource)
  {
    foreach (ResourceQuantity stockRatio in this.stockRatios)
    {
      if ((UnityEngine.Object) stockRatio.resource == (UnityEngine.Object) resource)
        return stockRatio.quantity * 0.01f;
    }
    return 0.0f;
  }

  public float GetMinStockQuantity(ResourceDefinition resource)
  {
    return this.inventory.storageMax * this.GetStockRatio(resource);
  }

  public float GetQuantityAvailable(ResourceDefinition resource)
  {
    return this.inventory.GetQuantityOf(resource) - this.GetMinStockQuantity(resource);
  }

  public float CanBuy(ResourceDefinition resource, float quantity, int factionCredits)
  {
    float minStockQuantity = this.GetMinStockQuantity(resource);
    int currentPrice = this.GetCurrentPrice(resource, true);
    float resupplyRatePerHour = this.resupplyRatePerHour;
    if (resource.type == ResourceType.VOLATILES || resource.type == ResourceType.DT_FUEL || resource.type == ResourceType.DH_FUEL)
      resupplyRatePerHour *= 10f;
    float quantityOf = this.inventory.GetQuantityOf(resource);
    float num1 = Mathf.Max(0.0f, quantityOf - minStockQuantity);
    float num2 = Mathf.Floor((float) (factionCredits / currentPrice));
    return Mathf.Floor(Mathf.Min(quantity, num1, quantityOf, num2, resupplyRatePerHour));
  }

  public void ExecuteTrade(
    ResourceDefinition resource,
    ref float quantity,
    ref int factionCredits,
    ref int totalCost)
  {
    int currentPrice = this.GetCurrentPrice(resource, (double) quantity > 0.0);
    float num1 = 0.0f;
    if ((double) quantity > 0.0)
    {
      float quantity1 = this.CanBuy(resource, quantity, factionCredits);
      if ((double) quantity1 > 0.0)
      {
        num1 = this.inventory.RemoveResource(resource, quantity1);
        totalCost = Mathf.RoundToInt((float) currentPrice * num1);
        factionCredits -= totalCost;
        if (this.thisFaction.factionID == 1)
          this.fm.ModPlayerCredits("BUYING RESOURCE", totalCost);
        else
          this.thisFaction.credits += totalCost;
      }
      quantity = num1;
    }
    else if ((double) quantity < 0.0)
    {
      float resupplyRatePerHour = this.resupplyRatePerHour;
      if (resource.type == ResourceType.VOLATILES || resource.type == ResourceType.DT_FUEL || resource.type == ResourceType.DH_FUEL)
        resupplyRatePerHour *= 10f;
      float quantity2 = Mathf.Min(Mathf.Abs(quantity), resupplyRatePerHour);
      float num2 = this.inventory.AddResource(resource, quantity2);
      if ((double) num2 > 0.0)
      {
        totalCost = Mathf.RoundToInt((float) currentPrice * num2);
        factionCredits += totalCost;
        if (this.thisFaction.factionID == 1)
          this.fm.ModPlayerCredits("SELLING RESOURCE", totalCost);
        else
          this.thisFaction.credits += totalCost;
      }
      quantity = -num2;
    }
    this.UpdatePrices();
  }

  public bool HasResource(ResourceDefinition resource) => this.inventory.HasResource(resource);

  public void ModifyDemand(ResourceDefinition resource, float mod)
  {
    foreach (ResourceQuantity stockRatio in this.stockRatios)
    {
      if ((UnityEngine.Object) stockRatio.resource == (UnityEngine.Object) resource)
      {
        stockRatio.quantity += mod;
        if ((double) stockRatio.quantity < 0.0)
          stockRatio.quantity = 0.0f;
      }
    }
  }

  public MarketData GetMarketData()
  {
    MarketData marketData = new MarketData();
    foreach (ResourceQuantity stockRatio in this.stockRatios)
      marketData.stockRatios.Add(stockRatio.GetResourceData());
    return marketData;
  }

  public void SetFromData(MarketData newMarket, InventoryData newInventory)
  {
    this.inventory.SetFromData(newInventory);
  }
}
