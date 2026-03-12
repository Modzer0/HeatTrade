// Decompiled with JetBrains decompiler
// Type: CaptainToggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using TMPro;
using UnityEngine;

#nullable disable
public class CaptainToggle : MonoBehaviour
{
  private TacticalInputs ti;
  public bool isOn;
  private Captain captain;
  [SerializeField]
  private Transform ordersImg;
  [Header("UI")]
  [SerializeField]
  private TMP_Text nameText;
  [SerializeField]
  private TMP_Text classText;
  [SerializeField]
  private TMP_Text indexText;
  [SerializeField]
  private ShipButton indexButton;
  [SerializeField]
  private GameObject selectedImg;
  [SerializeField]
  private uiBar healthBar;
  private bool isDead;

  private void Start() => this.Init();

  private void Init() => this.ti = TacticalInputs.current;

  private void Update()
  {
    if (!(bool) (UnityEngine.Object) this.captain)
      return;
    this.UpdateData();
  }

  private void UpdateData()
  {
    this.healthBar.SetBarSize(this.captain.ship.totalHealthRatio);
    if (this.captain.isTakingNewOrders)
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

  public void SetShip(Captain newCaptain)
  {
    this.Init();
    if ((UnityEngine.Object) newCaptain == (UnityEngine.Object) null || (UnityEngine.Object) newCaptain.gameObject == (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      this.captain = newCaptain;
      this.nameText.text = this.captain.track.trackName;
      this.classText.text = $"{this.captain.ship.className} {this.captain.ship.classType}";
      int num = -1;
      string str = "";
      if (this.ti.shipsOwned != null && this.ti.shipsOwned.Contains(this.captain.ship))
      {
        num = this.ti.shipsOwned.IndexOf(this.captain.ship) + 1;
        str = num != 10 ? num.ToString() : "0";
      }
      this.indexText.text = str;
      this.indexButton.index = num;
      this.UpdateData();
      this.captain.GetComponent<ShipController>().OnShipDeath += new Action<int, ShipController, bool>(this.OnShipDeath);
    }
  }

  private void OnShipDeath(int i, ShipController ship, bool isHardDeath)
  {
    if (!this.isDead)
      this.StartCoroutine((IEnumerator) this.LateDestroySelf());
    this.isDead = true;
  }

  private IEnumerator LateDestroySelf()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    CaptainToggle captainToggle = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      UnityEngine.Object.Destroy((UnityEngine.Object) captainToggle.gameObject);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void OnDestroy()
  {
    if (!(bool) (UnityEngine.Object) this.captain)
      return;
    this.captain.GetComponent<ShipController>().OnShipDeath -= new Action<int, ShipController, bool>(this.OnShipDeath);
  }
}
