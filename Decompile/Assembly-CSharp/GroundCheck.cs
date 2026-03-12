// Decompiled with JetBrains decompiler
// Type: GroundCheck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class GroundCheck : MonoBehaviour
{
  [Tooltip("Maximum distance from the ground.")]
  public float distanceThreshold = 0.15f;
  [Tooltip("Whether this transform is grounded now.")]
  public bool isGrounded = true;
  private const float OriginOffset = 0.001f;

  public event Action Grounded;

  private Vector3 RaycastOrigin => this.transform.position + Vector3.up * (1f / 1000f);

  private float RaycastDistance => this.distanceThreshold + 1f / 1000f;

  private void LateUpdate()
  {
    bool flag = Physics.Raycast(this.RaycastOrigin, Vector3.down, this.distanceThreshold * 2f);
    if (flag && !this.isGrounded)
    {
      Action grounded = this.Grounded;
      if (grounded != null)
        grounded();
    }
    this.isGrounded = flag;
  }

  private void OnDrawGizmosSelected()
  {
    Debug.DrawLine(this.RaycastOrigin, this.RaycastOrigin + Vector3.down * this.RaycastDistance, this.isGrounded ? Color.white : Color.red);
  }
}
