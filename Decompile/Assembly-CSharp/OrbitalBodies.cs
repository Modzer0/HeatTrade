// Decompiled with JetBrains decompiler
// Type: OrbitalBodies
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class OrbitalBodies : MonoBehaviour
{
  public static OrbitalBodies current;
  public Orbiter primaryBody;
  public List<Orbiter> secondaryBodies = new List<Orbiter>();

  private void Awake() => OrbitalBodies.current = this;

  private void Start()
  {
  }

  public Orbiter GetClosestBody(Vector3 position)
  {
    List<Orbiter> orbiterList = new List<Orbiter>();
    foreach (Orbiter secondaryBody in this.secondaryBodies)
    {
      if ((double) Vector3.Distance(position, secondaryBody.transform.position) < (double) secondaryBody.soiRadius)
        orbiterList.Add(secondaryBody);
    }
    if (orbiterList.Count == 0)
      return this.primaryBody;
    if (orbiterList.Count == 1)
      return orbiterList[0];
    float num1 = Vector3.Distance(position, orbiterList[0].transform.position);
    Orbiter closestBody = orbiterList[0];
    for (int index = 1; index < orbiterList.Count; ++index)
    {
      float num2 = Vector3.Distance(position, orbiterList[index].transform.position);
      if ((double) num2 < (double) num1)
      {
        num1 = num2;
        closestBody = orbiterList[index];
      }
    }
    return closestBody;
  }
}
