// Decompiled with JetBrains decompiler
// Type: PoolObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PoolObject : MonoBehaviour
{
  [HideInInspector]
  public bool IsPooling;

  public virtual void OnobjectReuse()
  {
  }

  public virtual void OnobjectReuse(Vector3 target, float speed)
  {
  }

  protected void Destroy(GameObject gameObject) => gameObject.SetActive(false);
}
