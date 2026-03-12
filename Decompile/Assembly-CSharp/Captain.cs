// Decompiled with JetBrains decompiler
// Type: Captain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Captain : MonoBehaviour
{
  private Pathfinder pf;
  public ShipController ship;
  public Transform target;
  public Vector3 targetPos;
  public Track track;
  private int thisTrackID;
  [Header("INPUTS")]
  public CaptainRole role;
  private Vector2 delayRange = new Vector2(1f, 3f);
  public bool isTakingNewOrders;
  public CaptainCommand command;
  public int dispersion;
  public CaptainRange range;
  private float engagementRange;
  [Range(100f, 10000f)]
  public float customRange = 200f;
  public CaptainDirection direction;
  private float evasionDistanceSlow = 20f;
  private float evasionDistanceHard = 200f;
  public CaptainWeapons weapons;
  public CaptainMissileSpend missileSpend;
  public CaptainMissileRole missileRole;
  private List<T_CrewModule> crews;
  private float totalCrewHealthRatio;
  [Header("MISSILES")]
  public bool isAutoLaunchOffensiveMissiles;
  [SerializeField]
  private float salvoLaunchChance = 10f;
  private float nextMissileCheck;
  private List<MissileSystem> missileSystems = new List<MissileSystem>();

  private void Start()
  {
    this.pf = this.GetComponent<Pathfinder>();
    this.ship = this.GetComponent<ShipController>();
    this.track = this.GetComponent<Track>();
    this.crews = this.ship.crews;
    this.thisTrackID = this.track.factionID;
    foreach (WeaponSystem weapon in this.ship.weapons)
    {
      if ((bool) (Object) weapon.GetComponent<MissileSystem>())
        this.missileSystems.Add(weapon.GetComponent<MissileSystem>());
    }
    this.nextMissileCheck = Time.time + 10f;
  }

  private void Update()
  {
    this.CheckCrewHealth();
    if ((double) Time.time >= (double) this.nextMissileCheck)
    {
      this.nextMissileCheck = Time.time + 10f;
      if (this.CanLaunchMissiles())
        this.MissileCheck();
    }
    if ((double) this.totalCrewHealthRatio < 0.10000000149011612 || !this.pf.IsDriveHealthy() || this.pf.state != PathfinderState.IDLE)
      return;
    this.UpdateCommand();
  }

  public void SetLaunchChance(int newValue) => this.salvoLaunchChance = (float) newValue;

  private bool CanLaunchMissiles()
  {
    return (bool) (Object) this.target && this.isAutoLaunchOffensiveMissiles && this.missileSystems.Count > 0 && this.target.GetComponent<Track>().factionID != this.thisTrackID;
  }

  private void MissileCheck()
  {
    int num = 0;
    foreach (MissileSystem missileSystem in this.missileSystems)
      num += missileSystem.missiles.Count;
    if (!(bool) (Object) this.target || num <= 0 || (double) Random.Range(0, 101) >= (double) this.salvoLaunchChance)
      return;
    this.LaunchMissiles();
  }

  private void LaunchMissiles()
  {
    foreach (MissileSystem missileSystem in this.missileSystems)
    {
      int resourceMax = (int) missileSystem.resourceMax;
      int num = 0;
      if (this.missileSpend == CaptainMissileSpend.LOW)
        num = resourceMax / 8;
      else if (this.missileSpend == CaptainMissileSpend.MID)
        num = resourceMax / 4;
      else if (this.missileSpend == CaptainMissileSpend.HIGH)
        num = resourceMax / 2;
      missileSystem.NewTarget(this.target.GetComponent<Track>());
      missileSystem.salvoCount = num;
      missileSystem.LaunchSalvo();
    }
  }

  private float GetRandomDelay()
  {
    float num = 0.0f;
    if ((double) this.totalCrewHealthRatio < 1.0)
      num = (float) (10.0 * (1.0 - (double) this.totalCrewHealthRatio));
    return Random.Range(this.delayRange.x, this.delayRange.y + 1f) + num;
  }

  private void CheckCrewHealth()
  {
    float num = 0.0f;
    if (this.crews == null || this.crews.Count == 0)
      return;
    foreach (T_CrewModule crew in this.crews)
      num += crew.health;
    this.totalCrewHealthRatio = Mathf.Clamp01(num / (float) this.crews.Count);
  }

  public bool IsStillAlive() => (double) this.totalCrewHealthRatio >= 0.10000000149011612;

  public void NewMoveOrder(Vector3 newTargetPos)
  {
    this.StartCoroutine((IEnumerator) this.MoveOrderDelay(newTargetPos));
  }

  private IEnumerator MoveOrderDelay(Vector3 newTargetPos)
  {
    this.isTakingNewOrders = true;
    yield return (object) new WaitForSeconds(this.GetRandomDelay());
    this.isTakingNewOrders = false;
    this.targetPos = newTargetPos;
    this.command = CaptainCommand.MOVE;
    this.UpdateCommand();
    this.NewOrderSetup();
  }

  public void NewEscortOrder(Transform newTarget)
  {
    this.StartCoroutine((IEnumerator) this.EscortOrderDelay(newTarget));
  }

  private IEnumerator EscortOrderDelay(Transform newTarget)
  {
    this.isTakingNewOrders = true;
    yield return (object) new WaitForSeconds(this.GetRandomDelay());
    this.isTakingNewOrders = false;
    this.target = newTarget;
    this.command = CaptainCommand.ESCORT;
    this.UpdateCommand();
    this.NewOrderSetup();
  }

  public void NewEngageOrder(Transform newTarget)
  {
    this.StartCoroutine((IEnumerator) this.EngageOrderDelay(newTarget));
  }

  private IEnumerator EngageOrderDelay(Transform newTarget)
  {
    this.isTakingNewOrders = true;
    yield return (object) new WaitForSeconds(this.GetRandomDelay());
    this.isTakingNewOrders = false;
    this.target = newTarget;
    this.command = CaptainCommand.ENGAGE;
    this.UpdateCommand();
    this.NewOrderSetup();
  }

  private void NewOrderSetup() => this.SetWeapons();

  public void UpdateCommand()
  {
    if (this.command == CaptainCommand.ENGAGE)
      this.UpdateEngage();
    else if (this.command == CaptainCommand.EVASIVE_HARD)
      this.UpdateEvasiveHard();
    else if (this.command == CaptainCommand.FLEE)
      this.UpdateFlee();
    else if (this.command == CaptainCommand.MOVE)
    {
      this.UpdateMove();
    }
    else
    {
      if (this.command != CaptainCommand.ESCORT)
        return;
      this.UpdateEscort();
    }
  }

  private void UpdateEngage()
  {
    if ((Object) this.target == (Object) null)
      return;
    this.engagementRange = 0.0f;
    if (this.range == CaptainRange.LOW)
    {
      this.engagementRange = 10000f;
      foreach (WeaponSystem weapon in this.ship.weapons)
      {
        if (weapon.type != WeaponType.MISSILE && (double) weapon.range < (double) this.engagementRange)
          this.engagementRange = (float) weapon.range;
      }
      this.engagementRange /= 2f;
    }
    else if (this.range == CaptainRange.MID)
    {
      float num1 = 10000f;
      float num2 = 0.0f;
      foreach (WeaponSystem weapon in this.ship.weapons)
      {
        if (weapon.type != WeaponType.MISSILE)
        {
          if ((double) weapon.range < (double) num1)
            num1 = (float) weapon.range;
          if ((double) weapon.range > (double) num2)
            num2 = (float) weapon.range;
        }
      }
      this.engagementRange = (float) (((double) num1 + (double) num2) / 2.0);
      this.engagementRange /= 2f;
    }
    else if (this.range == CaptainRange.HIGH)
    {
      foreach (WeaponSystem weapon in this.ship.weapons)
      {
        if (weapon.type != WeaponType.MISSILE && (double) weapon.range > (double) this.engagementRange)
          this.engagementRange = (float) weapon.range;
      }
      this.engagementRange /= 2f;
    }
    else if (this.range == CaptainRange.CUSTOM)
      this.engagementRange = this.customRange;
    Vector3 zero = Vector3.zero;
    Vector3 targetPosition = this.target.position;
    if ((bool) (Object) this.target.GetComponent<Pathfinder>() && this.target.GetComponent<Pathfinder>().state != PathfinderState.IDLE)
      targetPosition = this.target.GetComponent<Pathfinder>().targetPos;
    Vector3 targetDirection = this.GetTargetDirection(targetPosition);
    MonoBehaviour.print((object) ("target direction: " + targetDirection.ToString()));
    this.ship.Move(targetPosition + targetDirection * this.engagementRange);
  }

  private Vector3 GetTargetDirection(Vector3 targetPosition)
  {
    Vector3 targetDirection = Vector3.zero;
    Vector3 normalized = (targetPosition - this.transform.position).normalized;
    Vector2 vector2 = new Vector2();
    vector2 = this.direction != CaptainDirection.FLANK ? (this.direction != CaptainDirection.FRONT ? (this.direction != CaptainDirection.REAR ? new Vector2(-1f, 1f) : new Vector2(0.0f, 0.5f)) : new Vector2(-0.5f, 0.0f)) : new Vector2(-0.5f, -0.1f);
    for (int index = 0; index < 100; ++index)
    {
      Vector3 onUnitSphere = Random.onUnitSphere;
      float num = Vector3.Dot(onUnitSphere, normalized);
      targetDirection = onUnitSphere;
      if ((double) num > (double) vector2.x && (double) num < (double) vector2.y)
        break;
    }
    return targetDirection;
  }

  private void UpdateEvasiveHard()
  {
    if (!(bool) (Object) this.ship)
      return;
    this.ship.Move(this.GetRandomPosFromDist(this.evasionDistanceHard));
  }

  private void UpdateFlee()
  {
    if ((Object) this.target == (Object) null)
      return;
    this.ship.Move(this.transform.position + (this.transform.position - this.target.position).normalized * 10000f);
  }

  private void UpdateMove() => this.ship.Move(this.GetRandomPosAroundTarget(this.targetPos));

  private void UpdateEscort()
  {
    this.ship.Move(this.GetRandomPosAroundTarget(this.target.position));
  }

  private Vector3 GetRandomDirection(float lowEnd, float highEnd)
  {
    Vector3 normalized = (this.target.position - this.transform.position).normalized;
    Vector3 onUnitSphere;
    float num;
    do
    {
      onUnitSphere = Random.onUnitSphere;
      num = Vector3.Dot(onUnitSphere, normalized);
    }
    while ((double) num >= (double) lowEnd || (double) num <= (double) highEnd);
    return onUnitSphere;
  }

  private Vector3 GetRandomPosFromDist(float distance)
  {
    return this.transform.position + Random.onUnitSphere * distance;
  }

  private Vector3 GetRandomPosAroundTarget(Vector3 targetPos)
  {
    Vector3 onUnitSphere = Random.onUnitSphere;
    return targetPos + onUnitSphere * (float) this.dispersion;
  }

  public void SetWeapons()
  {
    if (this.weapons == CaptainWeapons.STANDARD)
    {
      foreach (WeaponSystem weapon in this.ship.weapons)
        weapon.SetMode(weapon.standardMode);
    }
    else if (this.weapons == CaptainWeapons.ATTACK)
      this.SetWeaponsMode(WeaponMode.OFFENSE);
    else if (this.weapons == CaptainWeapons.DEFEND)
    {
      this.SetWeaponsMode(WeaponMode.DEFENSE);
    }
    else
    {
      if (this.weapons != CaptainWeapons.TARGET)
        return;
      this.SetWeaponsMode(WeaponMode.TARGET);
    }
  }

  private void SetWeaponsMode(WeaponMode mode)
  {
    foreach (WeaponSystem weapon in this.ship.weapons)
      weapon.SetMode(mode);
  }
}
