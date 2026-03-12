// Decompiled with JetBrains decompiler
// Type: ColorToggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ColorToggle : MonoBehaviour
{
  [SerializeField]
  private MainMenu mainMenu;
  [SerializeField]
  private Image img;
  [SerializeField]
  private bool isPrimary;
  private bool isOn;

  private void Start()
  {
  }

  public void Toggle(bool newIsOn)
  {
    this.isOn = newIsOn;
    if (!this.isOn)
      return;
    if (this.isPrimary)
      this.mainMenu.SetPrimaryColor(this.img.color);
    else
      this.mainMenu.SetSecondaryColor(this.img.color);
  }
}
