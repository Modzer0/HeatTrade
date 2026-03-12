// Decompiled with JetBrains decompiler
// Type: EncounterPanelManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class EncounterPanelManager : MonoBehaviour
{
  public static EncounterPanelManager current;
  private AllStructures allStructures;
  private SceneTransitionManager stm;
  private TimeManager tm;
  private CameraManager cm;
  [SerializeField]
  private AudioManager am;
  [SerializeField]
  private uiAnimator encounterPanel;
  [SerializeField]
  private S_ShipDataUI shipDataPF;
  [SerializeField]
  private Transform ourShipsList;
  [SerializeField]
  private Transform hostileShipsList;
  [SerializeField]
  private TMP_Text logText;
  [SerializeField]
  private TMP_Text dateText;
  [SerializeField]
  private TMP_Text descriptionText;

  private void Awake()
  {
    if ((UnityEngine.Object) EncounterPanelManager.current != (UnityEngine.Object) null && (UnityEngine.Object) EncounterPanelManager.current != (UnityEngine.Object) this)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    else
      EncounterPanelManager.current = this;
  }

  private void Start()
  {
    this.tm = TimeManager.current;
    this.cm = CameraManager.current;
    this.stm = SceneTransitionManager.current;
    this.allStructures = AllStructures.current;
  }

  private void GetEncounterPanel()
  {
    GameObject gameObject = GameObject.Find("PANEL - Encounter");
    if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null) || !(bool) (UnityEngine.Object) gameObject.GetComponent<uiAnimator>())
      return;
    this.encounterPanel = GameObject.Find("PANEL - Encounter").GetComponent<uiAnimator>();
  }

  public void PressSurrender()
  {
  }

  public void PressAutoresolve()
  {
  }

  public void PressFight() => this.stm.NewEngagement();

  public void PromptOn(List<TacticalGroupData> groups, Vector3 encounterPos)
  {
    if ((bool) (UnityEngine.Object) this.am)
      this.am.PlaySFX(8);
    this.tm.isGettingInput = false;
    if ((UnityEngine.Object) this.encounterPanel == (UnityEngine.Object) null)
      this.GetEncounterPanel();
    if ((UnityEngine.Object) this.encounterPanel != (UnityEngine.Object) null)
    {
      this.encounterPanel.gameObject.SetActive(true);
      this.encounterPanel.Show();
    }
    this.tm.timeScale = 0.0f;
    this.tm.isGettingInput = false;
    this.cm.CenterCamAt(encounterPos);
    IEnumerator enumerator1 = (IEnumerator) this.ourShipsList.GetEnumerator();
    try
    {
      while (enumerator1.MoveNext())
        UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator1.Current).gameObject);
    }
    finally
    {
      if (enumerator1 is IDisposable disposable)
        disposable.Dispose();
    }
    IEnumerator enumerator2 = (IEnumerator) this.hostileShipsList.GetEnumerator();
    try
    {
      while (enumerator2.MoveNext())
        UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator2.Current).gameObject);
    }
    finally
    {
      if (enumerator2 is IDisposable disposable)
        disposable.Dispose();
    }
    TacticalGroupData tacticalGroupData1 = (TacticalGroupData) null;
    TacticalGroupData tacticalGroupData2 = (TacticalGroupData) null;
    foreach (TacticalGroupData group in groups)
    {
      if (group.factionID == 1)
        tacticalGroupData1 = group;
      else
        tacticalGroupData2 = group;
    }
    foreach (TacticalShipData tacticalShipData in tacticalGroupData1.objects)
    {
      MonoBehaviour.print((object) ("displaying player ship: " + tacticalShipData.publicName));
      S_ShipDataUI sShipDataUi = UnityEngine.Object.Instantiate<S_ShipDataUI>(this.shipDataPF);
      sShipDataUi.SetStructure(this.allStructures.GetShipFromID(tacticalShipData.trackID));
      sShipDataUi.transform.parent = this.ourShipsList;
      sShipDataUi.transform.localRotation = Quaternion.identity;
      sShipDataUi.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    foreach (TacticalShipData tacticalShipData in tacticalGroupData2.objects)
    {
      MonoBehaviour.print((object) ("displaying hostile ship: " + tacticalShipData.publicName));
      S_ShipDataUI sShipDataUi = UnityEngine.Object.Instantiate<S_ShipDataUI>(this.shipDataPF);
      S_Ship shipFromId = this.allStructures.GetShipFromID(tacticalShipData.trackID);
      if ((UnityEngine.Object) shipFromId == (UnityEngine.Object) null)
        MonoBehaviour.print((object) ("ship not found!: " + tacticalShipData.publicName));
      sShipDataUi.SetStructure(shipFromId);
      sShipDataUi.transform.parent = this.hostileShipsList;
      sShipDataUi.transform.localRotation = Quaternion.identity;
      sShipDataUi.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    this.logText.text = $"TRANSMISSION LOG #{UnityEngine.Random.Range(1, 1000)}";
    this.dateText.text = this.tm.GetDate();
    this.descriptionText.text = $"Fleet {tacticalGroupData1.trackID} has encountered hostile forces designated {tacticalGroupData2.trackID} at coordinates {encounterPos}. Awaiting orders from Command.";
  }

  public void PromptOff() => this.encounterPanel.Hide();
}
