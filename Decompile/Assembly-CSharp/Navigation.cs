// Decompiled with JetBrains decompiler
// Type: Navigation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Navigation : MonoBehaviour
{
  private TimeManager tm;
  private Camera cam;
  [SerializeField]
  private FleetManager fleet;
  [SerializeField]
  private Orbiter orbiter;
  private SceneTransitionManager stm;
  private FactionsManager fm;
  [SerializeField]
  private LayerMask collisionMask;
  private Track thisTrack;
  private ColorManager cm;
  private float gAccDay = 73234.4f;
  public NavigationState currentState = NavigationState.Orbiting;
  public FlightType flightType;
  public ArrivalType arrivalType;
  public Transform target;
  public float targetOrbitRadius;
  public float targetOrbitPeriod;
  public float targetSOIRadius;
  [SerializeField]
  private float distanceToTarget;
  public float distanceToIntercept;
  public float halfJourneyTime;
  public float totalJourneyTime;
  public Vector3 targetInterceptPosition;
  private Vector3 decelerationPoint;
  private Vector3 initialPos;
  private Vector3 finalPos;
  private Vector3 interceptAngle;
  public Vector3 currentVel = Vector3.zero;
  public Vector3 currentAcc = Vector3.zero;
  public Vector3 currentDir = Vector3.zero;
  public float fuelRequired;
  public float accTime;
  public float driftTime;
  public float decTime;
  public float accDist;
  public float driftDist;
  public float decDist;
  public Vector3 interceptVelocity;
  public float interceptSpeed;
  public float interceptTime;
  public float dVRequired;
  public float artificialGravity;
  public bool isCollisionWarning;
  public bool isInsufficientDv;
  public bool isMaxAccExceeded;
  public bool isLowAcc;
  public float topVelocity;
  public float accG = 1f;
  public float decG = 1f;
  public float accKkm = 73234.4f;
  public float decKkm = 73234.4f;
  public float accRatio = 0.5f;
  public float decRatio = 0.5f;
  public float orbitDistance;
  public float totalTime;
  [Header("MARKERS")]
  [SerializeField]
  private LineRenderer lrGray;
  [SerializeField]
  private LineRenderer lrGreen;
  [SerializeField]
  private LineRenderer lrRed;
  [SerializeField]
  private Transform startMarker;
  [SerializeField]
  private Transform accMarker;
  [SerializeField]
  private Transform decelMarker;
  [SerializeField]
  private Transform stopMarker;
  public Vector3 startMarkerPos;
  public Vector3 accMarkerPos;
  public Vector3 decelMarkerPos;
  public Vector3 stopMarkerPos;
  [Header("EVENTS")]
  public Action onArrival;

  private void Start()
  {
    this.tm = TimeManager.current;
    this.fleet = this.GetComponent<FleetManager>();
    this.orbiter = this.GetComponent<Orbiter>();
    this.cam = CameraManager.current.cam;
    this.stm = SceneTransitionManager.current;
    this.fm = FactionsManager.current;
    this.thisTrack = this.GetComponent<Track>();
    this.cm = ColorManager.current;
    if (this.currentState != NavigationState.Orbiting)
      return;
    this.orbiter.isOrbiting = true;
  }

  private void Update()
  {
    if (this.currentState == NavigationState.Orbiting)
      this.CalculateFlight();
    this.UpdateInterceptLinesSize();
  }

  private void FixedUpdate()
  {
    if (this.currentState == NavigationState.Accelerating)
      this.Accelerate();
    else if (this.currentState == NavigationState.Drifting)
      this.Drift();
    else if (this.currentState == NavigationState.Decelerating)
    {
      this.Decelerate();
    }
    else
    {
      int currentState = (int) this.currentState;
    }
  }

  public void StartFlight()
  {
    if (!(bool) (UnityEngine.Object) this.target || this.currentState == NavigationState.Accelerating || this.currentState == NavigationState.Drifting || this.currentState == NavigationState.Decelerating || this.fleet.ships.Count == 0)
      return;
    if ((bool) (UnityEngine.Object) this.target.GetComponent<Navigation>() && this.target.GetComponent<Navigation>().currentState != NavigationState.Orbiting)
    {
      NotificationsManager.current.NewNotif("[NAVIGATION] CANNOT TARGET MOVING FLEETS", "red");
    }
    else
    {
      if (this.flightType == FlightType.MOVE)
      {
        this.currentDir = (this.targetInterceptPosition - this.transform.position).normalized;
        this.currentState = NavigationState.Accelerating;
      }
      if ((UnityEngine.Object) this.transform.parent != (UnityEngine.Object) null && (bool) (UnityEngine.Object) this.transform.parent.GetComponent<Attachments>())
        this.transform.parent.GetComponent<Attachments>().Remove(this.fleet);
      this.lrGreen.gameObject.SetActive(false);
      this.lrRed.gameObject.SetActive(false);
    }
  }

  public void SetTarget(Transform newTarget)
  {
    if (!(bool) (UnityEngine.Object) this.fleet)
    {
      this.fleet = this.GetComponent<FleetManager>();
      this.fleet.Init();
    }
    if ((UnityEngine.Object) newTarget == (UnityEngine.Object) this.transform || (UnityEngine.Object) newTarget == (UnityEngine.Object) this.transform.parent)
      return;
    if ((UnityEngine.Object) newTarget == (UnityEngine.Object) null || (double) this.fleet.fleetDv == 0.0)
    {
      this.target = (Transform) null;
      this.lrGray.gameObject.SetActive(false);
      this.lrGreen.gameObject.SetActive(false);
      this.lrRed.gameObject.SetActive(false);
      this.startMarker.gameObject.SetActive(false);
      this.accMarker.gameObject.SetActive(false);
      this.decelMarker.gameObject.SetActive(false);
      this.stopMarker.gameObject.SetActive(false);
    }
    else if ((bool) (UnityEngine.Object) newTarget.GetComponent<Orbiter>())
    {
      this.target = newTarget;
      this.targetOrbitRadius = newTarget.GetComponent<Orbiter>().orbitRadius;
      this.targetOrbitPeriod = newTarget.GetComponent<Orbiter>().orbitPeriod;
      this.targetSOIRadius = newTarget.GetComponent<Orbiter>().soiRadius;
      if (!newTarget.GetComponent<Orbiter>().isOrbitable)
        return;
      this.orbitDistance = newTarget.GetComponent<Orbiter>().soiRadius;
    }
    else
      MonoBehaviour.print((object) ("no joy 2. target: " + newTarget.name));
  }

  [ContextMenu("Calculate Flight")]
  public void CalculateFlight()
  {
    if (!(bool) (UnityEngine.Object) this.target || float.IsNaN(this.accG) || float.IsNaN(this.decG))
      return;
    if (this.flightType == FlightType.MOVE)
    {
      if (this.arrivalType == ArrivalType.INTERCEPT)
      {
        this.initialPos = this.transform.position;
        this.accKkm = this.accG * this.gAccDay;
        this.distanceToTarget = Vector3.Distance(this.transform.position, this.target.position);
        this.accDist = this.distanceToTarget;
        this.driftDist = 0.0f;
        this.accTime = Mathf.Sqrt(2f * this.accDist / this.accKkm);
        this.topVelocity = this.accKkm * this.accTime;
        this.totalTime = this.accTime;
        this.targetInterceptPosition = this.target.GetComponent<Orbiter>().GetFuturePosition(this.totalTime, 0);
        this.totalTime = this.accTime + this.decTime;
        this.distanceToIntercept = Vector3.Distance(this.transform.position, this.targetInterceptPosition);
        this.decDist = this.distanceToIntercept;
        this.decKkm = this.decG * this.gAccDay;
        this.decTime = this.topVelocity / this.decKkm;
        this.currentDir = (this.targetInterceptPosition - this.transform.position).normalized;
        this.finalPos = this.targetInterceptPosition + this.topVelocity * this.currentDir * this.decTime + 0.5f * (this.decKkm * -this.currentDir) * Mathf.Pow(this.decTime, 2f);
        float num1 = this.accG * 9.81f;
        float num2 = this.decG * 9.81f;
        this.dVRequired = Mathf.Sqrt(2f * num1 * this.accDist) + Mathf.Sqrt(2f * num2 * this.decDist);
        this.interceptVelocity = this.currentDir * this.topVelocity - this.target.GetComponent<Orbiter>().progradeDirection * this.target.GetComponent<Orbiter>().orbitalVelocity;
        this.interceptSpeed = (float) ((double) this.interceptVelocity.magnitude * (double) Scaling.current.toKm / 86400.0);
        this.interceptTime = 100f / this.interceptSpeed;
      }
      else
      {
        this.initialPos = this.transform.position;
        this.accKkm = this.accG * this.gAccDay;
        this.distanceToTarget = this.arrivalType != ArrivalType.ORBIT || !this.target.GetComponent<Orbiter>().isOrbitable ? Vector3.Distance(this.transform.position, this.target.position) : Vector3.Distance(this.transform.position, this.target.position) - this.orbitDistance / 1000f;
        this.accDist = this.accRatio * this.distanceToTarget;
        this.driftDist = (float) (1.0 - ((double) this.accRatio + (double) this.decRatio)) * this.distanceToTarget;
        this.accTime = Mathf.Sqrt(2f * this.accDist / this.accKkm);
        this.topVelocity = this.accKkm * this.accTime;
        this.driftTime = this.driftDist / this.topVelocity;
        this.decTime = Mathf.Sqrt(2f * this.decDist / this.decKkm);
        this.totalTime = this.accTime + this.driftTime + this.decTime;
        if (this.arrivalType == ArrivalType.ORBIT && this.target.GetComponent<Orbiter>().isOrbitable)
        {
          this.currentDir = (this.targetInterceptPosition - this.transform.position).normalized;
          this.targetInterceptPosition = this.target.GetComponent<Orbiter>().GetFuturePosition(this.totalTime, 0);
          this.targetInterceptPosition -= this.currentDir * this.orbitDistance / 1000f;
        }
        else
          this.targetInterceptPosition = this.target.GetComponent<Orbiter>().GetFuturePosition(this.totalTime, 0);
        this.distanceToIntercept = Vector3.Distance(this.transform.position, this.targetInterceptPosition);
        this.decDist = this.decRatio * this.distanceToIntercept;
        this.decKkm = (float) ((double) this.topVelocity * (double) this.topVelocity / (2.0 * (double) this.decDist));
        this.decG = this.decKkm / this.gAccDay;
        this.decTime = Mathf.Sqrt(2f * this.decDist / this.decKkm);
        float num3 = this.accG * 9.81f;
        float num4 = this.decG * 9.81f;
        this.dVRequired = Mathf.Sqrt(2f * num3 * this.accDist) + Mathf.Sqrt(2f * num4 * this.decDist);
      }
      this.CheckForWarnings();
      this.UpdateMarkerPositions();
      if (this.thisTrack.factionID != 1)
        return;
      this.DrawInterceptLines();
    }
    else
    {
      if (this.flightType == FlightType.ALTITUDE)
        return;
      int flightType = (int) this.flightType;
    }
  }

  private void CheckForWarnings()
  {
    this.CheckForCollisionWarning();
    this.CheckForAccWarning();
    this.CheckForDvWarning();
    this.CheckForLowAcc();
  }

  private void CheckForAccWarning()
  {
    if ((double) this.accG > (double) this.fleet.maxAccG || (double) this.decG > (double) this.fleet.maxAccG)
      this.isMaxAccExceeded = true;
    else
      this.isMaxAccExceeded = false;
  }

  private void CheckForDvWarning()
  {
    if ((double) this.dVRequired > (double) this.fleet.fleetDv)
      this.isInsufficientDv = true;
    else
      this.isInsufficientDv = false;
  }

  private void CheckForCollisionWarning()
  {
    RaycastHit hitInfo = new RaycastHit();
    Vector3 normalized = (this.targetInterceptPosition - this.transform.position).normalized;
    if (Physics.Linecast(this.transform.position, this.targetInterceptPosition, out hitInfo, (int) this.collisionMask))
      this.isCollisionWarning = true;
    else
      this.isCollisionWarning = false;
  }

  private void CheckForLowAcc()
  {
    if ((double) this.accG < 0.0099999997764825821 || (double) this.decG < 0.0099999997764825821)
      this.isLowAcc = true;
    else
      this.isLowAcc = false;
  }

  private void UpdateMarkerPositions()
  {
    if (this.flightType == FlightType.MOVE && this.arrivalType == ArrivalType.INTERCEPT)
    {
      this.startMarkerPos = this.transform.position;
      this.accMarkerPos = Vector3.Lerp(this.transform.position, this.targetInterceptPosition, this.accRatio * 2f);
      Vector3 vector3 = Vector3.Lerp(this.targetInterceptPosition, this.finalPos, (float) (1.0 - (double) this.decRatio * 2.0));
      if ((double) vector3.x == double.NaN)
        return;
      this.decelMarkerPos = vector3;
      this.stopMarkerPos = this.finalPos;
    }
    else
    {
      this.startMarkerPos = this.transform.position;
      this.accMarkerPos = Vector3.Lerp(this.transform.position, this.targetInterceptPosition, this.accRatio);
      Vector3 vector3 = Vector3.Lerp(this.transform.position, this.targetInterceptPosition, 1f - this.decRatio);
      if ((double) vector3.x == double.NaN)
        return;
      this.decelMarkerPos = vector3;
      this.stopMarkerPos = this.targetInterceptPosition;
    }
  }

  public void DrawInterceptLines()
  {
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null || this.targetInterceptPosition == Vector3.zero || float.IsNaN(this.targetInterceptPosition.x) || float.IsNaN(this.targetInterceptPosition.z))
      this.LinesOff();
    else if (this.isCollisionWarning)
    {
      this.lrGray.gameObject.SetActive(false);
      this.lrGreen.gameObject.SetActive(false);
      this.lrRed.gameObject.SetActive(true);
      this.lrRed.SetPosition(0, this.startMarkerPos);
      this.lrRed.SetPosition(1, this.stopMarkerPos);
    }
    else
    {
      this.lrGray.gameObject.SetActive(true);
      this.lrGreen.gameObject.SetActive(true);
      this.lrRed.gameObject.SetActive(true);
      this.startMarker.position = this.startMarkerPos;
      this.accMarker.position = this.accMarkerPos;
      this.decelMarker.position = this.decelMarkerPos;
      this.stopMarker.position = this.stopMarkerPos;
      this.lrGray.SetPosition(0, this.startMarkerPos);
      this.lrGray.SetPosition(1, this.stopMarkerPos);
      this.lrGreen.SetPosition(0, this.startMarkerPos);
      this.lrGreen.SetPosition(1, this.accMarkerPos);
      this.lrRed.SetPosition(0, this.decelMarkerPos);
      this.lrRed.SetPosition(1, this.stopMarkerPos);
    }
  }

  private void UpdateInterceptLinesSize()
  {
    float num1 = this.cam.orthographicSize / 200f;
    this.lrGray.widthMultiplier = num1;
    this.lrGreen.widthMultiplier = num1;
    this.lrRed.widthMultiplier = num1;
    float num2 = num1 * 8f;
    Vector3 vector3 = new Vector3(num2, num2, num2);
    this.startMarker.localScale = vector3;
    this.accMarker.localScale = vector3 * 0.5f;
    this.decelMarker.localScale = vector3 * 0.5f;
    this.stopMarker.localScale = vector3;
  }

  private void LinesOff()
  {
    this.lrGray.gameObject.SetActive(false);
    this.lrGreen.gameObject.SetActive(false);
    this.lrRed.gameObject.SetActive(false);
    this.startMarker.gameObject.SetActive(false);
    this.accMarker.gameObject.SetActive(false);
    this.decelMarker.gameObject.SetActive(false);
    this.stopMarker.gameObject.SetActive(false);
  }

  private bool CheckPropulsionValid(float G) => true;

  private void Accelerate()
  {
    this.orbiter.isOrbiting = false;
    this.transform.parent = (Transform) null;
    this.GetComponent<Target>().enabled = true;
    this.artificialGravity = this.accG;
    this.currentAcc = this.currentDir * this.accKkm;
    this.currentVel += this.currentAcc * this.tm.timeScale * Time.fixedDeltaTime;
    this.transform.position += this.currentVel * this.tm.timeScale * Time.fixedDeltaTime;
    this.fleet.ConsumeDv(this.accG * 9.81f);
    if (!this.CheckNextMarkPos(this.startMarkerPos, this.accMarkerPos))
      return;
    this.currentState = NavigationState.Drifting;
  }

  private bool CheckNextMarkPos(Vector3 startPos, Vector3 endPos)
  {
    return (double) Vector3.Distance(this.transform.position, startPos) >= (double) Vector3.Distance(startPos, endPos);
  }

  private void Drift()
  {
    this.orbiter.isOrbiting = false;
    this.artificialGravity = 0.0f;
    this.transform.position += this.currentVel * this.tm.timeScale * Time.fixedDeltaTime;
    if (!this.CheckNextMarkPos(this.accMarkerPos, this.decelMarkerPos))
      return;
    this.currentState = NavigationState.Decelerating;
  }

  private void Decelerate()
  {
    if (!(bool) (UnityEngine.Object) this.target)
    {
      this.ArrivedOrbitSOI();
    }
    else
    {
      this.orbiter.isOrbiting = false;
      float num1 = 2f;
      float num2 = 0.1f;
      this.artificialGravity = this.decG;
      this.currentAcc = -this.currentDir * this.decKkm;
      Vector3 vector3 = this.currentAcc * Time.fixedDeltaTime * this.tm.timeScale;
      if ((double) (this.currentVel + vector3).magnitude > 100.0)
        this.currentVel += vector3;
      this.transform.position += this.currentVel * Time.fixedDeltaTime * this.tm.timeScale;
      this.fleet.ConsumeDv(this.decG * 9.81f);
      if (this.arrivalType == ArrivalType.STOP)
      {
        if ((double) Vector3.Distance(this.transform.position, this.target.position) <= (double) num1)
          this.ArrivedStop();
        else if (this.CheckNextMarkPos(this.decelMarkerPos, this.stopMarkerPos))
        {
          this.ArrivedStop();
        }
        else
        {
          if ((double) this.currentVel.magnitude >= (double) num2)
            return;
          this.ArrivedStop();
        }
      }
      else if (this.arrivalType == ArrivalType.ORBIT)
      {
        if ((double) Vector3.Distance(this.transform.position, this.target.position) <= (double) this.orbitDistance / 1000.0)
        {
          if (this.target.GetComponent<Orbiter>().isOrbitable)
            this.ArrivedOrbit();
          else
            this.ArrivedStop();
        }
        else if (this.CheckNextMarkPos(this.decelMarkerPos, this.stopMarkerPos))
        {
          this.ArrivedOrbit();
        }
        else
        {
          if ((double) this.currentVel.magnitude >= (double) num2)
            return;
          this.ArrivedOrbit();
        }
      }
      else
      {
        if (this.arrivalType != ArrivalType.INTERCEPT)
          return;
        if (this.CheckNextMarkPos(this.accMarkerPos, this.decelMarkerPos))
          this.Intercept();
        else if (this.CheckNextMarkPos(this.decelMarkerPos, this.stopMarkerPos))
        {
          this.Intercept();
        }
        else
        {
          if ((double) this.currentVel.magnitude >= (double) num2)
            return;
          this.Intercept();
        }
      }
    }
  }

  private void Intercept()
  {
  }

  private void EncounterSetup()
  {
    bool flag1 = false;
    bool flag2 = false;
    Faction factionFromId = this.fm.GetFactionFromID(this.GetComponent<Track>().factionID);
    Faction faction = (Faction) null;
    if ((bool) (UnityEngine.Object) this.target.GetComponent<FleetManager>())
      faction = this.fm.GetFactionFromID(this.target.GetComponent<Track>().factionID);
    if (factionFromId.factionID == 1 || faction != null && faction.factionID == 1)
      flag1 = true;
    if (faction != null && factionFromId.GetHostiles().Contains(faction.factionID))
      flag2 = true;
    if (flag2)
    {
      if (!flag1)
        return;
      List<TacticalGroupData> newGroups = new List<TacticalGroupData>();
      if ((bool) (UnityEngine.Object) this.target.GetComponent<FleetManager>())
        newGroups.Add(this.target.GetComponent<FleetManager>().ToTacticalGroup());
      newGroups.Add(this.fleet.ToTacticalGroup());
      this.stm.SetupEngagement(newGroups, this.transform.position);
    }
    else
    {
      this.GetComponent<Target>().enabled = false;
      this.target.GetComponent<Attachments>().attachedFleets.Add(this.fleet);
    }
  }

  private void ArrivedStop()
  {
    this.EncounterSetup();
    this.currentState = NavigationState.Orbiting;
    this.currentVel = Vector3.zero;
    this.currentAcc = Vector3.zero;
    this.artificialGravity = 0.0f;
    this.transform.parent = this.target;
    this.transform.localPosition = Vector3.zero;
    this.ArrivedEnd();
  }

  private void ArrivedOrbit()
  {
    this.currentState = NavigationState.Orbiting;
    this.currentVel = Vector3.zero;
    this.currentAcc = Vector3.zero;
    if ((double) Vector3.Distance(this.transform.position, this.target.position) > (double) this.target.GetComponent<Orbiter>().soiRadius)
      this.transform.position = this.targetInterceptPosition + this.currentDir * 0.1f;
    else
      this.transform.position = this.targetInterceptPosition;
    this.orbiter.SetTarget(this.target.GetComponent<Orbiter>());
    this.ArrivedEnd();
  }

  private void ArrivedOrbitSOI()
  {
    this.currentState = NavigationState.Orbiting;
    this.currentVel = Vector3.zero;
    this.currentAcc = Vector3.zero;
    Orbiter newTarget = (Orbiter) null;
    float num1 = float.PositiveInfinity;
    foreach (Orbiter orbiter in UnityEngine.Object.FindObjectsOfType<Orbiter>())
    {
      if (orbiter.isOrbitable)
      {
        float num2 = Vector3.Distance(this.transform.position, orbiter.transform.position);
        if ((double) num2 < (double) num1 && (double) num2 < (double) orbiter.soiRadius)
        {
          num1 = num2;
          newTarget = orbiter;
        }
      }
    }
    if ((UnityEngine.Object) newTarget == (UnityEngine.Object) null)
    {
      MonoBehaviour.print((object) "ERROR: No parent SOI found");
    }
    else
    {
      this.orbiter.SetTarget(newTarget);
      this.ArrivedEnd();
    }
  }

  private void ArrivedEnd()
  {
    this.SetTarget((Transform) null);
    Action onArrival = this.onArrival;
    if (onArrival == null)
      return;
    onArrival();
  }
}
