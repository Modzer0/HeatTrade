// Decompiled with JetBrains decompiler
// Type: OffScreenIndicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class OffScreenIndicator : MonoBehaviour
{
  [Tooltip("Distance offset of the indicators from the centre of the screen")]
  [SerializeField]
  private float screenBoundOffsetX = 0.56f;
  [SerializeField]
  private float screenBoundOffsetY = 0.85f;
  private TacticalInputs ti;
  private MapInputs mi;
  private CameraManager cm;
  private FactionsManager fm;
  private Camera mainCamera;
  private Transform origin;
  private Vector3 screenCentre;
  private Vector3 screenBounds;
  private List<Target> targets = new List<Target>();
  public static Action<Target, bool> TargetStateChanged;
  [SerializeField]
  private Camera currentCam;
  [SerializeField]
  private Sprite diamondImg;
  [SerializeField]
  private Sprite diamondSelectImg;
  [SerializeField]
  private Sprite triangleImg;
  [SerializeField]
  private Sprite circleImg;
  [SerializeField]
  private Sprite smallCircleImg;
  [SerializeField]
  private Sprite fCircleImg;
  [SerializeField]
  private Sprite circleSelectImg;
  [SerializeField]
  private Sprite squareImg;
  [SerializeField]
  private Sprite squareSelectImg;
  [SerializeField]
  private Color green;
  [SerializeField]
  private Color yellow;
  [SerializeField]
  private Color red;
  public bool isBodyVisible = true;
  public bool isOwnVisible = true;
  public bool isAlyVisible = true;
  public bool isNeuVisible = true;
  public bool isHosVisible = true;
  public bool isTactical;
  private int currentTextScale;
  private Canvas parentCanvas;
  private Camera canvasCam;

  private void Awake()
  {
    this.mainCamera = this.currentCam;
    this.screenCentre = new Vector3((float) Screen.width, (float) Screen.height, 0.0f) / 2f;
    this.screenBounds = new Vector3(this.screenCentre.x * this.screenBoundOffsetX, this.screenCentre.y * this.screenBoundOffsetY, 0.0f);
    OffScreenIndicator.TargetStateChanged += new Action<Target, bool>(this.HandleTargetStateChanged);
    if (!this.isTactical)
      return;
    this.origin = this.mainCamera.transform;
  }

  private void Start()
  {
    this.ti = TacticalInputs.current;
    this.mi = MapInputs.current;
    this.cm = CameraManager.current;
    this.fm = FactionsManager.current;
    this.parentCanvas = this.GetComponentInParent<Canvas>();
    this.canvasCam = this.parentCanvas.worldCamera;
  }

  private void LateUpdate() => this.DrawIndicators();

  private bool IsVisible(Transform target)
  {
    Vector3 screenPoint = this.mainCamera.WorldToScreenPoint(target.position);
    return (double) screenPoint.z > 0.0 && (double) screenPoint.x >= 0.0 && (double) screenPoint.x <= (double) Screen.width && (double) screenPoint.y >= 0.0 && (double) screenPoint.y <= (double) Screen.height;
  }

  private void DrawIndicators()
  {
    this.origin = !this.isTactical || !(bool) (UnityEngine.Object) this.ti.selectedShip ? (!(bool) (UnityEngine.Object) this.mi || !((UnityEngine.Object) this.mi.selectedFleet != (UnityEngine.Object) null) ? (!this.isTactical ? (Transform) null : this.mainCamera.transform) : this.mi.selectedFleet.transform) : this.ti.selectedShip.transform;
    if ((UnityEngine.Object) this.cm != (UnityEngine.Object) null)
      this.currentTextScale = this.cm.targetZoom > 100 ? (this.cm.targetZoom > 300 ? (this.cm.targetZoom > 500 ? 0 : 1) : 2) : 3;
    foreach (Target target in this.targets)
    {
      Track track = target.track;
      if ((track.type != TrackType.BODY || this.isBodyVisible) && (track.type != TrackType.FLEET || track.iff != IFF.OWNED || this.isOwnVisible) && (track.type != TrackType.FLEET || track.iff != IFF.FRIENDLY || this.isAlyVisible) && (track.type != TrackType.FLEET || track.iff != IFF.NEUTRAL || this.isNeuVisible) && (track.type != TrackType.FLEET || track.iff != IFF.HOSTILE || this.isHosVisible))
      {
        Vector3 screenPoint = this.mainCamera.WorldToScreenPoint(target.transform.position);
        double planeDistance = (double) this.parentCanvas.planeDistance;
        screenPoint.z = this.parentCanvas.planeDistance;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.transform as RectTransform, (Vector2) screenPoint, this.canvasCam, out localPoint);
        bool flag = !this.isTactical || this.IsVisible(target.transform);
        Indicator indicator1 = (Indicator) null;
        bool isArrow = false;
        if (target.NeedBoxIndicator)
        {
          if (target.NeedBoxIndicator & flag)
          {
            indicator1 = this.GetIndicator(ref target.indicator, IndicatorType.BOX);
            if ((bool) (UnityEngine.Object) indicator1)
            {
              if (flag)
              {
                indicator1.Activate(true);
              }
              else
              {
                indicator1.Activate(false);
                continue;
              }
            }
          }
          else if (target.NeedArrowIndicator)
          {
            Indicator indicator2 = this.GetIndicator(ref target.indicator, IndicatorType.ARROW);
            if ((bool) (UnityEngine.Object) indicator2)
            {
              indicator2.Activate(false);
              continue;
            }
            continue;
          }
        }
        if ((bool) (UnityEngine.Object) indicator1)
        {
          indicator1.SetTarget(target);
          TrackType type = track.type;
          indicator1.transform.localPosition = new Vector3(localPoint.x, localPoint.y, 0.0f);
          bool isSelected = false;
          bool isDead = false;
          if (this.isTactical && (UnityEngine.Object) this.ti != (UnityEngine.Object) null)
          {
            if ((bool) (UnityEngine.Object) this.ti.selectedShip && (UnityEngine.Object) this.ti.selectedShip == (UnityEngine.Object) target.GetComponent<ShipController>() || (bool) (UnityEngine.Object) this.ti.selectedSquadron && (UnityEngine.Object) this.ti.selectedSquadron == (UnityEngine.Object) target.GetComponent<Squadron>() || (bool) (UnityEngine.Object) this.ti.target && (UnityEngine.Object) this.ti.target == (UnityEngine.Object) track)
              isSelected = true;
            if ((bool) (UnityEngine.Object) target.GetComponent<ShipController>())
              isDead = target.GetComponent<ShipController>().isDead;
          }
          else if (!this.isTactical && (UnityEngine.Object) this.mi != (UnityEngine.Object) null && ((bool) (UnityEngine.Object) this.mi.selectedFleet && (UnityEngine.Object) this.mi.selectedFleet == (UnityEngine.Object) target.GetComponent<FleetManager>() || (UnityEngine.Object) this.mi.GetTargetTrack() == (UnityEngine.Object) track))
            isSelected = true;
          if ((UnityEngine.Object) target != (UnityEngine.Object) null && this.isTactical)
          {
            indicator1.SetSourceImg(this.SetTypeTactical(type, isSelected));
            indicator1.SetImageColor(target.TargetColor);
            string newText = "?";
            if ((UnityEngine.Object) track != (UnityEngine.Object) null && track.id != null)
              newText = track.id;
            if (indicator1.indicatorType == IndicatorType.BOX)
            {
              switch (type)
              {
                case TrackType.LOCATION:
                  indicator1.SetNameText($"[{track.publicName}]");
                  break;
                case TrackType.SHIP:
                  indicator1.SetNameText(track.trackName);
                  break;
                case TrackType.MISSILE:
                  indicator1.SetNameText(newText);
                  break;
                case TrackType.SQUADRON:
                  indicator1.SetNameText(track.publicName);
                  break;
              }
            }
            else
              indicator1.SetNameText("");
            indicator1.SetDistText(Mathf.Round(target.GetDistanceFrom(this.origin.position) * 10f));
            indicator1.SetColor(target.TargetColor, isDead);
          }
          else if ((UnityEngine.Object) target != (UnityEngine.Object) null && !this.isTactical)
          {
            indicator1.SetSourceImg(this.SetType(type, isArrow, isSelected));
            indicator1.SetImageColor(target.TargetColor);
            string trackID = "?";
            if ((UnityEngine.Object) track != (UnityEngine.Object) null && track.id != null)
              trackID = track.id;
            string attachmentsText = "";
            if ((bool) (UnityEngine.Object) target.GetComponent<Attachments>())
              attachmentsText = this.SetAttachmentsText(target.GetComponent<Attachments>());
            if (indicator1.indicatorType == IndicatorType.BOX)
            {
              this.SetNameText(indicator1, type, track, trackID, attachmentsText);
              this.SetDistText(indicator1, target);
            }
            else
            {
              indicator1.SetNameText("");
              indicator1.SetDistText(0.0f);
            }
            indicator1.SetColor(target.TargetColor, false);
          }
        }
      }
    }
  }

  private string SetAttachmentsText(Attachments attachments)
  {
    string str = "";
    if (attachments.isStation)
    {
      if (this.currentTextScale == 3)
      {
        if (attachments.attachedFleets.Count > 0)
          str = $"{str}+{attachments.attachedFleets.Count.ToString()}";
      }
      else if (this.currentTextScale == 2)
      {
        if (attachments.attachedFleets.Count > 0)
          str = $"{str}+{attachments.attachedFleets.Count.ToString()}";
      }
      else if (this.currentTextScale == 1 && attachments.attachedFleets.Count > 0)
        str = $"{str}+{attachments.attachedFleets.Count.ToString()}";
    }
    else if (this.currentTextScale == 3)
    {
      if ((UnityEngine.Object) attachments.station != (UnityEngine.Object) null && (UnityEngine.Object) attachments.station.track != (UnityEngine.Object) null)
        str = $" {this.fm.GetFactionCode(attachments.station.track.factionID)} {attachments.station.track.publicName}";
      if (attachments.attachedFleets.Count > 0)
        str = $"{str}+{attachments.attachedFleets.Count.ToString()}";
    }
    else if (this.currentTextScale == 2)
    {
      if ((UnityEngine.Object) attachments.station != (UnityEngine.Object) null && (UnityEngine.Object) attachments.station.track != (UnityEngine.Object) null)
        str = " " + attachments.station.track.publicName;
      if (attachments.attachedFleets.Count > 0)
        str = $"{str}+{attachments.attachedFleets.Count.ToString()}";
    }
    else if (this.currentTextScale == 1)
    {
      if ((UnityEngine.Object) attachments.station != (UnityEngine.Object) null && (UnityEngine.Object) attachments.station.track != (UnityEngine.Object) null)
        str = " " + attachments.station.track.publicName;
      if (attachments.attachedFleets.Count > 0)
        str = $"{str}+{attachments.attachedFleets.Count.ToString()}";
    }
    return str;
  }

  private void SetDistText(Indicator indicator, Target target)
  {
    float newDist = 0.0f;
    if ((UnityEngine.Object) this.origin == (UnityEngine.Object) null)
    {
      indicator.SetDistText(0.0f);
    }
    else
    {
      if (!this.isTactical)
      {
        if (this.currentTextScale == 3)
          newDist = target.GetDistanceFrom(this.origin.position);
        else if (this.currentTextScale == 2)
          newDist = target.GetDistanceFrom(this.origin.position);
      }
      else
        newDist = target.GetDistanceFrom(this.origin.position);
      indicator.SetDistText(newDist);
    }
  }

  private void SetNameText(
    Indicator indicator,
    TrackType type,
    Track track,
    string trackID,
    string attachmentsText)
  {
    string newText = "NULL";
    if (!this.isTactical)
    {
      if (this.currentTextScale == 3)
      {
        switch (type)
        {
          case TrackType.LOCATION:
            newText = $"[{track.publicName}]{attachmentsText}";
            break;
          case TrackType.FLEET:
            newText = $"{this.fm.GetFactionCode(track.factionID)} {track.id} - {track.publicName} {attachmentsText}";
            break;
          case TrackType.STRUCTURE:
          case TrackType.STATION:
            newText = $"{this.fm.GetFactionCode(track.factionID)} {track.id} - {track.publicName} {attachmentsText}";
            break;
          case TrackType.SHIP:
            newText = $"{this.fm.GetFactionCode(track.factionID)} {track.id} - {track.publicName}";
            break;
          case TrackType.MISSILE:
            newText = trackID;
            break;
          case TrackType.BODY:
            newText = track.publicName;
            break;
        }
      }
      else if (this.currentTextScale == 2)
      {
        switch (type)
        {
          case TrackType.LOCATION:
            newText = $"[{track.publicName}]{attachmentsText}";
            break;
          case TrackType.FLEET:
            newText = $"{this.fm.GetFactionCode(track.factionID)} {track.id} {attachmentsText}";
            break;
          case TrackType.STRUCTURE:
          case TrackType.STATION:
            newText = $"{this.fm.GetFactionCode(track.factionID)} {track.id} {attachmentsText}";
            break;
          case TrackType.SHIP:
            newText = track.trackName;
            break;
          case TrackType.MISSILE:
            newText = trackID;
            break;
          case TrackType.BODY:
            newText = track.publicName;
            break;
        }
      }
      else if (this.currentTextScale == 1)
      {
        switch (type)
        {
          case TrackType.LOCATION:
            newText = $"[{track.publicName}]{attachmentsText}";
            break;
          case TrackType.FLEET:
            newText = track.id + attachmentsText;
            break;
          case TrackType.STRUCTURE:
          case TrackType.STATION:
            newText = track.id;
            break;
          case TrackType.SHIP:
            newText = track.trackName;
            break;
          case TrackType.MISSILE:
            newText = trackID;
            break;
          case TrackType.BODY:
            newText = track.publicName;
            break;
        }
      }
      else
        newText = "";
    }
    else
    {
      switch (type)
      {
        case TrackType.LOCATION:
          newText = $"[{track.publicName}]{attachmentsText}";
          break;
        case TrackType.FLEET:
          newText = track.id + attachmentsText;
          break;
        case TrackType.STRUCTURE:
        case TrackType.STATION:
          newText = $"{this.fm.GetFactionCode(track.factionID)} {track.id} - {track.publicName}";
          break;
        case TrackType.SHIP:
          newText = track.trackName;
          break;
        case TrackType.MISSILE:
          newText = trackID;
          break;
        case TrackType.BODY:
          newText = track.publicName;
          break;
      }
    }
    indicator.SetNameText(newText);
  }

  private Sprite SetTypeTactical(TrackType type, bool isSelected)
  {
    Sprite sprite = this.diamondImg;
    switch (type)
    {
      case TrackType.LOCATION:
        sprite = this.circleImg;
        break;
      case TrackType.STRUCTURE:
      case TrackType.STATION:
        sprite = this.squareImg;
        break;
      case TrackType.SHIP:
        sprite = !((bool) (UnityEngine.Object) this.ti.selectedShip & isSelected) ? this.diamondImg : this.diamondSelectImg;
        break;
      case TrackType.MISSILE:
        sprite = this.triangleImg;
        break;
      case TrackType.BODY:
        sprite = this.fCircleImg;
        break;
      case TrackType.SQUADRON:
        sprite = !((bool) (UnityEngine.Object) this.ti.selectedSquadron & isSelected) ? this.circleImg : this.circleSelectImg;
        break;
    }
    return sprite;
  }

  private Sprite SetType(TrackType type, bool isArrow, bool isSelected)
  {
    Sprite sprite = this.diamondImg;
    if (isArrow || this.currentTextScale == 0)
      sprite = this.smallCircleImg;
    else if (this.currentTextScale == 3 || this.currentTextScale == 2 || this.currentTextScale == 1)
    {
      switch (type)
      {
        case TrackType.LOCATION:
          sprite = this.circleImg;
          break;
        case TrackType.FLEET:
        case TrackType.SHIP:
          sprite = !isSelected ? this.diamondImg : this.diamondSelectImg;
          break;
        case TrackType.STRUCTURE:
        case TrackType.STATION:
          sprite = !isSelected ? this.squareImg : this.squareSelectImg;
          break;
        case TrackType.MISSILE:
          sprite = this.triangleImg;
          break;
        case TrackType.BODY:
          sprite = this.fCircleImg;
          break;
      }
    }
    return sprite;
  }

  private void HandleTargetStateChanged(Target target, bool active)
  {
    if (active)
    {
      this.targets.Add(target);
    }
    else
    {
      target.indicator?.Activate(false);
      target.indicator = (Indicator) null;
      this.targets.Remove(target);
    }
  }

  private Indicator GetIndicator(ref Indicator indicator, IndicatorType type)
  {
    if ((UnityEngine.Object) indicator != (UnityEngine.Object) null)
    {
      if (indicator.Type != type)
      {
        indicator.Activate(false);
        indicator = type == IndicatorType.BOX ? BoxObjectPool.current.GetPooledObject() : ArrowObjectPool.current.GetPooledObject();
        indicator.Activate(true);
      }
    }
    else
    {
      indicator = type == IndicatorType.BOX ? BoxObjectPool.current.GetPooledObject() : ArrowObjectPool.current.GetPooledObject();
      indicator.Activate(true);
    }
    return indicator;
  }

  private void OnDestroy()
  {
    OffScreenIndicator.TargetStateChanged -= new Action<Target, bool>(this.HandleTargetStateChanged);
  }

  private void UpdateTracksVisibility()
  {
    foreach (Target target in this.targets)
    {
      if ((UnityEngine.Object) target != (UnityEngine.Object) null)
      {
        Track track = target.track;
        if (track.type == TrackType.BODY)
        {
          if (!this.isBodyVisible)
            target.SetVisibility(false);
          else
            target.SetVisibility(true);
        }
        else if (track.iff == IFF.OWNED)
        {
          if (!this.isOwnVisible)
            target.SetVisibility(false);
          else
            target.SetVisibility(true);
        }
        else if (track.iff == IFF.FRIENDLY)
        {
          if (!this.isAlyVisible)
            target.SetVisibility(false);
          else
            target.SetVisibility(true);
        }
        else if (track.iff == IFF.NEUTRAL)
        {
          if (!this.isNeuVisible)
            target.SetVisibility(false);
          else
            target.SetVisibility(true);
        }
        else if (track.iff == IFF.HOSTILE)
        {
          if (!this.isHosVisible)
            target.SetVisibility(false);
          else
            target.SetVisibility(true);
        }
      }
    }
  }

  public void SetBodiesVisible(bool isOn)
  {
    this.isBodyVisible = isOn;
    this.UpdateTracksVisibility();
  }

  public void SetFleetsVisible(int index)
  {
    switch (index)
    {
      case 1:
        this.isOwnVisible = !this.isOwnVisible;
        break;
      case 2:
        this.isAlyVisible = !this.isAlyVisible;
        break;
      case 3:
        this.isNeuVisible = !this.isNeuVisible;
        break;
      case 4:
        this.isHosVisible = !this.isHosVisible;
        break;
    }
    this.UpdateTracksVisibility();
  }
}
