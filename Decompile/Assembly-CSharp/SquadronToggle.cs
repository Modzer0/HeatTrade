// Decompiled with JetBrains decompiler
// Type: SquadronToggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class SquadronToggle : MonoBehaviour
{
  private PlayerFleetUI pfui;
  private TacticalInputs ti;
  [SerializeField]
  private GameObject selectedImg;
  [SerializeField]
  private Transform ordersImg;
  private Squadron squadron;
  public int squadronIndex;
  public bool isOn;
  [SerializeField]
  private TMP_Text indexText;

  private void Start()
  {
    this.pfui = PlayerFleetUI.current;
    this.ti = TacticalInputs.current;
  }

  private void Update()
  {
    if (!(bool) (Object) this.squadron)
      return;
    if (this.squadron.isTakingNewOrders)
    {
      this.ordersImg.gameObject.SetActive(true);
      this.ordersImg.Rotate(-Vector3.forward, Time.deltaTime * 360f);
    }
    else
    {
      this.ordersImg.gameObject.SetActive(false);
      this.ordersImg.rotation = Quaternion.identity;
    }
  }

  public void Toggle(bool newIsOn)
  {
    this.isOn = newIsOn;
    this.selectedImg.SetActive(this.isOn);
  }

  public void OnClick() => this.ti.SetSelectedSquadronIndex(this.squadronIndex);

  public void SetSquadron(Squadron newSquadron)
  {
    this.squadron = newSquadron;
    this.squadronIndex = this.squadron.index;
    this.indexText.text = "F" + (this.squadronIndex + 1).ToString();
  }
}
