// Decompiled with JetBrains decompiler
// Type: ProductionPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class ProductionPanel : MonoBehaviour
{
  private Factory factory;
  [SerializeField]
  private GameObject productionPanel;
  [SerializeField]
  private Transform recipeList;
  [SerializeField]
  private RecipeCard recipeCardPF;

  public void Setup(Factory newFactory)
  {
    this.factory = newFactory;
    IEnumerator enumerator = (IEnumerator) this.recipeList.GetEnumerator();
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
    foreach (RecipeCapacity recipe in this.factory.recipes)
      UnityEngine.Object.Instantiate<RecipeCard>(this.recipeCardPF, this.recipeList).Setup(recipe.Recipe);
  }
}
