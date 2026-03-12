// Decompiled with JetBrains decompiler
// Type: FactionIconsLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "FactionIconsLibrary", menuName = "ScriptableObjects/FactionIconsLibrary")]
public class FactionIconsLibrary : ScriptableObject
{
  public List<Sprite> icons = new List<Sprite>();

  public Sprite GetIconFromFactionID(int id)
  {
    return id >= this.icons.Count ? (Sprite) null : this.icons[id];
  }
}
