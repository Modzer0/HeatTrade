// Decompiled with JetBrains decompiler
// Type: WorldWidth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class WorldWidth : MonoBehaviour
{
  public static WorldWidth current;
  private Camera cam;
  private Transform camTransform;

  private void Start()
  {
    WorldWidth.current = this;
    this.cam = Camera.main;
    this.camTransform = this.cam.transform;
  }

  public float GetWorldWidth(Vector3 startPos)
  {
    return !(bool) (UnityEngine.Object) this.camTransform ? 0.0f : 2f * Vector3.Distance(this.camTransform.position, startPos) * Mathf.Tan(this.cam.fieldOfView * ((float) Math.PI / 180f) / 2f) / (float) Screen.height;
  }
}
