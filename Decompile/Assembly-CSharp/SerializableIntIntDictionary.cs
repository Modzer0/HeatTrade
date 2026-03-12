// Decompiled with JetBrains decompiler
// Type: SerializableIntIntDictionary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public struct SerializableIntIntDictionary
{
  public List<IntIntPair> kvps;

  public SerializableIntIntDictionary(Dictionary<int, int> dict)
  {
    this.kvps = new List<IntIntPair>();
    foreach (KeyValuePair<int, int> keyValuePair in dict)
      this.kvps.Add(new IntIntPair(keyValuePair.Key, keyValuePair.Value));
  }

  public Dictionary<int, int> ToDictionary()
  {
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    foreach (IntIntPair kvp in this.kvps)
      dictionary[kvp.key] = kvp.value;
    return dictionary;
  }
}
