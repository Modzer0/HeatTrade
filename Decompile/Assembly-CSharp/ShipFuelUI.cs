// Decompiled with JetBrains decompiler
// Type: ShipFuelUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ShipFuelUI : MonoBehaviour
{
  private S_Ship ship;
  [SerializeField]
  private TMP_Text nameText;
  [SerializeField]
  private Image icon;
  [SerializeField]
  private uiBar dvBar;
  [SerializeField]
  private Image dvBarBG;
  [SerializeField]
  private TMP_Text dvText;
  [SerializeField]
  private Color dtBar;
  [SerializeField]
  private Color dtBg;
  [SerializeField]
  private Color dtText;
  [SerializeField]
  private Color dhBar;
  [SerializeField]
  private Color dhBg;
  [SerializeField]
  private Color dhText;
  [SerializeField]
  private Color aBar;
  [SerializeField]
  private Color aBg;
  [SerializeField]
  private Color aText;

  private void Update()
  {
    if (!(bool) (Object) this.ship)
      return;
    float fuelMass = this.ship.GetFuelMass();
    float fuelMax = this.ship.GetFuelMax();
    this.dvBar.SetBarSize(fuelMass / fuelMax);
    TMP_Text dvText = this.dvText;
    int num = Mathf.RoundToInt(fuelMass);
    string str1 = num.ToString("#,0");
    num = Mathf.RoundToInt(fuelMax);
    string str2 = num.ToString("#,0");
    string str3 = $"{str1}/{str2} t";
    dvText.text = str3;
  }

  public void SetOn(S_Ship newShip)
  {
    this.ship = newShip;
    this.nameText.text = $"{this.ship.trackID} - {this.ship.publicName}";
    ResourceDefinition fuelResource = this.ship.GetFuelResource();
    this.icon.sprite = fuelResource.icon;
    this.dvBarBG = this.dvBar.transform.parent.GetComponent<Image>();
    this.SetColors(fuelResource);
    float dv = 0.0f;
    float dvMax = 0.0f;
    this.ship.GetDvAndMax(out dv, out dvMax);
    this.dvBar.SetBarSize(dv / dvMax);
    TMP_Text dvText = this.dvText;
    int num = Mathf.RoundToInt(dv);
    string str1 = num.ToString("#,0");
    num = Mathf.RoundToInt(dvMax);
    string str2 = num.ToString("#,0");
    string str3 = $"{str1}/{str2} t";
    dvText.text = str3;
  }

  private void SetColors(ResourceDefinition fuel)
  {
    Color color1 = this.dtBar;
    Color color2 = this.dtBg;
    Color color3 = this.dtText;
    if (fuel.type == ResourceType.DH_FUEL)
    {
      color1 = this.dhBar;
      color2 = this.dhBg;
      color3 = this.dhText;
    }
    else if (fuel.type == ResourceType.ANTIMATTER)
    {
      color1 = this.aBar;
      color2 = this.aBg;
      color3 = this.aText;
    }
    this.dvBar.GetComponent<Image>().color = color1;
    this.dvBarBG.color = color2;
    this.dvText.color = color3;
  }
}
