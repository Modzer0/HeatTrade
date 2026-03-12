// Decompiled with JetBrains decompiler
// Type: FirstPersonMovement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FirstPersonMovement : MonoBehaviour
{
  public float speed = 5f;
  [Header("Running")]
  public bool canRun = true;
  public float runSpeed = 9f;
  public KeyCode runningKey = KeyCode.LeftShift;
  public KeyCode upKey = KeyCode.Space;
  public KeyCode downKey = KeyCode.LeftControl;
  private Rigidbody thisRB;
  public List<Func<float>> speedOverrides = new List<Func<float>>();

  public bool IsRunning { get; private set; }

  private void Awake() => this.thisRB = this.GetComponent<Rigidbody>();

  private void FixedUpdate()
  {
    this.IsRunning = this.canRun && Input.GetKey(this.runningKey);
    float num1 = this.IsRunning ? this.runSpeed : this.speed;
    if (this.speedOverrides.Count > 0)
      num1 = this.speedOverrides[this.speedOverrides.Count - 1]();
    int num2 = 0;
    if (Input.GetKey(this.upKey))
      num2 = 1;
    else if (Input.GetKey(this.downKey))
      num2 = -1;
    Vector3 vector3 = new Vector3(Input.GetAxis("Horizontal") * num1, Input.GetAxis("Vertical") * num1, (float) num2 * (num1 / 2f));
    this.thisRB.velocity = this.transform.rotation * new Vector3(vector3.x, vector3.z, vector3.y);
  }
}
