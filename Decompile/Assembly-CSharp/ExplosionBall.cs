// Decompiled with JetBrains decompiler
// Type: ExplosionBall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class ExplosionBall : MonoBehaviour
{
  private ParticleSystem ps;
  private float startScale = 100f;
  private Vector3 startScaleVector = new Vector3(100f, 100f, 100f);
  private float currentScale;
  private float timeElapsed;
  private float blowupTime = 0.25f;
  private float explosionDuration = 3f;

  private void Start() => this.StartCoroutine((IEnumerator) this.Explode());

  private IEnumerator Explode()
  {
    ExplosionBall explosionBall = this;
    while ((double) explosionBall.timeElapsed - (double) explosionBall.blowupTime < (double) explosionBall.explosionDuration)
    {
      explosionBall.timeElapsed += Time.deltaTime;
      if ((double) explosionBall.timeElapsed < (double) explosionBall.blowupTime)
      {
        float t = Mathf.Clamp01(explosionBall.timeElapsed / explosionBall.blowupTime);
        explosionBall.SetScale(0.0f, explosionBall.startScale, t);
      }
      else
      {
        float t = Mathf.Clamp01((explosionBall.timeElapsed - explosionBall.blowupTime) / explosionBall.explosionDuration);
        explosionBall.SetScale(explosionBall.startScale, 0.0f, t);
      }
      yield return (object) null;
    }
    explosionBall.transform.localScale = Vector3.zero;
    explosionBall.gameObject.SetActive(false);
  }

  private void SetScale(float initialScale, float targetScale, float t)
  {
    this.currentScale = Mathf.Lerp(initialScale, targetScale, t);
    this.transform.localScale = new Vector3(this.currentScale, this.currentScale, this.currentScale);
  }

  private void OnTriggerEnter(Collider other)
  {
    IHealth health = (IHealth) null;
    if (other.transform.GetComponent<IHealth>() != null)
      health = other.transform.GetComponent<IHealth>();
    else if (other.transform.GetComponentInParent<IHealth>() != null)
      health = other.transform.GetComponentInParent<IHealth>();
    if (health == null)
      return;
    float damage = 1E+07f / Vector3.Distance(this.transform.position, other.transform.position);
    health.TryDamagePhotonic(damage);
  }
}
