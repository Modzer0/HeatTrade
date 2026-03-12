// Decompiled with JetBrains decompiler
// Type: Trader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Trader : MonoBehaviour
{
  private Navigation navigation;
  private FleetManager fleet;
  private GlobalMarket gm;
  private Track thisTrack;
  private TimeManager tm;
  private ColorManager cm;
  private NotificationsManager notifs;
  private FactionsManager fm;
  public bool isAutoTrading;
  public bool isTrading;
  public Market currentMarket;
  public Market targetMarket;
  public ResourceDefinition targetResource;
  public float targetQuantity;
  public float totalStorageSpace;
  public bool isBuying;
  public int buyPrice;
  public int bestSellPrice;
  public float profitMargin;
  public float minProfitMargin = 0.15f;
  public float currentMinProfitMargin = 0.15f;
  public bool isTradeValid;
  public List<ResourceInventory> cargoStorages = new List<ResourceInventory>();
  private bool hasArrived;

  public bool HasArrived => this.hasArrived;

  private void Start()
  {
    this.navigation = this.GetComponent<Navigation>();
    this.fleet = this.GetComponent<FleetManager>();
    this.gm = GlobalMarket.current;
    this.thisTrack = this.GetComponent<Track>();
    this.tm = TimeManager.current;
    this.cm = ColorManager.current;
    this.notifs = NotificationsManager.current;
    this.fm = FactionsManager.current;
    this.navigation.onArrival += new Action(this.OnArrival);
    this.tm.NewHour += new Action(this.NewHour);
    this.currentMinProfitMargin = this.minProfitMargin;
    this.CheckCurrentMarket();
    this.StartCoroutine((IEnumerator) this.StartTradeCycle());
  }

  private void CheckCurrentMarket()
  {
    this.currentMarket = (Market) null;
    if (!(bool) (UnityEngine.Object) this.transform.parent || !(bool) (UnityEngine.Object) this.transform.parent.GetComponent<Market>() || !(bool) (UnityEngine.Object) this.transform.parent.GetComponent<StationManager>())
      return;
    this.currentMarket = this.transform.parent.GetComponent<Market>();
  }

  public void SetOn(bool isOn)
  {
    this.isAutoTrading = isOn;
    this.isTrading = isOn;
    this.StopCoroutine((IEnumerator) this.StartTradeCycle());
    if (!isOn)
      return;
    this.StartCoroutine((IEnumerator) this.StartTradeCycle());
  }

  private IEnumerator StartTradeCycle(float initialDelay = 1f)
  {
    Trader trader = this;
    yield return (object) new WaitForSeconds(initialDelay);
    while (!trader.isTradeValid)
    {
      yield return (object) new WaitForSeconds(1f);
      trader.isTradeValid = trader.TradeCycle();
      if (!trader.enabled)
        trader.isTradeValid = false;
    }
    trader.navigation.accG = trader.fleet.maxAccG;
    trader.navigation.accRatio = 0.5f;
    trader.navigation.decRatio = 0.5f;
    trader.navigation.SetTarget(trader.targetMarket.transform);
    bool flag = false;
    float cycleModifier = 0.5f;
    while (!flag)
    {
      yield return (object) new WaitForSeconds(0.5f);
      flag = true;
      if (trader.navigation.isMaxAccExceeded || trader.navigation.isInsufficientDv || trader.navigation.isCollisionWarning)
      {
        trader.navigation.accG *= cycleModifier;
        trader.navigation.accRatio *= cycleModifier;
        trader.navigation.decRatio *= cycleModifier;
        flag = false;
      }
      if ((double) trader.fleet.fleetDv / (double) trader.fleet.fleetDvMax < 0.10000000149011612)
      {
        trader.fleet.SetStateTo(FleetState.REFUELING);
        flag = false;
        trader.StopCoroutine((IEnumerator) trader.StartTradeCycle());
      }
      if (flag && trader.enabled)
        break;
    }
    trader.isTradeValid = false;
    if ((UnityEngine.Object) trader.currentMarket == (UnityEngine.Object) trader.targetMarket)
      trader.OnArrival();
    else
      trader.StartFlight();
  }

  public void SetCargoStorages()
  {
    if (!(bool) (UnityEngine.Object) this.fleet)
      this.fleet = this.GetComponent<FleetManager>();
    this.cargoStorages = new List<ResourceInventory>();
    foreach (S_Ship ship in this.fleet.ships)
    {
      foreach (S_Module2 sModule2 in ship.GetModulesOfType(PartType.CARGO))
        this.cargoStorages.Add(sModule2.inv);
    }
    if (this.thisTrack.factionID != 1 || this.cargoStorages.Count != 0)
      return;
    this.Notify(" No cargo ships in fleet!", "orange");
  }

  private bool TradeCycle()
  {
    if (!this.isTrading)
      return false;
    this.targetMarket = (Market) null;
    this.bestSellPrice = 0;
    this.SetCargoStorages();
    bool flag = false;
    this.targetResource = (ResourceDefinition) null;
    this.targetQuantity = 0.0f;
    this.totalStorageSpace = 0.0f;
    foreach (ResourceInventory cargoStorage in this.cargoStorages)
    {
      if ((double) cargoStorage.GetTotalQuantity() > 0.0)
      {
        if ((UnityEngine.Object) this.targetResource == (UnityEngine.Object) null)
        {
          flag = true;
          this.targetResource = cargoStorage.GetResource();
          this.targetQuantity += cargoStorage.GetQuantityOf(this.targetResource);
        }
        else if ((UnityEngine.Object) cargoStorage.GetResource() == (UnityEngine.Object) this.targetResource)
          this.targetQuantity += cargoStorage.GetQuantityOf(this.targetResource);
      }
      this.totalStorageSpace += cargoStorage.storageMax;
    }
    if ((double) this.totalStorageSpace == 0.0)
      return false;
    if (flag && (UnityEngine.Object) this.targetResource != (UnityEngine.Object) null && (double) this.targetQuantity > 0.0)
    {
      this.isBuying = false;
      this.targetMarket = this.gm.GetBestBuyerOfExceptHostile(this.targetResource, this.thisTrack.GetFaction().factionID, ref this.bestSellPrice);
      if (this.bestSellPrice <= 0)
        this.bestSellPrice = 1;
      this.profitMargin = (float) (1.0 - (double) this.buyPrice / (double) this.bestSellPrice);
      if ((double) this.bestSellPrice < (double) this.buyPrice * (1.0 + (double) this.currentMinProfitMargin))
      {
        this.currentMinProfitMargin /= 2f;
        if ((double) this.currentMinProfitMargin < 1.0 / 1000.0)
          this.currentMinProfitMargin = -100000f;
        return false;
      }
      this.currentMinProfitMargin = this.minProfitMargin;
    }
    else
    {
      this.isBuying = true;
      this.targetResource = (ResourceDefinition) null;
      this.targetQuantity = this.totalStorageSpace;
      this.targetMarket = this.gm.GetBestSellerAnyExceptHostile(ref this.targetResource, this.currentMarket, this.thisTrack.GetFaction().factionID);
    }
    return !((UnityEngine.Object) this.targetMarket == (UnityEngine.Object) null) && !((UnityEngine.Object) this.targetResource == (UnityEngine.Object) null) && (double) this.targetQuantity != 0.0;
  }

  private void StartFlight()
  {
    if (!((UnityEngine.Object) this.targetMarket != (UnityEngine.Object) null))
      return;
    this.navigation.StartFlight();
  }

  private void OnArrival()
  {
    this.CheckCurrentMarket();
    this.hasArrived = false;
    if (!((UnityEngine.Object) this.currentMarket == (UnityEngine.Object) this.targetMarket))
      return;
    this.hasArrived = true;
  }

  private void NewHour()
  {
    if (!this.hasArrived || !this.isTrading || (double) this.targetQuantity == 0.0)
      return;
    int credits = this.thisTrack.GetFaction().credits;
    float realQuantity = this.isBuying ? this.targetQuantity : -this.targetQuantity;
    this.Trade(ref credits, ref realQuantity);
  }

  private void Trade(ref int credits, ref float realQuantity)
  {
    int totalCost = 0;
    this.targetMarket.ExecuteTrade(this.targetResource, ref realQuantity, ref credits, ref totalCost);
    if (this.thisTrack.factionID == 1)
    {
      if (this.isBuying && (double) realQuantity != 0.0)
        this.fm.ModPlayerCredits($"[{this.thisTrack.GetFullName()}] Bought {realQuantity.ToString("0.##")}t of {this.targetResource.resourceName}", -totalCost);
      else if (!this.isBuying && (double) realQuantity != 0.0)
        this.fm.ModPlayerCredits($"[{this.thisTrack.GetFullName()}] Sold {realQuantity.ToString("0.##")}t of {this.targetResource.resourceName}", totalCost);
    }
    else
      this.thisTrack.GetFaction().credits = credits;
    this.targetQuantity -= Mathf.Abs(realQuantity);
    if (!this.isBuying)
    {
      if ((double) realQuantity == 0.0)
      {
        this.StopTrade();
        return;
      }
      this.bestSellPrice = totalCost / Mathf.CeilToInt(Mathf.Abs(realQuantity));
      if ((double) this.bestSellPrice < (double) this.buyPrice * (1.0 + (double) this.currentMinProfitMargin))
      {
        this.currentMinProfitMargin /= 2f;
        if ((double) this.currentMinProfitMargin < 1.0 / 1000.0)
          this.currentMinProfitMargin = -100000f;
        this.StopTrade();
      }
    }
    if (this.isBuying && (double) realQuantity != 0.0)
      this.buyPrice = totalCost / (int) realQuantity;
    if ((double) realQuantity != 0.0)
      this.ModifyResourceInCargo(this.targetResource, realQuantity);
    bool flag1 = false;
    bool flag2 = true;
    if (this.isBuying)
    {
      if (credits <= 0)
        flag1 = true;
      if ((double) this.targetMarket.CanBuy(this.targetResource, this.targetQuantity, this.thisTrack.GetFaction().credits) <= 0.0)
        flag2 = false;
    }
    if (!((double) this.targetQuantity == 0.0 | flag1) && flag2)
      return;
    this.StopTrade();
  }

  private void StopTrade()
  {
    this.hasArrived = false;
    this.StartCoroutine((IEnumerator) this.StartTradeCycle(0.1f));
  }

  private string GetNotifMessage(float realQuantity, int totalCost)
  {
    if (this.isBuying && (double) realQuantity != 0.0)
      return $"[{this.thisTrack.GetFullName()}] Bought {realQuantity.ToString("0.##")}t of {this.targetResource.resourceName}";
    if (this.isBuying || (double) realQuantity == 0.0)
      return "";
    return $"[{this.thisTrack.GetFullName()}] Sold {realQuantity.ToString("0.##")}t of {this.targetResource.resourceName}";
  }

  private void NewFader(float realQuantity, int totalCost)
  {
    if (this.isBuying && (double) realQuantity != 0.0)
    {
      this.Notify($"Bought {realQuantity.ToString("0.##")}t of {this.targetResource.resourceName} for -{totalCost}cr", "orange");
    }
    else
    {
      if (this.isBuying || (double) realQuantity == 0.0)
        return;
      this.Notify($"Sold {realQuantity.ToString("0.##")}t of {this.targetResource.resourceName} for +{totalCost}cr", "green");
    }
  }

  private void Notify(string message, string color)
  {
    this.notifs.NewNotif($"[{this.thisTrack.GetFullName()}] {message}", color);
  }

  private void ModifyResourceInCargo(ResourceDefinition resource, float quantity)
  {
    bool flag = (double) quantity >= 0.0;
    foreach (S_Ship ship in this.fleet.ships)
    {
      foreach (S_Module2 sModule2 in ship.GetModulesOfType(PartType.CARGO))
      {
        if (flag)
        {
          float num = sModule2.AddCargo(resource, quantity);
          quantity -= num;
          if ((double) quantity <= 0.0)
            break;
        }
        else
        {
          float num = sModule2.RemoveCargo(resource, Mathf.Abs(quantity));
          quantity += num;
          if ((double) quantity >= 0.0)
            break;
        }
      }
    }
  }

  public string GetCurrentMarketID()
  {
    return (bool) (UnityEngine.Object) this.currentMarket ? this.currentMarket.GetComponent<Track>().id : "";
  }

  public string GetTargetMarketID()
  {
    return (bool) (UnityEngine.Object) this.targetMarket ? this.targetMarket.GetComponent<Track>().id : "";
  }

  public void SetHasArrived(bool newHasArrived) => this.hasArrived = newHasArrived;

  private void OnDestroy()
  {
    if ((bool) (UnityEngine.Object) this.navigation)
      this.navigation.onArrival -= new Action(this.OnArrival);
    if (!(bool) (UnityEngine.Object) this.tm)
      return;
    this.tm.NewHour -= new Action(this.NewHour);
  }
}
