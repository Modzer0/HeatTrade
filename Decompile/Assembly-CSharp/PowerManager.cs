// Decompiled with JetBrains decompiler
// Type: PowerManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PowerManager : MonoBehaviour
{
  private List<T_Module> modules = new List<T_Module>();
  private float netPower;

  public void Init(List<T_Module> allModules)
  {
    foreach (Component allModule in allModules)
      this.modules.Add(allModule.GetComponent<T_Module>());
  }
}
