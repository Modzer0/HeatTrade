// Decompiled with JetBrains decompiler
// Type: TacticalShipAudio
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TacticalShipAudio : MonoBehaviour
{
  public static TacticalShipAudio current;
  public AudioClip healthyAmbience;
  public AudioClip damagedAmbience;
  public AudioClip destroyedAmbience;
  public AudioClip enemyAmbience;

  private void Awake() => TacticalShipAudio.current = this;
}
