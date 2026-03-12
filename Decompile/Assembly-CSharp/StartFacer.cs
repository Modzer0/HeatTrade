// Decompiled with JetBrains decompiler
// Type: StartFacer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class StartFacer : MonoBehaviour
{
  [SerializeField]
  private Vector3 targetPos;

  private void Start()
  {
    this.transform.rotation = Quaternion.LookRotation(this.targetPos - this.transform.position);
  }
}
