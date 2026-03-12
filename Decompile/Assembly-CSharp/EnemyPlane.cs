// Decompiled with JetBrains decompiler
// Type: EnemyPlane
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EnemyPlane : MonoBehaviour
{
  public float Speed;
  public float TurnRate;

  private void FixedUpdate()
  {
    this.transform.Rotate(0.0f, this.TurnRate * Time.deltaTime, 0.0f);
    this.transform.Translate(0.0f, 0.0f, this.Speed * Time.deltaTime);
  }
}
