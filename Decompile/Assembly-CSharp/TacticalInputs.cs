// Decompiled with JetBrains decompiler
// Type: TacticalInputs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class TacticalInputs : MonoBehaviour
{
  public static TacticalInputs current;
  private HeightPointer hp;
  private FactionsManager fm;
  private AudioManager am;
  private PlayerFleetUI pfui;
  private Camera mainCam;
  private CommandTooltip ct;
  private SquadronAssignment squadAss;
  [SerializeField]
  private GameObject menuPanel;
  private uiAnimator menuAnim;
  private LaserToggler lt;
  private TimeController timeCon;
  [Header("UI")]
  [SerializeField]
  private GameObject ui;
  [SerializeField]
  private TMP_Text selectedText;
  [SerializeField]
  private TMP_Text currentCommandText;
  [SerializeField]
  private CaptainDataUI selectedCaptainData;
  [SerializeField]
  private SquadronDataUI selectedSquadronData;
  [Header("INPUTS")]
  private bool isMapOn;
  public bool isInputOn = true;
  [SerializeField]
  private bool isAllowMap = true;
  [SerializeField]
  private TacticalCamera tc;
  private bool mouseDown0;
  private bool mouseDown1;
  private float lastMouseDown0;
  private float doubleClickThresh = 0.3f;
  [Header("LAYERS")]
  [SerializeField]
  private LayerMask hudOnMask;
  [SerializeField]
  private LayerMask hudOffMask;
  [Header("SWIVEL")]
  [SerializeField]
  private float mouseSens;
  private float yaw;
  private float pitch;
  [Header("SCOPE")]
  [SerializeField]
  private Camera scopeCam;
  public bool isScopeOn;
  [Header("TARGET")]
  public Track tempTarget;
  public Track target;
  [Header("SELECTION")]
  [SerializeField]
  private SelectionMode selectionMode;
  [SerializeField]
  private GameObject selectedShipUI;
  [Header("SELECTED SHIP")]
  public List<ShipController> shipsOwned = new List<ShipController>();
  public ShipController selectedShip;
  public Pathfinder selectedPathfinder;
  public CommandMode mode;
  [Header("SELECTED SQUADRON")]
  public List<Squadron> squadronsOwned = new List<Squadron>();
  public Squadron selectedSquadron;
  public SquadronCommand sqdCommand;
  private LineRenderer lr;
  [Header("LINES")]
  [SerializeField]
  private LineRenderer lr2;
  [SerializeField]
  private LineRendererUI lruiGreen;
  [SerializeField]
  private LineRendererUI lruiRed;
  [SerializeField]
  private LineRendererUI lruiWhite1;
  [SerializeField]
  private LineRendererUI lruiWhite2;
  [SerializeField]
  private Transform dispersionSphere;
  public bool isHudOn = true;
  [SerializeField]
  private RectTransform canvasRect;
  [Header("MOVEMENT")]
  public Vector3 intersectionPoint;
  public float intersectionPointDist;
  [Range(-5000f, 5000f)]
  private float xDest;
  [Range(-5000f, 5000f)]
  private float yDest;
  [Range(-5000f, 5000f)]
  private float zDest;
  [Header("COLORS")]
  [SerializeField]
  private Color colorWhite;
  [SerializeField]
  private Color colorRed;
  [SerializeField]
  private Color colorGreen;
  [SerializeField]
  private Color colorBlue;
  private bool isDrawMoveline;
  private TacticalInputs.MovelineData currentMLD = new TacticalInputs.MovelineData(Vector3.zero, Vector3.zero, Vector3.zero, Color.white);
  private KeyCode hideHUDKey;
  private KeyCode toggleMenuKey;
  private KeyCode camForwardKey;
  private KeyCode camBackwardKey;
  private KeyCode camLeftKey;
  private KeyCode camRightKey;
  private KeyCode camUpKey;
  private KeyCode camDownKey;
  private KeyCode camFastKey;
  private KeyCode camSlowKey;
  private KeyCode toggleTacticalViewKey;
  private KeyCode addShipToSquadronKey;
  private KeyCode camFollowTargetKey;
  private KeyCode camFocusTargetKey;
  private KeyCode stopFocusKey;
  private KeyCode forceTargetKey;
  private KeyCode shipMoveKey;
  private KeyCode shipRadiatorsToggleKey;
  private KeyCode squadronMoveKey;
  private KeyCode squadronEngageKey;
  private KeyCode squadronEscortKey;
  [SerializeField]
  private bool isVersion2;

  public event Action toggleMoveOrder;

  public event Action newSelection;

  public event Action newTargetEvent;

  private void Awake()
  {
    if (!((UnityEngine.Object) TacticalInputs.current == (UnityEngine.Object) null))
      return;
    TacticalInputs.current = this;
  }

  private void Start()
  {
    this.mode = CommandMode.NULL;
    this.lr = this.GetComponent<LineRenderer>();
    this.hp = HeightPointer.current;
    this.fm = FactionsManager.current;
    this.am = AudioManager.current;
    this.pfui = PlayerFleetUI.current;
    this.mainCam = Camera.main;
    this.ct = CommandTooltip.current;
    this.squadAss = SquadronAssignment.current;
    this.lt = LaserToggler.current;
    this.timeCon = TimeController.current;
    this.menuAnim = this.menuPanel.GetComponent<uiAnimator>();
    this.ToggleScopeCam(true);
    this.SetSelectedSquadronIndex(0);
  }

  private void Update()
  {
    if (!this.isInputOn)
      return;
    this.isDrawMoveline = false;
    this.ClearLR();
    this.Click();
    this.RotateCamera();
    this.Scroll();
    this.Keys();
    this.UpdateHeightPointer();
    this.UpdateScopeCam();
    this.UpdateCommandTooltip();
  }

  private void LateUpdate()
  {
    if (!this.isDrawMoveline)
      return;
    this.DrawMoveLineLate();
  }

  private void UpdateScopeCam()
  {
    if ((UnityEngine.Object) this.selectedShip != (UnityEngine.Object) null && (bool) (UnityEngine.Object) this.selectedShip.scopeSpot && this.isScopeOn)
    {
      this.scopeCam.transform.position = this.selectedShip.scopeSpot.position;
      if (this.scopeCam.gameObject.activeSelf)
        return;
      this.scopeCam.gameObject.SetActive(true);
    }
    else
    {
      if (!this.scopeCam.gameObject.activeSelf)
        return;
      this.scopeCam.gameObject.SetActive(false);
    }
  }

  public void ToggleScopeCam(bool newValue) => this.isScopeOn = newValue;

  private void DrawMoveLine()
  {
    Vector3 screenPoint1 = this.mainCam.WorldToScreenPoint(this.selectedShip.transform.position);
    Vector3 screenPoint2 = this.mainCam.WorldToScreenPoint(this.selectedShip.pf.targetPos);
    if ((double) screenPoint1.z <= 0.0 || (double) screenPoint2.z <= 0.0)
      this.lruiGreen.CreateLine((Vector2) Vector3.zero, (Vector2) Vector3.zero, this.colorGreen);
    else
      this.lruiGreen.CreateLine((Vector2) screenPoint1, (Vector2) screenPoint2, this.colorGreen);
  }

  private void DrawMoveLineRed(Vector3 origin, Vector3 midPoint, Vector3 dest, Color color)
  {
    if (dest == Vector3.zero)
      return;
    Vector3 screenPoint1 = this.mainCam.WorldToScreenPoint(origin);
    Vector3 screenPoint2 = this.mainCam.WorldToScreenPoint(midPoint);
    Vector3 screenPoint3 = this.mainCam.WorldToScreenPoint(dest);
    if ((double) screenPoint1.z <= 0.0)
    {
      screenPoint1.x = (float) Screen.width - screenPoint1.x;
      screenPoint1.y = (float) Screen.height - screenPoint1.y;
    }
    float min = 0.0f;
    screenPoint1.x = Mathf.Clamp(screenPoint1.x, min, (float) Screen.width - min);
    screenPoint1.y = Mathf.Clamp(screenPoint1.y, min, (float) Screen.height - min);
    RectTransform parent = this.lruiRed.transform.parent as RectTransform;
    Vector2 localPoint1;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, (Vector2) screenPoint1, (Camera) null, out localPoint1);
    Vector2 localPoint2;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, (Vector2) screenPoint2, (Camera) null, out localPoint2);
    Vector2 localPoint3;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, (Vector2) screenPoint3, (Camera) null, out localPoint3);
    this.lruiRed.CreateLine(localPoint1, localPoint3, color);
    this.lruiWhite1.CreateLine(localPoint1, localPoint2, this.colorWhite);
    this.lruiWhite2.CreateLine(localPoint2, localPoint3, this.colorWhite);
    this.isDrawMoveline = true;
    this.currentMLD = new TacticalInputs.MovelineData((Vector3) localPoint1, (Vector3) localPoint2, (Vector3) localPoint3, color);
  }

  private void DrawMoveLineLate()
  {
    this.lruiRed.CreateLine((Vector2) this.currentMLD.shipPos, (Vector2) this.currentMLD.destPos, this.currentMLD.color);
    this.lruiWhite1.CreateLine((Vector2) this.currentMLD.shipPos, (Vector2) this.currentMLD.midPos, this.colorWhite);
    this.lruiWhite2.CreateLine((Vector2) this.currentMLD.midPos, (Vector2) this.currentMLD.destPos, this.colorWhite);
  }

  private void ClearLR()
  {
    this.lr.SetPosition(0, Vector3.zero);
    this.lr.SetPosition(1, Vector3.zero);
    this.lr2.SetPosition(0, Vector3.zero);
    this.lr2.SetPosition(1, Vector3.zero);
    this.lr2.SetPosition(2, Vector3.zero);
    this.lruiRed.CreateLine((Vector2) Vector3.zero, (Vector2) Vector3.zero, this.colorRed);
    this.lruiWhite1.CreateLine((Vector2) Vector3.zero, (Vector2) Vector3.zero, this.colorWhite);
    this.lruiWhite2.CreateLine((Vector2) Vector3.zero, (Vector2) Vector3.zero, this.colorWhite);
  }

  private void UpdateHeightPointer()
  {
    if (this.mode == CommandMode.MOVE || this.sqdCommand == SquadronCommand.MOVE)
      this.hp.SetHeightPoint(this.yDest);
    else if ((UnityEngine.Object) this.selectedShip != (UnityEngine.Object) null)
      this.hp.SetHeightPoint(this.selectedShip.transform.position.y);
    else if ((UnityEngine.Object) this.selectedSquadron != (UnityEngine.Object) null)
      this.hp.SetHeightPoint(this.selectedSquadron.transform.position.y);
    else
      this.hp.SetHeightPoint(0.0f);
  }

  private void UpdateCommandTooltip()
  {
    if (this.selectionMode == SelectionMode.SHIP && (bool) (UnityEngine.Object) this.selectedShip)
    {
      if (this.mode == CommandMode.MOVE)
        this.ct.Set(true, "MOVE", this.selectedShip.track.trackName);
      else if (this.mode == CommandMode.TURN)
        this.ct.Set(true, "TURN", this.selectedShip.track.trackName);
      else if (this.mode == CommandMode.DOCK)
        this.ct.Set(true, "DOCK", this.selectedShip.track.trackName);
      else
        this.ct.Set(false, "", "");
    }
    else if (this.selectionMode == SelectionMode.SQUADRON && (bool) (UnityEngine.Object) this.selectedSquadron)
    {
      if (this.sqdCommand == SquadronCommand.MOVE)
        this.ct.Set(true, "MOVE", this.selectedSquadron.track.trackName);
      else if (this.sqdCommand == SquadronCommand.ENGAGE)
        this.ct.Set(true, "ENGAGE", this.selectedSquadron.track.trackName);
      else if (this.sqdCommand == SquadronCommand.ESCORT)
        this.ct.Set(true, "ESCORT", this.selectedSquadron.track.trackName);
      else
        this.ct.Set(false, "", "");
    }
    else
      this.ct.Set(false, "", "");
  }

  public void SetTarget(Track newTarget, bool isForced)
  {
    MonoBehaviour.print((object) ("trying to set target: " + newTarget.name));
    if ((UnityEngine.Object) newTarget == (UnityEngine.Object) null || !isForced && (newTarget.iff == IFF.NEUTRAL || newTarget.iff == IFF.FRIENDLY || newTarget.iff == IFF.OWNED))
      return;
    this.target = newTarget;
    Action newTargetEvent = this.newTargetEvent;
    if (newTargetEvent != null)
      newTargetEvent();
    if ((bool) (UnityEngine.Object) this.selectedShip)
      this.selectedShip.NewTarget(newTarget);
    else if ((bool) (UnityEngine.Object) this.selectedSquadron)
      this.selectedSquadron.SetTarget(newTarget);
    this.am.PlaySFX(1);
  }

  public void ClearTarget()
  {
    this.target = (Track) null;
    Action newTargetEvent = this.newTargetEvent;
    if (newTargetEvent != null)
      newTargetEvent();
    if ((bool) (UnityEngine.Object) this.selectedShip)
      this.selectedShip.NewTarget((Track) null);
    this.am.PlaySFX(0);
  }

  public void Deselect()
  {
    if (this.selectionMode == SelectionMode.SHIP && (UnityEngine.Object) this.selectedShip != (UnityEngine.Object) null)
    {
      this.SelectNew(this.selectedShip);
    }
    else
    {
      if (this.selectionMode != SelectionMode.SQUADRON || !((UnityEngine.Object) this.selectedSquadron != (UnityEngine.Object) null))
        return;
      this.SelectNew(this.selectedSquadron);
    }
  }

  public void SelectNew(ShipController newShip)
  {
    if ((UnityEngine.Object) this.selectedShip == (UnityEngine.Object) newShip || !newShip.isSelectable || (UnityEngine.Object) newShip == (UnityEngine.Object) null)
    {
      this.DeselectAll();
      this.selectedShipUI.SetActive(false);
      this.SetSelectionMode(SelectionMode.NULL);
    }
    else
    {
      this.DeselectAll();
      this.selectedShipUI.SetActive(true);
      this.selectedShip = newShip;
      this.selectedPathfinder = this.selectedShip.GetComponent<Pathfinder>();
      this.SetSelectionMode(SelectionMode.SHIP);
    }
    this.am.PlaySFX(0);
    Action newSelection = this.newSelection;
    if (newSelection == null)
      return;
    newSelection();
  }

  public void SelectNew(Squadron newSquadron)
  {
    this.selectedShipUI.SetActive(false);
    this.DeselectAll();
    this.selectedSquadron = newSquadron;
    this.selectedSquadron.SetSelected(true);
    this.SetSelectionMode(SelectionMode.SQUADRON);
    this.am.PlaySFX(0);
    Action newSelection = this.newSelection;
    if (newSelection == null)
      return;
    newSelection();
  }

  private void DeselectAll()
  {
    this.selectedShip = (ShipController) null;
    this.selectedPathfinder = (Pathfinder) null;
    if (!(bool) (UnityEngine.Object) this.selectedSquadron)
      return;
    this.selectedSquadron.SetSelected(false);
    this.selectedSquadron = (Squadron) null;
  }

  private void SetSelectionMode(SelectionMode newMode)
  {
    this.selectionMode = newMode;
    if (this.selectionMode == SelectionMode.SHIP && (UnityEngine.Object) this.selectedShip != (UnityEngine.Object) null && (bool) (UnityEngine.Object) this.selectedShip.track)
      this.SetSelectedPanel(true, this.selectedShip.track.trackName);
    else if (this.selectionMode == SelectionMode.SQUADRON && (UnityEngine.Object) this.selectedSquadron != (UnityEngine.Object) null && (bool) (UnityEngine.Object) this.selectedSquadron.track)
      this.SetSelectedPanel(true, this.selectedSquadron.track.trackName);
    else
      this.SetSelectedPanel(false, "");
  }

  private void SetSelectedPanel(bool isOn, string newText)
  {
    if (this.isVersion2)
    {
      this.selectedCaptainData.gameObject.SetActive(false);
      this.selectedSquadronData.gameObject.SetActive(false);
      if ((bool) (UnityEngine.Object) this.selectedShip)
      {
        this.selectedCaptainData.SetShip(this.selectedShip.GetComponent<Captain>());
        this.selectedCaptainData.gameObject.SetActive(true);
      }
      else if ((bool) (UnityEngine.Object) this.selectedSquadron)
      {
        this.selectedSquadronData.SetSquadron(this.selectedSquadron);
        this.selectedSquadronData.gameObject.SetActive(true);
      }
      if (isOn)
        this.selectedText.transform.parent.parent.gameObject.SetActive(true);
      else
        this.selectedText.transform.parent.parent.gameObject.SetActive(false);
      this.selectedText.text = newText;
    }
    else
    {
      if (isOn)
        this.selectedText.transform.parent.parent.gameObject.SetActive(true);
      else
        this.selectedText.transform.parent.parent.gameObject.SetActive(false);
      this.selectedText.text = newText;
    }
  }

  public void AddPlayerShip(ShipController sc) => this.shipsOwned.Add(sc);

  public void SetSelectedShipIndex(int index)
  {
    MonoBehaviour.print((object) ("setting selected ship index: " + index.ToString()));
    if (this.shipsOwned.Count == 0)
      return;
    this.SelectNew(this.shipsOwned[index]);
  }

  public void SetSelectedSquadronIndex(int index)
  {
    if (this.squadronsOwned.Count == 0)
      return;
    this.SelectNew(this.squadronsOwned[index]);
  }

  private void RotateCamera()
  {
    if (!Input.GetMouseButton(1) || !this.isInputOn)
      return;
    this.tc.Rotate(Input.GetAxis("Mouse X") * this.mouseSens * Time.deltaTime, Input.GetAxis("Mouse Y") * this.mouseSens * Time.deltaTime);
  }

  private void Click()
  {
    this.dispersionSphere.gameObject.SetActive(false);
    this.mouseDown0 = Input.GetMouseButtonDown(0);
    this.mouseDown1 = Input.GetMouseButtonDown(1);
    if (this.mouseDown0)
    {
      if (((double) Time.time - (double) this.lastMouseDown0) / (double) this.timeCon.timeScale < (double) this.doubleClickThresh && (bool) (UnityEngine.Object) this.tempTarget && !this.isMapOn)
        this.tc.SetFocus(this.tempTarget.transform, 2);
      this.lastMouseDown0 = Time.time;
    }
    if (this.selectionMode == SelectionMode.SHIP && (UnityEngine.Object) this.selectedShip != (UnityEngine.Object) null)
    {
      if (this.mode == CommandMode.MOVE)
      {
        this.intersectionPoint = Vector3.zero;
        this.intersectionPointDist = 0.0f;
        Color color = this.colorRed;
        Vector3 destFromRay = this.GetDestFromRay();
        float num = Vector3.Distance(this.selectedShip.transform.position, destFromRay);
        if ((double) destFromRay.magnitude > 5100.0)
          return;
        if ((double) num < 100.0)
          color = Color.yellow;
        if (this.mouseDown0)
        {
          this.selectedShip.Move(destFromRay);
          this.mode = CommandMode.NULL;
        }
        else
          this.DrawMoveLineRed(this.selectedShip.transform.position, new Vector3(destFromRay.x, this.selectedShip.transform.position.y, destFromRay.z), destFromRay, color);
      }
      else
      {
        if (this.mode == CommandMode.TURN)
          return;
        if (this.mode == CommandMode.DOCK)
        {
          this.lr.startColor = this.colorGreen;
          this.lr.endColor = this.colorGreen;
          DockingPoint dp = (DockingPoint) null;
          RaycastHit hitInfo;
          if (Physics.Raycast(this.mainCam.ScreenPointToRay(Input.mousePosition), out hitInfo))
          {
            if (hitInfo.transform.CompareTag("DockingPoint") || (bool) (UnityEngine.Object) hitInfo.transform.GetComponent<DockingPoint>())
              dp = hitInfo.transform.GetComponent<DockingPoint>();
            else if ((bool) (UnityEngine.Object) hitInfo.transform.GetComponent<ShipController>())
            {
              float num1 = float.MaxValue;
              foreach (DockingPoint dockingPoint in hitInfo.transform.GetComponent<ShipController>().dockingPoints)
              {
                float num2 = Vector3.Distance(this.selectedShip.transform.position, dockingPoint.transform.position);
                if ((double) num2 < (double) num1)
                {
                  num1 = num2;
                  dp = dockingPoint;
                }
              }
            }
          }
          if (this.mouseDown0 && (UnityEngine.Object) dp != (UnityEngine.Object) null)
          {
            this.selectedShip.DockAt(dp);
            this.mode = CommandMode.NULL;
          }
          else if (this.mouseDown1)
            this.mode = CommandMode.NULL;
          else if ((UnityEngine.Object) dp != (UnityEngine.Object) null)
          {
            this.lr.SetPosition(0, this.selectedShip.transform.position);
            this.lr.SetPosition(1, dp.transform.position);
          }
          else
          {
            this.lr.SetPosition(0, Vector3.zero);
            this.lr.SetPosition(1, Vector3.zero);
          }
        }
        else
        {
          int mode = (int) this.mode;
        }
      }
    }
    else
    {
      if (this.selectionMode != SelectionMode.SQUADRON || !((UnityEngine.Object) this.selectedSquadron != (UnityEngine.Object) null))
        return;
      if (this.sqdCommand == SquadronCommand.MOVE)
      {
        Color color = this.colorRed;
        Vector3 destFromRay = this.GetDestFromRay();
        float num = Vector3.Distance(this.selectedSquadron.transform.position, destFromRay);
        if ((double) destFromRay.magnitude > 5100.0)
          return;
        if ((double) num < 100.0)
          color = Color.yellow;
        if (this.mouseDown0)
        {
          this.selectedSquadron.targetPos = destFromRay;
          this.selectedSquadron.NewCommand(this.sqdCommand);
          this.sqdCommand = SquadronCommand.NULL;
        }
        else
        {
          this.DrawMoveLineRed(this.selectedSquadron.transform.position, new Vector3(destFromRay.x, this.selectedSquadron.transform.position.y, destFromRay.z), destFromRay, color);
          this.SetDispersionSphere(destFromRay);
        }
      }
      else if (this.sqdCommand == SquadronCommand.ENGAGE)
      {
        Vector3 point = this.mainCam.ScreenPointToRay(Input.mousePosition).GetPoint(1f);
        this.DrawMoveLineRed(this.selectedSquadron.transform.position, point, point, this.colorRed);
        if (!this.mouseDown0 || !((UnityEngine.Object) this.tempTarget != (UnityEngine.Object) null))
          return;
        this.selectedSquadron.SetTarget(this.tempTarget);
        this.selectedSquadron.NewCommand(this.sqdCommand);
        this.sqdCommand = SquadronCommand.NULL;
      }
      else
      {
        if (this.sqdCommand != SquadronCommand.ESCORT)
          return;
        Vector3 point = this.mainCam.ScreenPointToRay(Input.mousePosition).GetPoint(1f);
        this.DrawMoveLineRed(this.selectedSquadron.transform.position, point, point, this.colorBlue);
        if (!this.mouseDown0 || !((UnityEngine.Object) this.tempTarget != (UnityEngine.Object) null))
          return;
        this.selectedSquadron.SetTarget(this.tempTarget);
        this.selectedSquadron.NewCommand(this.sqdCommand);
        this.sqdCommand = SquadronCommand.NULL;
      }
    }
  }

  private void SetDispersionSphere(Vector3 targetPos)
  {
    float num = (float) (this.selectedSquadron.dispersion * 2);
    this.dispersionSphere.gameObject.SetActive(true);
    this.dispersionSphere.transform.position = targetPos;
    this.dispersionSphere.transform.localScale = new Vector3(num, num, num);
  }

  private Vector3 GetDestFromRay()
  {
    this.intersectionPoint = Vector3.zero;
    Ray ray = this.mainCam.ScreenPointToRay(Input.mousePosition);
    if (Input.GetKey(KeyCode.LeftControl))
    {
      Vector3 right = Vector3.right;
      Vector3 planePoint = new Vector3(this.xDest, 0.0f, this.zDest);
      if (this.GetRayIntersectionWithPlane(ray.origin, ray.direction, right, planePoint, out this.intersectionPoint))
        this.yDest = this.intersectionPoint.y;
    }
    else
    {
      this.intersectionPoint = this.GetRayIntersectionWithYPlane(ray.origin, ray.direction, this.yDest);
      this.xDest = this.intersectionPoint.x;
      this.zDest = this.intersectionPoint.z;
    }
    return new Vector3(this.xDest, this.yDest, this.zDest);
  }

  private void Scroll()
  {
    if ((double) Input.mouseScrollDelta.y > 0.0)
    {
      this.tc.Zoom(1);
    }
    else
    {
      if ((double) Input.mouseScrollDelta.y >= 0.0)
        return;
      this.tc.Zoom(-1);
    }
  }

  private void Keys()
  {
    if (Input.GetKeyDown(this.toggleMenuKey))
      this.menuAnim.Toggle();
    if (this.menuAnim.IsShow)
      return;
    if (Input.GetKeyDown(this.toggleTacticalViewKey) && !this.tc.isMapDashing && this.isAllowMap)
    {
      this.isMapOn = !this.isMapOn;
      this.tc.MapMode(this.isMapOn);
    }
    if (!Input.GetKey(this.addShipToSquadronKey))
    {
      if (Input.GetKeyDown(KeyCode.Alpha1) && this.shipsOwned.Count > 0)
        this.SetSelectedShipIndex(0);
      else if (Input.GetKeyDown(KeyCode.Alpha2) && this.shipsOwned.Count > 1)
        this.SetSelectedShipIndex(1);
      else if (Input.GetKeyDown(KeyCode.Alpha3) && this.shipsOwned.Count > 2)
        this.SetSelectedShipIndex(2);
      else if (Input.GetKeyDown(KeyCode.Alpha4) && this.shipsOwned.Count > 3)
        this.SetSelectedShipIndex(3);
      else if (Input.GetKeyDown(KeyCode.Alpha5) && this.shipsOwned.Count > 4)
        this.SetSelectedShipIndex(4);
      else if (Input.GetKeyDown(KeyCode.Alpha6) && this.shipsOwned.Count > 5)
        this.SetSelectedShipIndex(5);
      else if (Input.GetKeyDown(KeyCode.Alpha7) && this.shipsOwned.Count > 6)
        this.SetSelectedShipIndex(6);
      else if (Input.GetKeyDown(KeyCode.Alpha8) && this.shipsOwned.Count > 7)
        this.SetSelectedShipIndex(7);
      else if (Input.GetKeyDown(KeyCode.Alpha9) && this.shipsOwned.Count > 8)
        this.SetSelectedShipIndex(8);
      else if (Input.GetKeyDown(KeyCode.Alpha0) && this.shipsOwned.Count > 9)
        this.SetSelectedShipIndex(9);
    }
    else if ((bool) (UnityEngine.Object) this.selectedShip)
    {
      if (Input.GetKeyDown(KeyCode.Alpha1))
        this.squadAss.AddShipToSquadron(this.squadronsOwned[0], this.selectedShip.GetComponent<Captain>());
      else if (Input.GetKeyDown(KeyCode.Alpha2))
        this.squadAss.AddShipToSquadron(this.squadronsOwned[1], this.selectedShip.GetComponent<Captain>());
      else if (Input.GetKeyDown(KeyCode.Alpha3))
        this.squadAss.AddShipToSquadron(this.squadronsOwned[2], this.selectedShip.GetComponent<Captain>());
      else if (Input.GetKeyDown(KeyCode.Alpha4))
        this.squadAss.AddShipToSquadron(this.squadronsOwned[3], this.selectedShip.GetComponent<Captain>());
    }
    if (Input.GetKeyDown(KeyCode.F1) && this.squadronsOwned.Count > 0)
      this.SetSelectedSquadronIndex(0);
    else if (Input.GetKeyDown(KeyCode.F2) && this.squadronsOwned.Count > 1)
      this.SetSelectedSquadronIndex(1);
    else if (Input.GetKeyDown(KeyCode.F3) && this.squadronsOwned.Count > 2)
      this.SetSelectedSquadronIndex(2);
    else if (Input.GetKeyDown(KeyCode.F4) && this.squadronsOwned.Count > 3)
      this.SetSelectedSquadronIndex(3);
    if (Input.GetKeyDown(this.camFollowTargetKey) && !this.isMapOn)
    {
      if (Input.GetKey(this.stopFocusKey))
        this.tc.SetFocus((Transform) null, 0);
      else if ((UnityEngine.Object) this.tempTarget != (UnityEngine.Object) null)
        this.tc.SetFocus(this.tempTarget.transform, 2);
      else if (this.selectionMode == SelectionMode.SHIP && (UnityEngine.Object) this.selectedShip != (UnityEngine.Object) null)
        this.tc.SetFocus(this.selectedShip.transform, 2);
      else if (this.selectionMode == SelectionMode.SQUADRON && (UnityEngine.Object) this.selectedSquadron != (UnityEngine.Object) null)
        this.tc.SetFocus(this.selectedSquadron.transform, 2);
      else if ((UnityEngine.Object) this.target != (UnityEngine.Object) null)
        this.tc.SetFocus(this.target.transform, 2);
      else
        this.tc.SetFocus((Transform) null, 0);
    }
    if (Input.GetKeyDown(this.camFocusTargetKey))
    {
      if ((UnityEngine.Object) this.tempTarget == (UnityEngine.Object) null)
        this.tc.SetFocus((Transform) null, 0);
      else
        this.tc.SetFocus(this.tempTarget.transform, 1);
    }
    if (Input.GetKeyDown(this.forceTargetKey) && (UnityEngine.Object) this.tempTarget != (UnityEngine.Object) null)
      this.SetTarget(this.tempTarget, true);
    if (this.selectionMode == SelectionMode.SHIP && (UnityEngine.Object) this.selectedShip != (UnityEngine.Object) null)
    {
      if (Input.GetKeyDown(this.shipMoveKey))
      {
        if (this.mode == CommandMode.MOVE)
        {
          this.mode = CommandMode.NULL;
        }
        else
        {
          this.mode = CommandMode.MOVE;
          this.yDest = this.selectedShip.transform.position.y;
        }
        Action toggleMoveOrder = this.toggleMoveOrder;
        if (toggleMoveOrder != null)
          toggleMoveOrder();
      }
      else if (Input.GetKeyDown(this.shipRadiatorsToggleKey))
        this.selectedShip.ToggleRadiators();
    }
    else if (this.selectionMode == SelectionMode.SQUADRON && (UnityEngine.Object) this.selectedSquadron != (UnityEngine.Object) null)
    {
      if (Input.GetKeyDown(this.squadronMoveKey))
      {
        if (this.sqdCommand == SquadronCommand.MOVE)
        {
          this.sqdCommand = SquadronCommand.NULL;
        }
        else
        {
          this.sqdCommand = SquadronCommand.MOVE;
          this.yDest = this.selectedSquadron.transform.position.y;
        }
        Action toggleMoveOrder = this.toggleMoveOrder;
        if (toggleMoveOrder != null)
          toggleMoveOrder();
      }
      else if (Input.GetKeyDown(this.squadronEngageKey))
      {
        this.sqdCommand = this.sqdCommand != SquadronCommand.ENGAGE ? SquadronCommand.ENGAGE : SquadronCommand.NULL;
        Action toggleMoveOrder = this.toggleMoveOrder;
        if (toggleMoveOrder != null)
          toggleMoveOrder();
      }
      else if (Input.GetKeyDown(this.squadronEscortKey))
      {
        this.sqdCommand = this.sqdCommand != SquadronCommand.ESCORT ? SquadronCommand.ESCORT : SquadronCommand.NULL;
        Action toggleMoveOrder = this.toggleMoveOrder;
        if (toggleMoveOrder != null)
          toggleMoveOrder();
      }
    }
    if (this.isMapOn)
      return;
    if (Input.GetKey(this.camForwardKey) || Input.GetKey(this.camLeftKey) || Input.GetKey(this.camBackwardKey) || Input.GetKey(this.camRightKey) || Input.GetKey(this.camUpKey) || Input.GetKey(this.camDownKey))
    {
      Vector3 zero = (Vector3) Vector2.zero;
      bool isShift = false;
      if (Input.GetKey(this.camForwardKey) || Input.GetKey(KeyCode.UpArrow))
        zero.z = 1f;
      else if (Input.GetKey(this.camBackwardKey) || Input.GetKey(KeyCode.DownArrow))
        zero.z = -1f;
      if (Input.GetKey(this.camLeftKey) || Input.GetKey(KeyCode.LeftArrow))
        zero.x = -1f;
      else if (Input.GetKey(this.camRightKey) || Input.GetKey(KeyCode.RightArrow))
        zero.x = 1f;
      if (Input.GetKey(this.camDownKey))
        zero.y = -1f;
      else if (Input.GetKey(this.camUpKey))
        zero.y = 1f;
      if (Input.GetKey(this.camFastKey))
        isShift = true;
      if (Input.GetKey(this.camSlowKey))
        zero *= 0.1f;
      this.tc.Move(zero, isShift);
    }
    else
      this.tc.ResetShiftHoldTime();
    if (!Input.GetKeyDown(this.hideHUDKey))
      return;
    this.ToggleHUD();
  }

  public void ToggleHUD()
  {
    this.isHudOn = !this.isHudOn;
    this.tc.overlayCamMain.cullingMask = !this.isHudOn ? (int) this.hudOffMask : (int) this.hudOnMask;
    if (this.isHudOn)
    {
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
    }
    else
    {
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
    }
    this.ui.SetActive(this.isHudOn);
  }

  private bool GetRayIntersectionWithPlane(
    Vector3 rayOrigin,
    Vector3 rayDirection,
    Vector3 planeNormal,
    Vector3 planePoint,
    out Vector3 intersection)
  {
    intersection = Vector3.zero;
    float f = Vector3.Dot(rayDirection, planeNormal);
    float num1 = 0.01f;
    if ((double) Mathf.Abs(f) < (double) Mathf.Epsilon)
      return false;
    float num2 = Vector3.Dot(planePoint - rayOrigin, planeNormal) / f;
    if ((double) num2 < (double) num1)
      return false;
    intersection = rayOrigin + num2 * rayDirection;
    double num3 = (double) Mathf.Clamp(intersection.x, -5000f, 5000f);
    double num4 = (double) Mathf.Clamp(intersection.y, -5000f, 5000f);
    double num5 = (double) Mathf.Clamp(intersection.z, -5000f, 5000f);
    return true;
  }

  private Vector3 GetRayIntersectionWithYPlane(Vector3 origin, Vector3 direction, float yPlane)
  {
    if ((double) Mathf.Abs(direction.y) < (double) Mathf.Epsilon)
      return Vector3.zero;
    float num = (yPlane - origin.y) / direction.y;
    return (double) num < 0.0 ? Vector3.zero : origin + num * direction;
  }

  public void SetRCSOnly(bool isOn)
  {
    MonoBehaviour.print((object) "SET RCS WHEN CLOSE");
    if ((bool) (UnityEngine.Object) this.selectedShip && (UnityEngine.Object) this.selectedShip.GetComponent<Pathfinder>() != (UnityEngine.Object) null)
      this.selectedShip.GetComponent<Pathfinder>().isRCSOnly = isOn;
    this.am.PlaySFX(0);
  }

  public void SetRCSWhenClose(bool isOn)
  {
    if (!(bool) (UnityEngine.Object) this.selectedShip || !((UnityEngine.Object) this.selectedShip.GetComponent<Pathfinder>() != (UnityEngine.Object) null))
      return;
    this.selectedShip.GetComponent<Pathfinder>().isRCSWhenClose = isOn;
  }

  public void SetMoveLine(bool isOn)
  {
    MonoBehaviour.print((object) "set move line");
    if ((bool) (UnityEngine.Object) this.selectedShip && (UnityEngine.Object) this.selectedShip.GetComponent<Pathfinder>() != (UnityEngine.Object) null)
      this.selectedShip.GetComponent<Pathfinder>().isShowMoveLine = isOn;
    this.am.PlaySFX(0);
  }

  public void MoveToTarget()
  {
    if (!(bool) (UnityEngine.Object) this.selectedShip || !((UnityEngine.Object) this.selectedShip.GetComponent<Pathfinder>() != (UnityEngine.Object) null))
      return;
    if ((bool) (UnityEngine.Object) this.selectedShip.currentTarget)
    {
      this.selectedShip.Move(this.selectedShip.currentTarget.transform.position);
      this.mode = CommandMode.NULL;
    }
    else
    {
      if (!(bool) (UnityEngine.Object) this.target)
        return;
      this.selectedShip.Move(this.target.transform.position);
      this.mode = CommandMode.NULL;
    }
  }

  public void FaceTarget()
  {
    if ((UnityEngine.Object) this.selectedShip == (UnityEngine.Object) null || (UnityEngine.Object) this.selectedPathfinder == (UnityEngine.Object) null)
      return;
    if ((bool) (UnityEngine.Object) this.selectedShip.currentTarget)
    {
      this.selectedPathfinder.targetPos = this.selectedShip.currentTarget.transform.position;
      this.selectedPathfinder.TrySetState(PathfinderState.FACE);
    }
    else
    {
      if (!(bool) (UnityEngine.Object) this.target)
        return;
      this.selectedPathfinder.targetPos = this.target.transform.position;
      this.selectedPathfinder.TrySetState(PathfinderState.FACE);
    }
  }

  public void Brake()
  {
    if (!(bool) (UnityEngine.Object) this.selectedShip || !((UnityEngine.Object) this.selectedShip.GetComponent<Pathfinder>() != (UnityEngine.Object) null))
      return;
    this.selectedShip.GetComponent<Pathfinder>().TrySetStateBrake();
  }

  public void UpdateInputs(Settings s)
  {
    this.hideHUDKey = s.GetKeyCode("hideHUD");
    this.toggleMenuKey = s.GetKeyCode("toggleMenu");
    this.camForwardKey = s.GetKeyCode("camForward");
    this.camBackwardKey = s.GetKeyCode("camBackward");
    this.camLeftKey = s.GetKeyCode("camLeft");
    this.camRightKey = s.GetKeyCode("camRight");
    this.camUpKey = s.GetKeyCode("camUp");
    this.camDownKey = s.GetKeyCode("camDown");
    this.camFastKey = s.GetKeyCode("camFast");
    this.camSlowKey = s.GetKeyCode("camSlow");
    this.toggleTacticalViewKey = s.GetKeyCode("toggleTacticalView");
    this.addShipToSquadronKey = s.GetKeyCode("addShipToSquadron");
    this.camFollowTargetKey = s.GetKeyCode("camFollowTarget");
    this.camFocusTargetKey = s.GetKeyCode("camFocusTarget");
    this.stopFocusKey = s.GetKeyCode("stopFocus");
    this.forceTargetKey = s.GetKeyCode("forceTarget");
    this.shipMoveKey = s.GetKeyCode("shipMove");
    this.shipRadiatorsToggleKey = s.GetKeyCode("shipRadiatorsToggle");
    this.squadronMoveKey = s.GetKeyCode("squadronMove");
    this.squadronEngageKey = s.GetKeyCode("squadronEngage");
    this.squadronEscortKey = s.GetKeyCode("squadronEscort");
  }

  private class MovelineData
  {
    public Vector3 shipPos = Vector3.zero;
    public Vector3 destPos = Vector3.zero;
    public Vector3 midPos = Vector3.zero;
    public Color color = Color.white;

    public MovelineData(Vector3 shipPos, Vector3 midPos, Vector3 destPos, Color color)
    {
      this.shipPos = shipPos;
      this.midPos = midPos;
      this.destPos = destPos;
      this.color = color;
    }
  }
}
