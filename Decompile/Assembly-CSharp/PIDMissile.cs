// Decompiled with JetBrains decompiler
// Type: PIDMissile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PIDMissile : MonoBehaviour
{
  [SerializeField]
  private Transform target;

  private void Start()
  {
  }

  private void FixedUpdate()
  {
    Vector3 vector3 = this.target.transform.position - this.transform.position;
    this.transform.LookAt(this.target);
  }
}
