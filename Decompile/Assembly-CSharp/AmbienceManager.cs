// Decompiled with JetBrains decompiler
// Type: AmbienceManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AmbienceManager : MonoBehaviour
{
  public static AmbienceManager current;
  [SerializeField]
  private AudioSource audioSource;
  private int intensity;
  [Header("AMBIENCE")]
  [SerializeField]
  private List<AudioClip> ambienceCalm;
  [SerializeField]
  private List<AudioClip> ambienceCreep;
  [SerializeField]
  private List<AudioClip> ambienceTense;
  [SerializeField]
  private List<AudioClip> ambienceCombat;
  [SerializeField]
  private List<AudioClip> ambienceDoom;

  private void Awake()
  {
    AmbienceManager.current = this;
    this.audioSource = this.GetComponent<AudioSource>();
  }

  private void Start()
  {
    this.audioSource.clip = this.ambienceCalm[Random.Range(0, this.ambienceCalm.Count)];
    this.audioSource.Play();
  }

  public void ModIntensity(int mod) => this.intensity += mod;
}
