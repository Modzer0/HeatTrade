// Decompiled with JetBrains decompiler
// Type: Target
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[DefaultExecutionOrder(0)]
public class Target : MonoBehaviour
{
  public string targetName = "NoName";
  public Track track;
  public bool isOwned;
  [Tooltip("Change this color to change the indicators color for this target")]
  public Color targetColor = Color.white;
  [Tooltip("Select if box indicator is required for this target")]
  [SerializeField]
  private bool needBoxIndicator = true;
  [Tooltip("Select if arrow indicator is required for this target")]
  [SerializeField]
  public bool needArrowIndicator = true;
  [Tooltip("Select if distance text is required for this target")]
  [SerializeField]
  private bool needDistanceText = true;
  private bool isVisible;
  private LineRenderer lr;
  public bool isTactical;
  [HideInInspector]
  public Indicator indicator;

  public Color TargetColor => this.targetColor;

  public bool NeedBoxIndicator => this.needBoxIndicator;

  public bool NeedArrowIndicator => this.needArrowIndicator;

  public bool NeedDistanceText => this.needDistanceText;

  private void OnEnable()
  {
    if (OffScreenIndicator.TargetStateChanged == null)
      return;
    OffScreenIndicator.TargetStateChanged(this, true);
  }

  private void OnDisable()
  {
    if (OffScreenIndicator.TargetStateChanged == null)
      return;
    OffScreenIndicator.TargetStateChanged(this, false);
  }

  public float GetDistanceFrom(Vector3 targetPosition)
  {
    return Vector3.Distance(targetPosition, this.transform.position);
  }

  private void Start()
  {
    this.track = this.GetComponent<Track>();
    this.lr = this.track.GetComponent<LineRenderer>();
  }

  public void SetVisibility(bool isOn)
  {
    this.isVisible = isOn;
    if ((Object) this.indicator != (Object) null)
      this.indicator.gameObject.SetActive(this.isVisible);
    if (!(bool) (Object) this.lr)
      return;
    this.lr.enabled = this.isVisible;
  }
}
