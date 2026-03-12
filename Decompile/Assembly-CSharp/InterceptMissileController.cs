// Decompiled with JetBrains decompiler
// Type: InterceptMissileController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class InterceptMissileController : MonoBehaviour
{
  [Header("Turret Settings")]
  [Tooltip("Pivot for horizontal rotation")]
  public Transform HorizontalPivot;
  [Header("Rotation Settings")]
  [Tooltip("If you want to limit turret rotation")]
  public bool RotationLimit;
  [Tooltip("Right rotation limit")]
  [Range(0.0f, 180f)]
  public float RightRotationLimit;
  [Tooltip("Left rotation limit")]
  [Range(0.0f, 180f)]
  public float LeftRotationLimit;
  [Tooltip("Turning speed")]
  [Range(0.0f, 300f)]
  public float TurnSpeed;
  [Header("Missile settings")]
  [Tooltip("How many missile in turret")]
  public float MissileCount;
  [Tooltip("Launcher Spot")]
  public Transform[] LaunchSpot;
  [Tooltip("Missile Prefab")]
  public InterceptMissile missile;
  [HideInInspector]
  public Transform target;
  [HideInInspector]
  public float loadedMissileCount;
  private List<InterceptMissile> loadedMissile = new List<InterceptMissile>();

  private void Start()
  {
    this.target = (Transform) null;
    this.SpawnMissile();
  }

  private void Update() => this.HorizontalRotation();

  private IEnumerator RespawnMissile()
  {
    yield return (object) new WaitForSeconds(2f);
    if ((double) this.MissileCount <= 0.0)
      yield return (object) 0;
    this.SpawnMissile();
  }

  private void SpawnMissile()
  {
    if (this.LaunchSpot.Length == 0)
      Debug.Log((object) "No LaunchSpot found, Please drag it into this script");
    foreach (Transform transform in this.LaunchSpot)
    {
      if ((double) this.MissileCount <= 0.0)
        break;
      InterceptMissile interceptMissile = UnityEngine.Object.Instantiate<InterceptMissile>(this.missile, transform.position, transform.rotation);
      interceptMissile.transform.parent = transform;
      Vector3 vector3 = new Vector3(0.0f, 8f, -0.3f);
      interceptMissile.transform.localPosition = vector3;
      this.loadedMissile.Add(interceptMissile);
      CameraManager2.CameraTargets.Add(interceptMissile.transform);
      ++this.loadedMissileCount;
      --this.MissileCount;
    }
  }

  private void HorizontalRotation()
  {
    if ((UnityEngine.Object) this.HorizontalPivot == (UnityEngine.Object) null || (UnityEngine.Object) this.target == (UnityEngine.Object) null)
      return;
    Vector3 target = this.transform.InverseTransformPoint(this.target.position) with
    {
      y = 0.0f
    };
    Vector3 forward = target;
    if (this.RotationLimit)
      forward = (double) target.x < 0.0 ? Vector3.RotateTowards(Vector3.forward, target, (float) Math.PI / 180f * this.LeftRotationLimit, 0.0f) : Vector3.RotateTowards(Vector3.forward, target, (float) Math.PI / 180f * this.RightRotationLimit, 0.0f);
    Debug.DrawLine(this.HorizontalPivot.position, this.target.position, Color.red);
    this.HorizontalPivot.localRotation = Quaternion.RotateTowards(this.HorizontalPivot.localRotation, Quaternion.LookRotation(forward), this.TurnSpeed * Time.deltaTime);
  }

  public void SetTargetMissile(Transform targetPosition)
  {
    this.target = targetPosition;
    this.Launch(targetPosition);
  }

  private void Launch(Transform targetPosition)
  {
    this.loadedMissile[(int) this.loadedMissileCount - 1].Launch(targetPosition);
    this.loadedMissile[(int) this.loadedMissileCount - 1].transform.parent = (Transform) null;
    this.loadedMissile.Remove(this.loadedMissile[(int) this.loadedMissileCount - 1]);
    --this.loadedMissileCount;
    if ((double) this.loadedMissileCount > 0.0)
      return;
    this.StartCoroutine((IEnumerator) this.RespawnMissile());
  }
}
