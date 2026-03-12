// Decompiled with JetBrains decompiler
// Type: RecipeCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using TMPro;
using UnityEngine;

#nullable disable
public class RecipeCard : MonoBehaviour
{
  [SerializeField]
  private ProductionRecipe recipe;
  [Header("UI")]
  [SerializeField]
  private TMP_Text recipeName;
  [SerializeField]
  private Transform inputList;
  [SerializeField]
  private Transform outputList;
  [SerializeField]
  private IngredientCard ingredientCardPF;
  private bool isSizeSet;

  private void Update()
  {
    if (this.isSizeSet)
      return;
    this.AdjustSize();
  }

  public void Setup(ProductionRecipe newRecipe)
  {
    this.recipe = newRecipe;
    this.recipeName.text = this.recipe.RecipeName;
    IEnumerator enumerator1 = (IEnumerator) this.inputList.GetEnumerator();
    try
    {
      while (enumerator1.MoveNext())
        UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator1.Current).gameObject);
    }
    finally
    {
      if (enumerator1 is IDisposable disposable)
        disposable.Dispose();
    }
    IEnumerator enumerator2 = (IEnumerator) this.outputList.GetEnumerator();
    try
    {
      while (enumerator2.MoveNext())
        UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator2.Current).gameObject);
    }
    finally
    {
      if (enumerator2 is IDisposable disposable)
        disposable.Dispose();
    }
    foreach (ResourceQuantity input in newRecipe.Inputs)
      UnityEngine.Object.Instantiate<IngredientCard>(this.ingredientCardPF, this.inputList).Setup(input, false);
    foreach (ResourceQuantity output in newRecipe.Outputs)
      UnityEngine.Object.Instantiate<IngredientCard>(this.ingredientCardPF, this.outputList).Setup(output, true);
  }

  private IEnumerator DelaySizeAdjust()
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.AdjustSize();
  }

  private void AdjustSize()
  {
    float y = this.inputList.GetComponent<RectTransform>().sizeDelta.y;
    if ((double) this.outputList.GetComponent<RectTransform>().sizeDelta.y > (double) y)
      y = this.outputList.GetComponent<RectTransform>().sizeDelta.y;
    this.GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x, y);
    this.isSizeSet = true;
  }
}
