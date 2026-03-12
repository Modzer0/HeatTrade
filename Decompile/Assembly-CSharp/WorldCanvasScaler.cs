// Decompiled with JetBrains decompiler
// Type: WorldCanvasScaler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class WorldCanvasScaler : MonoBehaviour
{
  [SerializeField]
  private Transform uiCam;
  public Vector2 referenceResolution = new Vector2(1920f, 1080f);
  public float distance = 1f;

  private void Start() => this.ApplyLayout();

  public void ApplyLayout()
  {
    this.transform.SetParent(this.uiCam);
    this.transform.localPosition = new Vector3(0.0f, 0.0f, this.distance);
    this.transform.localRotation = Quaternion.identity;
    double num = 2.0 * (double) this.distance * (double) Mathf.Tan((float) ((double) Camera.main.fieldOfView * 0.5 * (Math.PI / 180.0)));
    this.transform.localScale = new Vector3((float) num * Camera.main.aspect / this.referenceResolution.x, (float) num / this.referenceResolution.y, 1f);
  }
}
