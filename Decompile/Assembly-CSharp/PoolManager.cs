// Decompiled with JetBrains decompiler
// Type: PoolManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PoolManager : MonoBehaviour
{
  private Dictionary<int, Queue<PoolManager.ObjectInstance>> poolDictionary = new Dictionary<int, Queue<PoolManager.ObjectInstance>>();
  private static PoolManager _instance;

  public static PoolManager instance
  {
    get
    {
      if ((Object) PoolManager._instance == (Object) null)
        PoolManager._instance = Object.FindObjectOfType<PoolManager>();
      return PoolManager._instance;
    }
  }

  public void CreatePool(GameObject prefab, int poolSize)
  {
    int instanceId = prefab.GetInstanceID();
    if (this.poolDictionary.ContainsKey(instanceId))
      return;
    this.poolDictionary.Add(instanceId, new Queue<PoolManager.ObjectInstance>());
    GameObject gameObject = new GameObject(prefab.name + "pool");
    gameObject.transform.parent = this.transform;
    for (int index = 0; index < poolSize; ++index)
    {
      PoolManager.ObjectInstance objectInstance = new PoolManager.ObjectInstance(Object.Instantiate<GameObject>(prefab));
      this.poolDictionary[instanceId].Enqueue(objectInstance);
      objectInstance.SetParent(gameObject.transform);
    }
  }

  public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
  {
    int instanceId = prefab.GetInstanceID();
    if (!this.poolDictionary.ContainsKey(instanceId))
      return;
    PoolManager.ObjectInstance objectInstance = this.poolDictionary[instanceId].Dequeue();
    this.poolDictionary[instanceId].Enqueue(objectInstance);
    objectInstance.Reuse(position, rotation);
  }

  public void ReuseObject(
    GameObject prefab,
    Vector3 position,
    Quaternion rotation,
    Vector3 target,
    float speed)
  {
    int instanceId = prefab.GetInstanceID();
    if (!this.poolDictionary.ContainsKey(instanceId))
      return;
    PoolManager.ObjectInstance objectInstance = this.poolDictionary[instanceId].Dequeue();
    this.poolDictionary[instanceId].Enqueue(objectInstance);
    objectInstance.Reuse(position, rotation, target, speed);
  }

  public class ObjectInstance
  {
    private GameObject gameobject;
    private Transform transform;
    private bool hasPoolObjectComponent;
    private PoolObject poolObjectScript;

    public ObjectInstance(GameObject objectInstance)
    {
      this.gameobject = objectInstance;
      this.transform = this.gameobject.transform;
      this.gameobject.SetActive(false);
      if (!(bool) (Object) this.gameobject.GetComponent<PoolObject>())
        return;
      this.hasPoolObjectComponent = true;
      this.poolObjectScript = this.gameobject.GetComponent<PoolObject>();
      this.poolObjectScript.IsPooling = true;
    }

    public void Reuse(Vector3 position, Quaternion rotation)
    {
      this.gameobject.SetActive(true);
      this.transform.position = position;
      this.transform.rotation = rotation;
      if (!this.hasPoolObjectComponent)
        return;
      this.poolObjectScript.OnobjectReuse();
    }

    public void Reuse(Vector3 position, Quaternion rotation, Vector3 target, float speed)
    {
      this.gameobject.SetActive(true);
      this.transform.position = position;
      this.transform.rotation = rotation;
      if (!this.hasPoolObjectComponent)
        return;
      this.poolObjectScript.OnobjectReuse(target, speed);
    }

    public void SetParent(Transform parent) => this.transform.parent = parent;
  }
}
