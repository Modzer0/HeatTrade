// Decompiled with JetBrains decompiler
// Type: ColorblindManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

#nullable disable
public class ColorblindManager : MonoBehaviour
{
  private Volume colorblindVolume;
  private ChannelMixer channelMixer;

  private void Awake() => this.Init();

  private void Init()
  {
    if ((Object) this.channelMixer != (Object) null)
      return;
    foreach (Volume volume in Object.FindObjectsOfType<Volume>())
    {
      if (volume.gameObject.name == "VOLUME - Colorblind")
      {
        this.colorblindVolume = volume;
        this.colorblindVolume.profile.TryGet<ChannelMixer>(out this.channelMixer);
        return;
      }
    }
    if (!((Object) this.channelMixer == (Object) null))
      return;
    Debug.LogError((object) "ColorblindManager: Could not find Volume Profile");
  }

  public void OnColorblindSliderChanged(float newVal)
  {
    this.Init();
    if (!(bool) (Object) this.colorblindVolume)
      return;
    this.colorblindVolume.weight = newVal;
  }

  public void OnColorblindDropdownChanged(int index)
  {
    this.Init();
    if (!(bool) (Object) this.channelMixer)
      return;
    this.ResetMixer();
    switch (index)
    {
      case 1:
        this.channelMixer.redOutRedIn.value = 56.667f;
        this.channelMixer.redOutGreenIn.value = 43.333f;
        this.channelMixer.redOutBlueIn.value = 0.0f;
        this.channelMixer.greenOutRedIn.value = 55.833f;
        this.channelMixer.greenOutGreenIn.value = 44.167f;
        this.channelMixer.greenOutBlueIn.value = 0.0f;
        this.channelMixer.blueOutRedIn.value = 0.0f;
        this.channelMixer.blueOutGreenIn.value = 24.167f;
        this.channelMixer.blueOutBlueIn.value = 75.833f;
        break;
      case 2:
        this.channelMixer.redOutRedIn.value = 62.5f;
        this.channelMixer.redOutGreenIn.value = 37.5f;
        this.channelMixer.redOutBlueIn.value = 0.0f;
        this.channelMixer.greenOutRedIn.value = 70f;
        this.channelMixer.greenOutGreenIn.value = 30f;
        this.channelMixer.greenOutBlueIn.value = 0.0f;
        this.channelMixer.blueOutRedIn.value = 0.0f;
        this.channelMixer.blueOutGreenIn.value = 30f;
        this.channelMixer.blueOutBlueIn.value = 70f;
        break;
      case 3:
        this.channelMixer.redOutRedIn.value = 95f;
        this.channelMixer.redOutGreenIn.value = 5f;
        this.channelMixer.redOutBlueIn.value = 0.0f;
        this.channelMixer.greenOutRedIn.value = 0.0f;
        this.channelMixer.greenOutGreenIn.value = 43.333f;
        this.channelMixer.greenOutBlueIn.value = 56.667f;
        this.channelMixer.blueOutRedIn.value = 0.0f;
        this.channelMixer.blueOutGreenIn.value = 47.5f;
        this.channelMixer.blueOutBlueIn.value = 52.5f;
        break;
    }
  }

  private void ResetMixer()
  {
    this.channelMixer.redOutRedIn.value = 100f;
    this.channelMixer.redOutGreenIn.value = 0.0f;
    this.channelMixer.redOutBlueIn.value = 0.0f;
    this.channelMixer.greenOutRedIn.value = 0.0f;
    this.channelMixer.greenOutGreenIn.value = 100f;
    this.channelMixer.greenOutBlueIn.value = 0.0f;
    this.channelMixer.blueOutRedIn.value = 0.0f;
    this.channelMixer.blueOutGreenIn.value = 0.0f;
    this.channelMixer.blueOutBlueIn.value = 100f;
  }
}
