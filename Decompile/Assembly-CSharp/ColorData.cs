// Decompiled with JetBrains decompiler
// Type: ColorData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class ColorData
{
  public float r;
  public float g;
  public float b;
  public float a;

  public ColorData(Color color)
  {
    this.r = color.r;
    this.g = color.g;
    this.b = color.b;
    this.a = color.a;
  }

  public Color ToColor() => new Color(this.r, this.g, this.b, this.a);
}
