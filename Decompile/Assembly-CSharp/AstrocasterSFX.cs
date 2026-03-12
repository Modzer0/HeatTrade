// Decompiled with JetBrains decompiler
// Type: AstrocasterSFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AstrocasterSFX : MonoBehaviour
{
  [SerializeField]
  private AudioSource source;
  [Header("UI")]
  [SerializeField]
  private AudioClip audioStartup;
  [SerializeField]
  private AudioClip audioExit;
  [SerializeField]
  private AudioClip audioClick;
  [SerializeField]
  private AudioClip audioCashout;
  [Header("WINS")]
  [SerializeField]
  private AudioClip audioWin1;
  [SerializeField]
  private AudioClip audioWin2;
  [SerializeField]
  private AudioClip audioWin3;
  [SerializeField]
  private AudioClip audioWin4;
  [SerializeField]
  private AudioClip audioWin5;
  [Header("OTHER")]
  [SerializeField]
  private AudioClip audioCast;
  [SerializeField]
  private AudioClip audioCoin;
  [SerializeField]
  private AudioClip audioNibble;
  [SerializeField]
  private AudioClip audioLose;

  public void Play(string audioName)
  {
    this.source.pitch = Random.Range(0.9f, 1.1f);
    switch (audioName)
    {
      case "start":
        this.source.PlayOneShot(this.audioStartup);
        break;
      case "exit":
        this.source.PlayOneShot(this.audioExit);
        break;
      case "click":
        this.source.PlayOneShot(this.audioClick);
        break;
      case "cashout":
        this.source.PlayOneShot(this.audioCashout);
        break;
      case "win1":
        this.source.PlayOneShot(this.audioWin1);
        break;
      case "win2":
        this.source.PlayOneShot(this.audioWin2);
        break;
      case "win3":
        this.source.PlayOneShot(this.audioWin3);
        break;
      case "win4":
        this.source.PlayOneShot(this.audioWin4);
        break;
      case "win5":
        this.source.PlayOneShot(this.audioWin5);
        break;
      case "cast":
        this.source.PlayOneShot(this.audioCast);
        break;
      case "coin":
        this.source.PlayOneShot(this.audioCoin);
        break;
      case "nibble":
        this.source.PlayOneShot(this.audioNibble);
        break;
      case "lose":
        this.source.PlayOneShot(this.audioLose);
        break;
    }
  }
}
