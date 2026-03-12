// Decompiled with JetBrains decompiler
// Type: LoadingScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class LoadingScreen : MonoBehaviour
{
  [SerializeField]
  private RectTransform loadingBar;
  [SerializeField]
  private TMP_Text loadingText;
  [SerializeField]
  private uiAnimator uia;
  private float animationTick = 0.5f;
  private float loadProgress;
  [SerializeField]
  private TMP_Text consoleText;
  private float typingSpeed = 0.0001f;
  private LoadingScreen.LoadingMode loadingMode;

  private void Start()
  {
    if (!(bool) (Object) this.consoleText)
      return;
    this.consoleText.text = "";
  }

  private List<string> GetPreCombatLines()
  {
    return new List<string>()
    {
      "[SYSTEM] Booting up combat modules...",
      "[NAV] Targeting vector lock acquired",
      "[COMMS] Establishing fleet channel...",
      "[WEAPONS] Plasma lance charging...",
      "[DEFENSE] Shield matrix aligning...",
      "[DRIVE] Main engine coil warming..."
    };
  }

  private IEnumerator TypeLines()
  {
    LoadingScreen loadingScreen = this;
    List<(float, string)> valueTupleList;
    if (loadingScreen.loadingMode != LoadingScreen.LoadingMode.PreCombat)
    {
      valueTupleList = new List<(float, string)>()
      {
        (0.0f, "MONOLYTH Tactical Console Interface v2.9.14"),
        (0.0f, "Copyright (c) 2197 MONOLYTH."),
        (0.03f, "\\\\CORE-BOOT\\CONF\\start.sys loaded successfully."),
        (0.06f, "C:\\FleetOps\\LaunchControl.exe"),
        (0.1f, "[SYSTEM] Initializing postcombat routines..."),
        (0.13f, "[NAV] Calculating exit trajectory..."),
        (0.16f, "[AI] Compiling battle telemetry..."),
        (0.2f, "[HULL] Microfracture diagnostics initiated..."),
        (0.23f, "[THERMAL] Disabling combat mode..."),
        (0.26f, "[POWER] Weapons powering down..."),
        (0.3f, "[CREW] Autodoc systems standing by for triage."),
        (0.33f, "[WEAPONS] Diagnosing weapon systems..."),
        (0.36f, "[AI] Threat level nominal."),
        (0.4f, "[DAMAGE] Structural repairs queued..."),
        (0.43f, "[RCS] Stabilizing inertial drift..."),
        (0.46f, "[CREW] CONDITION GREEN. All hands stand down."),
        (0.5f, "C:\\CommandOps\\shutdown.bat"),
        (0.5f, ""),
        (0.5f, "[SYSTEM] Preparing combat debrief."),
        (0.5f, "> ENGAGEMENT COMPLETE")
      };
    }
    else
    {
      valueTupleList = new List<(float, string)>();
      valueTupleList.Add((0.0f, "MONOLYTH Tactical Console Interface v2.9.14"));
      valueTupleList.Add((0.0f, "Copyright (c) 2197 MONOLYTH."));
      valueTupleList.Add((0.03f, "\\\\CORE-BOOT\\CONF\\start.sys loaded successfully."));
      valueTupleList.Add((0.06f, "C:\\FleetOps\\LaunchControl.exe"));
      valueTupleList.Add((0.1f, "[SYSTEM] Initializing precombat routines..."));
      valueTupleList.Add((0.13f, "[NAV] Calculating intercept vectors..."));
      valueTupleList.Add((0.16f, "[DRIVE] Calculating deceleration burn..."));
      valueTupleList.Add((0.2f, "[WEAPONS] Calibrating all systems..."));
      valueTupleList.Add((0.23f, "[THERMAL] Initiating combat mode."));
      valueTupleList.Add((0.26f, "[POWER] Power to weapons."));
      valueTupleList.Add((0.3f, "[COMMS] Fleet link established."));
      valueTupleList.Add((0.33f, "[HULL] Microfracture diagnostics initiated..."));
      valueTupleList.Add((0.36f, "[AI] Threat modeling initiated."));
      valueTupleList.Add((0.4f, "[TARGETING] Calibrating targeting systems..."));
      valueTupleList.Add((0.43f, "[RCS] Maneuvering vector normalized."));
      valueTupleList.Add((0.46f, "[CREW] GENERAL QUARTERS. All hands man battle stations."));
      valueTupleList.Add((0.5f, "C:\\CommandOps\\launch.bat"));
      valueTupleList.Add((0.5f, ""));
      valueTupleList.Add((0.5f, "[SYSTEM] WEAPONS FREE."));
      valueTupleList.Add((0.5f, "> PREPARE FOR COMBAT!"));
    }
    List<(float, string)> lines = valueTupleList;
    int lineIndex = 0;
    while (lineIndex < lines.Count)
    {
      float num = lines[lineIndex].Item1;
      string line = lines[lineIndex].Item2;
      if ((double) loadingScreen.loadProgress >= (double) num)
      {
        yield return (object) loadingScreen.StartCoroutine((IEnumerator) loadingScreen.TypeLine(line));
        ++lineIndex;
      }
      else
        yield return (object) null;
    }
  }

  private IEnumerator TypeLine(string line)
  {
    this.consoleText.text += line;
    this.consoleText.text += "\n";
    yield return (object) null;
  }

  private IEnumerator AnimateText()
  {
    while (true)
    {
      this.loadingText.text = "LOADING";
      yield return (object) new WaitForSeconds(this.animationTick);
      this.loadingText.text = "LOADING.";
      yield return (object) new WaitForSeconds(this.animationTick);
      this.loadingText.text = "LOADING..";
      yield return (object) new WaitForSeconds(this.animationTick);
      this.loadingText.text = "LOADING...";
      yield return (object) new WaitForSeconds(this.animationTick);
    }
  }

  public void Show()
  {
    this.gameObject.SetActive(true);
    this.StartCoroutine((IEnumerator) this.AnimateText());
    this.uia.Show();
    this.StartCoroutine((IEnumerator) this.TypeLines());
  }

  public void Hide()
  {
    this.StopCoroutine((IEnumerator) this.AnimateText());
    this.uia.Hide(new float?(1f));
  }

  public void UpdateProgress(float progress)
  {
    this.loadProgress = progress;
    this.loadingBar.localScale = new Vector3(this.loadProgress, 1f, 1f);
  }

  public enum LoadingMode
  {
    PreCombat,
    PostCombat,
  }
}
