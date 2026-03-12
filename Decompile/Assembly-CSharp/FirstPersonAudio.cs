// Decompiled with JetBrains decompiler
// Type: FirstPersonAudio
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class FirstPersonAudio : MonoBehaviour
{
  public FirstPersonMovement character;
  public GroundCheck groundCheck;
  [Header("Step")]
  public AudioSource stepAudio;
  public AudioSource runningAudio;
  [Tooltip("Minimum velocity for moving audio to play")]
  public float velocityThreshold = 0.01f;
  private Vector2 lastCharacterPosition;
  [Header("Landing")]
  public AudioSource landingAudio;
  public AudioClip[] landingSFX;
  [Header("Jump")]
  public Jump jump;
  public AudioSource jumpAudio;
  public AudioClip[] jumpSFX;
  [Header("Crouch")]
  public Crouch crouch;
  public AudioSource crouchStartAudio;
  public AudioSource crouchedAudio;
  public AudioSource crouchEndAudio;
  public AudioClip[] crouchStartSFX;
  public AudioClip[] crouchEndSFX;

  private Vector2 CurrentCharacterPosition
  {
    get => new Vector2(this.character.transform.position.x, this.character.transform.position.z);
  }

  private AudioSource[] MovingAudios
  {
    get
    {
      return new AudioSource[3]
      {
        this.stepAudio,
        this.runningAudio,
        this.crouchedAudio
      };
    }
  }

  private void Reset()
  {
    this.character = this.GetComponentInParent<FirstPersonMovement>();
    this.groundCheck = (this.transform.parent ?? this.transform).GetComponentInChildren<GroundCheck>();
    this.stepAudio = this.GetOrCreateAudioSource("Step Audio");
    this.runningAudio = this.GetOrCreateAudioSource("Running Audio");
    this.landingAudio = this.GetOrCreateAudioSource("Landing Audio");
    this.jump = this.GetComponentInParent<Jump>();
    if ((bool) (UnityEngine.Object) this.jump)
      this.jumpAudio = this.GetOrCreateAudioSource("Jump audio");
    this.crouch = this.GetComponentInParent<Crouch>();
    if (!(bool) (UnityEngine.Object) this.crouch)
      return;
    this.crouchStartAudio = this.GetOrCreateAudioSource("Crouch Start Audio");
    this.crouchStartAudio = this.GetOrCreateAudioSource("Crouched Audio");
    this.crouchStartAudio = this.GetOrCreateAudioSource("Crouch End Audio");
  }

  private void OnEnable() => this.SubscribeToEvents();

  private void OnDisable() => this.UnsubscribeToEvents();

  private void FixedUpdate()
  {
    if ((double) Vector3.Distance((Vector3) this.CurrentCharacterPosition, (Vector3) this.lastCharacterPosition) >= (double) this.velocityThreshold && (bool) (UnityEngine.Object) this.groundCheck && this.groundCheck.isGrounded)
    {
      if ((bool) (UnityEngine.Object) this.crouch && this.crouch.IsCrouched)
        this.SetPlayingMovingAudio(this.crouchedAudio);
      else if (this.character.IsRunning)
        this.SetPlayingMovingAudio(this.runningAudio);
      else
        this.SetPlayingMovingAudio(this.stepAudio);
    }
    else
      this.SetPlayingMovingAudio((AudioSource) null);
    this.lastCharacterPosition = this.CurrentCharacterPosition;
  }

  private void SetPlayingMovingAudio(AudioSource audioToPlay)
  {
    foreach (AudioSource audioSource in ((IEnumerable<AudioSource>) this.MovingAudios).Where<AudioSource>((Func<AudioSource, bool>) (audio => (UnityEngine.Object) audio != (UnityEngine.Object) audioToPlay && (UnityEngine.Object) audio != (UnityEngine.Object) null)))
      audioSource.Pause();
    if (!(bool) (UnityEngine.Object) audioToPlay || audioToPlay.isPlaying)
      return;
    audioToPlay.Play();
  }

  private void PlayLandingAudio()
  {
    FirstPersonAudio.PlayRandomClip(this.landingAudio, this.landingSFX);
  }

  private void PlayJumpAudio() => FirstPersonAudio.PlayRandomClip(this.jumpAudio, this.jumpSFX);

  private void PlayCrouchStartAudio()
  {
    FirstPersonAudio.PlayRandomClip(this.crouchStartAudio, this.crouchStartSFX);
  }

  private void PlayCrouchEndAudio()
  {
    FirstPersonAudio.PlayRandomClip(this.crouchEndAudio, this.crouchEndSFX);
  }

  private void SubscribeToEvents()
  {
    this.groundCheck.Grounded += new Action(this.PlayLandingAudio);
    if ((bool) (UnityEngine.Object) this.jump)
      this.jump.Jumped += new Action(this.PlayJumpAudio);
    if (!(bool) (UnityEngine.Object) this.crouch)
      return;
    this.crouch.CrouchStart += new Action(this.PlayCrouchStartAudio);
    this.crouch.CrouchEnd += new Action(this.PlayCrouchEndAudio);
  }

  private void UnsubscribeToEvents()
  {
    this.groundCheck.Grounded -= new Action(this.PlayLandingAudio);
    if ((bool) (UnityEngine.Object) this.jump)
      this.jump.Jumped -= new Action(this.PlayJumpAudio);
    if (!(bool) (UnityEngine.Object) this.crouch)
      return;
    this.crouch.CrouchStart -= new Action(this.PlayCrouchStartAudio);
    this.crouch.CrouchEnd -= new Action(this.PlayCrouchEndAudio);
  }

  private AudioSource GetOrCreateAudioSource(string name)
  {
    AudioSource audioSource1 = Array.Find<AudioSource>(this.GetComponentsInChildren<AudioSource>(), (Predicate<AudioSource>) (a => a.name == name));
    if ((bool) (UnityEngine.Object) audioSource1)
      return audioSource1;
    AudioSource audioSource2 = new GameObject(name).AddComponent<AudioSource>();
    audioSource2.spatialBlend = 1f;
    audioSource2.playOnAwake = false;
    audioSource2.transform.SetParent(this.transform, false);
    return audioSource2;
  }

  private static void PlayRandomClip(AudioSource audio, AudioClip[] clips)
  {
    if (!(bool) (UnityEngine.Object) audio || clips.Length == 0)
      return;
    AudioClip clip = clips[UnityEngine.Random.Range(0, clips.Length)];
    if (clips.Length > 1)
    {
      while ((UnityEngine.Object) clip == (UnityEngine.Object) audio.clip)
        clip = clips[UnityEngine.Random.Range(0, clips.Length)];
    }
    audio.clip = clip;
    audio.Play();
  }
}
