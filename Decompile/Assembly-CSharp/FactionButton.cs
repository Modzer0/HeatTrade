// Decompiled with JetBrains decompiler
// Type: FactionButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FactionButton : MonoBehaviour
{
  private FactionsPanel fp;
  private int factionID;
  [SerializeField]
  private Image icon;
  [SerializeField]
  private TMP_Text nameText;
  [SerializeField]
  private Image bg;
  [SerializeField]
  private Image primaryImg;
  [SerializeField]
  private Image secondaryImg;
  private bool isInit;

  private void Start() => this.Init();

  private void Init()
  {
    if (this.isInit)
      return;
    this.fp = FactionsPanel.current;
    this.isInit = true;
  }

  public void Setup(Faction newFaction)
  {
    this.icon.sprite = newFaction.factionIcon;
    this.nameText.text = newFaction.factionName;
    this.nameText.color = newFaction.colorSecondary;
    this.primaryImg.color = newFaction.colorPrimary;
    this.secondaryImg.color = newFaction.colorSecondary;
    this.factionID = newFaction.factionID;
  }

  public void OnClick() => this.fp.SelectFaction(this.factionID);
}
