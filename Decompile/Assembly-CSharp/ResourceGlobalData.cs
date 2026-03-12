// Decompiled with JetBrains decompiler
// Type: ResourceGlobalData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ResourceGlobalData : MonoBehaviour
{
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
  private TMP_Text tradePriceText;
  [SerializeField]
  private TMP_Text supplyText;
  [SerializeField]
  private TMP_Text demandText;
  [SerializeField]
  private TMP_Text surplusText;
  [SerializeField]
  private Gradient colorGradient;

  private void Start()
  {
  }

  public void SetResource(
    ResourceDefinition newResource,
    int buyPrice,
    int sellPrice,
    float supply,
    float demand)
  {
    this.resource = newResource;
    this.icon.sprite = this.resource.icon;
    this.resourceText.text = this.resource.resourceName;
    this.UpdateData(buyPrice, sellPrice, supply, demand);
  }

  public void UpdateData(int buyPrice, int sellPrice, float supply, float demand)
  {
    this.buyPriceText.text = buyPrice.ToString("N0");
    this.sellPriceText.text = sellPrice.ToString("N0");
    int num1 = sellPrice - buyPrice;
    this.tradePriceText.text = num1.ToString("N0");
    this.supplyText.text = supply.ToString("N0");
    this.demandText.text = demand.ToString("N0");
    this.surplusText.text = (supply - demand).ToString("N0");
    int num2 = num1 < 0 ? 1 : 0;
    bool flag = num1 == 0;
    this.colorGradient.Evaluate(0.5f);
    this.buyPriceText.color = num2 == 0 ? (!flag ? this.colorGradient.Evaluate(1f) : this.colorGradient.Evaluate(0.5f)) : this.colorGradient.Evaluate(0.0f);
    this.colorGradient.Evaluate(0.5f);
    this.sellPriceText.color = num2 == 0 ? (!flag ? this.colorGradient.Evaluate(1f) : this.colorGradient.Evaluate(0.5f)) : this.colorGradient.Evaluate(0.0f);
    this.colorGradient.Evaluate(0.5f);
    this.tradePriceText.color = num1 <= 0 ? (num1 >= 0 ? this.colorGradient.Evaluate(0.5f) : this.colorGradient.Evaluate(0.0f)) : this.colorGradient.Evaluate(1f);
    if ((double) supply > (double) demand)
      this.surplusText.color = this.colorGradient.Evaluate(1f);
    else if ((double) supply < (double) demand)
      this.surplusText.color = this.colorGradient.Evaluate(0.0f);
    else
      this.surplusText.color = this.colorGradient.Evaluate(0.5f);
  }
}
