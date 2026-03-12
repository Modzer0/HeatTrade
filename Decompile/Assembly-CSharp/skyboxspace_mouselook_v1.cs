// Decompiled with JetBrains decompiler
// Type: skyboxspace_mouselook_v1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class skyboxspace_mouselook_v1 : MonoBehaviour
{
  private Vector2 _mouseAbsolute;
  private Vector2 _smoothMouse;
  public Vector2 clampInDegrees = new Vector2(360f, 180f);
  public Vector2 sensitivity = new Vector2(2f, 2f);
  public Vector2 smoothing = new Vector2(3f, 3f);
  public Vector2 targetDirection;
  public Vector2 targetCharacterDirection;
  public GameObject characterBody;

  private void Start()
  {
    this.targetDirection = (Vector2) this.transform.localRotation.eulerAngles;
    if (!(bool) (Object) this.characterBody)
      return;
    this.targetCharacterDirection = (Vector2) this.characterBody.transform.localRotation.eulerAngles;
  }

  private void Update()
  {
    Quaternion quaternion1 = Quaternion.Euler((Vector3) this.targetDirection);
    Quaternion quaternion2 = Quaternion.Euler((Vector3) this.targetCharacterDirection);
    Vector2 vector2 = Vector2.Scale(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")), new Vector2(this.sensitivity.x * this.smoothing.x, this.sensitivity.y * this.smoothing.y));
    this._smoothMouse.x = Mathf.Lerp(this._smoothMouse.x, vector2.x, 1f / this.smoothing.x);
    this._smoothMouse.y = Mathf.Lerp(this._smoothMouse.y, vector2.y, 1f / this.smoothing.y);
    this._mouseAbsolute += this._smoothMouse;
    if ((double) this.clampInDegrees.x < 360.0)
      this._mouseAbsolute.x = Mathf.Clamp(this._mouseAbsolute.x, (float) (-(double) this.clampInDegrees.x * 0.5), this.clampInDegrees.x * 0.5f);
    this.transform.localRotation = Quaternion.AngleAxis(-this._mouseAbsolute.y, quaternion1 * Vector3.right);
    if ((double) this.clampInDegrees.y < 360.0)
      this._mouseAbsolute.y = Mathf.Clamp(this._mouseAbsolute.y, (float) (-(double) this.clampInDegrees.y * 0.5), this.clampInDegrees.y * 0.5f);
    this.transform.localRotation *= quaternion1;
    if ((bool) (Object) this.characterBody)
    {
      this.characterBody.transform.localRotation = Quaternion.AngleAxis(this._mouseAbsolute.x, this.characterBody.transform.up);
      this.characterBody.transform.localRotation *= quaternion2;
    }
    else
      this.transform.localRotation *= Quaternion.AngleAxis(this._mouseAbsolute.x, this.transform.InverseTransformDirection(Vector3.up));
  }
}
