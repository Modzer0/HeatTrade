// Decompiled with JetBrains decompiler
// Type: ExhaustPlumes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ExhaustPlumes : MonoBehaviour
{
  public static ExhaustPlumes current;
  public bool isExhaustFriendlyFire;
  private List<ExhaustPlume> exhaustPlumes = new List<ExhaustPlume>();

  private void Awake() => ExhaustPlumes.current = this;

  public void Add(ExhaustPlume newExhaust)
  {
    this.exhaustPlumes.Add(newExhaust);
    newExhaust.isFriendlyFire = this.isExhaustFriendlyFire;
  }

  public void ToggleFriendlyFire(bool isOn)
  {
    this.isExhaustFriendlyFire = isOn;
    foreach (ExhaustPlume exhaustPlume in this.exhaustPlumes)
      exhaustPlume.isFriendlyFire = this.isExhaustFriendlyFire;
  }
}
