// Decompiled with JetBrains decompiler
// Type: CameraManager2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CameraManager2 : MonoBehaviour
{
  [Tooltip("")]
  public Camera Camera;
  [Tooltip("")]
  public Vector3 offset = new Vector3(-4f, 4f, 2f);
  [Tooltip("")]
  [HideInInspector]
  public static List<Transform> CameraTargets = new List<Transform>();
  private int currentTarget;

  private void Start() => this.currentTarget = 0;

  private void Update()
  {
    if (CameraManager2.CameraTargets.Count == 0)
      return;
    if (Input.GetKeyDown("x"))
    {
      ++this.currentTarget;
      if (this.currentTarget > CameraManager2.CameraTargets.Count - 1)
        this.currentTarget = 0;
    }
    if (Input.GetKeyDown("z"))
    {
      --this.currentTarget;
      if (this.currentTarget < 0)
        this.currentTarget = CameraManager2.CameraTargets.Count - 1;
    }
    if (!((Object) CameraManager2.CameraTargets[this.currentTarget] == (Object) null) || CameraManager2.CameraTargets.Count == 0)
      return;
    CameraManager2.CameraTargets.Remove(CameraManager2.CameraTargets[this.currentTarget]);
    ++this.currentTarget;
    if (this.currentTarget <= CameraManager2.CameraTargets.Count - 1)
      return;
    this.currentTarget = 0;
  }

  private void LateUpdate()
  {
    if (CameraManager2.CameraTargets.Count == 0)
      return;
    Transform cameraTarget = CameraManager2.CameraTargets[this.currentTarget];
    if ((Object) CameraManager2.CameraTargets[this.currentTarget] == (Object) null)
      return;
    this.Camera.transform.position = cameraTarget.position;
    this.Camera.transform.rotation = cameraTarget.rotation;
    this.Camera.transform.position += this.offset;
  }
}
