// Decompiled with JetBrains decompiler
// Type: ColorManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ColorManager : MonoBehaviour
{
  public static ColorManager current;
  [Header("COLORS")]
  public Color red;
  public Color darkRed;
  public Color green;
  public Color darkGreen;
  public Color blue;
  public Color yellow;
  public Color white;
  public Color gray;
  public Color darkGray;
  public Color black;
  [Header("HEX")]
  public string redHex;
  public string darkRedHex;
  public string greenHex;
  public string darkGreenHex;
  public string blueHex;
  public string yellowHex;
  public string whiteHex;
  public string grayHex;
  public string darkGrayHex;
  public string blackHex;
  [Header("LASERS")]
  public Color nirColor;
  public Color irColor;
  public Color uvColor;
  public Color xrayColor;
  public Gradient visibleGradient;

  private void Awake()
  {
    if ((Object) ColorManager.current != (Object) null && (Object) ColorManager.current != (Object) this)
      Object.Destroy((Object) this.gameObject);
    else
      ColorManager.current = this;
  }

  private void Start() => this.SetHex();

  private void SetHex()
  {
    this.redHex = ColorUtility.ToHtmlStringRGB(this.red);
    this.darkRedHex = ColorUtility.ToHtmlStringRGB(this.darkRed);
    this.greenHex = ColorUtility.ToHtmlStringRGB(this.green);
    this.darkGreenHex = ColorUtility.ToHtmlStringRGB(this.darkGreen);
    this.blueHex = ColorUtility.ToHtmlStringRGB(this.blue);
    this.yellowHex = ColorUtility.ToHtmlStringRGB(this.yellow);
    this.whiteHex = ColorUtility.ToHtmlStringRGB(this.white);
    this.grayHex = ColorUtility.ToHtmlStringRGB(this.gray);
    this.darkGrayHex = ColorUtility.ToHtmlStringRGB(this.darkGray);
    this.blackHex = ColorUtility.ToHtmlStringRGB(this.black);
  }
}
