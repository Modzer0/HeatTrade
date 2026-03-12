// Decompiled with JetBrains decompiler
// Type: FishCatchUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FishCatchUI : MonoBehaviour
{
  [Header("UI")]
  [SerializeField]
  private Image icon;
  [SerializeField]
  private TMP_Text nameText;
  [SerializeField]
  private TMP_Text chanceText;
  [Header("COLORS")]
  [SerializeField]
  private Gradient gradient;

  public void Setup(Sprite newIcon, string fishName, int chance)
  {
    if ((Object) newIcon == (Object) null)
      this.icon.color = Color.black;
    else
      this.icon.sprite = newIcon;
    this.nameText.text = fishName;
    this.chanceText.text = chance != 0 ? chance.ToString() + "%" : "?";
    this.chanceText.color = this.gradient.Evaluate((float) chance / 100f);
  }
}
