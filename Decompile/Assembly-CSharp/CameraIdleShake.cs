// Decompiled with JetBrains decompiler
// Type: CameraIdleShake
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class CameraIdleShake : MonoBehaviour
{
  public float shakeAmountDefault = 1f;
  public float shakeAmount = 0.1f;
  public float shakeSpeed = 1f;
  private Vector3 startPosition;
  private float offset;
  private bool isOn = true;

  private void Start()
  {
    this.startPosition = this.transform.position;
    this.offset = Random.Range(0.0f, 1000f);
  }

  private void Update()
  {
    if (!this.isOn)
      return;
    this.transform.position = this.startPosition + new Vector3(Mathf.PerlinNoise(Time.time * this.shakeSpeed + this.offset, 0.0f) - 0.5f, Mathf.PerlinNoise(0.0f, Time.time * this.shakeSpeed + this.offset) - 0.5f, 0.0f) * this.shakeAmount;
  }

  public void Shake() => this.StartCoroutine((IEnumerator) this.IShake());

  private IEnumerator IShake()
  {
    float t = 0.5f;
    while ((double) t > 0.0)
    {
      t -= Time.deltaTime;
      this.shakeSpeed += t * 0.05f;
      this.shakeAmount += t * 0.25f;
      yield return (object) null;
    }
    this.shakeSpeed = 1f;
    this.shakeAmount = this.shakeAmountDefault;
  }

  public void Toggle(bool newIsOn)
  {
    this.isOn = newIsOn;
    if (!this.isOn)
      return;
    this.StartCoroutine((IEnumerator) this.IShake());
  }
}
