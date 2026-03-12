// Decompiled with JetBrains decompiler
// Type: S_Attachment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class S_Attachment
{
  [Header("VARIABLES")]
  [Header("INFO")]
  public string prefabName;
  public string productName;
  public AttachmentType attachmentType;
  [Header("DATA")]
  public int health;
  public int healthMax;
  public int resource;
  public int resourceMax;
  public int value;
  public int mass;
  [Header("POWER AND HEAT")]
  public float power;
  public float heat;
  public HeatClass heatClass;
}
