// Decompiled with JetBrains decompiler
// Type: PID2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class PID2
{
  public float kP;
  public float kI;
  public float kD;
  private float p;
  private float i;
  private float d;
  private float previousError;

  public float Kp
  {
    get => this.kP;
    set => this.kP = value;
  }

  public float Ki
  {
    get => this.kI;
    set => this.kI = value;
  }

  public float Kd
  {
    get => this.kD;
    set => this.kD = value;
  }

  public PID2(float p, float i, float d)
  {
    this.kP = p;
    this.kI = i;
    this.kD = d;
  }

  public float GetOutput(float currentError, float deltaTime)
  {
    this.p = currentError;
    this.i += this.p * deltaTime;
    this.d = (this.p - this.previousError) / deltaTime;
    this.previousError = currentError;
    return (float) ((double) this.p * (double) this.kP + (double) this.i * (double) this.kI + (double) this.d * (double) this.kD);
  }

  public Vector3 GetOutputFull(Vector3 currentErrors, float deltaTime)
  {
    this.p = currentErrors.x;
    this.i += this.p * deltaTime;
    this.d = (this.p - this.previousError) / deltaTime;
    this.previousError = currentErrors.x;
    double x = (double) this.p * (double) this.kP + (double) this.i * (double) this.kI + (double) this.d * (double) this.kD;
    this.p = currentErrors.y;
    this.i += this.p * deltaTime;
    this.d = (this.p - this.previousError) / deltaTime;
    this.previousError = currentErrors.y;
    float num1 = (float) ((double) this.p * (double) this.kP + (double) this.i * (double) this.kI + (double) this.d * (double) this.kD);
    this.p = currentErrors.z;
    this.i += this.p * deltaTime;
    this.d = (this.p - this.previousError) / deltaTime;
    this.previousError = currentErrors.z;
    float num2 = (float) ((double) this.p * (double) this.kP + (double) this.i * (double) this.kI + (double) this.d * (double) this.kD);
    double y = (double) num1;
    double z = (double) num2;
    return new Vector3((float) x, (float) y, (float) z);
  }
}
