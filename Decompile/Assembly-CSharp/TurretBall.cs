// Decompiled with JetBrains decompiler
// Type: TurretBall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TurretBall : MonoBehaviour
{
  [SerializeField]
  private Transform target;
  [SerializeField]
  private float turnSpeed = 60f;
  [SerializeField]
  private float maxAngle = 90f;
  private Quaternion restRotation;

  private void Start() => this.restRotation = this.transform.localRotation;

  private void Update()
  {
    if ((Object) this.target == (Object) null)
      return;
    this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, this.LimitRotation(this.restRotation, Quaternion.LookRotation((this.target.position - this.transform.position).normalized), this.maxAngle), this.turnSpeed * Time.deltaTime);
  }

  private Quaternion LimitRotation(Quaternion from, Quaternion to, float maxDegrees)
  {
    float num = Quaternion.Angle(from, to);
    if ((double) num <= (double) maxDegrees)
      return to;
    float t = maxDegrees / num;
    return Quaternion.Slerp(from, to, t);
  }
}
