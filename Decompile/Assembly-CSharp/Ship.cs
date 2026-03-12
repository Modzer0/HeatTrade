// Decompiled with JetBrains decompiler
// Type: Ship
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Ship : MonoBehaviour, ITrackable
{
  public string trackName;
  [SerializeField]
  private int iff;

  private void Start()
  {
  }

  private void Update()
  {
  }

  public void TrackedBy(ITrackable tracker)
  {
  }

  public void UntrackedBy(ITrackable tracker)
  {
  }

  public string GetName() => this.trackName;

  public int GetIFF() => this.iff;
}
