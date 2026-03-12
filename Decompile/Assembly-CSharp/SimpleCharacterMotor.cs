// Decompiled with JetBrains decompiler
// Type: SimpleCharacterMotor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (CharacterController))]
public class SimpleCharacterMotor : MonoBehaviour
{
  public CursorLockMode cursorLockMode = CursorLockMode.Locked;
  public bool cursorVisible;
  [Header("Movement")]
  public float walkSpeed = 2f;
  public float runSpeed = 4f;
  public float gravity = 9.8f;
  [Space]
  [Header("Look")]
  public Transform cameraPivot;
  public float lookSpeed = 45f;
  public bool invertY = true;
  [Space]
  [Header("Smoothing")]
  public float movementAcceleration = 1f;
  private CharacterController controller;
  private Vector3 movement;
  private Vector3 finalMovement;
  private float speed;
  private Quaternion targetRotation;
  private Quaternion targetPivotRotation;

  private void Awake()
  {
    this.controller = this.GetComponent<CharacterController>();
    Cursor.lockState = this.cursorLockMode;
    Cursor.visible = this.cursorVisible;
    this.targetRotation = this.targetPivotRotation = Quaternion.identity;
  }

  private void Update()
  {
    this.UpdateTranslation();
    this.UpdateLookRotation();
  }

  private void UpdateLookRotation()
  {
    float axis1 = Input.GetAxis("Mouse Y");
    float axis2 = Input.GetAxis("Mouse X");
    float num = axis1 * (this.invertY ? -1f : 1f);
    this.targetRotation = this.transform.localRotation * Quaternion.AngleAxis(axis2 * this.lookSpeed * Time.deltaTime, Vector3.up);
    this.targetPivotRotation = this.cameraPivot.localRotation * Quaternion.AngleAxis(num * this.lookSpeed * Time.deltaTime, Vector3.right);
    this.transform.localRotation = this.targetRotation;
    this.cameraPivot.localRotation = this.targetPivotRotation;
  }

  private void UpdateTranslation()
  {
    if (this.controller.isGrounded)
    {
      float axis1 = Input.GetAxis("Horizontal");
      float axis2 = Input.GetAxis("Vertical");
      bool key = Input.GetKey(KeyCode.LeftShift);
      Vector3 vector3 = new Vector3(axis1, 0.0f, axis2);
      this.speed = key ? this.runSpeed : this.walkSpeed;
      this.movement = this.transform.TransformDirection(vector3 * this.speed);
    }
    else
      this.movement.y -= this.gravity * Time.deltaTime;
    this.finalMovement = Vector3.Lerp(this.finalMovement, this.movement, Time.deltaTime * this.movementAcceleration);
    int num = (int) this.controller.Move(this.finalMovement * Time.deltaTime);
  }
}
