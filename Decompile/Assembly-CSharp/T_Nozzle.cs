// Decompiled with JetBrains decompiler
// Type: T_Nozzle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class T_Nozzle : T_Module
{
  [Header("NOZZLE")]
  [SerializeField]
  private Renderer nozzleRenderer;
  private Material nozzleMat;
  [SerializeField]
  private Gradient heatGradient;
  [Range(0.0f, 1f)]
  private float onPercent;
  [SerializeField]
  private float onPercentChange = 0.0001f;
  [SerializeField]
  private float nozzleGlowIntensity = 5f;
  [SerializeField]
  private Color targetColor;

  public override void Start()
  {
    base.Start();
    this.isOn = false;
    this.nozzleRenderer = this.transform.childCount <= 1 ? this.transform.GetChild(0).GetComponent<Renderer>() : this.transform.GetChild(1).GetComponent<Renderer>();
    this.nozzleMat = this.nozzleRenderer.material;
  }

  private void Update()
  {
    if (!(bool) (Object) this.nozzleMat)
      return;
    this.targetColor = this.heatGradient.Evaluate(this.onPercent);
    float num = Mathf.LinearToGammaSpace(this.onPercent * this.nozzleGlowIntensity * this.healthRatio) * this.healthRatio;
    this.nozzleMat.SetColor("_BaseColor", this.targetColor);
    this.nozzleMat.SetFloat("_Temperature", 2000f * num);
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
}
