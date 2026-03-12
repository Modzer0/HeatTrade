// Decompiled with JetBrains decompiler
// Type: UnityTemplateProjects.SimpleCameraController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace UnityTemplateProjects;

public class SimpleCameraController : MonoBehaviour
{
  private SimpleCameraController.CameraState m_TargetCameraState = new SimpleCameraController.CameraState();
  private SimpleCameraController.CameraState m_InterpolatingCameraState = new SimpleCameraController.CameraState();
  [Header("Movement Settings")]
  [Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
  public float boost = 3.5f;
  [Tooltip("Time it takes to interpolate camera position 99% of the way to the target.")]
  [Range(0.001f, 1f)]
  public float positionLerpTime = 0.2f;
  [Header("Rotation Settings")]
  [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
  public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.5f, 0.0f, 5f),
    new Keyframe(1f, 2.5f, 0.0f, 0.0f)
  });
  [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target.")]
  [Range(0.001f, 1f)]
  public float rotationLerpTime = 0.01f;
  [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
  public bool invertY;

  private void OnEnable()
  {
    this.m_TargetCameraState.SetFromTransform(this.transform);
    this.m_InterpolatingCameraState.SetFromTransform(this.transform);
  }

  private Vector3 GetInputTranslationDirection()
  {
    Vector3 translationDirection = new Vector3();
    if (Input.GetKey(KeyCode.W))
      translationDirection += Vector3.forward;
    if (Input.GetKey(KeyCode.S))
      translationDirection += Vector3.back;
    if (Input.GetKey(KeyCode.A))
      translationDirection += Vector3.left;
    if (Input.GetKey(KeyCode.D))
      translationDirection += Vector3.right;
    if (Input.GetKey(KeyCode.Q))
      translationDirection += Vector3.down;
    if (Input.GetKey(KeyCode.E))
      translationDirection += Vector3.up;
    return translationDirection;
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(1))
      Cursor.lockState = CursorLockMode.Locked;
    if (Input.GetMouseButtonUp(1))
    {
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
    }
    if (Input.GetMouseButton(1))
    {
      Vector2 vector2 = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (this.invertY ? 1f : -1f));
      float num = this.mouseSensitivityCurve.Evaluate(vector2.magnitude);
      this.m_TargetCameraState.yaw += vector2.x * num;
      this.m_TargetCameraState.pitch += vector2.y * num;
    }
    Vector3 vector3 = this.GetInputTranslationDirection() * Time.deltaTime;
    if (Input.GetKey(KeyCode.LeftShift))
      vector3 *= 10f;
    this.boost += Input.mouseScrollDelta.y * 0.2f;
    this.m_TargetCameraState.Translate(vector3 * Mathf.Pow(2f, this.boost));
    this.m_InterpolatingCameraState.LerpTowards(this.m_TargetCameraState, 1f - Mathf.Exp(Mathf.Log(0.00999999f) / this.positionLerpTime * Time.deltaTime), 1f - Mathf.Exp(Mathf.Log(0.00999999f) / this.rotationLerpTime * Time.deltaTime));
    this.m_InterpolatingCameraState.UpdateTransform(this.transform);
  }

  private class CameraState
  {
    public float yaw;
    public float pitch;
    public float roll;
    public float x;
    public float y;
    public float z;

    public void SetFromTransform(Transform t)
    {
      this.pitch = t.eulerAngles.x;
      this.yaw = t.eulerAngles.y;
      this.roll = t.eulerAngles.z;
      this.x = t.position.x;
      this.y = t.position.y;
      this.z = t.position.z;
    }

    public void Translate(Vector3 translation)
    {
      Vector3 vector3 = Quaternion.Euler(this.pitch, this.yaw, this.roll) * translation;
      this.x += vector3.x;
      this.y += vector3.y;
      this.z += vector3.z;
    }

    public void LerpTowards(
      SimpleCameraController.CameraState target,
      float positionLerpPct,
      float rotationLerpPct)
    {
      this.yaw = Mathf.Lerp(this.yaw, target.yaw, rotationLerpPct);
      this.pitch = Mathf.Lerp(this.pitch, target.pitch, rotationLerpPct);
      this.roll = Mathf.Lerp(this.roll, target.roll, rotationLerpPct);
      this.x = Mathf.Lerp(this.x, target.x, positionLerpPct);
      this.y = Mathf.Lerp(this.y, target.y, positionLerpPct);
      this.z = Mathf.Lerp(this.z, target.z, positionLerpPct);
    }

    public void UpdateTransform(Transform t)
    {
      t.eulerAngles = new Vector3(this.pitch, this.yaw, this.roll);
      t.position = new Vector3(this.x, this.y, this.z);
    }
  }
}
