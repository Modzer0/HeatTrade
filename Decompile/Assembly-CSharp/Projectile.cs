// Decompiled with JetBrains decompiler
// Type: Projectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Projectile : MonoBehaviour, IPool
{
  private Pool pool;
  [Header("POOL")]
  [SerializeField]
  private string poolKey = "";
  [SerializeField]
  private string debrisPoolKey = "";
  [SerializeField]
  private string holePoolKey = "";
  [Header("PROJECTILE")]
  [SerializeField]
  private ProjectileType type;
  [SerializeField]
  private float lifetime;
  public Vector3 velocity;
  [SerializeField]
  private bool isOverpenetrate;
  private TrailRenderer trail;
  public bool hasHit;

  private void Start()
  {
    this.pool = Pool.current;
    this.trail = this.GetComponent<TrailRenderer>();
    this.transform.rotation = Quaternion.LookRotation(this.velocity);
  }

  private void Update() => this.transform.position += this.velocity * Time.deltaTime;

  private void OnTriggerEnter(Collider other)
  {
    this.hasHit = true;
    Rigidbody component = other.transform.root.GetComponent<Rigidbody>();
    float num1 = (this.velocity - ((Object) component != (Object) null ? component.velocity : Vector3.zero)).magnitude / 10f;
    if (this.type == ProjectileType.BULLET && other.gameObject.layer == LayerMask.NameToLayer("ExhaustPlume"))
    {
      this.StopCoroutine((IEnumerator) this.StartLifetime());
      this.Death();
    }
    else
    {
      if (other.GetComponent<IHealth>() != null)
      {
        IHealth health = (IHealth) null;
        if (other.transform.GetComponent<IHealth>() != null)
          health = other.transform.GetComponent<IHealth>();
        else if (other.GetComponentInParent<IHealth>() != null)
          health = other.GetComponentInParent<IHealth>();
        if (!health.TryDamageKinetic(-num1))
        {
          Vector3 vector3 = -this.velocity.normalized;
          float maxInclusive = 180f;
          float num2 = Random.Range(-maxInclusive, maxInclusive);
          double x = (double) Random.Range(-maxInclusive, maxInclusive);
          float num3 = Random.Range(-maxInclusive, maxInclusive);
          double y = (double) num2;
          double z = (double) num3;
          this.velocity = Quaternion.Euler((float) x, (float) y, (float) z) * vector3 * this.velocity.magnitude;
          this.transform.rotation = Quaternion.LookRotation(this.velocity);
          return;
        }
      }
      if (this.debrisPoolKey != string.Empty)
        this.SpawnDebris();
      if (this.holePoolKey != string.Empty)
        this.SpawnDecal(other.transform);
      if (this.isOverpenetrate)
        return;
      this.StopCoroutine((IEnumerator) this.StartLifetime());
      this.Death();
    }
  }

  public void Init() => this.StartCoroutine((IEnumerator) this.StartLifetime());

  private IEnumerator StartLifetime()
  {
    yield return (object) new WaitForSeconds(this.lifetime);
    this.Death();
  }

  private void SpawnDebris()
  {
    Vector3 vector3 = this.transform.position - this.transform.forward;
    this.pool.Get(this.debrisPoolKey, this.transform.position, this.transform.rotation);
  }

  private void SpawnDecal(Transform newParent)
  {
    this.pool.Get(this.holePoolKey, this.transform.position, this.transform.rotation).gameObject.transform.parent = newParent;
  }

  private void Death() => this.pool.Return(this.poolKey, (IPool) this);

  public void OnSpawn() => this.hasHit = false;

  public void OnDespawn()
  {
    if (!(bool) (Object) this.trail)
      return;
    this.trail.Clear();
  }

  [SpecialName]
  GameObject IPool.get_gameObject() => this.gameObject;
}
