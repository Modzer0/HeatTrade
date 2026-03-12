// Decompiled with JetBrains decompiler
// Type: TurretWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TurretWeapon : T_Mount
{
  [Header("TURRET WEAPON")]
  public Rigidbody parentRB;
  public Pathfinder parentPF;
  [Space(10f)]
  [Header("TARGET")]
  public Track target;
  public Vector3 currentTargetPos;
  public Transform transformTarget;
  public Rigidbody targetRB;
  public ShipController targetShip;
  public bool isTargetInRange;
  public Vector3 targetVelocity;
  public Vector3 targetPrevVelocity;
  public Vector3 targetAcceleration;
  [Space(10f)]
  [Header("STATS")]
  [SerializeField]
  private bool useDotProduct;
  public LayerMask targetMask;
  public int range;
  public float accError;
  public bool hasInfiniteAmmo;
  public WeaponType weaponType;
  [SerializeField]
  private bool isHeatImpulse;
  [Space(10f)]
  [Header("PARTS")]
  public Transform eye;
  public Transform muzzle;
  public Turret2 turret;
  [Space(10f)]
  [Header("FIRE ADJUST")]
  private float fireAdjustRate = 5f;
  private float nextAdjustUpdate;

  public override void Start()
  {
    base.Start();
    this.parentPF = this.parentRB.GetComponent<Pathfinder>();
    this.turret = this.GetComponent<Turret2>();
  }

  public override void Update()
  {
    base.Update();
    if (!this.isOn || (double) this.nextAdjustUpdate >= (double) Time.time || !((Object) this.targetShip != (Object) null) || !((Object) this.transformTarget != (Object) null))
      return;
    this.AdjustTarget();
  }

  private void FixedUpdate() => this.UpdateTargetData();

  public void ApplyPowerAndHeat(float percentage)
  {
    if ((double) percentage > 0.0)
    {
      if (this.isHeatImpulse)
        this.currentHeat = this.heat / Time.deltaTime * percentage;
      else
        this.currentHeat = this.heat * percentage;
    }
    else
      this.currentHeat = 0.0f;
  }

  public void UpdateTargetData()
  {
    if ((Object) this.target == (Object) null)
    {
      this.NoTarget();
    }
    else
    {
      this.currentTargetPos = !(bool) (Object) this.targetShip || !(bool) (Object) this.transformTarget ? this.target.transform.position : this.transformTarget.position;
      if (!((Object) this.targetRB != (Object) null))
        return;
      this.targetVelocity = this.targetRB.velocity;
      this.targetAcceleration = (this.targetVelocity - this.targetPrevVelocity) / Time.fixedDeltaTime;
      this.targetPrevVelocity = this.targetVelocity;
    }
  }

  private void AdjustTarget()
  {
    if (this.targetShip.isDead)
    {
      this.SetTarget((Track) null);
    }
    else
    {
      this.transformTarget = this.targetShip.GetModuleRandom();
      this.nextAdjustUpdate = Time.time + this.fireAdjustRate;
    }
  }

  public bool CanSee(Track newTarget)
  {
    if ((Object) newTarget == (Object) null)
    {
      this.NoTarget();
      return false;
    }
    bool flag = false;
    Color color = Color.yellow;
    if (this.useDotProduct && (double) Vector3.Dot(this.turret.transform.up, (newTarget.transform.position - this.eye.position).normalized) < 0.0)
      return flag;
    RaycastHit hitInfo1 = new RaycastHit();
    Vector3 normalized1 = (newTarget.transform.position - this.eye.position).normalized;
    if (Physics.Raycast(this.eye.position, normalized1, out hitInfo1, (float) this.range, (int) this.targetMask))
    {
      if ((Object) hitInfo1.transform == (Object) newTarget.transform)
      {
        flag = true;
        color = Color.green;
      }
      else
      {
        flag = false;
        color = Color.red;
      }
    }
    Debug.DrawRay(this.eye.position, normalized1 * (float) this.range, color, 0.1f);
    RaycastHit hitInfo2 = new RaycastHit();
    Vector3 normalized2 = (newTarget.transform.position - this.eye.position).normalized;
    if (Physics.Raycast(this.eye.position, normalized2, out hitInfo2, (float) this.range, (int) this.targetMask))
    {
      if ((Object) hitInfo2.transform == (Object) newTarget.transform)
      {
        flag = true;
        color = Color.green;
      }
      else
      {
        flag = false;
        color = Color.red;
      }
    }
    Debug.DrawRay(this.eye.position, normalized2 * (float) this.range, color, 0.1f);
    return flag;
  }

  public void SetTarget(Track newTarget)
  {
    if ((Object) newTarget == (Object) null)
    {
      this.NoTarget();
    }
    else
    {
      this.target = newTarget;
      this.targetRB = this.target.GetComponent<Rigidbody>();
      this.turret.IsIdle = false;
      this.turret.isAimed = false;
      if (this.target.type == TrackType.SHIP)
      {
        if ((Object) this.targetShip != (Object) this.target.GetComponent<ShipController>())
          this.targetShip = this.target.GetComponent<ShipController>();
        this.transformTarget = this.targetShip.GetModuleRandom();
        this.turret.targetPos = this.transformTarget.position;
      }
      else
      {
        this.targetShip = (ShipController) null;
        this.transformTarget = (Transform) null;
        this.turret.targetPos = this.target.transform.position;
      }
    }
  }

  private void NoTarget()
  {
    this.target = (Track) null;
    this.targetRB = (Rigidbody) null;
    this.turret.IsIdle = true;
    this.turret.isAimed = false;
  }
}
