// Decompiled with JetBrains decompiler
// Type: PropellerRotator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PropellerRotator : MonoBehaviour
{
  [SerializeField]
  private float rpm = 2.67f;
  private float degPerSec;

  private void Start() => this.degPerSec = (float) ((double) this.rpm * 360.0 / 60.0);

  private void Update() => this.transform.Rotate(Vector3.forward * this.degPerSec * Time.deltaTime);
}
