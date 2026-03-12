// Decompiled with JetBrains decompiler
// Type: ShipManagementPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class ShipManagementPanel : MonoBehaviour
{
  [SerializeField]
  private GameObject[] panels;
  [SerializeField]
  private string[] panelNames;
  [SerializeField]
  private TMP_Text nameText;
  private int currentIndex;

  public void PanelOn(int i)
  {
    foreach (GameObject panel in this.panels)
      panel.SetActive(false);
    if (this.currentIndex == i)
    {
      this.nameText.text = "SHIP MANAGEMENT";
      this.currentIndex = -1;
    }
    else
    {
      this.nameText.text = this.panelNames[i];
      this.panels[i].SetActive(!this.panels[i].activeSelf);
      this.currentIndex = i;
    }
  }
}
