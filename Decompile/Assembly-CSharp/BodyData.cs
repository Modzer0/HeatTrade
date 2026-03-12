// Decompiled with JetBrains decompiler
// Type: BodyData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class BodyData
{
  public string id;
  public string bodyName;
  public SerializableVector3 position;
  public SerializableQuaternion rotation;
  public float currentAngle;
  public float mass;
  public float rocheLimitRadius;
  public float soiRadius;
}
