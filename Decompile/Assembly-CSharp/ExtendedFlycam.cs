// Decompiled with JetBrains decompiler
// Type: ExtendedFlycam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ExtendedFlycam : MonoBehaviour
{
  public float cameraSensitivity = 90f;
  public float climbSpeed = 4f;
  public float normalMoveSpeed = 10f;
  public float slowMoveFactor = 0.25f;
  public float fastMoveFactor = 3f;
  private float rotationX;
  private float rotationY;

  private void Update()
  {
    this.rotationX += Input.GetAxis("Mouse X") * this.cameraSensitivity * Time.deltaTime;
    this.rotationY += Input.GetAxis("Mouse Y") * this.cameraSensitivity * Time.deltaTime;
    this.rotationY = Mathf.Clamp(this.rotationY, -90f, 90f);
    this.transform.localRotation = Quaternion.AngleAxis(this.rotationX, Vector3.up);
    this.transform.localRotation *= Quaternion.AngleAxis(this.rotationY, Vector3.left);
    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
    {
      this.transform.position += this.transform.forward * (this.normalMoveSpeed * this.fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
      this.transform.position += this.transform.right * (this.normalMoveSpeed * this.fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
    }
    else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
    {
      this.transform.position += this.transform.forward * (this.normalMoveSpeed * this.slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
      this.transform.position += this.transform.right * (this.normalMoveSpeed * this.slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
    }
    else
    {
      this.transform.position += this.transform.forward * this.normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
      this.transform.position += this.transform.right * this.normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
    }
    if (Input.GetKey(KeyCode.Q))
      this.transform.position += this.transform.up * this.climbSpeed * Time.deltaTime;
    if (!Input.GetKey(KeyCode.E))
      return;
    this.transform.position -= this.transform.up * this.climbSpeed * Time.deltaTime;
  }
}
