// Decompiled with JetBrains decompiler
// Type: Missile2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Missile2 : MonoBehaviour, IHealth, IPool
{
  private Track track;
  [Header("POOL ")]
  [SerializeField]
  private string poolKey = "";
  [Header("MISSILE STATS")]
  public bool isArmed;
  public MissileType type;
  private Pathfinder pf;
  private Rigidbody rb;
  [SerializeField]
  private int damage;
  public Transform target;
  [Space(10f)]
  [Header("ARMOR")]
  public float armorThickness;
  public float armorHealth;
  [Space(10f)]
  [Header("HEALTH")]
  public bool isImmortal;
  public float maxHealth;
  public float currentHealth;
  [SerializeField]
  private ParticleSystem explosionPS;
  private float nextSecUpdate;
  private float updateInterval = 10f;
  private float lastDist = float.PositiveInfinity;
  private bool isInit;

  private void Start() => this.Init();

  private void Init()
  {
    if (this.isInit)
      return;
    this.currentHealth = this.maxHealth;
    this.nextSecUpdate = Time.time + this.updateInterval;
    this.rb = this.GetComponent<Rigidbody>();
    this.track = this.GetComponent<Track>();
    this.pf = this.GetComponent<Pathfinder>();
    this.isInit = true;
  }

  private void Update()
  {
    if ((double) Time.time < (double) this.nextSecUpdate)
      return;
    this.nextSecUpdate = Time.time + this.updateInterval;
    if ((double) Vector3.Distance(this.transform.position, Vector3.zero) > 5500.0)
      this.Explode(this.transform.position);
    if ((Object) this.target != (Object) null)
    {
      float num = Vector3.Distance(this.transform.position, this.target.transform.position);
      if ((double) num > (double) this.lastDist)
        this.Explode(this.transform.position);
      else
        this.lastDist = num;
    }
    else
    {
      if (!this.isArmed || !((Object) this.target == (Object) null))
        return;
      this.Explode(this.transform.position);
    }
  }

  public void Activate(int newFactionID)
  {
    this.Init();
    this.track.enabled = true;
    this.track.factionID = newFactionID;
    this.rb.isKinematic = false;
    this.GetComponent<Collider>().enabled = true;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (!this.isArmed)
      return;
    Vector3 position = this.transform.position;
    Vector3 normalized = (other.transform.position - position).normalized;
    RaycastHit hitInfo;
    if (Physics.Raycast(position, normalized, out hitInfo, 10f))
    {
      Vector3 point = hitInfo.point;
    }
    if (other.GetComponent<IHealth>() != null && this.isArmed)
    {
      Track component = other.transform.root.GetComponent<Track>();
      if ((bool) (Object) component && component.factionID != this.track.factionID)
        this.DealDamageTo(other.GetComponent<IHealth>());
    }
    if (hitInfo.point != Vector3.zero)
      this.Explode(hitInfo.point);
    else
      this.Explode(this.transform.position);
  }

  private void DealDamageTo(IHealth targetHealth)
  {
    targetHealth.TryDamageKinetic((float) this.damage + this.rb.velocity.magnitude);
  }

  public float GetHealth() => this.currentHealth;

  public bool TryDamageKinetic(float relativeMagnitude)
  {
    bool flag = false;
    relativeMagnitude = Mathf.Abs(relativeMagnitude);
    float num1 = 0.1f;
    float damage = relativeMagnitude * num1;
    double num2 = (double) this.armorThickness - (double) damage;
    this.DamageArmor(damage);
    if (num2 < 0.0)
    {
      float num3 = relativeMagnitude;
      flag = true;
      this.ModifyHealth(-num3);
    }
    return flag;
  }

  private void DamageArmor(float damage)
  {
    this.armorHealth -= damage;
    if ((double) this.armorHealth > 0.0)
      return;
    this.armorHealth = 0.0f;
    this.armorThickness = 0.0f;
  }

  public void TryDamagePhotonic(float tryDamage)
  {
    float num1 = 0.0001f;
    float damage = tryDamage * num1;
    if ((double) this.armorHealth > 0.0 || (double) this.armorThickness > 0.0)
    {
      float num2 = this.armorHealth - damage;
      this.DamageArmor(damage);
      if ((double) this.armorHealth > 0.0)
        return;
      this.ModifyHealth(num2 * 10f);
    }
    else
    {
      this.armorHealth = 0.0f;
      this.ModifyHealth((float) (-(double) damage * 10.0));
    }
  }

  public void ModifyHealth(float mod)
  {
    if (this.isImmortal)
      return;
    this.currentHealth += mod;
    if ((double) this.currentHealth > (double) this.maxHealth)
      this.currentHealth = this.maxHealth;
    else if ((double) this.currentHealth < 0.0)
      this.currentHealth = 0.0f;
    if ((double) this.currentHealth != 0.0)
      return;
    this.Explode(this.transform.position);
  }

  private void Explode(Vector3 pos)
  {
    ParticleSystem particleSystem = Object.Instantiate<ParticleSystem>(this.explosionPS, pos, this.transform.rotation);
    particleSystem.gameObject.SetActive(true);
    particleSystem.GetComponent<Projectile>().velocity = this.rb.velocity;
    if ((bool) (Object) particleSystem.GetComponent<AudioSource>())
      particleSystem.GetComponent<AudioSource>().Play();
    particleSystem.Play();
    Object.Destroy((Object) this.gameObject);
  }

  public void OnSpawn()
  {
  }

  public void OnDespawn()
  {
  }

  [SpecialName]
  GameObject IPool.get_gameObject() => this.gameObject;
}
