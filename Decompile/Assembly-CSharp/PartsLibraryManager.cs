// Decompiled with JetBrains decompiler
// Type: PartsLibraryManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PartsLibraryManager : MonoBehaviour
{
  public static PartsLibraryManager current;
  public PartsLibrary library;

  private void Awake()
  {
    if ((Object) PartsLibraryManager.current != (Object) null && (Object) PartsLibraryManager.current != (Object) this)
      Object.Destroy((Object) this.gameObject);
    else
      PartsLibraryManager.current = this;
  }
}
