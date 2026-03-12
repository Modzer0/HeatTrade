// Decompiled with JetBrains decompiler
// Type: AudioManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AudioManager : MonoBehaviour
{
  public static AudioManager current;
  [SerializeField]
  private AudioSource audioSource;
  [SerializeField]
  private AudioSource moneySource;
  [SerializeField]
  private AudioSource zoomSource;
  [SerializeField]
  private List<AudioClip> sfx;
  [SerializeField]
  private bool playMultipleSFX;
  [SerializeField]
  private bool isSetAsSingleton = true;

  private void Awake()
  {
    if (!this.isSetAsSingleton)
      return;
    AudioManager.current = this;
  }

  public void PlaySFX(int index)
  {
    if (!this.playMultipleSFX && this.audioSource.isPlaying || index > this.sfx.Count - 1)
      return;
    this.audioSource.pitch = Random.Range(0.95f, 1.05f);
    this.audioSource.PlayOneShot(this.sfx[index]);
  }

  public void PlayMoney(bool isMoneyGoUp)
  {
    if (!(bool) (Object) this.moneySource || this.moneySource.isPlaying)
      return;
    this.moneySource.pitch = isMoneyGoUp ? 1.05f : 0.95f;
    this.moneySource.Play();
  }

  public void StopMoney()
  {
    if (!(bool) (Object) this.moneySource || !this.moneySource.isPlaying)
      return;
    this.moneySource.Stop();
  }

  public void PlayZoom(int dir)
  {
    if (!(bool) (Object) this.zoomSource)
      return;
    this.zoomSource.Stop();
    this.zoomSource.pitch = dir != 1 ? Random.Range(0.95f, 1f) : Random.Range(1f, 1.05f);
    this.zoomSource.Play();
  }
}
