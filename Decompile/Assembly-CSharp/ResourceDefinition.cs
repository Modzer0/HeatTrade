// Decompiled with JetBrains decompiler
// Type: ResourceDefinition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "NewResource", menuName = "ScriptableObjects/Resource Definition")]
public class ResourceDefinition : ScriptableObject
{
  public string resourceName;
  public string description;
  public Sprite icon;
  public ResourceType type;
  [Header("Economic Properties")]
  public int basePrice;
}
