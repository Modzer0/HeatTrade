// Decompiled with JetBrains decompiler
// Type: MissileGuidance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MissileGuidance : MonoBehaviour
{
  public Transform target;
  private Vector3 targetPos;
  private Rigidbody targetRB;
  private Pathfinder targetPF;
  [SerializeField]
  private float accError;
  public List<Pathfinder> missilesToGuide;
  [Header("Target Data")]
  public string state;
  private Vector3 currentVelocity;
  private Vector3 acceleration;
  private Vector3 previousVelocity;

  private void Start()
  {
    if (this.state == null)
      this.state = "GUIDE";
    this.previousVelocity = Vector3.zero;
  }

  private void Update() => this.RemoveDeadMissiles();

  private void FixedUpdate() => this.UpdateTargetData();

  public void SetTarget(Transform newTarget)
  {
    if ((Object) newTarget == (Object) null)
    {
      this.ClearTargetData();
    }
    else
    {
      this.target = newTarget;
      this.targetPos = this.target.position;
      if ((bool) (Object) newTarget.GetComponent<Rigidbody>())
        this.targetRB = newTarget.GetComponent<Rigidbody>();
      if ((bool) (Object) newTarget.GetComponent<Pathfinder>())
        this.targetPF = newTarget.GetComponent<Pathfinder>();
      this.UpdateTargetData();
    }
  }

  private void UpdateTargetData()
  {
    if ((Object) this.target == (Object) null)
      return;
    this.targetPos = this.target.position;
    if (!((Object) this.targetRB != (Object) null))
      return;
    this.currentVelocity = this.targetRB.velocity;
    this.acceleration = (this.currentVelocity - this.previousVelocity) / Time.fixedDeltaTime;
    this.previousVelocity = this.currentVelocity;
  }

  private void ClearTargetData()
  {
    this.target = (Transform) null;
    this.targetPos = Vector3.zero;
    this.targetRB = (Rigidbody) null;
    this.previousVelocity = Vector3.zero;
    this.targetPF = (Pathfinder) null;
  }

  public Vector3 GetTargetPos(Pathfinder missile)
  {
    if ((Object) this.target == (Object) null)
      return Vector3.zero;
    Vector3 targetPos = this.targetPos;
    if ((Object) this.targetRB != (Object) null)
    {
      if ((double) this.acceleration.sqrMagnitude > 0.0)
      {
        Vector3 position = missile.transform.position;
        float num1 = 0.01f;
        float num2 = 5000f;
        float num3 = float.PositiveInfinity;
        Vector3 vector3 = Vector3.zero;
        int num4 = 20;
        for (int index = 0; index < num4; ++index)
        {
          float t = (float) (((double) num1 + (double) num2) / 2.0);
          Vector3 accPos = this.GetAccPos(t);
          float num5 = Vector3.Distance(position, accPos);
          float num6 = 0.5f * missile.maxThrust * t * t;
          float num7 = Mathf.Abs(num5 - num6);
          if ((double) num6 > (double) num5)
            num2 = t;
          else
            num1 = t;
          if ((double) num7 < (double) num3)
          {
            num3 = num7;
            vector3 = accPos;
          }
        }
        targetPos = vector3;
      }
      else if ((double) this.currentVelocity.sqrMagnitude > 0.0)
      {
        MonoBehaviour.print((object) "guidance: velocity");
        Vector3 position = missile.transform.position;
        float num8 = 0.01f;
        float num9 = 1000f;
        float num10 = float.PositiveInfinity;
        Vector3 vector3_1 = Vector3.zero;
        int num11 = 20;
        for (int index = 0; index < num11; ++index)
        {
          float t = (float) (((double) num8 + (double) num9) / 2.0);
          Vector3 velPos = this.GetVelPos(t);
          float num12 = Vector3.Distance(position, velPos);
          float num13 = 0.5f * missile.maxThrust * t * t;
          float num14 = Mathf.Abs(num12 - num13);
          if ((double) num13 > (double) num12)
            num9 = t;
          else
            num8 = t;
          if ((double) num14 < (double) num10)
          {
            num10 = num14;
            vector3_1 = velPos;
          }
          Vector3 normalized = (velPos - missile.transform.position).normalized;
          Vector3 vector3_2 = missile.transform.position + normalized * num13;
        }
        targetPos = vector3_1;
      }
    }
    if ((double) this.accError > 0.0)
      targetPos += new Vector3(Random.Range(-this.accError, this.accError), Random.Range(-this.accError, this.accError), Random.Range(-this.accError, this.accError));
    if ((bool) (Object) this.target.GetComponent<ShipController>())
    {
      Vector3 zero = Vector3.zero;
      Vector3 vector3 = this.target.GetComponent<ShipController>().GetModuleRandom().position - this.target.position;
      targetPos += vector3;
    }
    return targetPos;
  }

  private Vector3 GetMissilePos(float t, Pathfinder missile, Vector3 dir)
  {
    Rigidbody component = missile.GetComponent<Rigidbody>();
    return missile.transform.position + component.velocity * t + 0.5f * missile.maxThrust * dir * t * t;
  }

  private Vector3 GetVelPos(float t) => this.targetPos + this.currentVelocity * t;

  private Vector3 GetAccPos(float t)
  {
    Vector3 position = this.targetRB.position;
    Vector3 velocity = this.targetRB.velocity;
    Vector3 vector3_1 = (this.targetPF.targetVelocity - this.targetRB.velocity).normalized * (this.targetPF.maxThrust / this.targetRB.mass);
    Vector3 vector3_2 = velocity * t;
    return position + vector3_2 + 0.5f * vector3_1 * t * t;
  }

  private void RemoveDeadMissiles()
  {
    List<Pathfinder> pathfinderList = new List<Pathfinder>();
    foreach (Pathfinder pathfinder in this.missilesToGuide)
    {
      if ((Object) pathfinder == (Object) null)
        pathfinderList.Add(pathfinder);
    }
    foreach (Pathfinder pathfinder in pathfinderList)
      this.missilesToGuide.Remove(pathfinder);
  }
}
