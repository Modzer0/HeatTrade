// Decompiled with JetBrains decompiler
// Type: TacticalModuleData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class TacticalModuleData
{
  public string bpKey;
  public float health;
  public float armorHealth;
  public float resource;
  public InventoryData inventoryData;
  public List<TacticalMountData> mounts;
}
