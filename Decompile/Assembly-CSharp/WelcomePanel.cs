// Decompiled with JetBrains decompiler
// Type: WelcomePanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WelcomePanel : MonoBehaviour
{
  [SerializeField]
  private uiAnimator welcomePanel;
  [SerializeField]
  private Toggle dontShowAgainToggle;

  private void Start()
  {
    MonoBehaviour.print((object) ("loading isHideWelcomePanel: " + PlayerPrefs.GetInt("MainMenu_isHideWelcomePanel", 0).ToString()));
    if (PlayerPrefs.GetInt("MainMenu_isHideWelcomePanel", 0) == 1)
      this.welcomePanel.Hide();
    else
      this.welcomePanel.Show();
    this.dontShowAgainToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt("MainMenu_isHideWelcomePanel", 0) == 1);
  }

  private void Update()
  {
    if (!Input.GetKeyDown(KeyCode.Escape))
      return;
    this.ClosePanel();
  }

  public void ClosePanel()
  {
    PlayerPrefs.SetInt("MainMenu_isHideWelcomePanel", this.dontShowAgainToggle.isOn ? 1 : 0);
    PlayerPrefs.Save();
    MonoBehaviour.print((object) ("saving isHideWelcomePanel: " + PlayerPrefs.GetInt("MainMenu_isHideWelcomePanel", 0).ToString()));
    this.welcomePanel.Hide();
  }
}
