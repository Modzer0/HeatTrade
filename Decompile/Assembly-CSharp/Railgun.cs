// Decompiled with JetBrains decompiler
// Type: Railgun
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Railgun : TurretWeapon
{
  [Header("RAILGUN")]
  private Pool pool;
  [Space(10f)]
  [Header("PROJECTILE")]
  [SerializeField]
  private string poolKey;
  [SerializeField]
  private GameObject projectilePrefab;
  [SerializeField]
  private float projectileSpeed;
  [SerializeField]
  private float shotCooldown;
  private float nextFireTime;
  private bool canSeeTarget;
  private bool hasSolution;
  private Vector3 leadPos;
  [Space(10f)]
  [Header("PARTICLES")]
  private bool isFXOn;
  public ParticleSystem ps;
  [Space(10f)]
  [Header("AUDIO")]
  public AudioSource audioFiring;

  public override void Start()
  {
    base.Start();
    this.pool = Pool.current;
  }

  public override void Update()
  {
    base.Update();
    this.ApplyPowerAndHeat(0.0f);
    if ((double) Time.time < (double) this.nextFireTime || !this.isOn || !((Object) this.target != (Object) null) || !this.CanSee(this.target))
      return;
    this.AimToShoot();
    if (!this.turret.IsAimed || !this.hasSolution || (double) this.resource <= 0.0 && !this.hasInfiniteAmmo)
      return;
    this.Fire();
    this.PlayFX();
    this.ApplyPowerAndHeat(1f);
  }

  private void PlayFX()
  {
    if ((bool) (Object) this.ps)
    {
      this.ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
      this.ps.Play(true);
    }
    if (!(bool) (Object) this.audioFiring || this.audioFiring.isPlaying)
      return;
    this.audioFiring.Play();
  }

  private void SetFX(bool isFXOn)
  {
    if (!(bool) (Object) this.ps || !(bool) (Object) this.audioFiring)
      return;
    if (isFXOn)
    {
      if (!this.ps.isPlaying)
        this.ps.Play();
      if (!(bool) (Object) this.audioFiring || this.audioFiring.isPlaying)
        return;
      this.audioFiring.Play();
    }
    else
    {
      if (this.ps.isPlaying)
        this.ps.Stop();
      this.ps.emissionRate = 0.0f;
      if (!(bool) (Object) this.audioFiring || !this.audioFiring.isPlaying)
        return;
      this.audioFiring.Stop();
    }
  }

  private bool CanSeeFromMuzzle()
  {
    if ((Object) this.target == (Object) null)
      return false;
    bool flag = false;
    Color color = Color.yellow;
    Vector3 normalized = (this.target.transform.position - this.muzzle.position).normalized;
    RaycastHit hitInfo;
    if (Physics.Raycast(this.muzzle.position, normalized, out hitInfo, (float) this.range, (int) this.targetMask))
    {
      if ((Object) hitInfo.transform == (Object) this.target.transform)
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
    Debug.DrawRay(this.muzzle.position, normalized * (float) this.range, color, 1f);
    return flag;
  }

  private void AimToShoot()
  {
    this.hasSolution = false;
    if ((Object) this.target == (Object) null || (Object) this.targetRB == (Object) null || !this.CanSeeFromMuzzle())
    {
      this.isFXOn = false;
      this.SetTarget((Track) null);
    }
    else
    {
      Vector3 rhs = this.currentTargetPos - this.muzzle.position;
      Vector3 lhs = this.targetRB.velocity - this.parentRB.velocity;
      float num1 = lhs.sqrMagnitude - this.projectileSpeed * this.projectileSpeed;
      float num2 = 2f * Vector3.Dot(lhs, rhs);
      float sqrMagnitude = rhs.sqrMagnitude;
      float f = (float) ((double) num2 * (double) num2 - 4.0 * (double) num1 * (double) sqrMagnitude);
      if ((double) f < 0.0)
        return;
      float num3 = Mathf.Sqrt(f);
      float num4 = (float) ((-(double) num2 - (double) num3) / (2.0 * (double) num1));
      float num5 = (float) ((-(double) num2 + (double) num3) / (2.0 * (double) num1));
      if ((double) num4 < 0.0 || (double) num5 < (double) num4 && (double) num5 >= 0.0)
        num4 = num5;
      if ((double) num4 < 0.0)
        return;
      this.leadPos = Vector3.zero;
      this.leadPos = (double) this.targetAcceleration.sqrMagnitude <= 0.0099999997764825821 ? this.currentTargetPos + lhs * num4 : this.currentTargetPos + lhs * num4 + 0.5f * this.targetAcceleration * num4 * num4;
      this.turret.targetPos = this.leadPos;
      if ((double) Vector3.Distance(this.muzzle.position, this.leadPos) > (double) this.range)
        return;
      this.hasSolution = true;
    }
  }

  private void Fire()
  {
    GameObject gameObject = this.pool.Get(this.poolKey, this.muzzle.position + this.muzzle.forward, this.muzzle.rotation).gameObject;
    Projectile component = gameObject.GetComponent<Projectile>();
    gameObject.SetActive(true);
    component.Init();
    Vector3 vector3 = this.leadPos - this.muzzle.position;
    vector3 = vector3.normalized + new Vector3(Random.Range(-this.accError, this.accError), Random.Range(-this.accError, this.accError), Random.Range(-this.accError, this.accError));
    Vector3 normalized = vector3.normalized;
    component.velocity = normalized * this.projectileSpeed;
    component.velocity += this.parentRB.velocity;
    this.nextFireTime = Time.time + this.shotCooldown;
    if (this.hasInfiniteAmmo)
      return;
    --this.resource;
  }
}
