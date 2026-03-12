// Decompiled with JetBrains decompiler
// Type: Astrofish3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "NewAstrofish3", menuName = "ScriptableObjects/Astrofish3")]
public class Astrofish3 : ScriptableObject
{
  public Sprite icon;
  public int basePrice;
  public string fishName;
  public string description;
  public FishRarity rarity;
  [Header("FISHING: Nibbling")]
  [SerializeField]
  private float nibbleStart = 1f;
  [SerializeField]
  private float nibbleEnd = 10f;
  [SerializeField]
  private float reactionTimeMin = 1f;
  [SerializeField]
  private float reactionTimeMax = 10f;
  [Header("DATA")]
  public float staminaMax = 100f;
  public float staminaRegenRate = 5f;
  public float constantPull = -1f;
  public Vector2 idleRange = new Vector2(3f, 6f);
  [Header("MOVES")]
  [SerializeField]
  private float struggleChance = 20f;
  public Vector2 struggleRange = new Vector2(0.5f, 2f);
  [SerializeField]
  private float lightPullChance = 40f;
  public AC3Attack lightPull;
  [SerializeField]
  private float heavyPullChance = 40f;
  public AC3Attack heavyPull;
  [SerializeField]
  private float comboChance;
  public List<AC3Attack> combo = new List<AC3Attack>();
  [Header("CATCHES")]
  public List<CatchChance3> catches = new List<CatchChance3>();

  public Astrofish3 GetLuredFish()
  {
    float maxInclusive = 0.0f;
    foreach (CatchChance3 catchChance3 in this.catches)
      maxInclusive += (float) catchChance3.chance;
    float num1 = Random.Range(0.0f, maxInclusive);
    float num2 = 0.0f;
    for (int index = 0; index < this.catches.Count; ++index)
    {
      num2 += (float) this.catches[index].chance;
      if ((double) num1 <= (double) num2)
        return this.catches[index].fish;
    }
    return (Astrofish3) null;
  }

  public float GetNibbleTime() => Random.Range(this.nibbleStart, this.nibbleEnd);

  public float GetReactionTime() => Random.Range(this.reactionTimeMin, this.reactionTimeMax);

  public int GetMove()
  {
    int num = Random.Range(0, 100);
    if ((double) num < (double) this.struggleChance)
      return 1;
    if ((double) num < (double) this.struggleChance + (double) this.lightPullChance)
      return 2;
    return (double) num < (double) this.struggleChance + (double) this.lightPullChance + (double) this.heavyPullChance ? 3 : 4;
  }
}
