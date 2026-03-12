// Decompiled with JetBrains decompiler
// Type: TacticalCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class TacticalCamera : MonoBehaviour
{
  public static TacticalCamera current;
  private TimeController tc;
  private AudioManager am;
  [SerializeField]
  private TMP_Text fovText;
  public Camera cam;
  public Camera overlayCamMain;
  public Camera overlayCamBodies;
  public Transform camTransform;
  public Transform camTarget;
  private Rigidbody camRB;
  private float pitch;
  private float yaw;
  private Vector3 lastPos;
  private float lastPitch;
  private float lastYaw;
  private Vector3 mapPos;
  private bool isCamInMapPos;
  [SerializeField]
  private LayerMask tacticalMask;
  [SerializeField]
  private LayerMask bodiesMask;
  [SerializeField]
  private LayerMask mapMask;
  [SerializeField]
  private Transform swivelPoint;
  [SerializeField]
  private Camera scopeCam;
  public Transform target;
  public Transform tempTarget;
  public Quaternion targetRot;
  [SerializeField]
  private int farClip = 30000;
  [SerializeField]
  private int mapFarClip = 30000;
  [SerializeField]
  private float moveMagnitude;
  private float shiftHoldTime = 1f;
  private bool isAccelerating;
  private float accelerationRate = 10f;
  public TacticalCamera.CameraState state;
  private Transform focusOn;
  private Vector3 focusOffset;
  public int zoomSens = 30;
  private int rotationSpeed = 15;
  private int defaultFOV = 90;
  private int targetFOV;
  private int targetMapFOV;
  public bool isDashing;
  public bool isMapDashing;
  private Vector3 followOffset;
  private Vector3 dashTargetPos;
  [SerializeField]
  private float dashSpeed = 20f;
  private float tempDashSpeed = 20f;
  private bool isDashLocal;
  private float dashTime;
  private float dashTimeMax = 1f;
  private LineRenderer lr;
  [Header("CINEMATIC MODE")]
  [SerializeField]
  private TurretWeapon pdFocus;
  [SerializeField]
  private bool isCinematicModeOn;
  private float timeMultiplier = 1f;
  private bool isInputPaused;
  public float mouseSensMult = 1f;

  private void Awake() => TacticalCamera.current = this;

  private void Start()
  {
    this.tc = TimeController.current;
    this.am = AudioManager.current;
    this.camRB = this.camTarget.GetComponent<Rigidbody>();
    this.lr = this.GetComponent<LineRenderer>();
    this.camTransform.position = this.camTarget.position;
    this.targetFOV = (int) this.overlayCamMain.fieldOfView;
    this.targetMapFOV = (int) this.overlayCamMain.fieldOfView;
    this.pitch = 90f;
    this.camTransform.rotation = Quaternion.Euler(this.pitch, 0.0f, 0.0f);
  }

  private void Update()
  {
    this.timeMultiplier = this.tc.GetTimeMultiplier();
    if (this.isCinematicModeOn && this.state != TacticalCamera.CameraState.FOCUS && (Object) this.pdFocus.target != (Object) null)
      this.SetFocus(this.pdFocus.target.transform, 2);
    if (this.state == TacticalCamera.CameraState.NULL)
      this.UpdateNull();
    else if (this.state == TacticalCamera.CameraState.FOCUS)
      this.UpdateFocus();
    else if (this.state == TacticalCamera.CameraState.FOLLOW)
      this.UpdateFollow();
    else if (this.state == TacticalCamera.CameraState.MAP)
      this.UpdateMap();
    float num = Mathf.Lerp(this.overlayCamMain.fieldOfView, this.state == TacticalCamera.CameraState.MAP ? (float) this.targetMapFOV : (float) this.targetFOV, 10f * Time.deltaTime * this.timeMultiplier);
    this.cam.fieldOfView = num;
    this.overlayCamMain.fieldOfView = num;
    if ((bool) (Object) this.overlayCamBodies)
      this.overlayCamBodies.fieldOfView = num;
    if ((bool) (Object) this.fovText)
      this.fovText.text = "FOV: " + Mathf.RoundToInt(this.overlayCamMain.fieldOfView).ToString();
    if (this.state != TacticalCamera.CameraState.MAP)
    {
      if (!this.isMapDashing && (Object) this.camTarget != (Object) null)
        this.camTransform.position = this.camTarget.position;
    }
    else if (!this.isCamInMapPos)
    {
      if ((double) Vector3.Distance(this.camTransform.position, this.mapPos) < 1.0)
      {
        this.camTransform.parent = this.swivelPoint;
        this.camTransform.localEulerAngles = Vector3.zero;
        this.isCamInMapPos = true;
      }
      else
        this.camTransform.position = Vector3.Lerp(this.camTransform.position, this.mapPos, 25f * this.timeMultiplier * Time.deltaTime);
    }
    if (!this.isDashing)
      return;
    this.UpdateDash();
  }

  private void UpdateRotation()
  {
    this.targetRot = Quaternion.Euler(this.pitch, this.yaw, 0.0f);
    this.camTransform.rotation = Quaternion.Slerp(this.camTransform.rotation, this.targetRot, (float) this.rotationSpeed * Time.deltaTime * this.timeMultiplier);
  }

  private void UpdateNull() => this.UpdateRotation();

  private void UpdateFocus()
  {
    if ((Object) this.focusOn == (Object) null)
    {
      this.SetFocus((Transform) null, 0);
    }
    else
    {
      this.camTransform.rotation = Quaternion.Slerp(this.camTransform.rotation, Quaternion.LookRotation(this.focusOn.position - this.camTransform.position), (float) this.rotationSpeed / 5f * Time.deltaTime * this.timeMultiplier);
      Vector3 eulerAngles = this.camTransform.rotation.eulerAngles;
      this.yaw = eulerAngles.y;
      this.pitch = (double) eulerAngles.x > 180.0 ? eulerAngles.x - 360f : eulerAngles.x;
      this.pitch = Mathf.Clamp(this.pitch, -90f, 90f);
    }
  }

  private void UpdateFollow()
  {
    if ((Object) this.focusOn == (Object) null)
    {
      this.SetFocus((Transform) null, 0);
    }
    else
    {
      if (!this.isDashing && (Object) this.camTarget != (Object) null)
        this.camTarget.position = this.focusOn.position + this.focusOffset;
      if (this.isInputPaused)
        return;
      this.UpdateRotation();
    }
  }

  private void UpdateMap()
  {
    this.overlayCamMain.fieldOfView = Mathf.Lerp(this.overlayCamMain.fieldOfView, (float) this.targetMapFOV, 10f * Time.deltaTime * this.timeMultiplier);
    if (!((Object) this.camTarget != (Object) null))
      return;
    this.swivelPoint.localRotation = Quaternion.Euler(this.pitch, this.yaw, 0.0f);
  }

  private void UpdateDash()
  {
    if ((Object) this.camTarget == (Object) null || (Object) this.focusOn == (Object) null)
    {
      this.isDashing = false;
    }
    else
    {
      this.dashTargetPos = this.focusOn.position - this.camTransform.forward * (float) this.focusOn.GetComponent<Track>().trackRadius;
      float num = Vector3.Distance(this.camTarget.position, this.dashTargetPos);
      this.camTarget.position = Vector3.Lerp(this.camTarget.position, this.dashTargetPos, Mathf.Max(this.dashSpeed, num / 250f) * Time.deltaTime * this.timeMultiplier);
      if ((double) num < 2.0)
      {
        this.isDashing = false;
        if ((Object) this.focusOn != (Object) null)
          this.focusOffset = this.camTarget.position - this.focusOn.position;
      }
      this.dashTime += Time.deltaTime * this.timeMultiplier;
      if ((double) this.dashTime <= (double) this.dashTimeMax)
        return;
      this.isDashing = false;
      if (!((Object) this.focusOn != (Object) null))
        return;
      this.focusOffset = this.camTarget.position - this.focusOn.position;
    }
  }

  public void ClearTarget() => this.target = (Transform) null;

  public void Zoom(int dir)
  {
    float fieldOfView = this.overlayCamMain.fieldOfView;
    if (this.state == TacticalCamera.CameraState.MAP)
      this.targetMapFOV = dir > 0 ? (int) Mathf.Max(10f, fieldOfView - (float) this.zoomSens) : (int) Mathf.Min(120f, fieldOfView + (float) this.zoomSens);
    else
      this.targetFOV = dir > 0 ? (int) Mathf.Max(10f, fieldOfView - (float) this.zoomSens) : (int) Mathf.Min(120f, fieldOfView + (float) this.zoomSens);
    this.am.PlayZoom(dir);
  }

  public void Move(Vector3 dir, bool isShift)
  {
    Vector3 vector3 = this.camTransform.TransformDirection(dir);
    this.shiftHoldTime += Time.deltaTime * 2f;
    if (isShift)
      vector3 *= this.accelerationRate;
    float num = this.moveMagnitude * this.shiftHoldTime;
    if ((double) Mathf.Round(this.tc.timeScale) != 1.0)
    {
      if (this.state == TacticalCamera.CameraState.FOLLOW)
        this.focusOffset += 3f * num * Time.deltaTime * this.timeMultiplier * vector3;
      else
        this.camTarget.position += 3f * num * Time.deltaTime * this.timeMultiplier * vector3;
    }
    else if (this.state == TacticalCamera.CameraState.NULL)
      this.camRB.AddForce(num * this.timeMultiplier * vector3, ForceMode.VelocityChange);
    else if (this.state == TacticalCamera.CameraState.FOCUS)
    {
      this.camRB.AddForce(num * this.timeMultiplier * vector3, ForceMode.VelocityChange);
    }
    else
    {
      if (this.state != TacticalCamera.CameraState.FOLLOW)
        return;
      this.focusOffset += 3f * num * Time.deltaTime * this.timeMultiplier * vector3;
    }
  }

  public void ResetShiftHoldTime() => this.shiftHoldTime = 1f;

  public void MapMode(bool isOn)
  {
    if (this.isMapDashing)
      return;
    if (isOn)
    {
      this.state = TacticalCamera.CameraState.MAP;
      this.lastPos = this.camTransform.position;
      this.lastPitch = this.pitch;
      this.lastYaw = this.yaw;
      this.mapPos = this.camTransform.position + -this.camTransform.forward.normalized * 20000f;
      this.mapPos = Vector3.ClampMagnitude(this.mapPos, 4990f);
      this.isCamInMapPos = false;
      this.overlayCamMain.cullingMask = (int) this.mapMask;
      this.cam.clearFlags = CameraClearFlags.Color;
      this.cam.backgroundColor = Color.black;
      this.overlayCamBodies.cullingMask = (int) this.mapMask;
      this.isMapDashing = false;
    }
    else
    {
      this.state = TacticalCamera.CameraState.NULL;
      this.camTransform.parent = (Transform) null;
      this.camTransform.position = this.camTransform.position;
      this.pitch = this.lastPitch;
      this.yaw = this.lastYaw;
      this.overlayCamMain.cullingMask = (int) this.tacticalMask;
      this.cam.clearFlags = CameraClearFlags.Skybox;
      this.cam.backgroundColor = Color.white;
      this.overlayCamBodies.cullingMask = (int) this.bodiesMask;
      this.isMapDashing = false;
    }
  }

  private void FocusMove(Vector3 targetPos)
  {
    float trackRadius = (float) this.focusOn.GetComponent<Track>().trackRadius;
    Vector3 normalized = (targetPos - this.camTransform.position).normalized;
    this.dashTargetPos = targetPos - normalized * trackRadius;
    this.isDashing = true;
  }

  private void FocusMoveRotate()
  {
    if (!(bool) (Object) this.focusOn)
      return;
    Vector3 eulerAngles = this.camTransform.rotation.eulerAngles;
    this.yaw = eulerAngles.y;
    this.pitch = eulerAngles.x;
    if ((double) this.pitch > 180.0)
      this.pitch -= 360f;
    this.pitch = Mathf.Clamp(this.pitch, -90f, 90f);
  }

  public void SetFocus(Transform newFocus, int stateIndex)
  {
    if (this.isDashing)
      return;
    if (stateIndex == 0 || (Object) newFocus == (Object) null)
    {
      this.state = TacticalCamera.CameraState.NULL;
      this.camRB.isKinematic = false;
      this.focusOn = (Transform) null;
    }
    else
    {
      switch (stateIndex)
      {
        case 1:
          this.state = TacticalCamera.CameraState.FOCUS;
          this.focusOn = newFocus;
          break;
        case 2:
          this.state = TacticalCamera.CameraState.FOLLOW;
          this.focusOn = newFocus;
          Vector3 position = this.focusOn.position;
          if ((bool) (Object) this.focusOn.GetComponent<Rigidbody>() && (double) this.focusOn.GetComponent<Rigidbody>().velocity.magnitude > 0.10000000149011612)
          {
            Vector3 vector3 = position + this.focusOn.GetComponent<Rigidbody>().velocity;
          }
          this.FocusMove2();
          break;
      }
    }
  }

  private void FocusMove2()
  {
    if (!(bool) (Object) this.focusOn)
      return;
    this.dashTargetPos = this.focusOn.position - this.camTransform.forward * (float) this.focusOn.GetComponent<Track>().trackRadius;
    this.isDashing = true;
    this.dashTime = 0.0f;
  }

  public void Rotate(float mouseX, float mouseY)
  {
    if (this.isInputPaused)
      return;
    if (this.state == TacticalCamera.CameraState.FOCUS)
      this.SetFocus((Transform) null, 0);
    mouseX *= this.mouseSensMult;
    mouseY *= this.mouseSensMult;
    float num = (float) this.targetFOV / (float) this.defaultFOV;
    mouseX *= num;
    mouseY *= num;
    mouseX = Mathf.Clamp(mouseX * this.timeMultiplier, -30f, 30f);
    mouseY = Mathf.Clamp(mouseY * this.timeMultiplier, -30f, 30f);
    this.yaw += mouseX;
    this.pitch -= mouseY;
    this.pitch = Mathf.Clamp(this.pitch, -90f, 90f);
  }

  public enum CameraState
  {
    NULL,
    FOCUS,
    FOLLOW,
    MAP,
  }
}
