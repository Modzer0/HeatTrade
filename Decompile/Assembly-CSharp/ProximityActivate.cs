// Decompiled with JetBrains decompiler
// Type: ProximityActivate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ProximityActivate : MonoBehaviour
{
  public Transform distanceActivator;
  public Transform lookAtActivator;
  public float distance;
  public Transform activator;
  public bool activeState;
  public CanvasGroup target;
  public bool lookAtCamera = true;
  public bool enableInfoPanel;
  public GameObject infoIcon;
  private float alpha;
  public CanvasGroup infoPanel;
  private Quaternion originRotation;
  private Quaternion targetRotation;

  private void Start()
  {
    this.originRotation = this.transform.rotation;
    this.alpha = this.activeState ? 1f : -1f;
    if ((Object) this.activator == (Object) null)
      this.activator = Camera.main.transform;
    this.infoIcon.SetActive((Object) this.infoPanel != (Object) null);
  }

  private bool IsTargetNear()
  {
    return (double) (this.distanceActivator.position - this.activator.position).sqrMagnitude < (double) this.distance * (double) this.distance && ((Object) this.lookAtActivator != (Object) null && (double) Vector3.Dot(this.activator.forward, (this.lookAtActivator.position - this.activator.position).normalized) > 0.949999988079071 || (double) Vector3.Dot(this.activator.forward, (this.target.transform.position - this.activator.position).normalized) > 0.949999988079071);
  }

  private void Update()
  {
    if (!this.activeState)
    {
      if (this.IsTargetNear())
      {
        this.alpha = 1f;
        this.activeState = true;
      }
    }
    else if (!this.IsTargetNear())
    {
      this.alpha = -1f;
      this.activeState = false;
      this.enableInfoPanel = false;
    }
    this.target.alpha = Mathf.Clamp01(this.target.alpha + this.alpha * Time.deltaTime);
    if ((Object) this.infoPanel != (Object) null)
    {
      if (Input.GetKeyDown(KeyCode.Space))
        this.enableInfoPanel = !this.enableInfoPanel;
      this.infoPanel.alpha = Mathf.Lerp(this.infoPanel.alpha, Mathf.Clamp01(this.enableInfoPanel ? this.alpha : 0.0f), Time.deltaTime * 10f);
    }
    if (!this.lookAtCamera)
      return;
    this.targetRotation = !this.activeState ? this.originRotation : Quaternion.LookRotation(this.activator.position - this.transform.position);
    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, this.targetRotation, Time.deltaTime);
  }
}
