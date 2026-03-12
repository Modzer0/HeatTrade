// Decompiled with JetBrains decompiler
// Type: ResourceQuantity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class ResourceQuantity
{
  public ResourceDefinition resource;
  public float quantity;

  public ResourceQuantity(ResourceDefinition resource, float quantity)
  {
    this.resource = resource;
    this.quantity = quantity;
  }

  public ResourceData GetResourceData()
  {
    return new ResourceData()
    {
      resourceName = this.resource.resourceName,
      quantity = this.quantity
    };
  }
}
