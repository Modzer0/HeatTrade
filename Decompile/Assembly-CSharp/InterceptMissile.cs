// Decompiled with JetBrains decompiler
// Type: InterceptMissile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class InterceptMissile : MonoBehaviour
{
  [Header("General Paramaters")]
  [Tooltip(" Missile traveling speed")]
  public float MissileSpeed;
  [Tooltip("Initial force before activate the missile")]
  public float InitialLaunchForce;
  [Tooltip("Missile acceleration during missile motor is active")]
  public float Acceleration = 20f;
  [Tooltip("Motor life time before it stops accelerating")]
  public float MotorLifeTime;
  [Tooltip("Time for missile automatically explode")]
  public float MissileLifeTime;
  [Tooltip("Missile turn rate towards target")]
  public float TurnRate = 90f;
  [Tooltip("Missile range for guidance towards target")]
  public float MissileViewRange;
  [Tooltip("Missile view angle in degree for guidance towards target")]
  [Range(0.0f, 360f)]
  public float MissileViewAngle;
  [Tooltip(" Set explosion active delay")]
  public bool isExplosionActiveDelay;
  [Tooltip("Set tracking delay")]
  public bool isTrackingDelay;
  [Tooltip("Missile Flame trail")]
  public GameObject MissileFlameTrail;
  [Tooltip("Missile Explsotion GameObject")]
  public GameObject MissileExplosion;
  [Tooltip("Missile launch Sound effect")]
  public AudioSource LaunchSFX;
  private bool targetTracking;
  private bool missileActive;
  private float MissileLaunchTime;
  private bool motorActive;
  private float MotorActiveTime;
  private Quaternion guideRotation;
  private Rigidbody rb;
  private bool isLaunch;
  private Transform target;
  private Vector3 targetlastPosition;
  private bool explosionActive;

  private void Awake() => this.rb = this.GetComponent<Rigidbody>();

  private void Start()
  {
    if (this.isLaunch)
      return;
    this.rb.isKinematic = true;
  }

  private void FixedUpdate()
  {
    this.Run();
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      return;
    this.GuideMissile();
  }

  private void OnCollisionEnter(Collision col)
  {
    if (!this.explosionActive)
      return;
    this.MissileFlameTrail.transform.parent = (Transform) null;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.MissileFlameTrail, 5f);
    this.DestroyMissile();
  }

  public void Launch(Transform target)
  {
    this.target = target;
    this.isLaunch = true;
    this.rb.isKinematic = false;
    this.MissileLaunchTime = Time.time;
    if (this.isExplosionActiveDelay)
      this.StartCoroutine((IEnumerator) this.ExplosionDelay());
    else
      this.explosionActive = true;
    if (this.isTrackingDelay)
      this.StartCoroutine((IEnumerator) this.TrackingDelay());
    else
      this.targetTracking = true;
    this.StartCoroutine((IEnumerator) this.ActiveDelay(1f));
  }

  private void Run()
  {
    if (!this.isLaunch || !this.missileActive)
      return;
    this.motorActive = (double) this.Since(this.MotorActiveTime) <= (double) this.MotorLifeTime;
    if (!this.missileActive)
      return;
    if (this.motorActive)
      this.MissileSpeed += this.Acceleration * Time.deltaTime;
    this.rb.velocity = this.transform.forward * this.MissileSpeed;
    if (this.targetTracking)
      this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, this.guideRotation, this.TurnRate * Time.deltaTime);
    if ((double) this.Since(this.MissileLaunchTime) <= (double) this.MissileLifeTime)
      return;
    this.DestroyMissile();
  }

  private void GuideMissile()
  {
    Vector3 forward = this.target.position - this.transform.position;
    double num1 = (double) Mathf.Abs(Vector3.Angle(this.transform.position.normalized, forward.normalized));
    float num2 = Vector3.Distance(this.target.position, this.transform.position);
    double missileViewAngle = (double) this.MissileViewAngle;
    if (num1 > missileViewAngle || (double) num2 > (double) this.MissileViewRange)
      this.targetTracking = false;
    if (!this.targetTracking)
      return;
    Vector3 vector3_1 = (this.target.position - this.targetlastPosition) / Time.deltaTime;
    float num3 = this.MissileSpeed + this.Acceleration * this.Since(this.MissileLaunchTime);
    float num4 = num2 / num3;
    Vector3 vector3_2 = this.target.position + vector3_1 * num4 - this.transform.position;
    forward = Vector3.RotateTowards(forward.normalized, vector3_2.normalized, (float) ((double) this.MissileViewAngle * (Math.PI / 180.0) * 0.89999997615814209), 0.0f);
    this.guideRotation = Quaternion.LookRotation(forward, this.transform.up);
    this.targetlastPosition = this.target.position;
  }

  private IEnumerator ExplosionDelay()
  {
    yield return (object) new WaitForSeconds(2f);
    this.explosionActive = true;
  }

  private IEnumerator TrackingDelay()
  {
    yield return (object) new WaitForSeconds(2f);
    this.targetTracking = true;
  }

  private IEnumerator ActiveDelay(float time)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    InterceptMissile interceptMissile = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      interceptMissile.ActivateMissile();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    interceptMissile.rb.velocity = interceptMissile.transform.forward * interceptMissile.InitialLaunchForce;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(time);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void ActivateMissile()
  {
    this.missileActive = true;
    this.motorActive = true;
    this.MotorActiveTime = Time.time;
    this.MissileFlameTrail.SetActive(true);
    this.LaunchSFX.Play();
  }

  private float Since(float Since) => Time.time - Since;

  private void DestroyMissile()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    UnityEngine.Object.Instantiate<GameObject>(this.MissileExplosion, this.transform.position, this.transform.rotation);
  }
}
