// Decompiled with JetBrains decompiler
// Type: Tooltip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class Tooltip : MonoBehaviour
{
  [SerializeField]
  private TextMeshProUGUI headerField;
  [SerializeField]
  private TextMeshProUGUI contentField;
  [SerializeField]
  private GameObject contentPanel;
  [SerializeField]
  private GameObject fuelBuyPanel;
  private TooltipType type;

  public RectTransform rectTransform => (RectTransform) this.transform;

  private void Update()
  {
    Vector2 vector2 = (Vector2) Input.mousePosition;
    bool flag = false;
    if ((double) vector2.x + (double) this.rectTransform.rect.width > (double) (Screen.width - 32 /*0x20*/))
      flag = true;
    vector2 = !flag ? new Vector2(vector2.x + 32f, vector2.y) : new Vector2(vector2.x - 220f, vector2.y);
    this.transform.position = (Vector3) vector2;
  }

  public void SetText(string newHeader, string newContent)
  {
    if (string.IsNullOrEmpty(newHeader))
    {
      this.headerField.gameObject.SetActive(false);
    }
    else
    {
      this.headerField.gameObject.SetActive(true);
      this.headerField.text = newHeader;
    }
    if (string.IsNullOrEmpty(newContent))
    {
      this.contentField.gameObject.SetActive(false);
    }
    else
    {
      this.contentField.gameObject.SetActive(true);
      this.contentField.text = newContent;
    }
  }

  public void SetTextFuelBuy()
  {
  }

  private void AllPanelsOff()
  {
    this.contentPanel.SetActive(false);
    this.fuelBuyPanel.SetActive(false);
  }
}
