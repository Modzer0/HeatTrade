// Decompiled with JetBrains decompiler
// Type: ProductionRecipe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "NewProductionRecipe", menuName = "ScriptableObjects/Production Recipe")]
public class ProductionRecipe : ScriptableObject
{
  public string RecipeName;
  [Header("Input Requirements")]
  [Tooltip("List of resources and quantities required to produce ONE batch.")]
  public List<ResourceQuantity> Inputs = new List<ResourceQuantity>();
  [Header("Output")]
  [Tooltip("List of resources and quantities produced in ONE batch.")]
  public List<ResourceQuantity> Outputs = new List<ResourceQuantity>();
  [Tooltip("The amount of time (in economic cycles) required to complete one batch.")]
  public int CyclesPerBatch = 1;
  public bool alwaysConsume;
}
