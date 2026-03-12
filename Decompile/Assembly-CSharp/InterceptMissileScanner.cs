// Decompiled with JetBrains decompiler
// Type: InterceptMissileScanner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class InterceptMissileScanner : MonoBehaviour
{
  [Header("Settings")]
  [Tooltip("How often to scan in second")]
  public float ScanSpeed;
  [Tooltip("Scanner view angle")]
  [Range(0.0f, 360f)]
  public float ViewAngle;
  [Tooltip(" Layers the scanner will detect")]
  public LayerMask Mask;
  [Tooltip("Get scanner range / or radius")]
  public float ScanRadius;
  [Tooltip("On or Off gizmos")]
  public bool ShowGizmos;
  [Tooltip("Turret Controller, Note: you can put Scanner in 'TurretController' or seperate it")]
  public InterceptMissileController InterceptMissileController;
  private List<Transform> targetList = new List<Transform>();
  private List<Transform> curretTargets = new List<Transform>();

  private void Start()
  {
    this.StartCoroutine((IEnumerator) this.ScanIteration());
    if (!((UnityEngine.Object) this.InterceptMissileController == (UnityEngine.Object) null))
      return;
    Debug.Log((object) "No controller found, Please drag it into this script");
  }

  private IEnumerator ScanIteration()
  {
    while (true)
    {
      yield return (object) new WaitForSeconds(this.ScanSpeed);
      this.ScanForTarget();
    }
  }

  public Vector3 GetViewAngle(float angle)
  {
    float f = (float) (((double) angle + (double) this.transform.eulerAngles.y) * (Math.PI / 180.0));
    return new Vector3(Mathf.Sin(f), 0.0f, Mathf.Cos(f));
  }

  private void OnDrawGizmos()
  {
    if (!this.ShowGizmos)
      return;
    Gizmos.color = Color.black;
    Gizmos.DrawWireSphere(this.transform.position, this.ScanRadius);
    Gizmos.DrawLine(this.transform.position, this.transform.position + this.GetViewAngle(this.ViewAngle / 2f) * this.ScanRadius);
    Gizmos.DrawLine(this.transform.position, this.transform.position + this.GetViewAngle((float) (-(double) this.ViewAngle / 2.0)) * this.ScanRadius);
    Gizmos.color = Color.red;
    if (this.targetList.Count == 0)
      return;
    foreach (Transform target in this.targetList)
    {
      if (!((UnityEngine.Object) target == (UnityEngine.Object) null))
        Gizmos.DrawLine(this.transform.position, target.position);
    }
  }

  public void ScanForTarget()
  {
    this.targetList.Clear();
    foreach (Component component in Physics.OverlapSphere(this.transform.position, this.ScanRadius, (int) this.Mask))
    {
      Transform transform = component.transform;
      if ((double) Vector3.Angle(this.transform.forward, (transform.position - this.transform.position).normalized) < (double) this.ViewAngle / 2.0)
      {
        this.targetList.Add(transform);
        this.SetTargetMissile(transform);
      }
    }
  }

  private IEnumerator ClearCurrentTarget(Transform current)
  {
    yield return (object) new WaitForSeconds(10f);
    this.curretTargets.Remove(current);
  }

  private void SetTargetMissile(Transform targetPosition)
  {
    foreach (UnityEngine.Object curretTarget in this.curretTargets)
    {
      if (curretTarget == (UnityEngine.Object) targetPosition)
        return;
    }
    if ((double) this.InterceptMissileController.loadedMissileCount <= 0.0)
      return;
    this.InterceptMissileController.SetTargetMissile(targetPosition);
    this.curretTargets.Add(targetPosition);
    this.StartCoroutine((IEnumerator) this.ClearCurrentTarget(targetPosition));
  }
}
