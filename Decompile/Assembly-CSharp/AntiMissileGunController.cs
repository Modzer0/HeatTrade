// Decompiled with JetBrains decompiler
// Type: AntiMissileGunController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class AntiMissileGunController : MonoBehaviour
{
  [Header("Turret Settings")]
  [Tooltip("Pivot for horizontal rotation")]
  public Transform HorizontalPivot;
  [Tooltip("Pivot for vertical rotation")]
  public Transform VerticalPivot;
  [Header("Horizontal Rotation Settings")]
  [Tooltip("If you want to limit horizontal turret rotation")]
  public bool HorizontalRotationLimit;
  [Tooltip("Right rotation limit")]
  [Range(0.0f, 180f)]
  public float RightRotationLimit;
  [Tooltip("Left rotation limit")]
  [Range(0.0f, 180f)]
  public float LeftRotationLimit;
  [Header("Vertical Rotation Settings")]
  [Tooltip("If you want to limit vertical turret rotation")]
  public bool VerticalRotationLimit;
  [Tooltip("Upwards rotation limit")]
  [Range(0.0f, 70f)]
  public float UpwardsRotationLimit;
  [Tooltip("Downwards rotation limit")]
  [Range(0.0f, 70f)]
  public float DownwardsRotationLimit;
  [Tooltip("Turning speed")]
  [Range(0.0f, 300f)]
  public float TurnSpeed;
  [Header("Gun Settings")]
  [Tooltip("Click if you want to use pooling")]
  public bool UsePooling;
  [Tooltip("Gun firing rate")]
  public float FireRate;
  [Tooltip("Projectile traveling speed")]
  public float ProjectileSpeed;
  [Tooltip("How many projectile in this turret")]
  public float ProjectileCount;
  [Tooltip("Projectile prefabs")]
  public GameObject ProjectilePrefab;
  [Tooltip("Adjust the efficiency of this turret")]
  [Range(3f, 4f)]
  public float Efficiency;
  [Tooltip("Barrel for instantiating projectile")]
  public Transform[] Barrel;
  [HideInInspector]
  public Transform target;
  [HideInInspector]
  public Vector3 predictedTargetPosition;
  [Header("Effects (Optional)")]
  [Tooltip("Shoot effect when firing the gun (optional)")]
  public GameObject ShootFX;
  public GameObject BulletShellFX;
  private Vector3 targetlastPosition;
  protected ParticleSystem bulletShellFX_PS;
  protected ParticleSystem shootFX_PS;
  protected float nextFireAllowed;
  protected bool IsAiming;

  protected virtual void Start()
  {
    this.target = (Transform) null;
    if ((UnityEngine.Object) this.HorizontalPivot == (UnityEngine.Object) null || (UnityEngine.Object) this.VerticalPivot == (UnityEngine.Object) null)
      Debug.Log((object) "There is no pivot found, Please drag your pivots into this script");
    else if (this.Barrel.Length == 0)
      Debug.Log((object) "There is no Barrel found, Please drag your pivots into this script");
    else if ((UnityEngine.Object) this.ProjectilePrefab == (UnityEngine.Object) null)
    {
      Debug.Log((object) "There is no projectile prefab found, Please drag your projectile prefab into this script");
    }
    else
    {
      if (this.UsePooling)
      {
        if ((UnityEngine.Object) PoolManager.instance == (UnityEngine.Object) null)
        {
          Debug.Log((object) "PoolManager is missing, Please create a GameObject and add PoolManager.cs");
          return;
        }
        PoolManager.instance.CreatePool(this.ProjectilePrefab, 100);
      }
      if ((UnityEngine.Object) this.BulletShellFX != (UnityEngine.Object) null)
      {
        this.BulletShellFX.SetActive(true);
        this.bulletShellFX_PS = this.BulletShellFX.GetComponent<ParticleSystem>();
        this.bulletShellFX_PS.Stop();
      }
      if (!((UnityEngine.Object) this.ShootFX != (UnityEngine.Object) null))
        return;
      this.ShootFX.SetActive(true);
      this.shootFX_PS = this.ShootFX.GetComponent<ParticleSystem>();
      this.shootFX_PS.Stop();
    }
  }

  private void FixedUpdate()
  {
    this.LeadTarget();
    this.HorizontalRotation();
    this.VerticalRotation();
    this.Fire();
  }

  private void LeadTarget()
  {
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      return;
    Vector3 vector3 = (this.target.position - this.targetlastPosition) / Time.deltaTime;
    float num1 = Vector3.Distance(this.transform.position, this.target.position) / Mathf.Max(this.ProjectileSpeed, 2f);
    float num2 = Vector3.Distance(this.transform.position, this.target.position + vector3 * this.Efficiency / 4f * num1) / Mathf.Max(this.ProjectileSpeed, 2f);
    this.predictedTargetPosition = this.target.position + vector3 * this.Efficiency / 4f * num2;
    Debug.DrawLine(this.transform.position, this.predictedTargetPosition, Color.blue);
    this.targetlastPosition = this.target.position;
  }

  private void HorizontalRotation()
  {
    if ((UnityEngine.Object) this.HorizontalPivot == (UnityEngine.Object) null && (UnityEngine.Object) this.VerticalPivot == (UnityEngine.Object) null || (UnityEngine.Object) this.target == (UnityEngine.Object) null)
      return;
    Vector3 target = this.transform.InverseTransformPoint(this.predictedTargetPosition) with
    {
      y = 0.0f
    };
    Vector3 forward = target;
    if (this.HorizontalRotationLimit)
    {
      forward = (double) target.x < 0.0 ? Vector3.RotateTowards(Vector3.forward, target, (float) Math.PI / 180f * this.LeftRotationLimit, 0.0f) : Vector3.RotateTowards(Vector3.forward, target, (float) Math.PI / 180f * this.RightRotationLimit, 0.0f);
    }
    else
    {
      this.RightRotationLimit = 0.0f;
      this.LeftRotationLimit = 0.0f;
    }
    this.HorizontalPivot.localRotation = Quaternion.RotateTowards(this.HorizontalPivot.localRotation, Quaternion.LookRotation(forward), this.TurnSpeed * Time.deltaTime);
  }

  private void VerticalRotation()
  {
    if ((UnityEngine.Object) this.HorizontalPivot == (UnityEngine.Object) null && (UnityEngine.Object) this.VerticalPivot == (UnityEngine.Object) null || (UnityEngine.Object) this.target == (UnityEngine.Object) null)
      return;
    Vector3 target = this.HorizontalPivot.transform.InverseTransformPoint(this.predictedTargetPosition) with
    {
      x = 0.0f
    };
    Vector3 forward = target;
    if (this.VerticalRotationLimit)
    {
      forward = (double) target.y < 0.0 ? Vector3.RotateTowards(Vector3.forward, target, (float) Math.PI / 180f * this.DownwardsRotationLimit, 0.0f) : Vector3.RotateTowards(Vector3.forward, target, (float) Math.PI / 180f * this.UpwardsRotationLimit, 0.0f);
    }
    else
    {
      this.UpwardsRotationLimit = 0.0f;
      this.DownwardsRotationLimit = 0.0f;
    }
    this.VerticalPivot.localRotation = Quaternion.RotateTowards(this.VerticalPivot.localRotation, Quaternion.LookRotation(forward), 2f * this.TurnSpeed * Time.deltaTime);
    if ((double) Mathf.Abs(Vector3.Angle(this.VerticalPivot.forward, (this.predictedTargetPosition - this.VerticalPivot.position).normalized)) < 5.0)
      this.IsAiming = true;
    else
      this.IsAiming = false;
  }

  public void SetTargetGun(Transform targetPosition) => this.target = targetPosition;

  protected virtual void Fire()
  {
    if (!((UnityEngine.Object) this.target != (UnityEngine.Object) null) || (double) this.ProjectileCount <= 0.0 || (double) Time.time <= (double) this.nextFireAllowed || !this.IsAiming)
      return;
    for (int index = 0; index < this.Barrel.Length; ++index)
    {
      if (this.UsePooling)
      {
        PoolManager.instance.ReuseObject(this.ProjectilePrefab, this.Barrel[index].position, this.Barrel[index].rotation, this.predictedTargetPosition, this.ProjectileSpeed);
      }
      else
      {
        AntiMissileProjectile component = UnityEngine.Object.Instantiate<GameObject>(this.ProjectilePrefab, this.Barrel[index].position, this.Barrel[index].rotation).GetComponent<AntiMissileProjectile>();
        component.transform.LookAt(this.predictedTargetPosition);
        component.Speed = this.ProjectileSpeed;
      }
      --this.ProjectileCount;
    }
    this.nextFireAllowed = Time.time + this.FireRate;
    if ((UnityEngine.Object) this.BulletShellFX != (UnityEngine.Object) null)
    {
      this.bulletShellFX_PS.Play();
      this.Invoke("StopBulletShellEffect", 1.2f);
    }
    if ((UnityEngine.Object) this.ShootFX == (UnityEngine.Object) null)
      return;
    this.shootFX_PS.Play();
  }

  private void StopBulletShellEffect() => this.bulletShellFX_PS.Stop();
}
