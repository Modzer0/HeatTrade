// Decompiled with JetBrains decompiler
// Type: AllRecipes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "AllRecipesList", menuName = "ScriptableObjects/AllRecipes")]
public class AllRecipes : ScriptableObject
{
  public List<ProductionRecipe> recipes;

  public ProductionRecipe GetRecipe(string name)
  {
    foreach (ProductionRecipe recipe in this.recipes)
    {
      if (recipe.RecipeName == name)
        return recipe;
    }
    return (ProductionRecipe) null;
  }
}
