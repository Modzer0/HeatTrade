// Decompiled with JetBrains decompiler
// Type: CamController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CamController : MonoBehaviour
{
  private Camera cam;
  [SerializeField]
  private FirstPersonMovement fpm;
  private bool isTab;
  private bool isHoloViewOn;
  [SerializeField]
  private LayerMask holoOnLayers;
  [SerializeField]
  private LayerMask holoOffLayers;
  [SerializeField]
  private GameObject commandPanel;
  [SerializeField]
  private GameObject controlPanel;

  private void Start() => this.cam = this.GetComponent<Camera>();

  private void Update()
  {
    if (!Input.GetKeyDown(KeyCode.Tab))
      return;
    this.commandPanel.SetActive(false);
    this.controlPanel.SetActive(false);
    this.isHoloViewOn = !this.isHoloViewOn;
    this.cam.cullingMask = (int) (this.isHoloViewOn ? this.holoOnLayers : this.holoOffLayers);
    this.fpm.speed = this.isHoloViewOn ? 30f : 5f;
    this.fpm.runSpeed = this.isHoloViewOn ? 90f : 15f;
    if (this.isHoloViewOn)
      this.commandPanel.SetActive(true);
    else
      this.controlPanel.SetActive(true);
  }
}
