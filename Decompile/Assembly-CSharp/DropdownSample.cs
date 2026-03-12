// Decompiled with JetBrains decompiler
// Type: DropdownSample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class DropdownSample : MonoBehaviour
{
  [SerializeField]
  private TextMeshProUGUI text;
  [SerializeField]
  private TMP_Dropdown dropdownWithoutPlaceholder;
  [SerializeField]
  private TMP_Dropdown dropdownWithPlaceholder;

  public void OnButtonClick()
  {
    this.text.text = this.dropdownWithPlaceholder.value > -1 ? $"Selected values:\n{this.dropdownWithoutPlaceholder.value.ToString()} - {this.dropdownWithPlaceholder.value.ToString()}" : "Error: Please make a selection";
  }
}
