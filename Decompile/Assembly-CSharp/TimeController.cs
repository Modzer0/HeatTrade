// Decompiled with JetBrains decompiler
// Type: TimeController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class TimeController : MonoBehaviour
{
  public static TimeController current;
  public float timeScale = 1f;
  private float targetTimeScale = 1f;
  [SerializeField]
  private float lerpSpeed = 0.1f;
  private float secsElapsed;
  [SerializeField]
  private TMP_Text timer;
  [SerializeField]
  private TMP_Text timeScaleText;

  private void Start()
  {
    TimeController.current = this;
    this.UpdateTimescaleText();
  }

  private void Update()
  {
    this.Inputs();
    float timeScale = Time.timeScale;
    if ((double) timeScale == (double) this.targetTimeScale)
      return;
    this.timeScale = Mathf.Lerp(timeScale, this.targetTimeScale, this.lerpSpeed);
    if ((double) Mathf.Abs(this.targetTimeScale - this.timeScale) < 0.10000000149011612)
      this.timeScale = this.targetTimeScale;
    Time.timeScale = this.timeScale;
    this.UpdateTimescaleText();
  }

  private void UpdateTimescaleText()
  {
    this.timeScaleText.text = $"TIMESCALE: {this.timeScale.ToString("0.##")}x";
  }

  private void Inputs()
  {
    if (Input.GetKeyDown(KeyCode.F8))
      this.SetTargetTimeScale(2f);
    else if (Input.GetKeyDown(KeyCode.F7))
      this.SetTargetTimeScale(1f);
    else if (Input.GetKeyDown(KeyCode.F6))
    {
      this.SetTargetTimeScale(0.1f);
    }
    else
    {
      if (!Input.GetKeyDown(KeyCode.F5))
        return;
      this.SetTargetTimeScale(0.01f);
    }
  }

  public float GetTimeMultiplier() => 1f / this.timeScale;

  public void SetTargetTimeScale(float newTargetTS) => this.targetTimeScale = newTargetTS;
}
