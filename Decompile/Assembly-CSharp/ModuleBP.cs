// Decompiled with JetBrains decompiler
// Type: ModuleBP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "ModuleBP", menuName = "ScriptableObjects/ModuleBP")]
public class ModuleBP : PartBP
{
  [SerializeField]
  private float length;
  [SerializeField]
  private float diameter;
  [SerializeField]
  private float volume;
  [SerializeField]
  private List<MountBP> mounts;

  public float Length => this.length;

  public float Diameter => this.diameter;

  public float Volume => this.volume;

  public List<MountBP> Mounts => this.mounts;

  public float GetSetVolume()
  {
    this.volume = 3.14159274f * Mathf.Pow(this.diameter / 2f, 2f) * this.length;
    return this.volume;
  }
}
