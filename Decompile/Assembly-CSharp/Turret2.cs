// Decompiled with JetBrains decompiler
// Type: Turret2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Turret2 : MonoBehaviour
{
  [Header("Rotations")]
  [Tooltip("Transform of the turret's azimuthal rotations.")]
  [SerializeField]
  private Transform turretBase;
  [Tooltip("Transform of the turret's elevation rotations. ")]
  [SerializeField]
  private Transform barrels;
  [Header("Elevation")]
  [Tooltip("Speed at which the turret's guns elevate up and down.")]
  public float ElevationSpeed = 30f;
  [Tooltip("Highest upwards elevation the turret's barrels can aim.")]
  public float MaxElevation = 60f;
  [Tooltip("Lowest downwards elevation the turret's barrels can aim.")]
  public float MaxDepression = 5f;
  [Header("Traverse")]
  [Tooltip("Speed at which the turret can rotate left/right.")]
  public float TraverseSpeed = 60f;
  [Tooltip("When true, the turret can only rotate horizontally with the given limits.")]
  [SerializeField]
  private bool hasLimitedTraverse;
  [Range(0.0f, 179f)]
  public float LeftLimit = 120f;
  [Range(0.0f, 179f)]
  public float RightLimit = 120f;
  [Header("Behavior")]
  [Tooltip("When idle, the turret does not aim at anything and simply points forwards.")]
  public bool IsIdle;
  [Tooltip("Position the turret will aim at when not idle. Set this to whatever you wantthe turret to actively aim at.")]
  public Vector3 AimPosition = Vector3.zero;
  [Tooltip("When the turret is within this many degrees of the target, it is considered aimed.")]
  [SerializeField]
  private float aimedThreshold = 5f;
  private float limitedTraverseAngle;
  [Header("Debug")]
  public bool DrawDebugRay = true;
  public bool DrawDebugArcs;
  private float angleToTarget;
  private float elevation;
  private bool hasBarrels;
  public bool isAimed;
  private bool isBaseAtRest;
  private bool isBarrelAtRest;
  public Vector3 targetPos;
  private float epsilon = 1E-06f;

  public bool HasLimitedTraverse => this.hasLimitedTraverse;

  public bool IsTurretAtRest => this.isBarrelAtRest && this.isBaseAtRest;

  public bool IsAimed => this.isAimed;

  public float AngleToTarget => !this.IsIdle ? this.angleToTarget : 999f;

  private void Awake()
  {
    this.hasBarrels = (Object) this.barrels != (Object) null;
    if (!((Object) this.turretBase == (Object) null))
      return;
    Debug.LogError((object) (this.name + ": TurretAim requires an assigned TurretBase!"));
  }

  private void Start() => this.IsIdle = true;

  private void Update()
  {
    this.isAimed = false;
    if (this.AimPosition != this.targetPos)
      this.AimPosition = this.targetPos;
    if (this.IsIdle)
    {
      if (!this.IsTurretAtRest)
        this.RotateTurretToIdle();
      this.isAimed = false;
    }
    else
    {
      this.RotateBaseToFaceTarget(this.AimPosition);
      if (this.hasBarrels)
        this.RotateBarrelsToFaceTarget(this.AimPosition);
      this.angleToTarget = this.GetTurretAngleToTarget(this.AimPosition);
      this.isAimed = (double) this.angleToTarget < (double) this.aimedThreshold;
      this.isBarrelAtRest = false;
      this.isBaseAtRest = false;
    }
  }

  private float GetTurretAngleToTarget(Vector3 targetPosition)
  {
    float num = 999f;
    float turretAngleToTarget;
    if (this.hasBarrels)
    {
      turretAngleToTarget = Vector3.Angle(targetPosition - this.barrels.position, this.barrels.forward);
    }
    else
    {
      Vector3 to1 = Vector3.ProjectOnPlane(targetPosition - this.turretBase.position, this.turretBase.up);
      Vector3 forward = this.turretBase.forward;
      Vector3 up = this.turretBase.up;
      Vector3 to2 = to1;
      Vector3 axis = up;
      num = Mathf.Abs(Vector3.SignedAngle(forward, to2, axis));
      turretAngleToTarget = Mathf.Abs(Vector3.SignedAngle(this.turretBase.forward, to1, this.turretBase.up));
    }
    return turretAngleToTarget;
  }

  private void RotateTurretToIdle()
  {
    if (this.hasLimitedTraverse)
    {
      this.limitedTraverseAngle = Mathf.MoveTowards(this.limitedTraverseAngle, 0.0f, this.TraverseSpeed * Time.deltaTime);
      if ((double) Mathf.Abs(this.limitedTraverseAngle) > (double) this.epsilon)
        this.turretBase.localEulerAngles = Vector3.up * this.limitedTraverseAngle;
      else
        this.isBaseAtRest = true;
    }
    else
    {
      this.turretBase.rotation = Quaternion.RotateTowards(this.turretBase.rotation, this.transform.rotation, this.TraverseSpeed * Time.deltaTime);
      this.isBaseAtRest = (double) Mathf.Abs(this.turretBase.localEulerAngles.y) < (double) this.epsilon;
    }
    if (this.hasBarrels)
    {
      this.elevation = Mathf.MoveTowards(this.elevation, 0.0f, this.ElevationSpeed * Time.deltaTime);
      if ((double) Mathf.Abs(this.elevation) > (double) this.epsilon)
        this.barrels.localEulerAngles = Vector3.right * -this.elevation;
      else
        this.isBarrelAtRest = true;
    }
    else
      this.isBarrelAtRest = true;
  }

  private void RotateBarrelsToFaceTarget(Vector3 targetPosition)
  {
    Vector3 vector3 = this.turretBase.InverseTransformDirection(targetPosition - this.barrels.position);
    float target = Mathf.Clamp(Vector3.Angle(Vector3.ProjectOnPlane(vector3, Vector3.up), vector3) * Mathf.Sign(vector3.y), -this.MaxDepression, this.MaxElevation);
    this.elevation = (double) Mathf.Abs(target - this.elevation) >= (double) this.aimedThreshold ? Mathf.MoveTowards(this.elevation, target, this.ElevationSpeed * Time.deltaTime) : target;
    if ((double) Mathf.Abs(this.elevation) <= (double) this.epsilon)
      return;
    this.barrels.localEulerAngles = Vector3.right * -this.elevation;
  }

  private void RotateBaseToFaceTarget(Vector3 targetPosition)
  {
    Vector3 up = this.transform.up;
    Vector3 vector3 = Vector3.ProjectOnPlane(targetPosition - this.turretBase.position, up);
    if (this.hasLimitedTraverse)
    {
      float target = Mathf.Clamp(Vector3.SignedAngle(this.transform.forward, vector3, up), -this.LeftLimit, this.RightLimit);
      this.limitedTraverseAngle = (double) Mathf.Abs(target - this.limitedTraverseAngle) >= (double) this.aimedThreshold ? Mathf.MoveTowards(this.limitedTraverseAngle, target, this.TraverseSpeed * Time.deltaTime) : target;
      if ((double) Mathf.Abs(this.limitedTraverseAngle) <= (double) this.epsilon)
        return;
      this.turretBase.localEulerAngles = Vector3.up * this.limitedTraverseAngle;
    }
    else
    {
      if ((double) vector3.sqrMagnitude <= (double) this.epsilon)
        return;
      Quaternion quaternion = Quaternion.LookRotation(vector3, up);
      if ((double) Quaternion.Angle(this.turretBase.rotation, quaternion) < (double) this.aimedThreshold)
        this.turretBase.rotation = quaternion;
      else
        this.turretBase.rotation = Quaternion.RotateTowards(this.turretBase.rotation, quaternion, this.TraverseSpeed * Time.deltaTime);
    }
  }
}
