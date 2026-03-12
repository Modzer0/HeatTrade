// Decompiled with JetBrains decompiler
// Type: Factory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Factory : MonoBehaviour
{
  private TimeManager tm;
  private ResourceInventory inventory;
  private Market market;
  [SerializeField]
  private AllRecipes allRecipes;
  [Tooltip("How many concurrent production lines/units are dedicated to each recipe.")]
  public List<RecipeCapacity> recipes = new List<RecipeCapacity>();

  private void Start()
  {
    this.tm = TimeManager.current;
    this.market = this.GetComponent<Market>();
    this.tm.NewDay += new Action(this.ProductionCycle);
    this.Init();
    bool flag = false;
    foreach (RecipeCapacity recipe in this.recipes)
    {
      if (recipe.Recipe.RecipeName == "Basic Refuel")
      {
        flag = true;
        break;
      }
    }
    if (flag)
      return;
    this.recipes.Add(new RecipeCapacity()
    {
      Recipe = this.allRecipes.GetRecipe("Basic Refuel")
    });
  }

  private void Init()
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
    this.StartCoroutine((IEnumerator) this.AddResourcesDelayed());
  }

  private IEnumerator AddResourcesDelayed()
  {
    yield return (object) new WaitForSeconds(0.25f);
    int num1 = 100;
    for (int index = 0; index < num1 && (double) this.inventory.GetTotalQuantity() < 0.75 * (double) this.inventory.storageMax; ++index)
    {
      foreach (RecipeCapacity recipe in this.recipes)
      {
        foreach (ResourceQuantity input in recipe.Recipe.Inputs)
        {
          this.market.ModifyDemand(input.resource, 0.1f);
          double num2 = (double) this.inventory.RemoveResource(input.resource, input.quantity);
        }
        foreach (ResourceQuantity output in recipe.Recipe.Outputs)
        {
          this.market.ModifyDemand(output.resource, -0.1f);
          double num3 = (double) this.inventory.AddResource(output.resource, output.quantity);
        }
      }
    }
  }

  private void ProductionCycle()
  {
    foreach (RecipeCapacity recipe1 in this.recipes)
    {
      ProductionRecipe recipe2 = recipe1.Recipe;
      if (!((UnityEngine.Object) recipe2 == (UnityEngine.Object) null) && this.CanProduceBatch(recipe2))
        this.ExecuteBatchProduction(recipe2);
    }
  }

  private bool CanProduceBatch(ProductionRecipe recipe)
  {
    if (recipe.alwaysConsume)
    {
      foreach (ResourceQuantity input in recipe.Inputs)
      {
        int factionCredits = 1000000;
        int totalCost = 0;
        float quantity = input.quantity;
        this.market.ExecuteTrade(input.resource, ref quantity, ref factionCredits, ref totalCost);
      }
      return true;
    }
    foreach (ResourceQuantity input in recipe.Inputs)
    {
      if ((double) this.inventory.GetQuantityOf(input.resource) < (double) input.quantity)
        return false;
    }
    foreach (ResourceQuantity output in recipe.Outputs)
    {
      if ((double) this.inventory.GetQuantityOf(output.resource) + (double) output.quantity > (double) this.inventory.storageMax)
        return false;
    }
    return true;
  }

  private void ExecuteBatchProduction(ProductionRecipe recipe)
  {
    foreach (ResourceQuantity input in recipe.Inputs)
    {
      double num = (double) this.inventory.RemoveResource(input.resource, input.quantity);
    }
    foreach (ResourceQuantity output in recipe.Outputs)
    {
      double num = (double) this.inventory.AddResource(output.resource, output.quantity);
    }
  }

  public List<ResourceQuantity> GetDemands()
  {
    List<ResourceQuantity> demands = new List<ResourceQuantity>();
    foreach (RecipeCapacity recipe in this.recipes)
    {
      foreach (ResourceQuantity input in recipe.Recipe.Inputs)
        demands.Add(new ResourceQuantity(input.resource, (float) Mathf.CeilToInt(input.quantity)));
    }
    return demands;
  }

  public List<ResourceQuantity> GetSupplys()
  {
    List<ResourceQuantity> supplys = new List<ResourceQuantity>();
    foreach (RecipeCapacity recipe in this.recipes)
    {
      foreach (ResourceQuantity output in recipe.Recipe.Outputs)
        supplys.Add(new ResourceQuantity(output.resource, (float) Mathf.CeilToInt(output.quantity)));
    }
    return supplys;
  }

  public FactoryData GetFactoryData()
  {
    FactoryData factoryData = new FactoryData();
    foreach (RecipeCapacity recipe in this.recipes)
      factoryData.recipeNames.Add(recipe.Recipe.RecipeName);
    return factoryData;
  }

  public void SetFromData(FactoryData newFactory)
  {
    this.recipes.Clear();
    foreach (string recipeName in newFactory.recipeNames)
      this.recipes.Add(new RecipeCapacity()
      {
        Recipe = this.allRecipes.GetRecipe(recipeName)
      });
  }

  private void OnDestroy()
  {
    if (!(bool) (UnityEngine.Object) this.tm)
      return;
    this.tm.NewDay -= new Action(this.ProductionCycle);
  }
}
