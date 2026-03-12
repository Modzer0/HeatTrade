// Decompiled with JetBrains decompiler
// Type: CameraController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CameraController : MonoBehaviour
{
  public bool LockMouse;
  [Range(0.0f, 50f)]
  public float Sensitivity = 20f;
  [Range(0.0f, 100f)]
  public float Speed = 20f;
  private float AdvanceSpeed = 1f;
  private float axisX;
  private float axisY;
  private float lastAxisX;
  private float lasAxisY;

  private void LateUpdate()
  {
    if (Input.GetMouseButtonDown(0))
    {
      this.LockMouse = !this.LockMouse;
      if (!this.LockMouse)
      {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
      }
      else
      {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
      }
    }
    if (Input.GetKeyDown(KeyCode.Escape))
      Application.Quit();
    this.CameraMovement();
  }

  private void CameraMovement()
  {
    if (this.LockMouse)
    {
      this.axisX = Input.GetAxis("Mouse X") + this.lastAxisX * 0.9f;
      this.axisY = (float) (-(double) Input.GetAxis("Mouse Y") + (double) this.lasAxisY * 0.89999997615814209);
      this.lastAxisX = this.axisX;
      this.lasAxisY = this.axisY;
      this.transform.Rotate(this.Sensitivity * this.axisY * Time.deltaTime, this.Sensitivity * this.axisX * Time.deltaTime, 0.0f);
    }
    this.AdvanceSpeed = !Input.GetKey(KeyCode.Space) ? 1f : 2f;
    this.transform.Translate(this.AdvanceSpeed * this.Speed * Input.GetAxis("Horizontal") * Time.deltaTime, 0.0f, this.AdvanceSpeed * this.Speed * Input.GetAxis("Vertical") * Time.deltaTime);
    this.transform.eulerAngles = this.transform.eulerAngles with
    {
      z = 0.0f
    };
  }
}
