// Decompiled with JetBrains decompiler
// Type: VersionText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class VersionText : MonoBehaviour
{
  private void Start() => this.GetComponent<TMP_Text>().text = "v" + Application.version;
}
