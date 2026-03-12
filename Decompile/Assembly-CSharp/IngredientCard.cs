// Decompiled with JetBrains decompiler
// Type: IngredientCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class IngredientCard : MonoBehaviour
{
  [SerializeField]
  private Image image;
  [SerializeField]
  private TMP_Text quantityText;
  [SerializeField]
  private Color inputColor;
  [SerializeField]
  private Color outputColor;

  public void Setup(ResourceQuantity newRQ, bool isOutput)
  {
    this.image.sprite = newRQ.resource.icon;
    this.quantityText.text = newRQ.quantity.ToString("N0") + "t";
    this.quantityText.color = this.inputColor;
    if (!isOutput)
      return;
    this.quantityText.color = this.outputColor;
  }
}
