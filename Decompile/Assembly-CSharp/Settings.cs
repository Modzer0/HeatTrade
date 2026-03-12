// Decompiled with JetBrains decompiler
// Type: Settings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

#nullable disable
public class Settings : MonoBehaviour
{
  private CameraIdleShake cis;
  private TacticalCamera tc;
  private ColorblindManager cbm;
  [Header("= DATA ==========")]
  [Header("Audio")]
  public AudioMixer audioMixer;
  [Header("Video")]
  private Resolution[] resolutions;
  [Header("= UI ==========")]
  [Header("Audio")]
  [SerializeField]
  private Slider volumeSlider;
  [SerializeField]
  private TMP_Text volumeText;
  [Header("Video")]
  [SerializeField]
  private TMP_Dropdown qualityDropdown;
  [SerializeField]
  private Toggle fullscreenToggle;
  [SerializeField]
  private TMP_Dropdown resolutionDropdown;
  [SerializeField]
  private Slider uiScaleSlider;
  [SerializeField]
  private TMP_Text uiScaleText;
  [SerializeField]
  private Slider colorblindSlider;
  [SerializeField]
  private TMP_Text colorblindText;
  [SerializeField]
  private TMP_Dropdown colorblindDropdown;
  private TacticalInputs ti;
  [Header("Game")]
  [Header("TACTICAL")]
  [SerializeField]
  private Toggle laserbeamToggle;
  private LaserToggler lt;
  [SerializeField]
  private Toggle exhaustDamageToggle;
  private ExhaustPlumes ep;
  [SerializeField]
  private Toggle screenshakeToggle;
  [SerializeField]
  private Toggle chromaticAberrationToggle;
  [SerializeField]
  private Slider mouseSensSlider;
  [SerializeField]
  private TMP_Text mouseSensText;
  private MapInputs mi;
  private KeyCode[] allKeys;
  private string keySaveHeader = "Settings_Key_";
  [Header("Controls")]
  public List<Keybind> keybinds = new List<Keybind>();

  private void Start()
  {
    this.cis = UnityEngine.Object.FindObjectOfType<CameraIdleShake>();
    this.tc = TacticalCamera.current;
    this.lt = LaserToggler.current;
    this.ep = ExhaustPlumes.current;
    this.ti = TacticalInputs.current;
    this.mi = MapInputs.current;
    this.cbm = this.GetComponent<ColorblindManager>();
    this.allKeys = (KeyCode[]) Enum.GetValues(typeof (KeyCode));
    this.LoadVolume();
    this.LoadQuality();
    this.LoadFullScreen();
    this.LoadResolution();
    this.LoadUIScale();
    this.LoadColorblindStrength();
    this.LoadColorblindMode();
    this.LoadLaserVisibility();
    this.LoadExhaustFF();
    this.LoadMouseSens();
    this.LoadChromaticAberration();
    this.LoadScreenShake();
    this.LoadKeybinds();
    this.UpdateInputs();
  }

  public void SetVolume(float sliderValue)
  {
    this.audioMixer.SetFloat("Volume", Mathf.Log10(sliderValue) * 20f);
    PlayerPrefs.SetFloat("Settings_SavedVolume", sliderValue);
    PlayerPrefs.Save();
    this.volumeText.text = Mathf.RoundToInt(sliderValue * 100f).ToString() + "%";
  }

  private void LoadVolume()
  {
    float sliderValue = PlayerPrefs.GetFloat("Settings_SavedVolume", 0.75f);
    this.volumeSlider.value = sliderValue;
    this.SetVolume(sliderValue);
  }

  public void SetQuality(int qualityIndex)
  {
    QualitySettings.SetQualityLevel(qualityIndex);
    PlayerPrefs.SetInt("Settings_QualityLevel", qualityIndex);
    PlayerPrefs.Save();
  }

  private void LoadQuality()
  {
    int qualityIndex = PlayerPrefs.GetInt("Settings_QualityLevel", 1);
    this.qualityDropdown.value = qualityIndex;
    this.SetQuality(qualityIndex);
  }

  public void SetFullScreen(bool isFullScreen)
  {
    Screen.fullScreen = isFullScreen;
    PlayerPrefs.SetInt("Settings_FullScreen", isFullScreen ? 1 : 0);
    PlayerPrefs.Save();
  }

  private void LoadFullScreen()
  {
    bool isFullScreen = PlayerPrefs.GetInt("Settings_FullScreen", 1) == 1;
    this.fullscreenToggle.isOn = isFullScreen;
    this.SetFullScreen(isFullScreen);
  }

  private void SetupResolution()
  {
    this.resolutions = Screen.resolutions;
    this.resolutionDropdown.ClearOptions();
    List<string> options = new List<string>();
    List<Resolution> resolutionList = new List<Resolution>();
    for (int index = 0; index < this.resolutions.Length; ++index)
    {
      string str = $"{this.resolutions[index].width.ToString()} x {this.resolutions[index].height.ToString()}";
      if (!options.Contains(str))
      {
        options.Add(str);
        resolutionList.Add(this.resolutions[index]);
      }
    }
    this.resolutions = resolutionList.ToArray();
    this.resolutionDropdown.AddOptions(options);
  }

  public void SetResolution(int resolutionIndex)
  {
    Resolution resolution = this.resolutions[resolutionIndex];
    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    this.resolutionDropdown.value = resolutionIndex;
    this.resolutionDropdown.RefreshShownValue();
    PlayerPrefs.SetInt("Settings_ResolutionIndex", resolutionIndex);
    PlayerPrefs.Save();
  }

  private void LoadResolution()
  {
    this.SetupResolution();
    this.SetResolution(Mathf.Clamp(PlayerPrefs.GetInt("Settings_ResolutionIndex", this.resolutions.Length - 1), 0, this.resolutions.Length - 1));
  }

  public void SetUIScale(float newScale)
  {
    newScale = (float) Mathf.RoundToInt(newScale * 100f) / 100f;
    foreach (CanvasScaler canvasScaler in UnityEngine.Object.FindObjectsOfType<CanvasScaler>())
      canvasScaler.scaleFactor = newScale;
    PlayerPrefs.SetFloat("Settings_uiScale", newScale);
    PlayerPrefs.Save();
    this.uiScaleText.text = Mathf.RoundToInt(newScale * 100f).ToString() + "%";
  }

  private void LoadUIScale()
  {
    float newScale = PlayerPrefs.GetFloat("Settings_uiScale", 1f);
    this.uiScaleSlider.value = newScale;
    this.SetUIScale(newScale);
  }

  public void SetColorblindStrength(float newValue)
  {
    newValue = (float) Mathf.RoundToInt(newValue * 100f) / 100f;
    this.cbm.OnColorblindSliderChanged(newValue);
    PlayerPrefs.SetFloat("Settings_ColorblindModeStrength", newValue);
    PlayerPrefs.Save();
    this.colorblindText.text = Mathf.RoundToInt(newValue * 100f).ToString() + "%";
  }

  private void LoadColorblindStrength()
  {
    float newValue = PlayerPrefs.GetFloat("Settings_ColorblindModeStrength", 1f);
    this.colorblindSlider.value = newValue;
    this.SetColorblindStrength(newValue);
  }

  public void SetColorblindMode(int index)
  {
    this.cbm.OnColorblindDropdownChanged(index);
    PlayerPrefs.SetInt("Settings_ColorblindModeIndex", index);
    PlayerPrefs.Save();
  }

  private void LoadColorblindMode()
  {
    int index = PlayerPrefs.GetInt("Settings_ColorblindModeIndex", 0);
    this.colorblindDropdown.value = index;
    this.SetColorblindMode(index);
  }

  public void ToggleLaserBeam()
  {
    bool isOn = this.laserbeamToggle.isOn;
    PlayerPrefs.SetInt("Settings_LasersVisible", isOn ? 1 : 0);
    PlayerPrefs.Save();
    if (!(bool) (UnityEngine.Object) this.lt)
      return;
    this.lt.ToggleLaserVisibility(new bool?(isOn));
  }

  private void LoadLaserVisibility()
  {
    this.laserbeamToggle.isOn = PlayerPrefs.GetInt("Settings_LasersVisible", 1) == 1;
    this.ToggleLaserBeam();
  }

  public void ToggleExhaustDamage()
  {
    bool isOn = this.exhaustDamageToggle.isOn;
    PlayerPrefs.SetInt("Settings_ExhaustFF", isOn ? 1 : 0);
    PlayerPrefs.Save();
    if (!(bool) (UnityEngine.Object) this.ep)
      return;
    this.ep.ToggleFriendlyFire(isOn);
  }

  private void LoadExhaustFF()
  {
    this.exhaustDamageToggle.isOn = PlayerPrefs.GetInt("Settings_ExhaustFF", 1) == 1;
    this.ToggleExhaustDamage();
  }

  public void ToggleScreenshake(bool isOn)
  {
    PlayerPrefs.SetInt("Settings_Screenshake", isOn ? 1 : 0);
    PlayerPrefs.Save();
    if (!(bool) (UnityEngine.Object) this.cis)
      return;
    this.cis.Toggle(isOn);
  }

  private void LoadScreenShake()
  {
    bool isOn = PlayerPrefs.GetInt("Settings_Screenshake", 1) == 1;
    this.screenshakeToggle.isOn = isOn;
    this.ToggleScreenshake(isOn);
  }

  public void ToggleChromaticAberration(bool isOn)
  {
    foreach (Volume volume in UnityEngine.Object.FindObjectsOfType<Volume>())
    {
      ChromaticAberration component;
      if ((UnityEngine.Object) volume != (UnityEngine.Object) null && (UnityEngine.Object) volume.profile != (UnityEngine.Object) null && volume.profile.TryGet<ChromaticAberration>(out component))
      {
        component.active = isOn;
        PlayerPrefs.SetInt("Settings_ChromaticAberration", isOn ? 1 : 0);
        PlayerPrefs.Save();
      }
    }
  }

  private void LoadChromaticAberration()
  {
    bool isOn = PlayerPrefs.GetInt("Settings_ChromaticAberration", 1) == 1;
    this.chromaticAberrationToggle.isOn = isOn;
    this.ToggleChromaticAberration(isOn);
  }

  public void SetMouseScrollSens(float newSens)
  {
    PlayerPrefs.SetFloat("Settings_MouseSensitivity", newSens);
    PlayerPrefs.Save();
    this.mouseSensText.text = Mathf.RoundToInt(newSens * 100f).ToString() + "%";
    if (!(bool) (UnityEngine.Object) this.tc)
      return;
    this.tc.mouseSensMult = newSens;
  }

  private void LoadMouseSens()
  {
    float newSens = PlayerPrefs.GetFloat("Settings_MouseSensitivity", 1f);
    this.mouseSensSlider.value = newSens;
    this.SetMouseScrollSens(newSens);
  }

  private void LoadKeybinds()
  {
    MonoBehaviour.print((object) "LOAD KEYBINDS");
    foreach (Keybind keybind in this.keybinds)
    {
      keybind.key = this.GetSavedKey(keybind.name, keybind.defaultKey);
      keybind.text.text = keybind.key.ToString();
    }
  }

  private KeyCode GetSavedKey(string keySaveName, KeyCode defaultKey)
  {
    string str = PlayerPrefs.GetString(this.keySaveHeader + keySaveName, "");
    KeyCode result;
    return str == "" || !Enum.TryParse<KeyCode>(str, out result) ? defaultKey : result;
  }

  private void UpdateInputs()
  {
    if ((bool) (UnityEngine.Object) this.ti)
    {
      this.ti.UpdateInputs(this);
    }
    else
    {
      if (!(bool) (UnityEngine.Object) this.mi)
        return;
      this.mi.UpdateInputs(this);
    }
  }

  public void StartWaitForKey(string keybindName)
  {
    TMP_Text keybindText = this.GetKeybindText(keybindName);
    if ((UnityEngine.Object) keybindText == (UnityEngine.Object) null)
      return;
    keybindText.text = "Press any key...";
    this.StartCoroutine((IEnumerator) this.WaitForKey(keybindName));
  }

  private TMP_Text GetKeybindText(string keybindName)
  {
    foreach (Keybind keybind in this.keybinds)
    {
      if (keybind.name == keybindName)
        return keybind.text;
    }
    return (TMP_Text) null;
  }

  private IEnumerator WaitForKey(string keybindName)
  {
    yield return (object) new WaitForEndOfFrame();
    bool done = false;
    while (!done)
    {
      foreach (KeyCode allKey in this.allKeys)
      {
        if (Input.GetKeyDown(allKey))
        {
          this.SetKeybind(keybindName, allKey);
          this.UpdateInputs();
          done = true;
        }
      }
      yield return (object) null;
    }
  }

  private void SetKeybind(string keybindName, KeyCode newKey)
  {
    this.GetKeybindText(keybindName).text = newKey.ToString();
    foreach (Keybind keybind in this.keybinds)
    {
      if (keybind.name == keybindName)
        keybind.key = newKey;
    }
    PlayerPrefs.SetString(this.keySaveHeader + keybindName, newKey.ToString());
    PlayerPrefs.Save();
  }

  public KeyCode GetKeyCode(string name)
  {
    foreach (Keybind keybind in this.keybinds)
    {
      if (keybind.name == name)
        return keybind.key;
    }
    Debug.LogError((object) ("Keybind not found: " + name));
    return KeyCode.None;
  }

  public void ResetKeybinds()
  {
    foreach (Keybind keybind in this.keybinds)
      this.SetKeybind(keybind.name, keybind.defaultKey);
    this.UpdateInputs();
  }
}
