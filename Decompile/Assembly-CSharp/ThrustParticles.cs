// Decompiled with JetBrains decompiler
// Type: ThrustParticles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ThrustParticles : MonoBehaviour
{
  public float exhaustRatio = 1f;
  [SerializeField]
  private List<ParticleSystem> mainThrusters;
  [SerializeField]
  private List<GameObject> mainFlares;
  [SerializeField]
  private List<ParticleSystem> mainTrails;
  [SerializeField]
  private List<ParticleSystem> ffThrusters;
  [SerializeField]
  private List<ParticleSystem> flThrusters;
  [SerializeField]
  private List<ParticleSystem> frThrusters;
  [SerializeField]
  private List<ParticleSystem> fuThrusters;
  [SerializeField]
  private List<ParticleSystem> fdThrusters;
  [SerializeField]
  private List<ParticleSystem> bbThrusters;
  [SerializeField]
  private List<ParticleSystem> blThrusters;
  [SerializeField]
  private List<ParticleSystem> brThrusters;
  [SerializeField]
  private List<ParticleSystem> buThrusters;
  [SerializeField]
  private List<ParticleSystem> bdThrusters;
  [Space(10f)]
  [Header("AUDIO")]
  private bool isUsingRCS;
  [SerializeField]
  private AudioClip clipRCS;
  [SerializeField]
  private AudioSource audioThruster;
  [SerializeField]
  private AudioSource audioRCS;
  [Header("NOZZLE")]
  [SerializeField]
  private T_Nozzle nozzle;
  [Header("EXHAUST PLUME")]
  [SerializeField]
  private float startSize = 0.1f;
  [SerializeField]
  private float startLifetime = 0.5f;
  [Header("DEBUG")]
  [SerializeField]
  private bool isMainThrusterAlwaysOn;

  private void Start()
  {
    this.audioThruster = this.GetComponentInChildren<AudioSource>();
    this.startSize = this.mainThrusters[0].startSize;
    this.startLifetime = this.mainThrusters[0].startLifetime;
    if ((bool) (Object) this.nozzle)
      return;
    this.nozzle = this.transform.parent.GetComponent<T_Nozzle>();
  }

  private void Update()
  {
    if ((bool) (Object) this.audioRCS && this.isUsingRCS && !this.audioRCS.isPlaying)
    {
      this.audioRCS.Play();
    }
    else
    {
      if (!(bool) (Object) this.audioRCS || this.isUsingRCS || !this.audioRCS.isPlaying)
        return;
      this.audioRCS.Stop();
    }
  }

  public void SetMain(bool isOn)
  {
    if (isOn || this.isMainThrusterAlwaysOn)
    {
      foreach (ParticleSystem mainThruster in this.mainThrusters)
      {
        if ((bool) (Object) mainThruster && !mainThruster.isPlaying)
          mainThruster.Play();
        mainThruster.emissionRate = 200f;
        mainThruster.startSize = this.startSize * this.exhaustRatio;
        mainThruster.startLifetime = this.startLifetime * this.exhaustRatio;
      }
      foreach (GameObject mainFlare in this.mainFlares)
        mainFlare.SetActive(true);
      foreach (ParticleSystem mainTrail in this.mainTrails)
      {
        if (!mainTrail.isPlaying)
          mainTrail.Play();
        mainTrail.emissionRate = 50f;
        mainTrail.trails.attachRibbonsToTransform = true;
      }
      if ((bool) (Object) this.nozzle)
        this.nozzle.isOn = true;
      if (!(bool) (Object) this.audioThruster || this.audioThruster.isPlaying)
        return;
      this.audioThruster.Play();
    }
    else
    {
      foreach (ParticleSystem mainThruster in this.mainThrusters)
        mainThruster.emissionRate = 0.0f;
      foreach (GameObject mainFlare in this.mainFlares)
        mainFlare.SetActive(false);
      foreach (ParticleSystem mainTrail in this.mainTrails)
      {
        if (!((Object) mainTrail == (Object) null))
        {
          mainTrail.emissionRate = 0.0f;
          mainTrail.trails.attachRibbonsToTransform = false;
        }
      }
      if ((bool) (Object) this.nozzle)
        this.nozzle.isOn = false;
      if (!(bool) (Object) this.audioThruster || !this.audioThruster.isPlaying)
        return;
      this.audioThruster.Stop();
    }
  }

  public void SetRCS(float dir)
  {
    if (!this.isUsingRCS)
      return;
    if ((double) dir > 0.10000000149011612)
    {
      this.PlayAll(this.bbThrusters);
      this.StopAll(this.ffThrusters);
    }
    else if ((double) dir < -0.10000000149011612)
    {
      this.StopAll(this.bbThrusters);
      this.PlayAll(this.ffThrusters);
    }
    else
    {
      this.StopAll(this.ffThrusters);
      this.StopAll(this.bbThrusters);
    }
  }

  public void Pitch(int dir)
  {
    if (dir > 0)
    {
      this.PlayAll(this.fuThrusters);
      this.PlayAll(this.bdThrusters);
      this.StopAll(this.fdThrusters);
      this.StopAll(this.buThrusters);
    }
    else if (dir < 0)
    {
      this.StopAll(this.fuThrusters);
      this.StopAll(this.bdThrusters);
      this.PlayAll(this.fdThrusters);
      this.PlayAll(this.buThrusters);
    }
    else
    {
      this.StopAll(this.fuThrusters);
      this.StopAll(this.bdThrusters);
      this.StopAll(this.fdThrusters);
      this.StopAll(this.buThrusters);
    }
  }

  public void Yaw(int dir)
  {
    if (dir > 0)
    {
      this.PlayAll(this.flThrusters);
      this.PlayAll(this.brThrusters);
      this.StopAll(this.frThrusters);
      this.StopAll(this.blThrusters);
    }
    else if (dir < 0)
    {
      this.StopAll(this.flThrusters);
      this.StopAll(this.brThrusters);
      this.PlayAll(this.frThrusters);
      this.PlayAll(this.blThrusters);
    }
    else
    {
      this.StopAll(this.flThrusters);
      this.StopAll(this.brThrusters);
      this.StopAll(this.frThrusters);
      this.StopAll(this.blThrusters);
    }
  }

  public void Roll(int dir)
  {
    if (dir > 0)
    {
      this.PlayAll(this.fuThrusters);
      this.PlayAll(this.bdThrusters);
      this.StopAll(this.fdThrusters);
      this.StopAll(this.buThrusters);
    }
    else if (dir < 0)
    {
      this.StopAll(this.fuThrusters);
      this.StopAll(this.bdThrusters);
      this.PlayAll(this.fdThrusters);
      this.PlayAll(this.buThrusters);
    }
    else
    {
      this.StopAll(this.fuThrusters);
      this.StopAll(this.bdThrusters);
      this.StopAll(this.fdThrusters);
      this.StopAll(this.buThrusters);
    }
  }

  private void PlayAll(List<ParticleSystem> thrusters)
  {
    foreach (ParticleSystem thruster in thrusters)
    {
      if (!thruster.isPlaying)
        thruster.Play();
      thruster.emissionRate = 25f;
      this.isUsingRCS = true;
    }
  }

  private void StopAll(List<ParticleSystem> thrusters)
  {
    foreach (ParticleSystem thruster in thrusters)
      thruster.Stop();
  }

  public void Clear()
  {
    this.Pitch(0);
    this.Yaw(0);
    this.Roll(0);
    this.SetMain(false);
  }

  public void EntryBurn() => this.StartCoroutine((IEnumerator) this.StartEntryBurn());

  private IEnumerator StartEntryBurn()
  {
    float t = 0.0f;
    while ((double) t < 5.0)
    {
      t += Time.deltaTime;
      this.SetMain(true);
      yield return (object) null;
    }
    this.SetMain(false);
  }

  public void OnShipDeath()
  {
    this.StopAll(this.ffThrusters);
    this.StopAll(this.bbThrusters);
    this.StopAll(this.fuThrusters);
    this.StopAll(this.bdThrusters);
    this.StopAll(this.fdThrusters);
    this.StopAll(this.buThrusters);
    this.StopAll(this.flThrusters);
    this.StopAll(this.brThrusters);
    this.StopAll(this.frThrusters);
    this.StopAll(this.blThrusters);
    this.enabled = false;
  }
}
