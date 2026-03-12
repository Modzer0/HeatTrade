// Decompiled with JetBrains decompiler
// Type: Toggler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Toggler : MonoBehaviour
{
  [SerializeField]
  private GameObject toToggle;

  private void Start()
  {
  }

  private void Update()
  {
  }

  public void Toggle() => this.toToggle.SetActive(!this.toToggle.activeSelf);

  public void ToggleOn() => this.toToggle.SetActive(true);

  public void ToggleOff() => this.toToggle.SetActive(false);
}
