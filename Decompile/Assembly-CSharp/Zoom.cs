// Decompiled with JetBrains decompiler
// Type: Zoom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class Zoom : MonoBehaviour
{
  private Camera thisCam;
  public float defaultFOV = 60f;
  public float maxZoomFOV = 15f;
  [Range(0.0f, 1f)]
  public float currentZoom;
  public float sensitivity = 1f;

  private void Awake()
  {
    this.thisCam = this.GetComponent<Camera>();
    if (!(bool) (Object) this.thisCam)
      return;
    this.defaultFOV = this.thisCam.fieldOfView;
  }

  private void Update()
  {
    this.currentZoom += (float) ((double) Input.mouseScrollDelta.y * (double) this.sensitivity * 0.05000000074505806);
    this.currentZoom = Mathf.Clamp01(this.currentZoom);
    this.thisCam.fieldOfView = Mathf.Lerp(this.defaultFOV, this.maxZoomFOV, this.currentZoom);
  }
}
