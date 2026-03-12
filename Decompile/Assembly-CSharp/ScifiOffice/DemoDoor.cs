// Decompiled with JetBrains decompiler
// Type: ScifiOffice.DemoDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ScifiOffice;

public class DemoDoor : MonoBehaviour
{
  private Animator anim;

  private void Start() => this.anim = this.GetComponent<Animator>();

  private void OnTriggerEnter(Collider other)
  {
    if (!(other.gameObject.name == "Player"))
      return;
    this.anim.SetTrigger("Open");
  }
}
