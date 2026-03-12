// Decompiled with JetBrains decompiler
// Type: MainMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MainMenu : MonoBehaviour
{
  private AudioManager audioManager;
  private FactionsManager fm;
  [SerializeField]
  private EndScreen es;
  private SceneTransitionManager stm;
  [Header("= UI ====================")]
  [Header("NEW GAME")]
  [SerializeField]
  private Image customFlag;
  [SerializeField]
  private TMP_InputField customName;
  [SerializeField]
  private TMP_InputField customWords;
  [SerializeField]
  private TMP_InputField customDesc;
  [SerializeField]
  private TMP_InputField customCode;
  [Header("FLAGS")]
  [SerializeField]
  private TMP_Text flagsPathText;
  [SerializeField]
  private Color colorPrimary;
  [SerializeField]
  private Color colorSecondary;
  [Header("BACKGROUND")]
  [SerializeField]
  private TMP_Text backgroundText;
  private PlayerBackground currentBackground;
  private int currentBackgroundIndex;
  [SerializeField]
  private List<PlayerBackground> backgrounds = new List<PlayerBackground>();
  [Header("EXAMPLE")]
  [SerializeField]
  private Image exampleImg;
  [SerializeField]
  private TMP_Text exampleText;
  private string flagsPath;

  private void Start()
  {
    Time.timeScale = 1f;
    this.audioManager = AudioManager.current;
    this.fm = FactionsManager.current;
    this.stm = SceneTransitionManager.current;
    this.flagsPath = this.fm.flagsPath;
    this.SetBackground(0);
  }

  public void SetBackground(int newIndex)
  {
    this.currentBackgroundIndex = newIndex;
    this.currentBackground = this.backgrounds[this.currentBackgroundIndex];
    this.backgroundText.text = $"{this.currentBackground.name}\n\n{this.currentBackground.desc}";
  }

  public void SetExampleText() => this.exampleText.text = this.customName.text;

  public void SetPrimaryColor(Color newColor)
  {
    this.colorPrimary = newColor;
    this.exampleImg.color = this.colorPrimary;
  }

  public void SetSecondaryColor(Color newColor)
  {
    this.colorSecondary = newColor;
    this.exampleText.color = this.colorSecondary;
  }

  public void Refresh()
  {
    MonoBehaviour.print((object) "REFRESHING");
    string text = this.customName.text;
    string path = Path.Combine(this.flagsPath, text + ".png");
    if (!File.Exists(path))
    {
      MonoBehaviour.print((object) ("flagPath NOT FOUND: " + path));
      this.customFlag.gameObject.SetActive(false);
      this.audioManager.PlaySFX(4);
    }
    else
    {
      MonoBehaviour.print((object) ("flagPath FOUND: " + path));
      this.customFlag.gameObject.SetActive(true);
      this.customFlag.sprite = this.fm.GetCustomSprite(text);
      this.customFlag.gameObject.SetActive(true);
      this.audioManager.PlaySFX(1);
    }
  }

  public void OpenFilePath()
  {
    if (!Directory.Exists(this.flagsPath))
      Directory.CreateDirectory(this.flagsPath);
    Process.Start("explorer.exe", this.flagsPath.Replace("/", "\\"));
    MonoBehaviour.print((object) ("flagsPath: " + this.flagsPath));
    this.audioManager.PlaySFX(1);
  }

  public void NewGamePrometheus()
  {
    FactionsManager.current.playerBackground = (PlayerBackground) null;
    FactionsManager.current.SetupNewGame(true, (Faction) null);
    this.StartNewGame();
  }

  public void NewGameCustom()
  {
    bool flag = false;
    if (this.customName.text == "")
      flag = true;
    else if (this.customCode.text == "")
      flag = true;
    else if (this.customName.text == "Prometheus" || this.customName.text == "'Prometheus' is not allowed!")
    {
      flag = true;
      this.customName.text = "'Prometheus' is not allowed!";
    }
    if (flag)
    {
      this.audioManager.PlaySFX(3);
    }
    else
    {
      Faction playerFaction = new Faction(this.customName.text, this.customWords.text, this.customDesc.text, this.customCode.text.ToUpper(), this.colorPrimary, this.colorSecondary, this.customFlag.sprite);
      FactionsManager.current.playerBackground = this.currentBackground;
      FactionsManager.current.SetupNewGame(false, playerFaction);
      this.StartNewGame();
    }
  }

  public void NewGameLoad() => this.StartNewGame();

  public void NewGameSkirmish()
  {
    FactionsManager.current.playerBackground = (PlayerBackground) null;
    FactionsManager.current.SetupNewGame(true, (Faction) null);
  }

  private void StartNewGame()
  {
    this.audioManager.PlaySFX(1);
    this.es.LoadScene("SCENE - Prometheus");
  }

  public void ExitGame() => Application.Quit();
}
