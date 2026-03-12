// Decompiled with JetBrains decompiler
// Type: SaraStep
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class SaraStep
{
  [SerializeField]
  private int stepNum;
  [TextArea]
  public string saraText;
  [TextArea]
  public string replyText;
  public bool isShowReplyButton;
  public Vector3 moveToPos;
}
