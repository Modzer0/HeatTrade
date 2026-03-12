// Decompiled with JetBrains decompiler
// Type: FleetNearbyUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FleetNearbyUI : MonoBehaviour
{
  private FleetManagerUI fmui;
  public FleetManager fm;
  [Header("UI")]
  [SerializeField]
  private TMP_Text nameText;
  [SerializeField]
  private int panelIndex;
  [SerializeField]
  private UnityEngine.UI.Toggle toggle;

  private void Start()
  {
    this.fmui = FleetManagerUI.current;
    this.toggle.group = this.transform.parent.GetComponent<ToggleGroup>();
  }

  public void Setup(FleetManager newFleet)
  {
    this.fm = newFleet;
    Track component = this.fm.GetComponent<Track>();
    this.nameText.text = $"FLEET [{component.id}] - {component.publicName}";
  }

  public void Toggle(bool isOn)
  {
    if (!isOn)
      return;
    this.fmui.SelectThisFleet(this.fm, this.panelIndex);
  }

  public void SetToggle(bool isOn)
  {
    if (!(bool) (Object) this.toggle)
      this.toggle = this.GetComponent<UnityEngine.UI.Toggle>();
    this.toggle.isOn = isOn;
  }
}
