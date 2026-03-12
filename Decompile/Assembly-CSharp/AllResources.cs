// Decompiled with JetBrains decompiler
// Type: AllResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "AllResourcesList", menuName = "ScriptableObjects/AllResources")]
public class AllResources : ScriptableObject
{
  public List<ResourceDefinition> resources;

  public ResourceDefinition GetResource(ResourceType type)
  {
    foreach (ResourceDefinition resource in this.resources)
    {
      if (resource.type == type)
        return resource;
    }
    return (ResourceDefinition) null;
  }

  public ResourceDefinition GetResource(string name)
  {
    foreach (ResourceDefinition resource in this.resources)
    {
      if (resource.resourceName == name)
        return resource;
    }
    return (ResourceDefinition) null;
  }
}
