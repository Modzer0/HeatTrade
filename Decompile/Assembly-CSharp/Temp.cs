// Decompiled with JetBrains decompiler
// Type: Temp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Temp : MonoBehaviour
{
  private Pathfinder missile;
  private Vector3 targetPos;

  private void Start()
  {
    this.missile = this.GetComponent<Pathfinder>();
    this.NewTargetPos();
  }

  private void Update()
  {
    if (this.transform.position == this.targetPos)
      this.NewTargetPos();
    else
      this.missile.targetPos = this.targetPos;
  }

  private void NewTargetPos()
  {
    this.targetPos = new Vector3(Random.Range(-25f, 26f), Random.Range(-25f, 26f), Random.Range(-25f, 26f));
  }
}
