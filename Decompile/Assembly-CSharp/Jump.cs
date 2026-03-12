// Decompiled with JetBrains decompiler
// Type: Jump
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Jump : MonoBehaviour
{
  private Rigidbody thisRB;
  public float jumpStrength = 2f;
  [SerializeField]
  [Tooltip("Prevents jumping when the transform is in mid-air.")]
  private GroundCheck groundCheck;

  public event Action Jumped;

  private void Reset() => this.groundCheck = this.GetComponentInChildren<GroundCheck>();

  private void Awake() => this.thisRB = this.GetComponent<Rigidbody>();

  private void LateUpdate()
  {
    if (!Input.GetButtonDown(nameof (Jump)) || (bool) (UnityEngine.Object) this.groundCheck && !this.groundCheck.isGrounded)
      return;
    this.thisRB.AddForce(Vector3.up * 100f * this.jumpStrength);
    Action jumped = this.Jumped;
    if (jumped == null)
      return;
    jumped();
  }
}
