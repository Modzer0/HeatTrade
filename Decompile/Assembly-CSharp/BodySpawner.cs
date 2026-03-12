// Decompiled with JetBrains decompiler
// Type: BodySpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BodySpawner : MonoBehaviour
{
  [SerializeField]
  private bool isSun;
  [SerializeField]
  private float radius;
  public float realDistance;
  public float unityDistance;
  [SerializeField]
  private float scale;
  private Vector3 direction;
  private float scaledRadius;
  [SerializeField]
  private Transform lightParent;
  [SerializeField]
  private Transform mainCam;

  private void Start()
  {
    this.mainCam = Camera.main.transform;
    if (!this.isSun)
      return;
    this.direction = Vector3.forward * -1f;
  }

  private void Update()
  {
    this.transform.position = this.mainCam.position + this.direction * (this.unityDistance + this.scaledRadius);
  }

  public void SetFromBodyInfo(BodyInfo bodyInfo)
  {
    if (this.isSun)
      return;
    MonoBehaviour.print((object) $"setting body: {bodyInfo.bodyName} dist: {bodyInfo.distance.ToString()} dir: {bodyInfo.direction.ToString()}");
    this.unityDistance = 10000f;
    this.realDistance = bodyInfo.distance;
    this.direction = bodyInfo.direction * -1f;
    this.scaledRadius = this.radius / this.realDistance * this.unityDistance;
    this.scaledRadius = Mathf.Abs(this.scaledRadius);
    this.scaledRadius = Mathf.Min(this.scaledRadius, 5000f);
    this.transform.localScale = Vector3.one * this.scaledRadius * 2f;
    this.transform.position = this.direction * (this.unityDistance + this.scaledRadius);
  }
}
