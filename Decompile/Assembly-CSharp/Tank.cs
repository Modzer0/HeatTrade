// Decompiled with JetBrains decompiler
// Type: Tank
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

#nullable disable
public class Tank : MonoBehaviour
{
  public float Speed;
  public float Health;
  public float TankLifeTime;
  public float[] RandomTurnRate;
  public float LerpSpeed;
  public GameObject DamageFX;
  public GameObject ExplosionFX;
  private Rigidbody _rb;
  private bool _isDead;
  private NavMeshAgent _agent;
  private Transform[] _waypoints;
  private Vector3 _targetPoint;

  private void Start()
  {
    this._rb = this.GetComponent<Rigidbody>();
    this.transform.Rotate(0.0f, Random.Range(this.RandomTurnRate[0], this.RandomTurnRate[1]), 0.0f);
    this.DamageFX.SetActive(false);
    this.ExplosionFX.SetActive(false);
    this._agent = this.GetComponent<NavMeshAgent>();
    this._waypoints = Manager.GetInstance().Waypoints1;
    this.RandomPoint(this._waypoints[Random.Range(0, this._waypoints.Length)].position, 2f, out this._targetPoint);
    this.NavAgentControl(true, false);
    this.Invoke("Destroy", this.TankLifeTime);
  }

  private void Update()
  {
    if (this._isDead)
      this.Speed = Mathf.Lerp(this.Speed, 0.0f, this.LerpSpeed * Time.fixedDeltaTime);
    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(this._agent.desiredVelocity), Time.deltaTime * 5f);
    if ((double) Vector3.Distance(this.transform.position, this._targetPoint) < (double) this._agent.stoppingDistance)
      this.RandomPoint(this._waypoints[Random.Range(0, this._waypoints.Length)].position, 2f, out this._targetPoint);
    if (!this._agent.isPathStale && this._agent.hasPath && this._agent.pathStatus == NavMeshPathStatus.PathComplete)
      return;
    this.RandomPoint(this._waypoints[Random.Range(0, this._waypoints.Length)].position, 2f, out this._targetPoint);
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (LayerMask.LayerToName(collision.gameObject.layer) != "Projectile" || this._isDead)
      return;
    --this.Health;
    if ((double) this.Health > 0.0)
      return;
    this._isDead = true;
    this.DamageFX.SetActive(true);
    this._rb.useGravity = true;
    this.Invoke("Destroy", 10f);
  }

  public void NavAgentControl(bool positionUpdate, bool rotationUpdate)
  {
    if (!(bool) (Object) this._agent)
      return;
    this._agent.updatePosition = positionUpdate;
    this._agent.updateRotation = rotationUpdate;
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
    this._agent.SetDestination(result);
  }

  private void Destroy()
  {
    if (!this.DamageFX.activeInHierarchy)
      this.DamageFX.SetActive(true);
    this.DamageFX.transform.SetParent((Transform) null);
    this.ExplosionFX.SetActive(true);
    this.ExplosionFX.transform.SetParent((Transform) null);
    Object.Destroy((Object) this.gameObject);
  }
}
