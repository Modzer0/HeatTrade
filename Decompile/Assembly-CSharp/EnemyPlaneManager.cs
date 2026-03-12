// Decompiled with JetBrains decompiler
// Type: EnemyPlaneManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EnemyPlaneManager : MonoBehaviour
{
  public Vector3 AreaSize;
  public Plane PlanesPrefabs;
  public float RespawnCount;
  public float LaunchRate;
  private float nextlaunch;
  private bool Fire;

  private void Update()
  {
    this.Fire = Input.GetKey(KeyCode.RightShift);
    if (!this.Fire || (double) Time.time < (double) this.nextlaunch)
      return;
    this.nextlaunch = Time.time + this.LaunchRate;
    this.LaunchPlane();
  }

  private void LaunchPlane()
  {
    for (int index = 0; (double) index < (double) this.RespawnCount; ++index)
      Object.Instantiate<Plane>(this.PlanesPrefabs, this.transform.localPosition + new Vector3(Random.Range((float) (-(double) this.AreaSize.x / 2.0), this.AreaSize.x / 2f), Random.Range((float) (-(double) this.AreaSize.y / 2.0), this.AreaSize.y / 2f), Random.Range((float) (-(double) this.AreaSize.z / 2.0), this.AreaSize.z / 2f)), this.transform.rotation);
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.grey;
    Gizmos.DrawCube(this.transform.localPosition, this.AreaSize);
  }
}
