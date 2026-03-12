// Decompiled with JetBrains decompiler
// Type: TacticalObjectSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class TacticalObjectSource : MonoBehaviour
{
  public int factionID;
  public string trackID;
  public string publicName;
  public string objectClass;
  public string description;
  public string prefabKey;
  public float maxAcceleration;

  public TacticalShipData ToTacticalData()
  {
    TacticalShipData tacticalData = new TacticalShipData();
    tacticalData.factionID = this.factionID;
    if (string.IsNullOrEmpty(this.trackID) || this.trackID == null)
      Debug.LogError((object) "This object has no track ID! An ID is always required.");
    tacticalData.trackID = this.trackID;
    tacticalData.publicName = this.publicName;
    tacticalData.cruiseAcceleration = this.maxAcceleration;
    IEnumerator enumerator = (IEnumerator) this.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if ((bool) (UnityEngine.Object) current.GetComponent<S_Module>())
          tacticalData.modules.Add(current.GetComponent<S_Module>().ToTacticalData());
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    return tacticalData;
  }
}
