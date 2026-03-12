// Decompiled with JetBrains decompiler
// Type: SkirmishMapFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class SkirmishMapFollower : MonoBehaviour
{
  private Transform target;
  private Vector3 offset;
  private bool isFollowing;

  private void Start()
  {
    this.target = Camera.main.transform;
    this.StartCoroutine((IEnumerator) this.LateStart());
  }

  private IEnumerator LateStart()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    SkirmishMapFollower skirmishMapFollower = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      skirmishMapFollower.offset = skirmishMapFollower.transform.position - skirmishMapFollower.target.position;
      skirmishMapFollower.isFollowing = true;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(3f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void Update()
  {
    if (!this.isFollowing)
      return;
    this.transform.position = this.target.position + this.offset;
  }
}
