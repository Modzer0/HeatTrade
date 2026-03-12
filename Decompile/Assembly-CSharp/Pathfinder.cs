// Decompiled with JetBrains decompiler
// Type: Pathfinder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Pathfinder : MonoBehaviour
{
  private ShipController sc;
  [SerializeField]
  private T_Drive drive;
  private bool isMissile;
  public Vector3 targetPos;
  [SerializeField]
  private float maxSpeed;
  public float maxThrust;
  public float currentThrust;
  public float targetThrust;
  [SerializeField]
  private float maxAngularVelocity;
  [SerializeField]
  private float maxRCSThrust;
  [SerializeField]
  private float rcsThrust;
  private float prevVel;
  public float acceleration;
  public DockingPoint dockTarget;
  public Vector3 targetVelocity;
  private Vector3 tgtDir;
  public float tgtDist;
  private Vector3 initPos;
  [SerializeField]
  [Range(-10f, 10f)]
  private float xP;
  [SerializeField]
  [Range(-10f, 10f)]
  private float xI;
  [SerializeField]
  [Range(-10f, 10f)]
  private float xD;
  [SerializeField]
  [Range(-10f, 10f)]
  private float yP;
  [SerializeField]
  [Range(-10f, 10f)]
  private float yI;
  [SerializeField]
  [Range(-10f, 10f)]
  private float yD;
  [SerializeField]
  [Range(-10f, 10f)]
  private float zP;
  [SerializeField]
  [Range(-10f, 10f)]
  private float zI;
  [SerializeField]
  [Range(-10f, 10f)]
  private float zD;
  [SerializeField]
  private float vP;
  [SerializeField]
  private float vI;
  [SerializeField]
  private float vD;
  private PID2 xPID;
  private PID2 yPID;
  private PID2 zPID;
  private PID2 vPID;
  private Rigidbody rb;
  [Space(10f)]
  [Header("CONTROL")]
  public PathfinderState state;
  [SerializeField]
  private int lookAtTarget;
  public List<string> commandList = new List<string>();
  public bool isTurning;
  public bool isRCSOnly;
  public bool isRCSWhenClose = true;
  private bool isBraking;
  [Space(10f)]
  [Header("PARTICLES AND FXA")]
  public ThrustParticles tp;
  [SerializeField]
  private MoveLine ml;
  public bool isShowMoveLine;
  private float velocityChange;
  private Vector3 velocityError;
  private Vector3 velocityCorrection;
  private Vector3 thrustForce;

  private void Start()
  {
    this.sc = this.GetComponent<ShipController>();
    this.rb = this.GetComponent<Rigidbody>();
    this.rb.maxAngularVelocity = this.maxAngularVelocity;
    this.xPID = new PID2(this.xP, this.xI, this.xD);
    this.yPID = new PID2(this.yP, this.yI, this.yD);
    this.zPID = new PID2(this.zP, this.zI, this.zD);
    this.vPID = new PID2(this.vP, this.vI, this.vD);
    this.targetPos = this.transform.position;
    if (!(bool) (UnityEngine.Object) this.tp)
      this.tp = this.GetComponent<ThrustParticles>();
    if (!(bool) (UnityEngine.Object) this.tp)
      this.tp = this.GetComponentsInChildren<ThrustParticles>()[0];
    if ((bool) (UnityEngine.Object) this.GetComponent<Missile2>())
      this.isMissile = true;
    if (!(bool) (UnityEngine.Object) this.drive)
      this.drive = this.GetComponentsInChildren<T_Drive>()[0];
    if (!(bool) (UnityEngine.Object) this.drive)
      Debug.LogError((object) (this.name + " Drive not found!"));
    if (!(bool) (UnityEngine.Object) this.sc)
      return;
    this.sc.OnShipDeath += new Action<int, ShipController, bool>(this.OnShipDeath);
  }

  private void Update()
  {
    if (!this.IsDriveHealthy())
    {
      if (this.state != PathfinderState.IDLE)
        this.state = PathfinderState.IDLE;
      if ((bool) (UnityEngine.Object) this.ml && !this.ml.enabled)
        this.ml.enabled = false;
    }
    this.Particles();
  }

  private void LateUpdate() => this.ShowMoveLine();

  private void FixedUpdate()
  {
    if (!this.IsDriveHealthy())
    {
      this.state = PathfinderState.IDLE;
    }
    else
    {
      if (this.state == PathfinderState.IDLE && (double) this.rb.velocity.magnitude < 0.10000000149011612 && this.commandList.Count == 0)
        return;
      Vector3 velocity = this.rb.velocity;
      this.velocityChange = velocity.magnitude - this.prevVel;
      this.acceleration = this.velocityChange / Time.deltaTime;
      velocity = this.rb.velocity;
      this.prevVel = velocity.magnitude;
      this.tgtDist = Vector3.Distance(this.transform.position, this.targetPos);
      if (this.commandList.Count == 0)
        return;
      string command = this.commandList[0];
      this.tgtDir = this.transform.position - this.targetPos;
      this.tgtDir = -this.tgtDir;
      switch (command)
      {
        case "SetStationary":
          velocity = this.rb.velocity;
          if ((double) velocity.magnitude <= 0.10000000149011612)
          {
            this.commandList.RemoveAt(0);
            this.rb.isKinematic = true;
            this.rb.isKinematic = false;
            if (this.commandList.Count > 0)
            {
              if (this.commandList[0] == "MoveToPosition")
                this.SetupMoveToPosition();
              else if (this.commandList[0] == "Collide")
                this.SetupCollide();
            }
            else
              this.SetToIdle();
            this.rb.velocity = Vector3.zero;
            break;
          }
          this.lookAtTarget = 1;
          this.tgtDir = -this.rb.velocity;
          this.targetVelocity = Vector3.zero;
          this.ReachTargetVelocity();
          break;
        case "MoveToPosition":
          if ((double) this.tgtDist < 2.0)
          {
            this.lookAtTarget = 0;
            velocity = this.rb.velocity;
            if ((double) velocity.magnitude < 0.10000000149011612)
              this.SetToIdle();
          }
          if ((double) Vector3.Distance(this.transform.position, this.initPos) >= (double) this.tgtDist)
          {
            velocity = this.rb.velocity;
            if ((double) velocity.magnitude <= 0.10000000149011612 && (double) this.tgtDist >= 0.5)
              this.TrySetState(PathfinderState.MOVE);
            this.targetVelocity = Vector3.zero;
            if ((double) this.tgtDist >= 0.5)
            {
              if (this.isRCSOnly)
                this.isBraking = true;
              else
                this.lookAtTarget = (double) this.tgtDist >= 2.0 ? -1 : 0;
            }
          }
          this.ReachTargetVelocity();
          break;
        case "Collide":
          this.lookAtTarget = 1;
          this.Collide(this.tgtDir);
          break;
        case "DockApproach":
          MonoBehaviour.print((object) "dock: approach");
          this.targetVelocity = this.tgtDir.normalized * this.maxSpeed;
          this.lookAtTarget = 1;
          if ((double) this.tgtDist < 1.0)
          {
            MonoBehaviour.print((object) "close to target");
            this.lookAtTarget = 0;
            velocity = this.rb.velocity;
            if ((double) velocity.magnitude < 0.10000000149011612)
              this.SetToIdle();
          }
          if ((double) Vector3.Distance(this.transform.position, this.initPos) >= (double) this.tgtDist)
          {
            this.targetVelocity = Vector3.zero;
            velocity = this.rb.velocity;
            if ((double) velocity.magnitude <= 0.10000000149011612 && (double) this.tgtDist >= 0.5)
              this.transform.position = this.targetPos;
            if ((double) this.tgtDist >= 0.5)
              this.isBraking = true;
          }
          this.ReachTargetVelocity();
          break;
        case "DockLock":
          MonoBehaviour.print((object) "dock: lock");
          this.targetVelocity = this.tgtDir.normalized * this.maxSpeed;
          this.lookAtTarget = 1;
          if ((double) this.tgtDist < 0.10000000149011612)
          {
            this.transform.LookAt(this.dockTarget.transform.position);
            this.transform.position = Vector3.Lerp(this.transform.position, this.targetPos, 10f * Time.deltaTime);
            velocity = this.rb.velocity;
            if ((double) velocity.magnitude < 0.10000000149011612)
              this.SetToIdle();
          }
          if ((double) Vector3.Distance(this.transform.position, this.initPos) >= (double) this.tgtDist)
          {
            this.targetVelocity = Vector3.zero;
            velocity = this.rb.velocity;
            if ((double) velocity.magnitude <= 0.10000000149011612 && (double) this.tgtDist >= 0.10000000149011612)
              this.transform.position = this.targetPos;
            if ((double) this.tgtDist >= 0.5)
              this.isBraking = true;
          }
          this.ReachTargetVelocity();
          break;
        case "FaceTarget":
          this.lookAtTarget = 1;
          break;
      }
      this.UpdateRotation(this.tgtDir * (float) this.lookAtTarget);
    }
  }

  public void InitThrust(float newMaxThrust)
  {
    this.maxThrust = newMaxThrust;
    this.targetThrust = newMaxThrust;
    this.rcsThrust = this.targetThrust;
    this.maxAngularVelocity = this.rcsThrust;
  }

  private void SetNewThrust(float newThrust)
  {
    this.targetThrust = newThrust;
    this.rcsThrust = this.targetThrust * 0.1f;
    this.maxAngularVelocity = this.rcsThrust;
  }

  public bool IsDriveHealthy()
  {
    return this.isMissile || (bool) (UnityEngine.Object) this.drive && (double) this.drive.health > 0.0 && (!(bool) (UnityEngine.Object) this.sc || !this.sc.isDead);
  }

  [ContextMenu("Set State: MOVE")]
  private void SetStateMove() => this.TrySetState(PathfinderState.MOVE);

  public void TrySetStateBrake()
  {
    this.commandList.Clear();
    this.commandList.Add("SetStationary");
    this.commandList.Add("FaceTarget");
  }

  public void TrySetState(PathfinderState newState)
  {
    if (!this.IsDriveHealthy())
    {
      this.state = PathfinderState.IDLE;
    }
    else
    {
      this.SetNewThrust(this.maxThrust * this.drive.healthRatio);
      this.tp.exhaustRatio = this.drive.healthRatio;
      this.state = newState;
      this.tgtDist = Vector3.Distance(this.transform.position, this.targetPos);
      this.isBraking = false;
      this.commandList.Clear();
      if (this.state == PathfinderState.MOVE)
      {
        if ((double) this.tgtDist < 0.5)
          return;
        this.commandList.Add("SetStationary");
        this.commandList.Add("MoveToPosition");
      }
      else if (this.state == PathfinderState.COLLIDE)
      {
        this.commandList.Add("SetStationary");
        this.commandList.Add("Collide");
      }
      else if (this.state == PathfinderState.DOCK)
      {
        if ((UnityEngine.Object) this.dockTarget == (UnityEngine.Object) null || !(bool) (UnityEngine.Object) this.dockTarget.GetComponent<DockingPoint>())
          return;
        this.commandList.Add("SetStationary");
        this.commandList.Add("DockApproach");
        this.SetupDockApproach();
        this.commandList.Add("DockLock");
      }
      else
      {
        if (this.state != PathfinderState.FACE)
          return;
        this.commandList.Add("FaceTarget");
      }
    }
  }

  private void SetupMoveToPosition()
  {
    this.initPos = this.transform.position;
    this.lookAtTarget = 1;
    this.targetVelocity = this.tgtDir.normalized * this.maxSpeed;
    this.SetThrust(this.targetThrust);
    if ((double) this.tgtDist < 100.0)
      this.isRCSOnly = true;
    else
      this.isRCSOnly = false;
  }

  private void SetupCollide()
  {
    this.lookAtTarget = 1;
    this.targetVelocity = this.tgtDir.normalized * this.maxSpeed;
    this.SetThrust(this.targetThrust);
  }

  private void SetupDockApproach()
  {
    this.targetPos = this.dockTarget.approachPoint.position;
    this.initPos = this.transform.position;
    this.lookAtTarget = 1;
    this.targetVelocity = this.tgtDir.normalized * this.maxSpeed;
    this.isRCSOnly = true;
    this.SetThrust(this.targetThrust);
  }

  private void SetupDockLock()
  {
    this.targetPos = this.dockTarget.lockPoint.position;
    this.initPos = this.transform.position;
    this.lookAtTarget = 1;
    this.targetVelocity = this.tgtDir.normalized * this.maxSpeed;
    this.isRCSOnly = true;
    this.SetThrust(this.targetThrust);
  }

  private void SetThrust(float newThrust)
  {
    this.targetThrust = newThrust;
    if ((double) this.targetThrust <= (double) this.maxThrust)
      return;
    this.targetThrust = this.maxThrust;
  }

  private void SetToIdle()
  {
    if (this.commandList.Count > 0)
      this.commandList.RemoveAt(0);
    this.rb.isKinematic = true;
    this.rb.isKinematic = false;
    this.tgtDist = 10000f;
    this.tp.Clear();
    this.isBraking = false;
    this.targetThrust = 0.0f;
    this.state = PathfinderState.IDLE;
  }

  private void Particles()
  {
    if (this.state == PathfinderState.IDLE)
    {
      this.tp.SetMain(false);
      this.tp.SetRCS(0.0f);
    }
    else
    {
      if (!(bool) (UnityEngine.Object) this.tp || this.commandList.Count == 0)
        return;
      if (this.state == PathfinderState.COLLIDE && (double) this.rb.velocity.magnitude > 0.10000000149011612)
        this.tp.SetMain(true);
      if (this.isRCSOnly)
      {
        if ((double) this.rb.velocity.magnitude > 0.10000000149011612)
        {
          if (this.isBraking)
            this.tp.SetRCS(-1f);
          else
            this.tp.SetRCS(1f);
        }
        else
          this.tp.SetRCS(0.0f);
        if (!this.isTurning)
          return;
        this.tp.SetRCS(0.0f);
      }
      else
      {
        if (this.isTurning)
          this.tp.SetMain(false);
        else if (this.state == PathfinderState.FACE)
          this.tp.SetMain(false);
        else if ((double) this.rb.velocity.magnitude > 0.10000000149011612)
          this.tp.SetMain(true);
        else
          this.tp.SetMain(false);
        this.tp.SetRCS(0.0f);
      }
    }
  }

  private void UpdateRotation(Vector3 tgtDir)
  {
    if (this.lookAtTarget == 0)
    {
      this.isTurning = false;
    }
    else
    {
      if (tgtDir == Vector3.zero)
        return;
      Quaternion quaternion = Quaternion.LookRotation(tgtDir);
      Vector3 vector3 = (Quaternion.Inverse(this.transform.rotation) * quaternion).eulerAngles;
      vector3 = new Vector3(Mathf.DeltaAngle(0.0f, vector3.x), Mathf.DeltaAngle(0.0f, vector3.y), Mathf.DeltaAngle(0.0f, vector3.z));
      float output1 = this.xPID.GetOutput(vector3.x, Time.fixedDeltaTime);
      float output2 = this.yPID.GetOutput(vector3.y, Time.fixedDeltaTime);
      float output3 = this.zPID.GetOutput(vector3.z, Time.fixedDeltaTime);
      this.rb.AddRelativeTorque(output1 * Vector3.right);
      this.rb.AddRelativeTorque(output2 * Vector3.up);
      this.rb.AddRelativeTorque(output3 * Vector3.forward);
      this.isTurning = (double) new Vector3(output1, output2, output3).magnitude >= 10.0;
      if (!(bool) (UnityEngine.Object) this.tp)
        return;
      if ((double) output1 > 0.10000000149011612)
        this.tp.Pitch(1);
      else if ((double) output1 < -0.10000000149011612)
        this.tp.Pitch(-1);
      else
        this.tp.Pitch(0);
      if ((double) output2 > 0.10000000149011612)
        this.tp.Yaw(1);
      else if ((double) output2 < -0.10000000149011612)
        this.tp.Yaw(-1);
      else
        this.tp.Yaw(0);
    }
  }

  private void ReachTargetVelocity()
  {
    this.currentThrust = this.targetThrust;
    if (this.isRCSOnly)
      this.currentThrust = this.rcsThrust;
    this.vPID = new PID2(this.vP, this.vI, this.vD);
    Vector3 vector3 = this.targetVelocity - this.rb.velocity;
    this.rb.AddForce(new Vector3(this.vPID.GetOutput(vector3.x, Time.fixedDeltaTime), this.vPID.GetOutput(vector3.y, Time.fixedDeltaTime), this.vPID.GetOutput(vector3.z, Time.fixedDeltaTime)).normalized * this.currentThrust, ForceMode.Force);
  }

  private void Collide(Vector3 tgtDir)
  {
    this.vPID = new PID2(this.vP, this.vI, this.vD);
    this.targetVelocity = tgtDir.normalized * this.maxSpeed;
    this.velocityError = this.targetVelocity - this.rb.velocity;
    this.velocityCorrection = this.vPID.GetOutputFull(this.velocityError, Time.fixedDeltaTime);
    this.thrustForce = this.velocityCorrection.normalized * this.targetThrust;
    this.rb.AddForce(this.thrustForce, ForceMode.Force);
  }

  private void ShowMoveLine()
  {
    if ((bool) (UnityEngine.Object) this.ml && (double) this.rb.velocity.magnitude >= 0.10000000149011612)
    {
      Vector3 targetPos = this.targetPos;
      if (this.isShowMoveLine)
      {
        if (this.isShowMoveLine || this.state != PathfinderState.IDLE)
        {
          this.ml.UpdateLine(this.targetPos);
          return;
        }
        if (!(bool) (UnityEngine.Object) this.ml)
          return;
        this.ml.Off();
        return;
      }
    }
    if (!(bool) (UnityEngine.Object) this.ml)
      return;
    this.ml.Off();
  }

  private void OnTriggerEnter(Collider other)
  {
  }

  public void EntryBurn()
  {
    if (!(bool) (UnityEngine.Object) this.tp)
      this.tp = this.GetComponentInChildren<ThrustParticles>();
    this.tp.EntryBurn();
  }

  private void OnShipDeath(int factionID, ShipController sc, bool isHardDeath)
  {
    this.tp.OnShipDeath();
  }

  private void OnDestroy()
  {
    if (!(bool) (UnityEngine.Object) this.sc)
      return;
    this.sc.OnShipDeath -= new Action<int, ShipController, bool>(this.OnShipDeath);
  }
}
