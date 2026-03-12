// Decompiled with JetBrains decompiler
// Type: Crouch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Crouch : MonoBehaviour
{
  public KeyCode key = KeyCode.LeftControl;
  [Header("Slow Movement")]
  [Tooltip("Movement to slow down when crouched.")]
  public FirstPersonMovement movement;
  [Tooltip("Movement speed when crouched.")]
  public float movementSpeed = 2f;
  [Header("Low Head")]
  [Tooltip("Head to lower when crouched.")]
  public Transform headToLower;
  [HideInInspector]
  public float? defaultHeadYLocalPosition;
  public float crouchYHeadPosition = 1f;
  [Tooltip("Collider to lower when crouched.")]
  public CapsuleCollider colliderToLower;
  [HideInInspector]
  public float? defaultColliderHeight;

  public bool IsCrouched { get; private set; }

  public event Action CrouchStart;

  public event Action CrouchEnd;

  private void Reset()
  {
    this.movement = this.GetComponentInParent<FirstPersonMovement>();
    this.headToLower = this.movement.GetComponentInChildren<Camera>().transform;
    this.colliderToLower = this.movement.GetComponentInChildren<CapsuleCollider>();
  }

  private void LateUpdate()
  {
    if (Input.GetKey(this.key))
    {
      if ((bool) (UnityEngine.Object) this.headToLower)
      {
        if (!this.defaultHeadYLocalPosition.HasValue)
          this.defaultHeadYLocalPosition = new float?(this.headToLower.localPosition.y);
        this.headToLower.localPosition = new Vector3(this.headToLower.localPosition.x, this.crouchYHeadPosition, this.headToLower.localPosition.z);
      }
      if ((bool) (UnityEngine.Object) this.colliderToLower)
      {
        if (!this.defaultColliderHeight.HasValue)
          this.defaultColliderHeight = new float?(this.colliderToLower.height);
        this.colliderToLower.height = Mathf.Max(this.defaultColliderHeight.Value - (!this.defaultHeadYLocalPosition.HasValue ? this.defaultColliderHeight.Value * 0.5f : this.defaultHeadYLocalPosition.Value - this.crouchYHeadPosition), 0.0f);
        this.colliderToLower.center = Vector3.up * this.colliderToLower.height * 0.5f;
      }
      if (this.IsCrouched)
        return;
      this.IsCrouched = true;
      this.SetSpeedOverrideActive(true);
      Action crouchStart = this.CrouchStart;
      if (crouchStart == null)
        return;
      crouchStart();
    }
    else
    {
      if (!this.IsCrouched)
        return;
      if ((bool) (UnityEngine.Object) this.headToLower)
        this.headToLower.localPosition = new Vector3(this.headToLower.localPosition.x, this.defaultHeadYLocalPosition.Value, this.headToLower.localPosition.z);
      if ((bool) (UnityEngine.Object) this.colliderToLower)
      {
        this.colliderToLower.height = this.defaultColliderHeight.Value;
        this.colliderToLower.center = Vector3.up * this.colliderToLower.height * 0.5f;
      }
      this.IsCrouched = false;
      this.SetSpeedOverrideActive(false);
      Action crouchEnd = this.CrouchEnd;
      if (crouchEnd == null)
        return;
      crouchEnd();
    }
  }

  private void SetSpeedOverrideActive(bool state)
  {
    if (!(bool) (UnityEngine.Object) this.movement)
      return;
    if (state)
    {
      if (this.movement.speedOverrides.Contains(new Func<float>(this.SpeedOverride)))
        return;
      this.movement.speedOverrides.Add(new Func<float>(this.SpeedOverride));
    }
    else
    {
      if (!this.movement.speedOverrides.Contains(new Func<float>(this.SpeedOverride)))
        return;
      this.movement.speedOverrides.Remove(new Func<float>(this.SpeedOverride));
    }
  }

  private float SpeedOverride() => this.movementSpeed;
}
