// Decompiled with JetBrains decompiler
// Type: RadiatorCalculator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class RadiatorCalculator : MonoBehaviour
{
  [Header("INPUTS")]
  [SerializeField]
  private float width;
  [SerializeField]
  private float height;
  private float sb = 5.67E-08f;
  [SerializeField]
  private float operatingTemp;
  [Header("OUTPUTS")]
  [SerializeField]
  private float area;
  [SerializeField]
  private float dissipation;

  private void Update()
  {
    this.area = this.height * this.width;
    this.dissipation = 2f * this.area * this.sb * Mathf.Pow(this.operatingTemp, 4f) / 1000000f;
  }
}
