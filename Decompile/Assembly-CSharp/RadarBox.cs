// Decompiled with JetBrains decompiler
// Type: RadarBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RadarBox : MonoBehaviour
{
  [SerializeField]
  private ITrackable tracker;
  public List<GameObject> potentialTracks;

  private void Start()
  {
  }

  private void Update()
  {
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.GetComponent<ITrackable>() == null)
      return;
    this.potentialTracks.Add(other.gameObject);
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.GetComponent<ITrackable>() == null)
      return;
    MonoBehaviour.print((object) "EXIT");
  }
}
