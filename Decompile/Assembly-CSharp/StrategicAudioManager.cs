// Decompiled with JetBrains decompiler
// Type: StrategicAudioManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class StrategicAudioManager : MonoBehaviour
{
  public static StrategicAudioManager current;
  private AudioSource audioSource;

  private void Awake()
  {
    if ((Object) StrategicAudioManager.current != (Object) null && (Object) StrategicAudioManager.current != (Object) this)
    {
      Object.Destroy((Object) this.gameObject);
    }
    else
    {
      StrategicAudioManager.current = this;
      this.audioSource = this.GetComponent<AudioSource>();
    }
  }

  public void SetMainEngine(bool isOn)
  {
    if (isOn)
    {
      if (this.audioSource.isPlaying)
        return;
      this.audioSource.Play();
    }
    else
    {
      if (!this.audioSource.isPlaying)
        return;
      this.audioSource.Stop();
    }
  }
}
