// Decompiled with JetBrains decompiler
// Type: ResourceCargoData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ResourceCargoData : MonoBehaviour
{
  private GlobalMarket gm;
  public ResourceDefinition resource;
  [SerializeField]
  private Image icon;
  [SerializeField]
  private TMP_Text resourceText;
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
    int quantity,
    int maxQuantity)
  {
    if (!(bool) (Object) this.gm)
      this.gm = GlobalMarket.current;
    this.resource = newResource;
    this.icon.sprite = newIcon;
    this.resourceText.text = resourceName;
    this.UpdateData(quantity, maxQuantity);
  }

  public void UpdateData(int quantity, int maxQuantity)
  {
    this.gm.GetAverageBuyPriceOf(this.resource);
    this.quantityText.text = "x" + quantity.ToString("N0");
    this.quantityBar.SetBarSize((float) quantity / (float) maxQuantity);
  }
}
