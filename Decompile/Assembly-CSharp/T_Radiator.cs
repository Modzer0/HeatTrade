// Decompiled with JetBrains decompiler
// Type: T_Radiator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class T_Radiator : T_Mount
{
  [Header("= RADIATOR ====================")]
  private RadiatorMountBP radiatorBP;
  [Header("TOGGLING")]
  [SerializeField]
  private Transform pivot;
  private float pivotTargetScale;
  private bool isToggling;
  [Header("PANEL")]
  [SerializeField]
  private Transform panel;
  [SerializeField]
  private Transform stand;
  [Header("GLOW")]
  private float onPercent = 1f;
  private float onPercentChange = 0.005f;
  public float targetDissipationRatio;
  private float currentDissipationRatio;
  [SerializeField]
  private bool isAlwaysOn;

  public override void Start()
  {
    base.Start();
    if (!(this.BP is RadiatorMountBP))
      return;
    this.radiatorBP = this.BP as RadiatorMountBP;
    this.heat = (float) (-(double) this.radiatorBP.HeatDissipation * 1000.0);
    float y = (float) ((double) this.radiatorBP.Height / 10.0 * 0.89999997615814209);
    float z = this.radiatorBP.Length / 10f;
    this.stand.localScale = new Vector3(0.1f, 0.1f, z);
    this.panel = this.moduleRenderer.GetComponent<Transform>();
    this.panel.localScale = new Vector3(0.025f, y, z);
    this.panel.transform.localPosition = new Vector3(0.0f, y / 2f, 0.0f);
  }

  public override void Update()
  {
    base.Update();
    if (this.isAlwaysOn)
    {
      this.currentHeat = this.heat;
      this.targetDissipationRatio = 1f;
      this.currentDissipationRatio = 1f;
    }
    else if (this.isOn)
      this.currentHeat = this.heat * this.healthRatio;
    else
      this.currentHeat = 0.0f;
    if (this.isToggling)
    {
      this.pivot.localScale = new Vector3(1f, Mathf.Lerp(this.pivot.localScale.y, this.pivotTargetScale, 5f * Time.deltaTime), 1f);
      if ((double) Mathf.Abs(this.pivot.localScale.y - this.pivotTargetScale) < 0.0099999997764825821)
      {
        this.pivot.localScale = new Vector3(1f, this.pivotTargetScale, 1f);
        this.isToggling = false;
      }
    }
    this.currentDissipationRatio = Mathf.Lerp(this.currentDissipationRatio, this.targetDissipationRatio, 0.5f * Time.deltaTime);
    float num = this.onPercent * this.healthRatio * this.currentDissipationRatio;
    if ((bool) (Object) this.radiatorBP)
      this.material.SetFloat("_Temperature", this.radiatorBP.OperatingTemp * this.currentDissipationRatio * Mathf.LinearToGammaSpace(num));
    else
      this.material.SetFloat("_Temperature", 3000f * this.onPercent * Mathf.LinearToGammaSpace(num));
  }

  private void FixedUpdate()
  {
    if (this.isOn && (double) this.onPercent <= 1.0)
    {
      this.onPercent += this.onPercentChange;
    }
    else
    {
      if (this.isOn || (double) this.onPercent < 0.0)
        return;
      this.onPercent -= this.onPercentChange;
    }
  }

  public void Toggle(bool newIsOn)
  {
    this.isOn = newIsOn;
    this.pivotTargetScale = !this.isOn ? 0.0f : 1f;
    this.isToggling = true;
  }
}
