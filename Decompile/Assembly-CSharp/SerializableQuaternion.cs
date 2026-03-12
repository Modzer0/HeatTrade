// Decompiled with JetBrains decompiler
// Type: SerializableQuaternion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public struct SerializableQuaternion
{
  public float x;
  public float y;
  public float z;
  public float w;

  public SerializableQuaternion(Quaternion q)
  {
    this.x = q.x;
    this.y = q.y;
    this.z = q.z;
    this.w = q.w;
  }

  public Quaternion ToQuaternion() => new Quaternion(this.x, this.y, this.z, this.w);
}
