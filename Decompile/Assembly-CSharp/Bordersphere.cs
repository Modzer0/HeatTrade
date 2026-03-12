// Decompiled with JetBrains decompiler
// Type: Bordersphere
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Bordersphere : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    MonoBehaviour.print((object) ("EXIT: " + other.transform.root.gameObject.name));
    if (!(bool) (Object) other.transform.root.gameObject.GetComponent<ShipController>())
      return;
    other.transform.root.gameObject.GetComponent<ShipController>().SoftDeath();
  }
}
