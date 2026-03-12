// Decompiled with JetBrains decompiler
// Type: DebugStrat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DebugStrat : MonoBehaviour
{
  private uiAnimator uia;
  private bool isOn;

  private void Start() => this.uia = this.GetComponent<uiAnimator>();

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.BackQuote))
      this.Toggle();
    if (!this.isOn)
      return;
    this.Inputs();
  }

  private void Inputs()
  {
    if (!Input.GetKeyDown(KeyCode.Alpha0))
      return;
    FactionsManager.current.ModPlayerCredits("DEBUG", 1000000);
  }

  private void Toggle()
  {
    this.isOn = !this.isOn;
    if (this.isOn)
      this.uia.Show();
    else
      this.uia.Hide();
  }
}
