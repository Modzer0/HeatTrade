// Decompiled with JetBrains decompiler
// Type: LinkOpener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class LinkOpener : MonoBehaviour
{
  [SerializeField]
  private string dest;

  public void OpenLink()
  {
    if (string.IsNullOrEmpty(this.dest))
      Debug.LogWarning((object) "Destination path is empty! Give me something to work with.");
    else
      Application.OpenURL(this.dest);
  }
}
