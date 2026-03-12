// Decompiled with JetBrains decompiler
// Type: Revolver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Revolver : MonoBehaviour
{
  private TimeManager tm;
  [SerializeField]
  private bool isRevolving;
  [SerializeField]
  private float revolvePeriod;
  private float currentAngle;

  private void Start() => this.tm = TimeManager.current;

  private void Update()
  {
    if (!this.isRevolving || (double) this.revolvePeriod == 0.0)
      return;
    this.transform.RotateAround(this.transform.position, Vector3.up, -(360f / this.revolvePeriod * Time.deltaTime * this.tm.timeScale));
  }
}
