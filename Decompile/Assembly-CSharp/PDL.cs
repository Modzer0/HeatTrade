// Decompiled with JetBrains decompiler
// Type: PDL
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PDL : TurretWeapon, IMount
{
  [Space(10f)]
  [Header("LASER")]
  [SerializeField]
  private PDL.DutyCycle dutyCycle;
  [SerializeField]
  private float shotCooldown;
  [SerializeField]
  private int wavelength;
  [SerializeField]
  private float apertureM;
  [SerializeField]
  private float intensity100km;
  private float laserPower;
  private float nextFireTime;
  private bool canShoot = true;
  [Space(10f)]
  [Header("PARTICLES")]
  private bool isTurnPSOn;
  private bool isPSOn;
  [SerializeField]
  private GameObject laserGlow;
  [SerializeField]
  private GameObject laserGlowBillboard;
  [SerializeField]
  private ParticleSystem laserHitPS;
  [SerializeField]
  private ParticleSystem laserDebrisPS;
  [Space(10f)]
  [Header("AUDIO")]
  [SerializeField]
  private AudioSource laserAudio;
  [SerializeField]
  private AudioSource sparksAudio;
  [Header("LINE")]
  private Color laserColor;
  private float emissionMultiplier = 1f;
  private LineRenderer lr;
  private Vector3 hitPoint;

  public override void Start()
  {
    base.Start();
    this.turret = this.GetComponent<Turret2>();
    this.sparksAudio = this.laserHitPS.GetComponent<AudioSource>();
    this.sparksAudio.volume = 0.1f;
    this.laserPower = this.power - this.heat;
    this.CalculateLaser();
    this.lr = this.GetComponent<LineRenderer>();
    this.lr.useWorldSpace = true;
    this.SetLineColor();
    if (!(bool) (Object) LaserToggler.current)
      return;
    LaserToggler.current.AddThisLaser(this.lr);
  }

  public override void Update()
  {
    base.Update();
    if ((Object) this.target != (Object) null && this.turret.IsAimed && this.isOn)
    {
      this.turret.targetPos = this.currentTargetPos;
      this.Fire();
      this.ApplyPowerAndHeat(1f);
    }
    else
    {
      this.isTurnPSOn = false;
      this.ApplyPowerAndHeat(0.0f);
    }
    this.SetFX(this.isTurnPSOn);
  }

  private void CalculateLaser()
  {
    this.intensity100km = this.laserPower / (3.14159274f * Mathf.Pow((float) (2.440000057220459 * (double) this.wavelength * 100000.0 / (double) this.apertureM * 9.9999997171806854E-10) / 2f, 2f));
  }

  private void SetFX(bool isFXOn)
  {
    this.isPSOn = isFXOn;
    if (isFXOn)
    {
      if ((bool) (Object) this.laserGlow)
        this.laserGlow.SetActive(true);
      if ((bool) (Object) this.laserGlowBillboard)
        this.laserGlowBillboard.SetActive(true);
      this.laserHitPS.gameObject.SetActive(true);
      if (!this.laserHitPS.isPlaying)
        this.laserHitPS.Play();
      this.laserDebrisPS.gameObject.SetActive(true);
      if (!this.laserDebrisPS.isPlaying)
      {
        this.laserDebrisPS.Play();
        this.laserDebrisPS.transform.LookAt(this.transform);
      }
      if ((bool) (Object) this.laserAudio && this.laserAudio.enabled && !this.laserAudio.isPlaying)
        this.laserAudio.Play();
      if ((bool) (Object) this.sparksAudio && this.sparksAudio.enabled && !this.sparksAudio.isPlaying)
        this.sparksAudio.Play();
      if (!(bool) (Object) this.lr)
        return;
      this.lr.SetPosition(0, this.muzzle.position);
      this.lr.SetPosition(1, this.hitPoint);
    }
    else
    {
      if ((bool) (Object) this.laserGlow)
        this.laserGlow.SetActive(false);
      if ((bool) (Object) this.laserGlowBillboard)
        this.laserGlowBillboard.SetActive(false);
      this.laserHitPS.gameObject.SetActive(false);
      if (this.laserHitPS.isPlaying)
        this.laserHitPS.Stop();
      this.laserDebrisPS.gameObject.SetActive(false);
      if (this.laserDebrisPS.isPlaying)
        this.laserDebrisPS.Stop();
      if ((bool) (Object) this.laserAudio && this.laserAudio.isPlaying)
        this.laserAudio.Stop();
      if ((bool) (Object) this.sparksAudio && this.sparksAudio.isPlaying)
        this.sparksAudio.Stop();
      if (!(bool) (Object) this.lr)
        return;
      this.lr.SetPosition(0, Vector3.zero);
      this.lr.SetPosition(1, Vector3.zero);
    }
  }

  private void GetLaserColor()
  {
    ColorManager current = ColorManager.current;
    this.emissionMultiplier = 1f;
    if ((double) this.wavelength == 1064.0)
    {
      this.laserColor = current.nirColor;
      this.emissionMultiplier = 1f;
    }
    else if ((double) this.wavelength == 1000.0)
    {
      this.laserColor = current.irColor;
      this.emissionMultiplier = 2f;
    }
    else if ((double) this.wavelength == 200.0)
    {
      this.laserColor = current.uvColor;
      this.emissionMultiplier = 3f;
    }
    else if ((double) this.wavelength == 10.0)
    {
      this.laserColor = current.xrayColor;
      this.emissionMultiplier = 4f;
    }
    else
    {
      if ((double) this.wavelength > 750.0 && (double) this.wavelength < 400.0)
        return;
      float time = (float) (((double) this.wavelength - 400.0) / 350.0);
      this.laserColor = current.visibleGradient.Evaluate(time);
      this.emissionMultiplier = 1.5f;
    }
  }

  private void SetLineColor()
  {
    if ((Object) ColorManager.current == (Object) null)
      return;
    this.GetLaserColor();
    this.lr.material.SetColor("_Color", this.laserColor);
    this.lr.material.SetColor("_EmissionColor", this.laserColor * Mathf.LinearToGammaSpace(this.emissionMultiplier));
  }

  private void Fire()
  {
    RaycastHit hitInfo = new RaycastHit();
    Vector3 normalized = (this.currentTargetPos - this.muzzle.position).normalized;
    if (Physics.Raycast(this.muzzle.position, normalized, out hitInfo, 10000f, (int) this.targetMask))
    {
      if (!((Object) hitInfo.collider.transform != (Object) null) || (Object) hitInfo.transform == (Object) this.parentShipTransform)
        return;
      this.hitPoint = hitInfo.point;
      this.laserDebrisPS.transform.position = this.hitPoint;
      this.laserHitPS.transform.position = this.hitPoint - normalized * 0.1f;
      IHealth health = (IHealth) null;
      if (hitInfo.collider.transform.GetComponent<IHealth>() != null)
        health = hitInfo.collider.transform.GetComponent<IHealth>();
      else if (hitInfo.collider.GetComponentInParent<IHealth>() != null)
        health = hitInfo.collider.GetComponentInParent<IHealth>();
      if (health == null)
        return;
      float damage = this.laserPower / (3.14159274f * Mathf.Pow((float) (2.440000057220459 * (double) this.wavelength * (double) hitInfo.distance * 10.0 / (double) this.apertureM * 9.9999997171806854E-10) / 2f, 2f)) * Time.deltaTime * 0.5f;
      health.TryDamagePhotonic(damage);
      this.turret.isAimed = false;
      this.isTurnPSOn = true;
    }
    else
      this.isTurnPSOn = false;
  }

  public void SetTeam(int newTeam)
  {
  }

  private enum DutyCycle
  {
    CONTINUOUS,
    PULSED,
  }
}
