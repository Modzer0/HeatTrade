// Decompiled with JetBrains decompiler
// Type: SelfDestroy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SelfDestroy : MonoBehaviour
{
  public float TimeToDestroy;

  private void Start() => Object.Destroy((Object) this.gameObject, this.TimeToDestroy);
}
