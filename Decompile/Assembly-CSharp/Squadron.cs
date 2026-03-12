// Decompiled with JetBrains decompiler
// Type: Squadron
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Squadron : MonoBehaviour
{
  [SerializeField]
  private List<Captain> ships = new List<Captain>();
  public Track targetTrack;
  public Vector3 targetPos;
  private bool isSelected;
  public int index;
  public Track track;
  private List<LineRenderer> lines = new List<LineRenderer>();
  private Target thisTarget;
  [Header("DATA")]
  public SquadronRole role;
  public SquadronCommand command;
  [Range(100f, 1000f)]
  public int dispersion;
  private Vector2 delayRange = new Vector2(1f, 3f);
  public bool isTakingNewOrders;
  [Header("LINES")]
  [SerializeField]
  private Color playerColor;
  [SerializeField]
  private Color playerColorSelected;
  [SerializeField]
  private Color hostileColor;
  private Color color;
  private WorldWidth ww;
  private float worldWidth;

  private void Start()
  {
    this.track = this.GetComponent<Track>();
    this.thisTarget = this.GetComponent<Target>();
    this.ww = WorldWidth.current;
    this.color = this.track.factionID != 1 ? this.hostileColor : this.playerColor;
    IEnumerator enumerator = (IEnumerator) this.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if ((bool) (UnityEngine.Object) current.GetComponent<LineRenderer>())
        {
          LineRenderer component = current.GetComponent<LineRenderer>();
          this.lines.Add(component);
          component.positionCount = 2;
          component.startColor = this.color;
          component.endColor = this.color;
        }
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    this.StartCoroutine((IEnumerator) this.CheckTargetCycle());
  }

  private void Update()
  {
    if ((UnityEngine.Object) this.thisTarget == (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) this.GetComponent<Target>() == (UnityEngine.Object) null)
        return;
      this.thisTarget = this.GetComponent<Target>();
    }
    bool flag = false;
    foreach (Component ship in this.ships)
    {
      if (ship.gameObject.activeSelf)
      {
        flag = true;
        break;
      }
    }
    if (!flag)
    {
      this.thisTarget.enabled = false;
    }
    else
    {
      if (this.ships.Count <= 1)
      {
        this.thisTarget.enabled = false;
        if (this.ships.Count == 1)
          this.UpdatePosition();
      }
      else
      {
        this.thisTarget.enabled = true;
        this.UpdatePosition();
      }
      this.UpdateLines();
    }
  }

  public void SetTarget(Track newTarget)
  {
    this.targetTrack = newTarget;
    foreach (Captain ship in this.ships)
      ship.ship.NewTarget(newTarget);
  }

  public void SetMissileUseToAuto()
  {
    foreach (Captain ship in this.ships)
      ship.isAutoLaunchOffensiveMissiles = true;
  }

  public void SetDispersion(int newDispersion)
  {
    this.dispersion = newDispersion;
    foreach (Captain ship in this.ships)
      ship.dispersion = this.dispersion;
  }

  public void NewCommand(SquadronCommand newCommand)
  {
    this.StartCoroutine((IEnumerator) this.CommandDelay(UnityEngine.Random.Range(this.delayRange.x, this.delayRange.y + 1f), newCommand));
  }

  private IEnumerator CommandDelay(float delay, SquadronCommand newCommand)
  {
    this.isTakingNewOrders = true;
    yield return (object) new WaitForSeconds(delay);
    this.isTakingNewOrders = false;
    this.SetCommand(newCommand);
  }

  private IEnumerator CheckTargetCycle()
  {
    yield return (object) new WaitForSeconds(1f);
    while (true)
    {
      if ((bool) (UnityEngine.Object) this.targetTrack)
      {
        if (this.targetTrack.type == TrackType.SQUADRON && !this.targetTrack.GetComponent<Squadron>().HasHealthyShip())
          this.targetTrack = (Track) null;
        else if (this.targetTrack.type == TrackType.SHIP && !this.targetTrack.GetComponent<Captain>().IsStillAlive())
          this.targetTrack = (Track) null;
      }
      yield return (object) new WaitForSeconds(10f);
    }
  }

  private void SetCommand(SquadronCommand newCommand)
  {
    this.command = newCommand;
    if (this.command == SquadronCommand.ENGAGE)
    {
      if ((this.targetTrack.type == TrackType.SQUADRON ? 1 : 0) != 0)
      {
        foreach (Captain ship1 in this.ships)
        {
          Transform newTarget = (Transform) null;
          float num1 = 10000f;
          foreach (Captain ship2 in this.targetTrack.GetComponent<Squadron>().ships)
          {
            float num2 = Vector3.Distance(ship1.transform.position, ship2.transform.position);
            if ((double) num2 < (double) num1)
            {
              num1 = num2;
              newTarget = ship2.transform;
            }
          }
          ship1.NewEngageOrder(newTarget);
        }
      }
      else
      {
        foreach (Captain ship in this.ships)
          ship.NewEngageOrder(this.targetTrack.transform);
      }
    }
    else if (this.command == SquadronCommand.MOVE)
    {
      foreach (Captain ship in this.ships)
      {
        ship.dispersion = this.dispersion;
        ship.NewMoveOrder(this.targetPos);
      }
    }
    else
    {
      if (this.command != SquadronCommand.ESCORT)
        return;
      foreach (Captain ship in this.ships)
      {
        ship.dispersion = this.dispersion;
        ship.NewEscortOrder(this.targetTrack.transform);
      }
    }
  }

  private void UpdatePosition()
  {
    Vector3 zero = Vector3.zero;
    foreach (Captain ship in this.ships)
      zero += ship.transform.position;
    this.transform.position = zero / (float) this.ships.Count;
  }

  private void ClearLines()
  {
    foreach (LineRenderer line in this.lines)
      line.positionCount = 0;
  }

  public void UpdateLineCount()
  {
    this.ClearLines();
    foreach (LineRenderer line in this.lines)
      line.positionCount = 2;
  }

  private void UpdateLines()
  {
    if (this.ships.Count <= 1)
    {
      this.ClearLines();
    }
    else
    {
      if ((UnityEngine.Object) this.ww == (UnityEngine.Object) null)
        this.ww = WorldWidth.current;
      float worldWidth = this.ww.GetWorldWidth(this.transform.position);
      for (int index = 0; index < this.ships.Count; ++index)
      {
        Vector3 position = this.ships[index].transform.position;
        this.lines[index].positionCount = 2;
        this.lines[index].SetPosition(0, position);
        this.lines[index].SetPosition(1, this.transform.position);
        this.worldWidth = this.ww.GetWorldWidth(position);
        this.lines[index].startWidth = this.worldWidth;
        this.lines[index].endWidth = worldWidth;
      }
    }
  }

  public void SetSelected(bool newIsSelected)
  {
    this.isSelected = newIsSelected;
    Color color = this.playerColor;
    if (this.isSelected)
      color = this.playerColorSelected;
    foreach (LineRenderer line in this.lines)
    {
      line.startColor = color;
      line.endColor = color;
    }
  }

  public void AddShipToSquadron(Captain ship)
  {
    this.ships.Add(ship.GetComponent<Captain>());
    ship.GetComponent<ShipController>().OnShipDeath += new Action<int, ShipController, bool>(this.OnShipDeath);
    this.UpdateLineCount();
  }

  private void OnShipDeath(int i, ShipController ship, bool isHardDeath)
  {
    this.RemoveShipFromSquadron(ship.GetComponent<Captain>());
  }

  private void RemoveShipFromSquadron(Captain ship)
  {
    this.ships.Remove(ship.GetComponent<Captain>());
    this.UpdateLineCount();
  }

  public void ClearShips() => this.ships.Clear();

  private bool HasShip(Captain ship) => this.ships.Contains(ship);

  public void RemoveIfContains(Captain ship)
  {
    if (!this.HasShip(ship))
      return;
    this.RemoveShipFromSquadron(ship);
  }

  public int GetShipCount() => this.ships.Count;

  public List<Captain> GetShips() => this.ships;

  public bool HasHealthyShip()
  {
    foreach (Captain ship in this.ships)
    {
      if ((bool) (UnityEngine.Object) ship && (bool) (UnityEngine.Object) ship.ship && !ship.ship.isDead)
        return true;
    }
    return false;
  }
}
