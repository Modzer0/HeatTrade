// Decompiled with JetBrains decompiler
// Type: aiAdmiral
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class aiAdmiral : MonoBehaviour
{
  [Header("SQUADRONS")]
  public List<Squadron> aiSquadrons = new List<Squadron>();
  public List<Squadron> playerSquadrons = new List<Squadron>();
  private List<Squadron> targetableSquadrons = new List<Squadron>();
  private bool initStrategySet;

  private void Update()
  {
    if (this.initStrategySet)
      return;
    this.StartCoroutine((IEnumerator) this.CheckTargetCycle());
    this.initStrategySet = true;
  }

  private IEnumerator CheckTargetCycle()
  {
    yield return (object) new WaitForSeconds(10f);
    while (true)
    {
      this.SetStrategy();
      yield return (object) new WaitForSeconds(10f);
    }
  }

  private void SetStrategy()
  {
    bool flag = false;
    this.targetableSquadrons.Clear();
    foreach (Squadron playerSquadron in this.playerSquadrons)
    {
      if (playerSquadron.GetShipCount() > 0 && playerSquadron.HasHealthyShip())
      {
        flag = true;
        this.targetableSquadrons.Add(playerSquadron);
      }
    }
    if (!flag)
      return;
    foreach (Squadron aiSquadron in this.aiSquadrons)
    {
      if ((Object) aiSquadron.targetTrack == (Object) null && aiSquadron.HasHealthyShip())
      {
        Squadron targetableSquadron = this.targetableSquadrons[Random.Range(0, this.targetableSquadrons.Count)];
        aiSquadron.targetTrack = targetableSquadron.track;
        aiSquadron.NewCommand(SquadronCommand.ENGAGE);
      }
    }
  }
}
