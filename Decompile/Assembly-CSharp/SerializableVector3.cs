// Decompiled with JetBrains decompiler
// Type: SerializableVector3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public struct SerializableVector3
{
  public float x;
  public float y;
  public float z;

  public SerializableVector3(Vector3 v)
  {
    this.x = v.x;
    this.y = v.y;
    this.z = v.z;
  }

  public Vector3 ToVector3() => new Vector3(this.x, this.y, this.z);
}
