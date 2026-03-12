// Decompiled with JetBrains decompiler
// Type: ResupplyCycle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "NewResupplyCycle", menuName = "ScriptableObjects/ResupplyCycle")]
[Serializable]
public class ResupplyCycle : ScriptableObject
{
  public string cycleName;
  public ResourceQuantity input;
  public int output;
}
