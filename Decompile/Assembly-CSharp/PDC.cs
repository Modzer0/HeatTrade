// Decompiled with JetBrains decompiler
// Type: PDC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PDC : TurretWeapon, IMount
{
  private Pool pool;
  [Space(10f)]
  [Header("PROJECTILE")]
  [SerializeField]
  private string poolKey;
  [SerializeField]
  private float projectileSpeed;
  [SerializeField]
  private float shotCooldown;
  private float nextFireTime;
  private bool canShoot = true;
  private bool hasSolution;
  private Vector3 leadPos;
  [Space(10f)]
  [Header("PARTICLES")]
  private bool isFXOn;
  private bool shotInPrevFireTime;
  public ParticleSystem ps;
  [SerializeField]
  private bool isFXConstant;
  [Space(10f)]
  [Header("AUDIO")]
  public AudioSource audioFiring;

  public override void Start()
  {
    base.Start();
    this.pool = Pool.current;
    this.audioFiring = this.GetComponentInChildren<AudioSource>();
  }

  public override void Update()
  {
    base.Update();
    this.isFXOn = true;
    this.TryFire();
    if (!this.isOn || !this.shotInPrevFireTime || !(bool) (Object) this.target)
      this.isFXOn = false;
    if (!this.isFXConstant)
      return;
    this.SetFX(this.isFXOn);
  }

  private void TryFire()
  {
    if ((double) Time.time < (double) this.nextFireTime || !this.isOn || !((Object) this.target != (Object) null) || !this.CanSee(this.target))
      return;
    if ((double) this.targetRB.velocity.sqrMagnitude > (double) this.projectileSpeed * (double) this.projectileSpeed)
    {
      this.SetTarget((Track) null);
    }
    else
    {
      this.AimToShoot();
      if (this.turret.IsAimed && this.hasSolution && ((double) this.resource > 0.0 || this.hasInfiniteAmmo))
      {
        this.Fire();
        this.shotInPrevFireTime = true;
      }
      else
        this.shotInPrevFireTime = false;
    }
  }

  private void SetFX(bool isFXOn)
  {
    if (!(bool) (Object) this.ps && !(bool) (Object) this.audioFiring)
      return;
    if (isFXOn)
    {
      if (!this.ps.isPlaying)
        this.ps.Play();
      this.ps.emissionRate = 10f;
      if (this.audioFiring.isPlaying)
        return;
      this.audioFiring.Play();
    }
    else
    {
      if (this.ps.isPlaying)
        this.ps.Stop();
      this.ps.emissionRate = 0.0f;
      if (!this.audioFiring.isPlaying)
        return;
      this.audioFiring.Stop();
    }
  }

  private void SetSFX(bool isSFXOn)
  {
    if (!(bool) (Object) this.audioFiring)
      return;
    if (this.isFXOn)
    {
      if (this.audioFiring.isPlaying)
        return;
      this.audioFiring.Play();
    }
    else
    {
      if (!this.audioFiring.isPlaying)
        return;
      this.audioFiring.Stop();
    }
  }

  private void AimToShoot()
  {
    this.hasSolution = false;
    if ((Object) this.target == (Object) null || (Object) this.targetRB == (Object) null || !this.CanSee(this.target))
    {
      this.isFXOn = false;
      this.SetTarget((Track) null);
    }
    else
    {
      Vector3 rhs = this.currentTargetPos - this.muzzle.position;
      Vector3 lhs = this.targetRB.velocity - this.parentRB.velocity;
      float f1 = lhs.sqrMagnitude - this.projectileSpeed * this.projectileSpeed;
      float num1 = 2f * Vector3.Dot(lhs, rhs);
      float sqrMagnitude = rhs.sqrMagnitude;
      if ((double) Mathf.Abs(f1) < 1.0 / 1000.0)
        return;
      float f2 = (float) ((double) num1 * (double) num1 - 4.0 * (double) f1 * (double) sqrMagnitude);
      if ((double) f2 < 0.0)
        return;
      float num2 = Mathf.Sqrt(f2);
      float num3 = (float) ((-(double) num1 - (double) num2) / (2.0 * (double) f1));
      float num4 = (float) ((-(double) num1 + (double) num2) / (2.0 * (double) f1));
      if ((double) num3 < 0.0 || (double) num4 < (double) num3 && (double) num4 >= 0.0)
        num3 = num4;
      if ((double) num3 < 0.0)
        return;
      float num5 = float.MaxValue;
      if ((double) num3 >= 0.0)
        num5 = num3;
      if ((double) num4 >= 0.0 && (double) num4 < (double) num5)
        num5 = num4;
      if ((double) num5 == 3.4028234663852886E+38)
        return;
      this.leadPos = Vector3.zero;
      this.leadPos = (double) this.targetAcceleration.sqrMagnitude <= 0.0099999997764825821 ? this.currentTargetPos + lhs * num5 : this.currentTargetPos + lhs * num5 + 0.5f * this.targetAcceleration * num5 * num5;
      this.turret.targetPos = this.leadPos;
      if ((double) Vector3.Distance(this.muzzle.position, this.leadPos) > (double) this.range)
        return;
      this.hasSolution = true;
    }
  }

  private void Fire()
  {
    GameObject gameObject = this.pool.Get(this.poolKey, this.muzzle.position, this.muzzle.rotation).gameObject;
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
    if (this.isFXConstant)
      return;
    this.SetFX(this.isOn);
  }

  public void SetTeam(int newTeam)
  {
  }
}
