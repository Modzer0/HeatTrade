// Decompiled with JetBrains decompiler
// Type: ResourceMarketData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ResourceMarketData : MonoBehaviour
{
  private GlobalMarket gm;
  public ResourceDefinition resource;
  [SerializeField]
  private Image icon;
  [SerializeField]
  private TMP_Text resourceText;
  [SerializeField]
  private TMP_Text buyPriceText;
  [SerializeField]
  private TMP_Text sellPriceText;
  [SerializeField]
  private TMP_Text averageBuyPriceText;
  [SerializeField]
  private TMP_Text averageSellPriceText;
  [SerializeField]
  private TMP_Text quantityText;
  [SerializeField]
  private uiBar quantityBar;
  [SerializeField]
  private Gradient colorGradient;

  public void Setup(
    ResourceDefinition newResource,
    Sprite newIcon,
    string resourceName,
    int buyPrice,
    int sellPrice,
    int quantity,
    int maxQuantity)
  {
    if (!(bool) (Object) this.gm)
      this.gm = GlobalMarket.current;
    this.resource = newResource;
    this.icon.sprite = newIcon;
    this.resourceText.text = resourceName;
    this.UpdateData(buyPrice, sellPrice, quantity, maxQuantity);
  }

  public void UpdateData(int buyPrice, int sellPrice, int quantity, int maxQuantity)
  {
    int averageBuyPriceOf = this.gm.GetAverageBuyPriceOf(this.resource);
    int averageSellPriceOf = this.gm.GetAverageSellPriceOf(this.resource);
    this.averageBuyPriceText.text = $"({averageBuyPriceOf.ToString("N0")})";
    this.averageSellPriceText.text = $"({averageSellPriceOf.ToString("N0")})";
    Color color1 = this.colorGradient.Evaluate(0.5f);
    if (buyPrice > averageBuyPriceOf)
      color1 = this.colorGradient.Evaluate((float) (0.5 - 0.5 * ((double) (buyPrice - averageBuyPriceOf) / ((double) averageBuyPriceOf * 0.25))));
    else if (buyPrice < averageBuyPriceOf)
      color1 = this.colorGradient.Evaluate((float) (0.5 + 0.5 * ((double) (averageBuyPriceOf - buyPrice) / ((double) averageBuyPriceOf * 0.25))));
    this.buyPriceText.text = buyPrice.ToString("N0");
    this.buyPriceText.color = color1;
    Color color2 = this.colorGradient.Evaluate(0.5f);
    if (sellPrice > averageSellPriceOf)
      color2 = this.colorGradient.Evaluate((float) (0.5 + 0.5 * ((double) (sellPrice - averageSellPriceOf) / ((double) averageSellPriceOf * 0.25))));
    else if (sellPrice < averageSellPriceOf)
      color2 = this.colorGradient.Evaluate((float) (0.5 - 0.5 * ((double) (averageSellPriceOf - sellPrice) / ((double) averageSellPriceOf * 0.25))));
    this.sellPriceText.text = sellPrice.ToString("N0");
    this.sellPriceText.color = color2;
    this.quantityText.text = "x" + quantity.ToString("N0");
    this.quantityBar.SetBarSize((float) quantity / (float) maxQuantity);
  }
}
