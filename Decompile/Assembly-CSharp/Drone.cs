// Decompiled with JetBrains decompiler
// Type: Drone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Drone : MonoBehaviour, IHealth
{
  [Space(10f)]
  [Header("ARMOR")]
  public float armorThickness;
  public float armorHealth;
  [Header("HEALTH")]
  public float maxHealth;
  public float currentHealth;
  [Header("PARTICLES")]
  [SerializeField]
  private ParticleSystem armorHitPS;
  [SerializeField]
  private ParticleSystem healthHitPS;
  [SerializeField]
  private ParticleSystem explosionPS;

  private void Start()
  {
  }

  private void Update()
  {
  }

  public float GetHealth() => this.currentHealth;

  public bool TryDamageKinetic(float relativeMagnitude)
  {
    float damage = 0.0f;
    double num = (double) this.armorThickness - (double) relativeMagnitude;
    this.DamageArmor(damage);
    if (num > 0.0)
      return false;
    this.ModifyHealth(-relativeMagnitude);
    return true;
  }

  private void DamageArmor(float damage)
  {
    this.armorHealth -= damage;
    if ((double) this.armorHealth >= 0.0)
      return;
    this.armorThickness = 0.0f;
  }

  public void TryDamagePhotonic(float tryDamage)
  {
  }

  public void ModifyHealth(float mod)
  {
    MonoBehaviour.print((object) ("drone modify health: " + mod.ToString()));
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
    MonoBehaviour.print((object) (this.name + " exploding!"));
    ParticleSystem particleSystem = Object.Instantiate<ParticleSystem>(this.explosionPS, pos, this.transform.rotation);
    particleSystem.gameObject.SetActive(true);
    particleSystem.GetComponent<Projectile>().velocity = this.GetComponent<Rigidbody>().velocity;
    particleSystem.Play();
    Object.Destroy((Object) this.gameObject);
  }
}
