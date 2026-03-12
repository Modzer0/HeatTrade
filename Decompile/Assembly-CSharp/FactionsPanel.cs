// Decompiled with JetBrains decompiler
// Type: FactionsPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FactionsPanel : MonoBehaviour
{
  public static FactionsPanel current;
  private FactionsManager fm;
  private Faction currentFaction;
  [Header("UI")]
  [SerializeField]
  private GameObject factionsPanel;
  [Header("LIST")]
  [SerializeField]
  private Transform factionsList;
  [SerializeField]
  private GameObject factionButtonPrefab;
  [Header("FACTION INFO")]
  [SerializeField]
  private Image factionIcon;
  [SerializeField]
  private TMP_Text factionText;
  [SerializeField]
  private TMP_Text wordsText;
  [SerializeField]
  private TMP_Text descriptionText;
  [SerializeField]
  private Image primaryColorImg;
  [SerializeField]
  private Image secondaryColorImg;

  private void Awake() => FactionsPanel.current = this;

  private void Start()
  {
  }

  private void UpdateUI()
  {
    this.fm = FactionsManager.current;
    foreach (Faction faction in this.fm.factions)
      UnityEngine.Object.Instantiate<GameObject>(this.factionButtonPrefab, this.factionsList, false).GetComponent<FactionButton>().Setup(faction);
  }

  public void Toggle(bool isOn)
  {
    this.factionsPanel.SetActive(isOn);
    if (!isOn)
      return;
    this.ClearFactionsList();
    this.UpdateUI();
    this.SelectFaction(1);
  }

  private void ClearFactionsList()
  {
    IEnumerator enumerator = (IEnumerator) this.factionsList.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
        UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator.Current).gameObject);
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
  }

  public void SelectFaction(int id)
  {
    this.currentFaction = this.fm.GetFactionFromID(id);
    this.factionIcon.sprite = this.currentFaction.factionIcon;
    this.factionText.text = this.currentFaction.factionName;
    this.factionText.color = this.currentFaction.colorSecondary;
    this.primaryColorImg.color = this.currentFaction.colorPrimary;
    this.secondaryColorImg.color = this.currentFaction.colorSecondary;
    this.wordsText.text = this.currentFaction.factionWords;
    this.descriptionText.text = this.currentFaction.factionDescription;
  }
}
