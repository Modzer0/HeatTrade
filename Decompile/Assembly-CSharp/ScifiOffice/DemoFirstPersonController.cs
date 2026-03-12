// Decompiled with JetBrains decompiler
// Type: ScifiOffice.DemoFirstPersonController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ScifiOffice;

public class DemoFirstPersonController : MonoBehaviour
{
  private Rigidbody rb;
  private CapsuleCollider col;
  private bool isCrouching;
  public Transform playerBody;
  public DemoFirstPersonController.ControlType controlType;
  [Header("Movement")]
  public float speed = 3f;
  public float accelerationRate = 12f;
  public float crouchFactor = 0.5f;
  public float decelerationFactor = 1f;
  public float mouseSensitivity = 50f;
  private float xRot;
  private float horizontalMovement;
  private float verticalMovement;
  [Header("HUD")]
  public GameObject canvas;

  private void Start()
  {
    this.rb = this.playerBody.GetComponent<Rigidbody>();
    this.col = this.playerBody.GetComponent<CapsuleCollider>();
    if (this.controlType != DemoFirstPersonController.ControlType.keyboardMouse)
      return;
    Cursor.lockState = CursorLockMode.Locked;
  }

  private void Update()
  {
    this.Walk();
    this.Look();
    if (Input.GetKeyDown(KeyCode.E))
    {
      if (this.controlType == DemoFirstPersonController.ControlType.keyboardMouse)
      {
        this.controlType = DemoFirstPersonController.ControlType.keyboard;
        this.xRot = 0.0f;
      }
      else
        this.controlType = DemoFirstPersonController.ControlType.keyboardMouse;
    }
    else if (this.controlType == DemoFirstPersonController.ControlType.android)
    {
      this.canvas.SetActive(true);
    }
    else
    {
      this.Crouch();
      this.canvas.SetActive(false);
    }
  }

  public void Look()
  {
    float num1 = 0.0f;
    float num2;
    switch (this.controlType)
    {
      case DemoFirstPersonController.ControlType.android:
        num2 = this.horizontalMovement * Time.deltaTime * this.mouseSensitivity;
        break;
      case DemoFirstPersonController.ControlType.keyboard:
        num2 = Input.GetAxis("Horizontal") * this.mouseSensitivity * Time.deltaTime;
        num1 = 0.0f;
        break;
      default:
        num2 = Input.GetAxis("Mouse X") * this.mouseSensitivity * Time.deltaTime;
        num1 = Input.GetAxis("Mouse Y") * this.mouseSensitivity * Time.deltaTime;
        break;
    }
    this.xRot -= num1;
    this.xRot = Mathf.Clamp(this.xRot, -90f, 90f);
    this.transform.localRotation = Quaternion.Euler(this.xRot, 0.0f, 0.0f);
    this.playerBody.Rotate(Vector3.up * num2);
  }

  private void Walk()
  {
    float speed = this.speed;
    float accelerationRate = this.accelerationRate;
    if (this.isCrouching)
    {
      speed *= this.crouchFactor;
      accelerationRate *= this.crouchFactor;
    }
    Vector3 vector3;
    switch (this.controlType)
    {
      case DemoFirstPersonController.ControlType.android:
        vector3 = this.playerBody.transform.forward * this.verticalMovement;
        break;
      case DemoFirstPersonController.ControlType.keyboard:
        vector3 = this.playerBody.transform.forward * Input.GetAxis("Vertical");
        break;
      default:
        vector3 = this.playerBody.transform.forward * Input.GetAxis("Vertical") + this.playerBody.transform.right * Input.GetAxis("Horizontal");
        break;
    }
    float magnitude1 = vector3.magnitude;
    if ((double) magnitude1 > 0.0)
    {
      this.rb.velocity += vector3 / magnitude1 * Time.deltaTime * accelerationRate;
      if ((double) this.rb.velocity.magnitude <= (double) speed)
        return;
      this.rb.velocity = this.rb.velocity.normalized * this.speed;
    }
    else
    {
      float magnitude2 = this.rb.velocity.magnitude;
      float num = this.accelerationRate * this.decelerationFactor * Time.deltaTime;
      if ((double) magnitude2 < (double) num)
        this.rb.velocity = Vector3.zero;
      else
        this.rb.velocity -= this.rb.velocity.normalized * num;
    }
  }

  private void Crouch()
  {
    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftShift))
    {
      this.col.height = 0.5f;
      this.isCrouching = true;
    }
    else
    {
      this.col.height = 2f;
      this.isCrouching = false;
    }
  }

  public void MobileCrouch()
  {
    if (this.isCrouching)
    {
      this.col.height = 2f;
      this.isCrouching = false;
    }
    else
    {
      this.col.height = 0.5f;
      this.isCrouching = true;
    }
  }

  public void MobileWalk(int direction)
  {
    if (direction * direction == 1)
      this.horizontalMovement = (float) direction;
    else if (direction == 3)
    {
      this.horizontalMovement = 0.0f;
      this.verticalMovement = 0.0f;
    }
    else
      this.verticalMovement = (float) (direction - 1);
  }

  public enum ControlType
  {
    android,
    keyboard,
    keyboardMouse,
  }
}
