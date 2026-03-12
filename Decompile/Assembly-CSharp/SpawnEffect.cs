// Decompiled with JetBrains decompiler
// Type: SpawnEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SpawnEffect : MonoBehaviour
{
  public float spawnEffectTime = 2f;
  public float pause = 1f;
  public AnimationCurve fadeIn;
  private ParticleSystem ps;
  private float timer;
  private Renderer _renderer;
  private int shaderProperty;

  private void Start()
  {
    this.shaderProperty = Shader.PropertyToID("_cutoff");
    this._renderer = this.GetComponent<Renderer>();
    this.ps = this.GetComponentInChildren<ParticleSystem>();
    this.ps.main.duration = this.spawnEffectTime;
    this.ps.Play();
  }

  private void Update()
  {
    if ((double) this.timer < (double) this.spawnEffectTime + (double) this.pause)
    {
      this.timer += Time.deltaTime;
    }
    else
    {
      this.ps.Play();
      this.timer = 0.0f;
    }
    this._renderer.material.SetFloat(this.shaderProperty, this.fadeIn.Evaluate(Mathf.InverseLerp(0.0f, this.spawnEffectTime, this.timer)));
  }
}
