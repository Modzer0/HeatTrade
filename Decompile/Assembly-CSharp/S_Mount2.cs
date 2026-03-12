// Decompiled with JetBrains decompiler
// Type: S_Mount2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class S_Mount2 : MonoBehaviour
{
  public MountBP bp;
  public float health;
  public float resource;

  public void NewMount(MountBP newBP)
  {
    this.bp = newBP;
    this.health = 100f;
    this.resource = 0.0f;
  }

  public void AddHealth(float mod)
  {
    this.health += mod;
    this.health = Mathf.Clamp(this.health, 0.0f, 100f);
  }

  public void AddResource(int mod)
  {
    this.resource += (float) mod;
    this.resource = Mathf.Clamp(this.resource, 0.0f, (this.bp as IResupplyable).GetResourceMax());
  }

  public TacticalMountData ToTacticalData()
  {
    return new TacticalMountData()
    {
      bpKey = this.bp.PrefabKey,
      health = this.health,
      resource = this.resource
    };
  }

  public void InitMountData(TacticalMountData mountData)
  {
    this.health = mountData.health;
    this.resource = mountData.resource;
  }
}
