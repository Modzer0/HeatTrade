// Decompiled with JetBrains decompiler
// Type: GlobalMarket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class GlobalMarket : MonoBehaviour
{
  public static GlobalMarket current;
  private FactionsManager fm;
  private TimeManager tm;
  private NotificationsManager notifs;
  public AllResources allResources;
  private List<Market> allMarkets = new List<Market>();
  public List<ResourcePrice> buyPrices = new List<ResourcePrice>();
  public List<ResourcePrice> sellPrices = new List<ResourcePrice>();
  public List<ResourceQuantity> supplys = new List<ResourceQuantity>();
  public List<ResourceQuantity> demands = new List<ResourceQuantity>();

  private void Awake()
  {
    GlobalMarket.current = this;
    this.allMarkets.Clear();
  }

  private void Start()
  {
    this.fm = FactionsManager.current;
    this.tm = TimeManager.current;
    if ((bool) (UnityEngine.Object) this.tm)
      this.tm.NewHour += new Action(this.UpdateAveragePrices);
    this.notifs = NotificationsManager.current;
    this.StartCoroutine((IEnumerator) this.Startup());
    foreach (ResourceDefinition resource in this.allResources.resources)
    {
      this.buyPrices.Add(new ResourcePrice(resource, resource.basePrice));
      this.sellPrices.Add(new ResourcePrice(resource, resource.basePrice));
      this.supplys.Add(new ResourceQuantity(resource, 0.0f));
      this.demands.Add(new ResourceQuantity(resource, 0.0f));
    }
  }

  public void RegisterMarket(Market newMarket)
  {
    if (newMarket.thisFaction == null)
      return;
    this.allMarkets.Add(newMarket);
  }

  private IEnumerator Startup()
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.UpdateAveragePrices();
  }

  private void ClearDestroyedMarkets()
  {
    this.allMarkets.RemoveAll((Predicate<Market>) (x => (UnityEngine.Object) x == (UnityEngine.Object) null));
  }

  private void UpdateAveragePrices()
  {
    if (this.allMarkets.Count == 0)
      return;
    this.ClearDestroyedMarkets();
    foreach (ResourcePrice buyPrice in this.buyPrices)
    {
      int num = 0;
      foreach (Market allMarket in this.allMarkets)
        num += allMarket.GetCurrentPrice(buyPrice.resource, true);
      buyPrice.price = num / this.allMarkets.Count;
    }
    foreach (ResourcePrice sellPrice in this.sellPrices)
    {
      int num = 0;
      foreach (Market allMarket in this.allMarkets)
        num += allMarket.GetCurrentPrice(sellPrice.resource, false);
      sellPrice.price = num / this.allMarkets.Count;
    }
    foreach (ResourceQuantity supply in this.supplys)
      supply.quantity = 0.0f;
    foreach (Market allMarket in this.allMarkets)
    {
      if ((bool) (UnityEngine.Object) allMarket.GetComponent<Factory>())
      {
        foreach (ResourceQuantity supply in allMarket.GetComponent<Factory>().GetSupplys())
        {
          ResourceQuantity rp = supply;
          this.supplys.Find((Predicate<ResourceQuantity>) (x => (UnityEngine.Object) x.resource == (UnityEngine.Object) rp.resource)).quantity += rp.quantity;
        }
      }
    }
    foreach (ResourceQuantity demand in this.demands)
      demand.quantity = 0.0f;
    foreach (Market allMarket in this.allMarkets)
    {
      if ((bool) (UnityEngine.Object) allMarket.GetComponent<Factory>())
      {
        foreach (ResourceQuantity demand in allMarket.GetComponent<Factory>().GetDemands())
        {
          ResourceQuantity rp = demand;
          this.demands.Find((Predicate<ResourceQuantity>) (x => (UnityEngine.Object) x.resource == (UnityEngine.Object) rp.resource)).quantity += rp.quantity;
        }
      }
    }
  }

  public int GetAverageBuyPriceOf(ResourceDefinition resource)
  {
    foreach (ResourcePrice buyPrice in this.buyPrices)
    {
      if ((UnityEngine.Object) buyPrice.resource == (UnityEngine.Object) resource)
        return buyPrice.price;
    }
    return 0;
  }

  public int GetAverageSellPriceOf(ResourceDefinition resource)
  {
    foreach (ResourcePrice sellPrice in this.sellPrices)
    {
      if ((UnityEngine.Object) sellPrice.resource == (UnityEngine.Object) resource)
        return sellPrice.price;
    }
    return 0;
  }

  public ResourceDefinition GetRandomResource()
  {
    return this.allResources.resources[UnityEngine.Random.Range(0, this.allResources.resources.Count - 1)];
  }

  public Market GetBestBuyerOf(ResourceDefinition resource, ref int bestPrice)
  {
    return this.GetBestBuyerFromList(resource, this.allMarkets, ref bestPrice);
  }

  public Market GetBestBuyerOfExceptHostile(
    ResourceDefinition resource,
    int mainFactionID,
    ref int bestPrice)
  {
    List<int> hostiles = this.fm.GetFactionFromID(mainFactionID).GetHostiles();
    List<Market> marketsToCheck = new List<Market>();
    foreach (Market allMarket in this.allMarkets)
    {
      if (!hostiles.Contains(allMarket.thisFaction.factionID))
        marketsToCheck.Add(allMarket);
    }
    return this.GetBestBuyerFromList(resource, marketsToCheck, ref bestPrice);
  }

  public Market GetBestBuyerFromList(
    ResourceDefinition resource,
    List<Market> marketsToCheck,
    ref int bestPrice)
  {
    bestPrice = 0;
    List<Market> marketList = new List<Market>();
    foreach (Market market in marketsToCheck)
    {
      int currentPrice = market.GetCurrentPrice(resource, false);
      if (!market.inventory.IsFull())
      {
        if (currentPrice > bestPrice)
        {
          bestPrice = currentPrice;
          marketList.Clear();
          marketList.Add(market);
        }
        else if (currentPrice == bestPrice)
          marketList.Add(market);
      }
    }
    Market bestBuyerFromList = (Market) null;
    if (marketList.Count > 0)
      bestBuyerFromList = marketList[UnityEngine.Random.Range(0, marketList.Count)];
    if ((UnityEngine.Object) bestBuyerFromList == (UnityEngine.Object) null)
      this.notifs.NewNotif("[GLOBAL MARKET] No market buying " + resource.resourceName, "orange");
    return bestBuyerFromList;
  }

  private List<ResourceDefinition> GetRandomizedResources()
  {
    List<ResourceDefinition> source = new List<ResourceDefinition>();
    foreach (ResourceDefinition resource in this.allResources.resources)
      source.Add(resource);
    return source.OrderBy<ResourceDefinition, float>((Func<ResourceDefinition, float>) (x => UnityEngine.Random.value)).ToList<ResourceDefinition>();
  }

  public Market GetBestSellerAny(ref ResourceDefinition refResource)
  {
    foreach (ResourceDefinition randomizedResource in this.GetRandomizedResources())
    {
      Market bestSellerOf = this.GetBestSellerOf(randomizedResource);
      if ((UnityEngine.Object) bestSellerOf != (UnityEngine.Object) null)
      {
        refResource = randomizedResource;
        return bestSellerOf;
      }
    }
    return (Market) null;
  }

  public Market GetBestSellerAnyExceptHostile(
    ref ResourceDefinition refResource,
    Market currentMarket,
    int mainFactionID)
  {
    List<int> hostiles = this.fm.GetFactionFromID(mainFactionID).GetHostiles();
    List<Market> marketsToCheck = new List<Market>();
    foreach (Market allMarket in this.allMarkets)
    {
      if (!hostiles.Contains(allMarket.thisFaction.factionID))
        marketsToCheck.Add(allMarket);
    }
    foreach (ResourceDefinition randomizedResource in this.GetRandomizedResources())
    {
      if (this.GetAverageBuyPriceOf(randomizedResource) < this.GetAverageSellPriceOf(randomizedResource))
      {
        Market bestSellerFromList = this.GetBestSellerFromList(randomizedResource, marketsToCheck);
        if ((UnityEngine.Object) bestSellerFromList != (UnityEngine.Object) null)
        {
          refResource = randomizedResource;
          return bestSellerFromList;
        }
      }
    }
    return (Market) null;
  }

  public Market GetBestSellerOf(ResourceDefinition resource)
  {
    return this.GetBestSellerFromList(resource, this.allMarkets);
  }

  public Market GetBestSellerOfExcept(ResourceDefinition resource, Market marketToExcept)
  {
    List<Market> list = this.allMarkets.Where<Market>((Func<Market, bool>) (x => (UnityEngine.Object) x != (UnityEngine.Object) marketToExcept)).ToList<Market>();
    return this.GetBestSellerFromList(resource, list);
  }

  public Market GetBestSellerFromList(ResourceDefinition resource, List<Market> marketsToCheck)
  {
    int num = int.MaxValue;
    List<Market> marketList = new List<Market>();
    foreach (Market market in marketsToCheck)
    {
      if ((double) market.CanBuy(resource, 1f, 1000000) > 0.0)
      {
        int currentPrice = market.GetCurrentPrice(resource, true);
        if (currentPrice <= this.GetAverageBuyPriceOf(resource))
        {
          if (currentPrice < num && market.HasResource(resource))
          {
            num = currentPrice;
            marketList.Clear();
            marketList.Add(market);
          }
          else if (currentPrice == num && market.HasResource(resource))
            marketList.Add(market);
        }
      }
    }
    Market bestSellerFromList = (Market) null;
    if (marketList.Count > 0)
      bestSellerFromList = marketList[UnityEngine.Random.Range(0, marketList.Count)];
    return bestSellerFromList;
  }

  public float GetSupplyOf(ResourceDefinition resource)
  {
    foreach (ResourceQuantity supply in this.supplys)
    {
      if ((UnityEngine.Object) supply.resource == (UnityEngine.Object) resource)
        return supply.quantity;
    }
    return 0.0f;
  }

  public float GetDemandOf(ResourceDefinition resource)
  {
    foreach (ResourceQuantity demand in this.demands)
    {
      if ((UnityEngine.Object) demand.resource == (UnityEngine.Object) resource)
        return demand.quantity;
    }
    return 0.0f;
  }
}
