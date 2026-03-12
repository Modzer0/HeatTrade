// Decompiled with JetBrains decompiler
// Type: RandomRotator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RandomRotator : MonoBehaviour
{
  [SerializeField]
  private float tumble;

  private void Start()
  {
    this.GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * this.tumble;
  }
}
