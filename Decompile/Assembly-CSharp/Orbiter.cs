// Decompiled with JetBrains decompiler
// Type: Orbiter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Orbiter : MonoBehaviour
{
  private TimeManager tm;
  private OrbitalBodies ob;
  [Header("DATA")]
  public float orbitPeriod;
  public float currentAngle;
  public float orbitalVelocity;
  public Vector3 progradeDirection;
  [Header("INPUTS")]
  public Transform parent;
  public Orbiter parentOrbiter;
  public float mass;
  public bool isOrbiting;
  public bool isOrbitable;
  public float orbitRadius;
  public float rocheLimitRadius;
  public float soiRadius;
  [SerializeField]
  private bool calculateOrbitFromStart;
  [SerializeField]
  private bool isTidalLocked;
  [SerializeField]
  private bool isInheritAngleFromParent;
  [Header("SET CUSTOM ORBIT ON START")]
  [SerializeField]
  private bool isSetOrbitFromStart;
  [SerializeField]
  private bool isStartRadiusRandom;
  [SerializeField]
  private Vector2 startOrbitRange;
  [SerializeField]
  private float startOrbitRadius;
  [SerializeField]
  private bool isStartAngleRandom;
  [SerializeField]
  private float startAngle;
  private float gravConst = 6.6743E-11f;

  private void Start()
  {
    this.tm = TimeManager.current;
    this.ob = OrbitalBodies.current;
    if ((bool) (UnityEngine.Object) this.parent && (bool) (UnityEngine.Object) this.parent.GetComponent<Orbiter>() && !this.isInheritAngleFromParent)
      this.parentOrbiter = this.parent.GetComponent<Orbiter>();
    if (this.isSetOrbitFromStart)
      this.SetOrbitCustom();
    if (!this.calculateOrbitFromStart)
      return;
    this.SetTarget(this.parentOrbiter);
  }

  private void Update()
  {
    if (!this.isOrbiting || (double) this.orbitPeriod == 0.0)
      return;
    if (this.isInheritAngleFromParent)
      this.InheritAngleFromParent();
    else
      this.Orbit();
    this.UpdateProgradeDirection();
    if (!this.isTidalLocked)
      return;
    this.transform.rotation = Quaternion.LookRotation(this.progradeDirection, Vector3.up);
  }

  public void StartRandomOrbit()
  {
    this.isSetOrbitFromStart = true;
    this.isStartRadiusRandom = true;
    this.isStartAngleRandom = true;
  }

  private void SetOrbitCustom()
  {
    this.orbitRadius = this.GetStartOrbitRadius();
    this.currentAngle = this.GetStartAngle();
    float f = this.currentAngle * ((float) Math.PI / 180f);
    this.transform.position = new Vector3(this.orbitRadius * Mathf.Cos(f), 0.0f, this.orbitRadius * Mathf.Sin(f)) + this.parent.position;
  }

  private float GetStartAngle()
  {
    return this.isStartAngleRandom ? UnityEngine.Random.Range(0.0f, 360f) : this.startAngle;
  }

  private float GetStartOrbitRadius()
  {
    if (!this.isStartRadiusRandom)
      return this.startOrbitRadius;
    return (bool) (UnityEngine.Object) this.parentOrbiter ? UnityEngine.Random.Range(this.parentOrbiter.rocheLimitRadius, this.parentOrbiter.soiRadius) : UnityEngine.Random.Range(this.startOrbitRange.x, this.startOrbitRange.y);
  }

  private void SetParent(Transform newParent)
  {
    if (this.isInheritAngleFromParent)
    {
      this.parent = newParent;
    }
    else
    {
      this.parent = newParent;
      this.parentOrbiter = this.parent.GetComponent<Orbiter>();
    }
  }

  public void ModifyOrbitRadius(float mod)
  {
    if ((double) Vector3.Distance(this.transform.position, this.parentOrbiter.transform.position) >= (double) this.parentOrbiter.soiRadius)
      return;
    this.parent = this.parentOrbiter.transform;
    this.isOrbiting = true;
    this.orbitRadius += mod;
    this.UpdateOrbitData(this.orbitRadius * 1000000f);
  }

  private void InheritAngleFromParent()
  {
    if ((UnityEngine.Object) this.parent == (UnityEngine.Object) null || (UnityEngine.Object) this.parentOrbiter == (UnityEngine.Object) null)
      return;
    float f = this.parentOrbiter.currentAngle * ((float) Math.PI / 180f);
    float x = this.orbitRadius * Mathf.Cos(f);
    float z = this.orbitRadius * Mathf.Sin(f);
    Vector3 vector3 = this.parent.position;
    Orbiter component = this.parent.GetComponent<Orbiter>();
    if ((component != null ? (component.isOrbiting ? 1 : 0) : 0) != 0)
      vector3 = this.parentOrbiter.GetFuturePosition(Time.deltaTime * this.tm.timeScale, 0);
    this.transform.position = new Vector3(x, 0.0f, z) + vector3;
    this.currentAngle = this.parentOrbiter.currentAngle;
  }

  private void Orbit()
  {
    this.currentAngle += 360f / this.orbitPeriod * Time.deltaTime * this.tm.timeScale;
    this.currentAngle %= 360f;
    float f = this.currentAngle * ((float) Math.PI / 180f);
    float x = this.orbitRadius * Mathf.Cos(f);
    float z = this.orbitRadius * Mathf.Sin(f);
    Vector3 vector3 = this.parent.position;
    if ((UnityEngine.Object) this.parentOrbiter != (UnityEngine.Object) null && this.parentOrbiter.isOrbiting)
      vector3 = this.parentOrbiter.GetFuturePosition(Time.deltaTime * this.tm.timeScale, 0);
    this.transform.position = new Vector3(x, 0.0f, z) + vector3;
  }

  public void SetTarget(Orbiter newTarget)
  {
    if ((bool) (UnityEngine.Object) this.ob && !this.CompareTag("Body"))
      newTarget = this.ob.GetClosestBody(this.transform.position);
    if ((UnityEngine.Object) newTarget == (UnityEngine.Object) null || (double) Vector3.Distance(this.transform.position, newTarget.transform.position) > (double) newTarget.soiRadius)
      return;
    this.SetParent(newTarget.transform);
    this.isOrbiting = true;
    this.orbitRadius = Vector3.Distance(this.transform.position, this.parent.position);
    this.UpdateOrbitData(this.orbitRadius * 1000000f);
  }

  private void UpdateOrbitData(float orbitRadiusMeters)
  {
    if ((double) this.orbitRadius == 42.16400146484375)
    {
      this.orbitPeriod = 1f;
    }
    else
    {
      this.orbitPeriod = 6.28318548f * Mathf.Sqrt(Mathf.Pow(orbitRadiusMeters, 3f) / (this.gravConst * this.parentOrbiter.mass));
      this.orbitPeriod /= 86400f;
    }
    this.orbitalVelocity = Mathf.Sqrt(this.gravConst * this.parentOrbiter.mass / orbitRadiusMeters);
    Vector3 vector3 = this.transform.position - this.parent.position;
    this.currentAngle = Mathf.Atan2(vector3.z, vector3.x) * 57.29578f;
    if ((double) this.currentAngle < 0.0)
      this.currentAngle += 360f;
    else
      this.currentAngle %= 360f;
  }

  private void UpdateProgradeDirection()
  {
    if (!this.isOrbiting)
      return;
    Vector3 normalized = (this.transform.position - this.parent.position).normalized;
    this.progradeDirection = new Vector3(-normalized.z, 0.0f, normalized.x).normalized;
  }

  public Vector3 GetFuturePosition(float timeOffset, int iterations)
  {
    ++iterations;
    if (iterations >= 10)
    {
      MonoBehaviour.print((object) $"{this.name} TOO MANY ITERATIONS. parent: {((object) this.parentOrbiter)?.ToString()}");
      return Vector3.zero;
    }
    if (this.isInheritAngleFromParent && (UnityEngine.Object) this.parentOrbiter != (UnityEngine.Object) null)
      return this.GetInheritedFuturePosition(timeOffset);
    Vector3 futurePosition = this.parent.position;
    if ((UnityEngine.Object) this.parentOrbiter != (UnityEngine.Object) null && this.parentOrbiter.isOrbiting)
      futurePosition = this.parentOrbiter.GetFuturePosition(timeOffset, iterations);
    if ((double) this.currentAngle == 0.0 && (double) this.orbitPeriod == 0.0)
      return futurePosition;
    float f = (float) (((double) this.currentAngle + (double) (360f / this.orbitPeriod) * (double) timeOffset) % 360.0 * (Math.PI / 180.0));
    double x = (double) this.orbitRadius * (double) Mathf.Cos(f);
    float num = this.orbitRadius * Mathf.Sin(f);
    if ((UnityEngine.Object) this.parentOrbiter != (UnityEngine.Object) null && this.parentOrbiter.isOrbiting)
      futurePosition = this.parentOrbiter.GetFuturePosition(timeOffset, iterations);
    double z = (double) num;
    return new Vector3((float) x, 0.0f, (float) z) + futurePosition;
  }

  private float GetFutureAngle(float timeOffset)
  {
    return (float) (((double) this.currentAngle + (double) (360f / this.orbitPeriod) * (double) timeOffset) % 360.0);
  }

  private Vector3 GetInheritedFuturePosition(float timeOffset)
  {
    float f = this.parentOrbiter.GetFutureAngle(timeOffset) * ((float) Math.PI / 180f);
    double x = (double) this.orbitRadius * (double) Mathf.Cos(f);
    float num = this.orbitRadius * Mathf.Sin(f);
    Vector3 vector3 = this.parent.position;
    Orbiter component = this.parent.GetComponent<Orbiter>();
    if ((component != null ? (component.isOrbiting ? 1 : 0) : 0) != 0)
      vector3 = this.parentOrbiter.GetFuturePosition(timeOffset, 0);
    double z = (double) num;
    return new Vector3((float) x, 0.0f, (float) z) + vector3;
  }

  public OrbitData GetOrbitData()
  {
    return new OrbitData()
    {
      orbitPeriod = this.orbitPeriod,
      currentAngle = this.currentAngle,
      orbitRadius = this.orbitRadius,
      isInheritAngleFromParent = this.isInheritAngleFromParent,
      isCalculateOrbitOnStart = this.calculateOrbitFromStart,
      isSetOrbitOnStart = this.isSetOrbitFromStart
    };
  }

  public void SetFromData(OrbitData od)
  {
    this.orbitPeriod = od.orbitPeriod;
    this.currentAngle = od.currentAngle;
    this.orbitRadius = od.orbitRadius;
    this.isInheritAngleFromParent = od.isInheritAngleFromParent;
    this.calculateOrbitFromStart = od.isCalculateOrbitOnStart;
    this.isSetOrbitFromStart = od.isSetOrbitOnStart;
  }
}
