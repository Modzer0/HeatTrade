// Decompiled with JetBrains decompiler
// Type: SFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SFX : MonoBehaviour
{
  public static SFX current;
  private AudioSource audioSource;
  [SerializeField]
  private AudioSource audioSourceRCS;
  [SerializeField]
  private AudioSource audioSourceEngine;
  [SerializeField]
  private AudioSource alarmMissileAS;
  [SerializeField]
  private AudioSource alarmBreachAS;
  [SerializeField]
  private AudioSource alarmEvacuationAS;
  [SerializeField]
  private List<AudioClip> sfxList;

  private void Awake() => SFX.current = this;

  private void Start() => this.audioSource = this.GetComponent<AudioSource>();

  private void Update()
  {
  }

  public void Play(int i)
  {
    this.audioSource.pitch = Random.Range(0.75f, 1.25f);
    this.audioSource.PlayOneShot(this.sfxList[i]);
  }

  public void PlayRCS()
  {
    if (this.audioSourceRCS.isPlaying)
      return;
    this.audioSourceRCS.pitch = Random.Range(0.75f, 1.25f);
    this.audioSourceRCS.Play();
  }

  public void StopRCS() => this.audioSourceRCS.Stop();

  public void PlayEngine()
  {
    if (this.audioSourceEngine.isPlaying)
      return;
    this.audioSourceEngine.pitch = Random.Range(0.75f, 1.25f);
    this.audioSourceEngine.Play();
  }

  public void StopEngine() => this.audioSourceEngine.Stop();
}
