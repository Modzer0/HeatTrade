// Decompiled with JetBrains decompiler
// Type: Turret
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Turret : MonoBehaviour
{
  [SerializeField]
  private Transform target;
  [SerializeField]
  private Vector3 targetPos;
  private float lastFaceTime;
  private Transform baseTransform;
  [SerializeField]
  private Transform rotator;
  [SerializeField]
  private string baseOrientation;
  [SerializeField]
  private string rotatorOrientation;

  private void Start() => this.baseTransform = this.transform;

  private void Update()
  {
    if (!(bool) (Object) this.target)
      return;
    this.CheckFacing();
  }

  private void CheckFacing()
  {
    int layerMask = 1 << LayerMask.NameToLayer("Trackable") | 1 << LayerMask.NameToLayer("Obstacle");
    Vector3 vector3 = this.target.transform.position - this.transform.position;
    Debug.DrawRay(this.transform.position, vector3, Color.green, 0.1f);
    RaycastHit hitInfo;
    if (!Physics.Raycast(this.transform.position, vector3, out hitInfo, 100f, layerMask) || hitInfo.collider.gameObject.GetComponent<ITrackable>() == null || !(hitInfo.collider.gameObject.GetComponent<ITrackable>().GetName() == this.target.GetComponent<ITrackable>().GetName()))
      return;
    this.FaceTarget();
  }

  private void FaceTarget()
  {
    this.transform.Rotate(new Vector3(0.0f, this.Vector3AngleOnPlane(this.target.position, this.transform.position, -this.transform.up, this.transform.forward), 0.0f), Space.Self);
    this.rotator.transform.Rotate(new Vector3((float) (-(double) Vector3.Angle(this.target.position, this.rotator.transform.up) + 90.0), 0.0f, 0.0f), Space.Self);
    Vector3 eulerAngles = Quaternion.LookRotation(this.target.position - this.transform.position).eulerAngles;
    this.rotator.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, 0.0f);
  }

  private float Vector3AngleOnPlane(
    Vector3 from,
    Vector3 to,
    Vector3 planeNormal,
    Vector3 toZeroAngle)
  {
    return Vector3.SignedAngle(Vector3.ProjectOnPlane(from - to, planeNormal), toZeroAngle, planeNormal);
  }

  public void SetTarget(Transform newTarget)
  {
    if ((Object) newTarget == (Object) null)
      this.target = (Transform) null;
    else
      this.target = newTarget;
  }
}
