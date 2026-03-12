// Decompiled with JetBrains decompiler
// Type: AdjustTimeScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

#nullable disable
public class AdjustTimeScale : MonoBehaviour
{
  private TextMeshProUGUI textMesh;

  private void Start() => this.textMesh = this.GetComponent<TextMeshProUGUI>();

  private void Update()
  {
    if ((double) Input.GetAxis("Mouse ScrollWheel") > 0.0)
    {
      if ((double) Time.timeScale < 1.0)
        Time.timeScale += 0.1f;
      Time.fixedDeltaTime = 0.02f * Time.timeScale;
      if (!((UnityEngine.Object) this.textMesh != (UnityEngine.Object) null))
        return;
      this.textMesh.text = "Time Scale : " + Math.Round((double) Time.timeScale, 2).ToString();
    }
    else
    {
      if ((double) Input.GetAxis("Mouse ScrollWheel") >= 0.0)
        return;
      if ((double) Time.timeScale >= 0.20000000298023224)
        Time.timeScale -= 0.1f;
      Time.fixedDeltaTime = 0.02f * Time.timeScale;
      if (!((UnityEngine.Object) this.textMesh != (UnityEngine.Object) null))
        return;
      this.textMesh.text = "Time Scale : " + Math.Round((double) Time.timeScale, 2).ToString();
    }
  }

  private void OnApplicationQuit()
  {
    Time.timeScale = 1f;
    Time.fixedDeltaTime = 0.02f;
  }
}
