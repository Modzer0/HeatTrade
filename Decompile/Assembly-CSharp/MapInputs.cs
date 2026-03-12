// Decompiled with JetBrains decompiler
// Type: MapInputs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class MapInputs : MonoBehaviour
{
  private TooltipSystem ts;
  private LTDescr delayTween;
  public static MapInputs current;
  public bool isInputOn;
  private CameraManager cm;
  private UIManager uim;
  [SerializeField]
  private Camera miniCam;
  private AudioManager am;
  private StrategicAudioManager sam;
  private StationHarborUI shui;
  private SelectedPanelManager spm;
  private TimeManager tm;
  private Track selected;
  public FleetManager selectedFleet;
  [SerializeField]
  private Orbiter orbiter;
  [SerializeField]
  private Navigation navigation;
  [SerializeField]
  private CamInfo target;
  [SerializeField]
  private CamInfo tempTarget;
  [SerializeField]
  private GameObject uiCanvas;
  [SerializeField]
  private GameObject menuPanel;
  private uiAnimator menuAnim;
  [SerializeField]
  private GameObject settingsPanel;
  [SerializeField]
  private GameObject saveLoadPanel;
  [SerializeField]
  private GameObject selectedPanel;
  private KeyCode hideHUDKey;
  private KeyCode toggleMenuKey;
  private KeyCode camForwardKey;
  private KeyCode camBackwardKey;
  private KeyCode camLeftKey;
  private KeyCode camRightKey;
  private KeyCode camFastKey;
  private KeyCode camSlowKey;

  private void Awake()
  {
    if ((UnityEngine.Object) MapInputs.current != (UnityEngine.Object) null && (UnityEngine.Object) MapInputs.current != (UnityEngine.Object) this)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    else
      MapInputs.current = this;
  }

  private void Start()
  {
    this.cm = CameraManager.current;
    this.uim = UIManager.current;
    this.am = AudioManager.current;
    this.sam = StrategicAudioManager.current;
    this.ts = TooltipSystem.current;
    this.spm = SelectedPanelManager.current;
    this.tm = TimeManager.current;
    this.shui = StationHarborUI.current;
    this.menuAnim = this.menuPanel.GetComponent<uiAnimator>();
  }

  private void Update()
  {
    if (this.isInputOn)
    {
      this.Click();
      this.Scroll();
      this.Keys();
    }
    if ((bool) (UnityEngine.Object) this.tempTarget)
      this.miniCam.transform.LookAt(this.tempTarget.transform);
    else if ((bool) (UnityEngine.Object) this.target)
      this.miniCam.transform.LookAt(this.target.transform);
    if ((bool) (UnityEngine.Object) this.navigation && (this.navigation.currentState == NavigationState.Accelerating || this.navigation.currentState == NavigationState.Decelerating))
      this.sam.SetMainEngine(true);
    else
      this.sam.SetMainEngine(false);
  }

  private void Click()
  {
    if (Input.GetMouseButtonDown(0))
    {
      if (!((UnityEngine.Object) this.tempTarget != (UnityEngine.Object) null))
        return;
      this.SetTarget(this.tempTarget);
    }
    else if (Input.GetMouseButtonDown(1))
    {
      if (!(bool) (UnityEngine.Object) this.tempTarget)
        return;
      this.SetTarget(this.tempTarget);
      Track component = this.tempTarget.GetComponent<Track>();
      if (!((UnityEngine.Object) this.tempTarget != (UnityEngine.Object) null) || !(bool) (UnityEngine.Object) component)
        return;
      if (component.factionID == 1)
      {
        this.SelectTarget();
      }
      else
      {
        this.SelectTarget();
        this.SetNavTarget();
      }
    }
    else
    {
      if (!Input.GetMouseButtonDown(2))
        return;
      this.CenterCam();
    }
  }

  public void CenterCam()
  {
    if ((UnityEngine.Object) this.tempTarget != (UnityEngine.Object) null)
      this.cm.CenterCam(this.tempTarget.transform);
    else if ((UnityEngine.Object) this.navigation != (UnityEngine.Object) null && this.navigation.gameObject.activeSelf)
      this.cm.CenterCam(this.navigation.transform);
    else if ((UnityEngine.Object) this.target != (UnityEngine.Object) null)
    {
      this.cm.CenterCam(this.target.transform);
    }
    else
    {
      if (!((UnityEngine.Object) this.cm.cam.transform.parent != (UnityEngine.Object) null))
        return;
      this.cm.CenterCam(this.cm.cam.transform.parent);
    }
  }

  private void Scroll()
  {
    int dir = 0;
    if ((double) Input.mouseScrollDelta.y > 0.0)
      dir = 1;
    else if ((double) Input.mouseScrollDelta.y < 0.0)
      dir = -1;
    if (Input.GetKey(KeyCode.LeftShift))
      dir *= 5;
    if (dir == 0)
      return;
    this.cm.Zoom(dir);
  }

  private void Keys()
  {
    this.CamMovement();
    if (Input.GetKeyDown(this.hideHUDKey))
    {
      this.uiCanvas.SetActive(!this.uiCanvas.activeSelf);
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }
    if (!Input.GetKeyDown(this.toggleMenuKey))
      return;
    this.ToggleMenu();
  }

  private void CamMovement()
  {
    Vector3 zero = Vector3.zero;
    if (Input.GetKey(this.camForwardKey) || Input.GetKey(KeyCode.UpArrow))
      zero.z = 1f;
    else if (Input.GetKey(this.camBackwardKey) || Input.GetKey(KeyCode.DownArrow))
      zero.z = -1f;
    if (Input.GetKey(this.camLeftKey) || Input.GetKey(KeyCode.LeftArrow))
      zero.x = -1f;
    else if (Input.GetKey(this.camRightKey) || Input.GetKey(KeyCode.RightArrow))
      zero.x = 1f;
    if (Input.GetKey(this.camFastKey))
      zero *= 5f;
    else if (Input.GetKey(this.camSlowKey))
      zero *= 0.1f;
    this.cm.Move(zero);
  }

  public void SetTarget(CamInfo newTarget)
  {
    this.target = this.tempTarget;
    this.cm.target = this.tempTarget;
    this.am.PlaySFX(0);
  }

  public void HailTarget()
  {
    if (!((UnityEngine.Object) this.target.GetComponent<StationManager>() != (UnityEngine.Object) null))
      return;
    this.shui.SetOn(this.target.GetComponent<StationManager>());
  }

  public void ClearAll() => this.ClearTarget();

  public void ClearSelected()
  {
    this.selected = (Track) null;
    this.selectedFleet = (FleetManager) null;
    this.orbiter = (Orbiter) null;
    this.navigation = (Navigation) null;
  }

  public void ClearTarget()
  {
    this.cm.ClearTarget();
    if ((bool) (UnityEngine.Object) this.navigation)
      this.navigation.SetTarget((Transform) null);
    this.uim.ClearInfo();
  }

  public void SetNavTarget()
  {
    if (!(bool) (UnityEngine.Object) this.navigation || !(bool) (UnityEngine.Object) this.target)
      return;
    this.navigation.SetTarget(this.target.GetComponent<Transform>());
  }

  public void SetSelectedFromTrack(Track newSelected)
  {
    if (newSelected.factionID != 1)
    {
      CamInfo component = newSelected.GetComponent<CamInfo>();
      this.EnterTarget(component);
      this.SetTarget(component);
    }
    else
    {
      if ((bool) (UnityEngine.Object) this.navigation)
        this.navigation.SetTarget((Transform) null);
      this.selected = newSelected;
      this.selectedFleet = newSelected.GetComponent<FleetManager>();
      this.navigation = this.selectedFleet.navigation;
      this.orbiter = this.selectedFleet.orbiter;
      this.cm.CenterCam(this.navigation.transform);
      this.spm.NewSelected(this.selected);
      this.selectedPanel.SetActive((UnityEngine.Object) this.selected != (UnityEngine.Object) null);
    }
  }

  public void SelectTarget() => this.SetSelectedFromTrack(this.target.GetComponent<Track>());

  public void StartFlight()
  {
    if (!(bool) (UnityEngine.Object) this.navigation)
      return;
    this.navigation.StartFlight();
  }

  public void EnterTarget(CamInfo newTarget)
  {
    this.tempTarget = newTarget;
    this.cm.tempTarget = newTarget;
    this.uim.SetInfo(this.tempTarget);
    this.TooltipOn(this.tempTarget);
  }

  public void ExitTarget(CamInfo newTarget)
  {
    if (!((UnityEngine.Object) this.tempTarget == (UnityEngine.Object) newTarget))
      return;
    this.tempTarget = (CamInfo) null;
    this.cm.tempTarget = (CamInfo) null;
    this.uim.ClearInfo();
    this.TooltipOff();
  }

  public void TooltipOn(CamInfo newTarget)
  {
    Track component = newTarget.GetComponent<Track>();
    string header = "";
    string body = "";
    if (this.delayTween != null)
      LeanTween.cancel(this.delayTween.uniqueId);
    if (component.type == TrackType.STATION && (bool) (UnityEngine.Object) component.GetComponent<ResourceInventory>())
    {
      header = component.publicName;
      foreach (ResourceQuantity resourceQuantity in component.GetComponent<ResourceInventory>().GetAllStock())
        body = $"{body}{resourceQuantity.resource.name}: {resourceQuantity.quantity.ToString()}\n";
    }
    this.delayTween = LeanTween.delayedCall(0.5f, (Action) (() =>
    {
      this.ts.ShowDefault(header, body);
      this.delayTween = (LTDescr) null;
    }));
  }

  public void TooltipOff()
  {
    if (this.delayTween != null)
    {
      LeanTween.cancel(this.delayTween.uniqueId);
      this.delayTween = (LTDescr) null;
    }
    this.ts.Hide();
  }

  public void ToggleMenu()
  {
    this.menuAnim.Toggle();
    this.tm.timeScale = this.menuAnim.IsShow ? 0.0f : 0.01f;
  }

  public void ToggleSettings() => this.settingsPanel.SetActive(!this.settingsPanel.activeSelf);

  public void ToggleSaveLoadPanel() => this.saveLoadPanel.SetActive(!this.saveLoadPanel.activeSelf);

  public void ExitToMainMenu() => SceneManager.LoadSceneAsync("SCENE - Main Menu");

  public bool HasTarget() => (UnityEngine.Object) this.target != (UnityEngine.Object) null;

  public Track GetTargetTrack()
  {
    return (bool) (UnityEngine.Object) this.target ? this.target.GetComponent<Track>() : (Track) null;
  }

  public void UpdateInputs(Settings s)
  {
    MonoBehaviour.print((object) "map inputs update inputs");
    this.hideHUDKey = s.GetKeyCode("hideHUD");
    this.toggleMenuKey = s.GetKeyCode("toggleMenu");
    this.camForwardKey = s.GetKeyCode("camForward");
    this.camBackwardKey = s.GetKeyCode("camBackward");
    this.camLeftKey = s.GetKeyCode("camLeft");
    this.camRightKey = s.GetKeyCode("camRight");
    this.camFastKey = s.GetKeyCode("camFast");
    this.camSlowKey = s.GetKeyCode("camSlow");
  }
}
