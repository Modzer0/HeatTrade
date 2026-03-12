// Decompiled with JetBrains decompiler
// Type: Assets.PID
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Assets;

public class PID
{
  private float kP;
  private float kI;
  private float kD;
  private float p;
  private float i;
  private float d;
  private float previousError;

  public PID(float p, float i, float d)
  {
    this.kP = p;
    this.kI = i;
    this.kD = d;
  }

  public float GetOutput(float currentError, float deltaTime)
  {
    this.p = currentError;
    this.i = this.p * deltaTime;
    this.d = (this.p - this.previousError) / deltaTime;
    this.previousError = currentError;
    return (float) ((double) this.p * (double) this.kP + (double) this.i * (double) this.kI + (double) this.d * (double) this.kD);
  }
}
