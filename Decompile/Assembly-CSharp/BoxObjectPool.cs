// Decompiled with JetBrains decompiler
// Type: BoxObjectPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BoxObjectPool : MonoBehaviour
{
  public static BoxObjectPool current;
  [Tooltip("Assign the box prefab.")]
  public Indicator pooledObject;
  [Tooltip("Initial pooled amount.")]
  public int pooledAmount = 1;
  [Tooltip("Should the pooled amount increase.")]
  public bool willGrow = true;
  private List<Indicator> pooledObjects;

  private void Awake() => BoxObjectPool.current = this;

  private void Start()
  {
    this.pooledObjects = new List<Indicator>();
    for (int index = 0; index < this.pooledAmount; ++index)
    {
      Indicator indicator = Object.Instantiate<Indicator>(this.pooledObject);
      indicator.transform.SetParent(this.transform, false);
      indicator.Activate(false);
      this.pooledObjects.Add(indicator);
    }
  }

  public Indicator GetPooledObject()
  {
    for (int index = 0; index < this.pooledObjects.Count; ++index)
    {
      if (!this.pooledObjects[index].Active)
        return this.pooledObjects[index];
    }
    if (!this.willGrow)
      return (Indicator) null;
    Indicator pooledObject = Object.Instantiate<Indicator>(this.pooledObject);
    pooledObject.transform.SetParent(this.transform, false);
    pooledObject.Activate(false);
    this.pooledObjects.Add(pooledObject);
    return pooledObject;
  }

  public void DeactivateAllPooledObjects()
  {
    foreach (Indicator pooledObject in this.pooledObjects)
      pooledObject.Activate(false);
  }
}
