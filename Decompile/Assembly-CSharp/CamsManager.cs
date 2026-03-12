// Decompiled with JetBrains decompiler
// Type: CamsManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CamsManager : MonoBehaviour
{
  public static CamsManager current;
  public Camera mainCam;
  private Transform mct;
  public bool isCamFree;
  [SerializeField]
  private List<Transform> camPosList;
  private int tweenRotateId = -1;
  private int tweenMoveId = -1;
  [SerializeField]
  private float camMoveSpeed;
  [SerializeField]
  private float camRotSpeed;

  private void Awake() => CamsManager.current = this;

  private void Start() => this.mct = this.mainCam.transform;

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.F1) && this.camPosList.Count >= 1)
      this.RepositionCam(this.camPosList[0]);
    else if (Input.GetKeyDown(KeyCode.F2) && this.camPosList.Count >= 2)
      this.RepositionCam(this.camPosList[1]);
    else if (Input.GetKeyDown(KeyCode.F3) && this.camPosList.Count >= 3)
      this.RepositionCam(this.camPosList[2]);
    else if (Input.GetKeyDown(KeyCode.F4) && this.camPosList.Count >= 4)
      this.RepositionCam(this.camPosList[3]);
    else if (Input.GetKeyDown(KeyCode.F5) && this.camPosList.Count >= 5)
      this.RepositionCam(this.camPosList[4]);
    if (!this.isCamFree)
      return;
    float num1 = this.camMoveSpeed * (Input.GetKey(KeyCode.LeftShift) ? 3f : 1f);
    if (Input.GetKey(KeyCode.LeftControl))
    {
      float num2 = this.camRotSpeed * (Input.GetKey(KeyCode.LeftShift) ? 3f : 1f);
      float num3 = (float) ((Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0));
      float num4 = (float) ((Input.GetKey(KeyCode.S) ? 1 : 0) - (Input.GetKey(KeyCode.W) ? 1 : 0));
      Quaternion rotation = this.mct.rotation;
      float x1 = rotation.eulerAngles.x - num4 * num2 * Time.deltaTime;
      rotation = this.mct.rotation;
      float y1 = rotation.eulerAngles.y + num3 * num2 * Time.deltaTime;
      this.mct.rotation = Quaternion.Euler(x1, y1, 0.0f);
      Transform mct = this.mct;
      rotation = this.mct.rotation;
      double x2 = (double) rotation.eulerAngles.x;
      double y2 = (double) y1;
      rotation = this.mct.rotation;
      double z = (double) rotation.eulerAngles.z;
      Quaternion quaternion = Quaternion.Euler((float) x2, (float) y2, (float) z);
      mct.rotation = quaternion;
    }
    else
      this.mct.position += (Vector3.zero + this.mct.forward * (float) ((Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0)) + this.mct.right * (float) ((Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0)) + this.mct.up * (float) ((Input.GetKey(KeyCode.E) ? 1 : 0) - (Input.GetKey(KeyCode.Q) ? 1 : 0))).normalized * num1 * Time.deltaTime;
  }

  public void RepositionCam(Transform newParent)
  {
    this.SetCamFree(false);
    if (this.tweenRotateId != -1)
      LeanTween.cancel(this.tweenRotateId);
    if (this.tweenMoveId != -1)
      LeanTween.cancel(this.tweenMoveId);
    this.mct.SetParent(newParent);
    this.tweenRotateId = LeanTween.rotateLocal(this.mct.gameObject, Vector3.zero, 0.5f).setOnComplete((Action) (() => this.tweenRotateId = -1)).id;
    this.tweenMoveId = LeanTween.moveLocal(this.mct.gameObject, Vector3.zero, 0.5f).setOnComplete((Action) (() => this.tweenMoveId = -1)).id;
  }

  public void MoveCamTowards(Transform targetTransform)
  {
    this.SetCamFree(true);
    if (this.tweenMoveId != -1)
      LeanTween.cancel(this.tweenMoveId);
    this.tweenRotateId = LeanTween.rotate(this.mct.gameObject, Quaternion.LookRotation(targetTransform.position - this.mainCam.transform.position).eulerAngles, 0.5f).setOnComplete((Action) (() =>
    {
      this.tweenRotateId = -1;
      if ((double) Vector3.Distance(this.mct.position, targetTransform.position) <= 5.0)
        return;
      this.MoveCamTo(targetTransform);
    })).id;
  }

  private void MoveCamTo(Transform targetTransform)
  {
    Vector3 position = targetTransform.position;
    this.tweenMoveId = LeanTween.move(this.mainCam.gameObject, position - (position - this.mainCam.transform.position).normalized * 3f, 0.25f).setOnComplete((Action) (() => this.tweenMoveId = -1)).id;
  }

  public void SetCamFree(bool isFree)
  {
    this.isCamFree = isFree;
    if (!isFree)
      return;
    this.mct.SetParent((Transform) null);
  }
}
