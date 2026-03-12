// Decompiled with JetBrains decompiler
// Type: EnemySoldier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

#nullable disable
public class EnemySoldier : MonoBehaviour
{
  public GameObject Parachute;
  public NavMeshAgent Agent;
  public float SoldierLifeTime;
  public Transform Anchor;
  private Transform[] _waypoints;
  private Vector3 _targetPoint;
  private Rigidbody[] _bodyPartsRB;
  private Collider[] _bodyPartsCollder;
  private Animator _animator;
  private bool _isdead;
  private int _animatorRunHash = Animator.StringToHash("Run");

  private void Awake()
  {
    if ((bool) (Object) this.Anchor)
    {
      this._bodyPartsRB = this.Anchor.GetComponentsInChildren<Rigidbody>();
      this._bodyPartsCollder = this.Anchor.GetComponentsInChildren<Collider>();
    }
    this._animator = this.GetComponent<Animator>();
  }

  private void Start()
  {
    if ((bool) (Object) this.Anchor)
    {
      for (int index = 0; index < this._bodyPartsRB.Length; ++index)
      {
        this._bodyPartsRB[index].isKinematic = true;
        this._bodyPartsCollder[index].enabled = false;
      }
    }
    this._waypoints = Manager.GetInstance().Waypoints1;
    this.Invoke("Destroy", this.SoldierLifeTime);
  }

  private void Update()
  {
    if (this.Agent.enabled && (double) this.Agent.speed > 0.1)
      this._animator.SetBool(this._animatorRunHash, true);
    else if ((double) this.Agent.speed < 0.1)
      this._animator.SetBool(this._animatorRunHash, false);
    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(this.Agent.desiredVelocity), Time.deltaTime * 5f);
    if (!this.Agent.enabled)
      return;
    if ((double) Vector3.Distance(this.transform.position, this._targetPoint) < (double) this.Agent.stoppingDistance)
      this.RandomPoint(this._waypoints[Random.Range(0, this._waypoints.Length)].position, 2f, out this._targetPoint);
    if (!this.Agent.isPathStale && this.Agent.hasPath && this.Agent.pathStatus == NavMeshPathStatus.PathComplete)
      return;
    this.RandomPoint(this._waypoints[Random.Range(0, this._waypoints.Length)].position, 2f, out this._targetPoint);
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (this._isdead)
      return;
    if (LayerMask.LayerToName(collision.gameObject.layer) == "Projectile")
      this.Dead();
    if (!(LayerMask.LayerToName(collision.gameObject.layer) == "Ground"))
      return;
    this.Parachute.SetActive(false);
    this.Agent.enabled = true;
    this._animator.applyRootMotion = true;
    this.NavAgentControl(true, false);
    this.RandomPoint(this._waypoints[Random.Range(0, this._waypoints.Length)].position, 2f, out this._targetPoint);
  }

  private void RandomPoint(Vector3 center, float range, out Vector3 result)
  {
    result = Vector3.zero;
    for (int index = 0; index < 10; ++index)
    {
      NavMeshHit hit;
      if (NavMesh.SamplePosition((center + (Vector3) Random.insideUnitCircle * range) with
      {
        y = 0.0f
      }, out hit, 2f, -1))
      {
        result = hit.position;
        break;
      }
    }
    this.Agent.SetDestination(result);
  }

  public void NavAgentControl(bool positionUpdate, bool rotationUpdate)
  {
    if (!(bool) (Object) this.Agent)
      return;
    this.Agent.updatePosition = positionUpdate;
    this.Agent.updateRotation = rotationUpdate;
  }

  private void Dead()
  {
    this._isdead = true;
    if (this.Parachute.activeInHierarchy)
      this.Parachute.SetActive(false);
    if ((bool) (Object) this.Anchor)
      this.Anchor.transform.SetParent((Transform) null);
    if (this.Agent.enabled)
      this.Agent.isStopped = true;
    this._animator.enabled = false;
    if ((bool) (Object) this.Anchor)
    {
      for (int index = 0; index < this._bodyPartsRB.Length; ++index)
      {
        this._bodyPartsRB[index].isKinematic = false;
        this._bodyPartsCollder[index].enabled = true;
      }
    }
    this.Destroy();
  }

  private void Destroy() => Object.Destroy((Object) this.gameObject);
}
