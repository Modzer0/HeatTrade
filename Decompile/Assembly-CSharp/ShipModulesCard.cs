// Decompiled with JetBrains decompiler
// Type: ShipModulesCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using TMPro;
using UnityEngine;

#nullable disable
public class ShipModulesCard : MonoBehaviour
{
  private TimeManager tm;
  private S_Ship ship;
  [SerializeField]
  private bool isHealth;
  [Header("UI")]
  [SerializeField]
  private TMP_Text nameText;
  [SerializeField]
  private Transform modulesList;
  [SerializeField]
  private S_PartHealth partHealthPF;

  private void Start()
  {
    this.tm = TimeManager.current;
    this.tm.NewHour += new Action(this.NewHour);
  }

  private void NewHour()
  {
    if (!(bool) (UnityEngine.Object) this.ship)
      return;
    this.Setup(this.ship);
  }

  public void Setup(S_Ship newShip)
  {
    this.ship = newShip;
    this.nameText.text = $"{this.ship.trackID} - {this.ship.publicName}";
    IEnumerator enumerator = (IEnumerator) this.modulesList.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if (!((UnityEngine.Object) current == (UnityEngine.Object) null))
          UnityEngine.Object.Destroy((UnityEngine.Object) current.gameObject);
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    if (this.isHealth)
    {
      foreach (S_Module2 module in this.ship.modules)
        UnityEngine.Object.Instantiate<S_PartHealth>(this.partHealthPF, this.modulesList).Setup(module);
    }
    else
    {
      foreach (S_Module2 module in this.ship.modules)
      {
        if (module.bp.PartType == PartType.WEAPON && (module.mounts.Count <= 0 || module.mounts[0].bp.PartType != PartType.BEAM))
          UnityEngine.Object.Instantiate<S_PartHealth>(this.partHealthPF, this.modulesList).Setup(module);
      }
    }
  }

  public void OnDestroy()
  {
    if (!(bool) (UnityEngine.Object) this.tm)
      this.tm = TimeManager.current;
    this.tm.NewHour -= new Action(this.NewHour);
  }
}
