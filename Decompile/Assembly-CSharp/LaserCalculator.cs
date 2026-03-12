// Decompiled with JetBrains decompiler
// Type: LaserCalculator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class LaserCalculator : MonoBehaviour
{
  [Header("Laser Settings")]
  public LaserType laserType;
  [Header("Inputs")]
  public float inputPowerMW = 100f;
  [Range(0.0f, 1f)]
  public float efficiency = 0.4f;
  public float wavelengthNM = 532f;
  public float apertureSizeM = 2f;
  [Header("Outputs (calculated)")]
  public float beamPowerMW;
  public float wasteHeatMW;
  public float divergenceMrad;
  public Color beamColor;
  public float rangeKm;
  private readonly float[] commonWavelengths = new float[4]
  {
    450f,
    532f,
    650f,
    1064f
  };

  private void Update() => this.Calculate();

  private void Calculate()
  {
    this.beamPowerMW = this.inputPowerMW * this.efficiency;
    this.wasteHeatMW = this.inputPowerMW - this.beamPowerMW;
    this.divergenceMrad = (float) (1.2200000286102295 * (double) (this.wavelengthNM * 1E-09f) / (double) this.apertureSizeM * 1000.0);
    this.rangeKm = (float) (10.0 / ((double) this.divergenceMrad * (1.0 / 1000.0)) / 1000.0);
    this.beamColor = this.WavelengthToRGB(this.wavelengthNM);
  }

  private Color WavelengthToRGB(float nm)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    if ((double) this.wavelengthNM >= 380.0 && (double) this.wavelengthNM < 440.0)
    {
      num1 = (float) (-((double) this.wavelengthNM - 440.0) / 60.0);
      num3 = 1f;
    }
    else if ((double) this.wavelengthNM >= 440.0 && (double) this.wavelengthNM < 490.0)
    {
      num2 = (float) (((double) this.wavelengthNM - 440.0) / 50.0);
      num3 = 1f;
    }
    else if ((double) this.wavelengthNM >= 490.0 && (double) this.wavelengthNM < 510.0)
    {
      num2 = 1f;
      num3 = (float) (-((double) this.wavelengthNM - 510.0) / 20.0);
    }
    else if ((double) this.wavelengthNM >= 510.0 && (double) this.wavelengthNM < 580.0)
    {
      num1 = (float) (((double) this.wavelengthNM - 510.0) / 70.0);
      num2 = 1f;
    }
    else if ((double) this.wavelengthNM >= 580.0 && (double) this.wavelengthNM < 645.0)
    {
      num1 = 1f;
      num2 = (float) (-((double) this.wavelengthNM - 645.0) / 65.0);
    }
    else if ((double) this.wavelengthNM >= 645.0 && (double) this.wavelengthNM <= 780.0)
      num1 = 1f;
    float num4 = 1f;
    if ((double) this.wavelengthNM > 700.0)
      num4 = (float) (0.30000001192092896 + 0.699999988079071 * (780.0 - (double) this.wavelengthNM) / 80.0);
    else if ((double) this.wavelengthNM < 420.0)
      num4 = (float) (0.30000001192092896 + 0.699999988079071 * ((double) this.wavelengthNM - 380.0) / 40.0);
    return new Color(num1 * num4, num2 * num4, num3 * num4);
  }
}
