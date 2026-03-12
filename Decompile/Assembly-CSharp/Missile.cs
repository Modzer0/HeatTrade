// Decompiled with JetBrains decompiler
// Type: Missile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Missile : MonoBehaviour
{
  [Header("General Parameter")]
  [Tooltip(" Missile traveling speed")]
  public float MissileSpeed;
  [Tooltip("Missile acceleration during missile motor is active")]
  public float Acceleration = 20f;
  [Tooltip("Time for missile automatically explode")]
  public float MissileLifeTime = 20f;
  [Tooltip("Time delay before activate the missile")]
  public float LaunchDelayTime = 3f;
  [Tooltip("Time delay before start to guide(tracking target) missile")]
  public float TrackingDelay = 5f;
  [Tooltip("Initial force before activate the missile")]
  public float InitialLaunchForce = 15f;
  [Tooltip("Motor life time before it stops accelerating")]
  public float MotorLifeTime = 15f;
  [Tooltip("Missile turn rate towards target")]
  public float TurnRate = 90f;
  [Tooltip("Missile Explsotion GameObject")]
  public GameObject MissileExplosion;
  [Tooltip("Missile Flame trail")]
  public GameObject MissileFlameTrail;
  [Tooltip("Missile launch Sound effect")]
  public AudioSource LaunchSFX;
  [HideInInspector]
  public Transform Target;
  private bool targetTracking;
  private bool missileActive;
  private bool motorActive;
  private bool explosionActive;
  private float MissileLaunchTime;
  private float MotorActiveTime;
  private Quaternion guideRotation;
  private Rigidbody rb;

  private void Start()
  {
    this.rb = this.GetComponent<Rigidbody>();
    this.MissileLaunchTime = Time.time;
    this.StartCoroutine((IEnumerator) this.LaunchDelay(this.LaunchDelayTime));
  }

  private void ActivateMissile()
  {
    this.missileActive = true;
    this.motorActive = true;
    this.MotorActiveTime = Time.time;
    this.MissileFlameTrail.SetActive(true);
    this.LaunchSFX.Play();
  }

  private void FixedUpdate()
  {
    this.Run();
    this.GuideMissile();
  }

  private void OnCollisionEnter(Collision col)
  {
    if (!this.explosionActive)
      return;
    this.MissileFlameTrail.transform.parent = (Transform) null;
    Object.Destroy((Object) this.MissileFlameTrail, 5f);
    this.DestroyMissile();
  }

  private void Run()
  {
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
    if ((Object) this.Target == (Object) null)
      return;
    Vector3 vector3 = this.Target.position - this.transform.position;
    if (!this.targetTracking)
      return;
    this.guideRotation = Quaternion.LookRotation(this.Target.position - this.transform.position, this.transform.up);
  }

  private IEnumerator LaunchDelay(float time)
  {
    Missile missile = this;
    missile.rb.velocity = missile.transform.forward * missile.InitialLaunchForce;
    yield return (object) new WaitForSeconds(time);
    missile.ActivateMissile();
    yield return (object) new WaitForSeconds(Random.Range(missile.TrackingDelay, missile.TrackingDelay + 3f));
    missile.targetTracking = true;
    missile.explosionActive = true;
  }

  private float Since(float Since) => Time.time - Since;

  private void DestroyMissile()
  {
    Object.Destroy((Object) this.gameObject);
    Object.Instantiate<GameObject>(this.MissileExplosion, this.transform.position, this.transform.rotation);
  }
}
