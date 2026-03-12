// Decompiled with JetBrains decompiler
// Type: LinePointer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class LinePointer : MonoBehaviour
{
  [SerializeField]
  private LineRendererUI lrui;
  private Transform targetTransform;
  private Vector3 targetStaticPos;
  private bool isStaticPos;
  [SerializeField]
  private RectTransform origin;
  private readonly Vector2 referenceResolution = new Vector2(1920f, 1080f);

  private Camera mainCam => Camera.main;

  private void LateUpdate()
  {
    if ((Object) this.targetTransform != (Object) null)
    {
      this.DrawUILevelLine(this.mainCam.WorldToScreenPoint(this.targetTransform.position));
    }
    else
    {
      if (!this.isStaticPos)
        return;
      this.DrawUILevelLine(this.targetStaticPos);
    }
  }

  private void DrawUILevelLine(Vector3 screenPoint)
  {
    if (!(bool) (Object) this.lrui || !(bool) (Object) this.origin)
      return;
    RectTransform parent = this.lrui.transform.parent as RectTransform;
    Vector2 localPoint1;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, (Vector2) this.origin.position, (Camera) null, out localPoint1);
    Vector2 localPoint2;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, (Vector2) screenPoint, (Camera) null, out localPoint2);
    this.lrui.CreateLine(localPoint1, localPoint2, Color.yellow);
  }

  public void NewTarget(Transform newTarget)
  {
    this.isStaticPos = false;
    this.targetTransform = newTarget;
    this.transform.parent.gameObject.SetActive(true);
    this.lrui.gameObject.SetActive(true);
  }

  public void NewTarget(Vector3 newTargetPos)
  {
    this.targetTransform = (Transform) null;
    float num1 = (float) Screen.width / this.referenceResolution.x;
    float num2 = (float) Screen.height / this.referenceResolution.y;
    this.targetStaticPos = new Vector3(newTargetPos.x * num1, newTargetPos.y * num2, 0.0f);
    this.isStaticPos = true;
    this.transform.parent.gameObject.SetActive(true);
    this.lrui.gameObject.SetActive(true);
  }

  public void NewTargetUI(Vector3 newTargetUIPos)
  {
    this.targetTransform = (Transform) null;
    this.targetStaticPos = new Vector3(newTargetUIPos.x, newTargetUIPos.y, 0.0f);
    this.isStaticPos = true;
    this.transform.parent.gameObject.SetActive(true);
    this.lrui.gameObject.SetActive(true);
  }

  public void Off()
  {
    this.targetTransform = (Transform) null;
    this.isStaticPos = false;
    if ((bool) (Object) this.lrui)
      this.lrui.gameObject.SetActive(false);
    this.transform.parent.gameObject.SetActive(false);
  }
}
