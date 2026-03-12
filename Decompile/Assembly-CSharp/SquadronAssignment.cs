// Decompiled with JetBrains decompiler
// Type: SquadronAssignment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SquadronAssignment : MonoBehaviour
{
  public static SquadronAssignment current;
  private PlayerFleetUI pfui;
  [SerializeField]
  private List<Squadron> playerSquadrons;
  [SerializeField]
  private List<Squadron> hostileSquadrons;
  [SerializeField]
  private List<Squadron> allSquadrons;
  [SerializeField]
  private List<Captain> playerShips = new List<Captain>();
  [SerializeField]
  private List<Captain> hostileShips = new List<Captain>();
  private int hostileFactionID;
  [SerializeField]
  private bool isFindShipsOnStart = true;

  private void Awake() => SquadronAssignment.current = this;

  private void Start()
  {
    this.pfui = PlayerFleetUI.current;
    this.ClearSquadrons();
    if (this.isFindShipsOnStart)
      this.GetShips();
    this.AssignShipsToSquadrons();
    this.SetAIShipsMissiles();
  }

  private void ClearSquadrons()
  {
    foreach (Squadron playerSquadron in this.playerSquadrons)
      this.allSquadrons.Add(playerSquadron);
    foreach (Squadron hostileSquadron in this.hostileSquadrons)
      this.allSquadrons.Add(hostileSquadron);
    foreach (Squadron allSquadron in this.allSquadrons)
      allSquadron.ClearShips();
  }

  private void GetShips()
  {
    foreach (Captain captain in Object.FindObjectsOfType<Captain>())
    {
      if (captain.GetComponent<Track>().factionID == 1)
      {
        this.playerShips.Add(captain);
      }
      else
      {
        this.hostileShips.Add(captain);
        this.hostileFactionID = captain.GetComponent<Track>().factionID;
      }
    }
    foreach (Squadron hostileSquadron in this.hostileSquadrons)
    {
      hostileSquadron.GetComponent<Track>().factionID = this.hostileFactionID;
      hostileSquadron.GetComponent<Track>().iff = IFF.HOSTILE;
    }
  }

  private void AssignShipsToSquadrons()
  {
    foreach (Captain playerShip in this.playerShips)
    {
      if (playerShip.role == CaptainRole.LANCER)
        this.playerSquadrons[0].AddShipToSquadron(playerShip);
      else if (playerShip.role == CaptainRole.CATAPHRACT)
        this.playerSquadrons[1].AddShipToSquadron(playerShip);
      else if (playerShip.role == CaptainRole.ARTILLERY || playerShip.role == CaptainRole.CARRIER)
        this.playerSquadrons[2].AddShipToSquadron(playerShip);
      else if (playerShip.role == CaptainRole.SUPPORT)
        this.playerSquadrons[3].AddShipToSquadron(playerShip);
    }
    foreach (Captain hostileShip in this.hostileShips)
    {
      if (hostileShip.role == CaptainRole.LANCER)
        this.hostileSquadrons[0].AddShipToSquadron(hostileShip);
      else if (hostileShip.role == CaptainRole.CATAPHRACT)
        this.hostileSquadrons[1].AddShipToSquadron(hostileShip);
      else if (hostileShip.role == CaptainRole.ARTILLERY || hostileShip.role == CaptainRole.CARRIER)
        this.hostileSquadrons[2].AddShipToSquadron(hostileShip);
      else if (hostileShip.role == CaptainRole.SUPPORT)
        this.hostileSquadrons[3].AddShipToSquadron(hostileShip);
      hostileShip.GetComponent<Pathfinder>().isShowMoveLine = false;
    }
  }

  private void SetAIShipsMissiles()
  {
    foreach (Squadron hostileSquadron in this.hostileSquadrons)
      hostileSquadron.SetMissileUseToAuto();
  }

  public void AddShipToSquadron(Squadron selectedSquadron, Captain ship)
  {
    MonoBehaviour.print((object) $"adding ship: {ship.name} to squadron: {selectedSquadron.name}");
    foreach (Squadron allSquadron in this.allSquadrons)
      allSquadron.RemoveIfContains(ship);
    selectedSquadron.AddShipToSquadron(ship);
    this.pfui.RefreshSquadronList();
    selectedSquadron.UpdateLineCount();
  }
}
