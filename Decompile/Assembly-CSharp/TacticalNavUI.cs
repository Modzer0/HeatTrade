// Decompiled with JetBrains decompiler
// Type: TacticalNavUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class TacticalNavUI : MonoBehaviour
{
  private TacticalInputs ti;
  private bool shipSelected;
  private Pathfinder pf;
  private Rigidbody ssrb;
  [SerializeField]
  private GameObject navPanel;
  [SerializeField]
  private TMP_Text velocityText;
  [SerializeField]
  private TMP_Text massText;
  [SerializeField]
  private TMP_Text statusText;
  [SerializeField]
  private TMP_Text moveDistText;
  [SerializeField]
  private TMP_Text moveTimeText;
  [SerializeField]
  private Transform deltaVBar;
  [SerializeField]
  private TMP_Text deltaVText;
  [SerializeField]
  private Toggle showMoveLineToggle;
  [SerializeField]
  private Toggle rcsOnlyToggle;

  private void Start()
  {
    this.ti = TacticalInputs.current;
    this.ti.newSelection += new Action(this.NewShipSelected);
  }

  private void Update()
  {
    if (!this.navPanel.activeSelf || !this.shipSelected || !((UnityEngine.Object) this.ti.selectedShip != (UnityEngine.Object) null))
      return;
    float magnitude = this.ssrb.velocity.magnitude;
    string str1 = "";
    if ((double) magnitude != 0.0)
    {
      float num = this.pf.acceleration * 10f;
      str1 = ((double) num > 0.0 ? 1 : 0) != 0 ? $" (+{num.ToString("0.##")})" : $" ({num.ToString("0.##")})";
    }
    TMP_Text velocityText = this.velocityText;
    float num1 = Mathf.Round(magnitude * 10f);
    string str2 = $"{num1.ToString()}m/s{str1}";
    velocityText.text = str2;
    PathfinderState state = this.pf.state;
    this.statusText.text = state.ToString() ?? "";
    if ((state == PathfinderState.MOVE || state == PathfinderState.COLLIDE) && (double) this.ti.intersectionPointDist == 0.0)
    {
      TMP_Text moveDistText = this.moveDistText;
      num1 = Mathf.Round(this.pf.tgtDist * 10f);
      string str3 = num1.ToString() + "m";
      moveDistText.text = str3;
    }
    else if ((double) this.ti.intersectionPointDist != 0.0)
    {
      TMP_Text moveDistText = this.moveDistText;
      num1 = Mathf.Round(this.ti.intersectionPointDist * 10f);
      string str4 = num1.ToString() + "m";
      moveDistText.text = str4;
    }
    else
      this.moveDistText.text = "0m";
    int num2 = (UnityEngine.Object) this.ti.selectedShip.currentTarget != (UnityEngine.Object) null ? 1 : 0;
  }

  private void NewShipSelected()
  {
    if ((UnityEngine.Object) this.ti.selectedShip == (UnityEngine.Object) null)
    {
      this.shipSelected = false;
      this.navPanel.SetActive(false);
    }
    else
    {
      this.shipSelected = true;
      this.navPanel.SetActive(true);
      this.ssrb = this.ti.selectedShip.GetComponent<Rigidbody>();
      this.pf = this.ti.selectedShip.GetComponent<Pathfinder>();
      this.rcsOnlyToggle.isOn = this.ti.selectedShip.GetComponent<Pathfinder>().isRCSOnly;
      this.showMoveLineToggle.isOn = this.ti.selectedShip.GetComponent<Pathfinder>().isShowMoveLine;
    }
  }
}
