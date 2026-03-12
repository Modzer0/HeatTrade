// Decompiled with JetBrains decompiler
// Type: AC3Attack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "NewAC3Attack", menuName = "ScriptableObjects/AC3Attack")]
public class AC3Attack : ScriptableObject
{
  public string attackName = "";
  public float pullDist = 10f;
  public float pullTime = 0.5f;
  public float waitTime = 0.1f;
  public float pushTime = 0.2f;
  public float staminaCost = 15f;
  [Tooltip("Distance/progress change (positive for player, negative for fish)")]
  public float progressChange = 25f;
  public float lineDamage = 10f;
}
