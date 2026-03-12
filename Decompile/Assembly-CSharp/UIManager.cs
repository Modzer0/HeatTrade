// Decompiled with JetBrains decompiler
// Type: UIManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIManager : MonoBehaviour
{
  public static UIManager current;
  public FactionsManager fm;
  private Scaling scaling;
  private CameraManager cm;
  private StationHarborUI shui;
  [SerializeField]
  private TMP_Text locationText;
  [Header("INFO PANEL")]
  private Track track;
  private bool isTrackShown;
  [SerializeField]
  private GameObject mainInfoPanel;
  [SerializeField]
  private TMP_Text mainInfoTitle;
  [Header("TOGGLES")]
  [SerializeField]
  private GameObject basicToggle;
  [SerializeField]
  private GameObject marketToggle;
  [SerializeField]
  private GameObject fleetToggle;
  [SerializeField]
  private GameObject attachmentsToggle;
  [SerializeField]
  private GameObject productionToggle;
  [Header("BASIC DATA")]
  [SerializeField]
  private TMP_Text factionText;
  [SerializeField]
  private TMP_Text trackID;
  [SerializeField]
  private TMP_Text nameText;
  [SerializeField]
  private Image nameImage;
  [SerializeField]
  private TMP_Text typeText;
  [SerializeField]
  private GameObject selectButton;
  [SerializeField]
  private GameObject hailButton;
  [Header("TRANSPONDER")]
  [Header("ORBITER")]
  [SerializeField]
  private GameObject orbiterPanel;
  [SerializeField]
  private TMP_Text orbiting;
  [SerializeField]
  private TMP_Text orbitRadius;
  [SerializeField]
  private TMP_Text orbitalPeriod;
  [SerializeField]
  private TMP_Text currentAngle;
  [SerializeField]
  private TMP_Text soi;
  [SerializeField]
  private TMP_Text mass;
  [Header("FLEET")]
  [SerializeField]
  private FleetPanel fp;
  [SerializeField]
  private GameObject fleetPanel;
  [Header("ATTACHMENTS")]
  [SerializeField]
  private AttachmentsPanel ap;
  [SerializeField]
  private GameObject attachmentsPanel;
  [Header("MARKET")]
  [SerializeField]
  private MarketPanelManager mpm;
  [SerializeField]
  private GameObject marketPanel;
  [Header("PRODUCTION")]
  [SerializeField]
  private ProductionPanel pp;
  [SerializeField]
  private GameObject productionPanel;
  [SerializeField]
  private List<GameObject> panels = new List<GameObject>();
  private bool noPanelOn;
  [Header("COLORS")]
  [SerializeField]
  private Gradient colorGradient;
  [SerializeField]
  private Gradient bgColorGradient;
  [SerializeField]
  private Color darkGreen;
  [SerializeField]
  private Color green;
  [SerializeField]
  private Color red;
  [Header("SCALE TEXT (Bottom)")]
  [SerializeField]
  private TMP_Text scaleText;

  private void Awake()
  {
    if ((Object) UIManager.current != (Object) null && (Object) UIManager.current != (Object) this)
      Object.Destroy((Object) this.gameObject);
    else
      UIManager.current = this;
  }

  private void Start()
  {
    this.scaling = Scaling.current;
    this.fm = FactionsManager.current;
    this.cm = CameraManager.current;
    this.shui = StationHarborUI.current;
    this.mainInfoPanel.SetActive(false);
  }

  private void Update()
  {
    this.scaleText.text = (this.cm.cam.orthographicSize * 1000f).ToString("N0") + " km";
  }

  public void SetLocationName(string newName) => this.locationText.text = newName;

  public void SetInfo(CamInfo newTarget)
  {
    if (!(bool) (Object) newTarget.GetComponent<Track>() || (Object) this.track == (Object) newTarget.GetComponent<Track>())
      return;
    this.track = newTarget.GetComponent<Track>();
    string factionCode = this.fm.GetFactionCode(this.track.factionID);
    string id = this.track.id;
    string fullName = this.track.GetFullName();
    this.mainInfoTitle.text = $"{factionCode}{id} - {fullName}";
    Color green = this.green;
    Color darkGreen = this.darkGreen;
    if (this.track.factionID != 1)
    {
      int relations = this.fm.GetFactionFromID(1).GetRelations(this.track.factionID);
      green = this.colorGradient.Evaluate((float) (((double) relations + 100.0) / 200.0));
      darkGreen = this.bgColorGradient.Evaluate((float) (((double) relations + 100.0) / 200.0));
    }
    this.factionText.text = factionCode;
    this.factionText.color = green;
    this.trackID.text = id;
    this.nameText.text = fullName;
    this.nameText.color = green;
    this.nameImage.color = darkGreen;
    this.typeText.text = this.track.type.ToString() ?? "";
    this.selectButton.SetActive(this.track.factionID == 1);
    this.hailButton.SetActive(this.track.type == TrackType.STATION);
    this.fleetToggle.SetActive(false);
    this.attachmentsToggle.SetActive(false);
    this.marketToggle.SetActive(false);
    this.productionToggle.SetActive(false);
    if ((bool) (Object) newTarget.GetComponent<FleetManager>())
    {
      this.fleetToggle.SetActive(true);
      this.fp.Setup(newTarget.GetComponent<FleetManager>());
    }
    else
      this.fleetPanel.SetActive(false);
    if ((bool) (Object) newTarget.GetComponent<Attachments>())
    {
      this.attachmentsToggle.SetActive(true);
      this.ap.Setup(newTarget.GetComponent<Attachments>());
    }
    else
      this.attachmentsPanel.SetActive(false);
    if ((bool) (Object) newTarget.GetComponent<Market>())
    {
      this.marketToggle.SetActive(true);
      this.mpm.Setup(newTarget.GetComponent<Market>());
    }
    else
      this.marketPanel.SetActive(false);
    if ((bool) (Object) newTarget.GetComponent<Factory>() || this.productionToggle.activeSelf && this.productionToggle.GetComponent<Toggle>().isOn)
    {
      this.productionToggle.SetActive(true);
      this.pp.Setup(newTarget.GetComponent<Factory>());
    }
    else
      this.productionPanel.SetActive(false);
  }

  public void UpdateInfo(CamInfo newTarget)
  {
    if (!(bool) (Object) newTarget.GetComponent<Track>())
      return;
    if ((Object) this.track != (Object) newTarget.GetComponent<Track>())
      this.SetInfo(newTarget);
    this.mainInfoPanel.SetActive(true);
    if (!(bool) (Object) newTarget.GetComponent<Orbiter>())
      return;
    Orbiter component = newTarget.GetComponent<Orbiter>();
    this.orbiterPanel.SetActive(true);
    if (component.isOrbiting)
    {
      this.orbiting.text = component.parent.GetComponent<Track>().publicName;
      this.orbiting.color = this.green;
    }
    else
    {
      this.orbiting.text = "NULL";
      this.orbiting.color = this.red;
    }
    this.orbitRadius.text = (component.orbitRadius * this.scaling.toKm).ToString() + " km";
    this.orbitalPeriod.text = component.orbitPeriod.ToString("F2") + " days";
    this.currentAngle.text = component.currentAngle.ToString("F2") + " deg";
    this.soi.text = (component.soiRadius * this.scaling.toKm).ToString() + " km";
    this.mass.text = component.mass.ToString() + " kg";
  }

  public void ClearInfo()
  {
    if (!((Object) this.mainInfoPanel != (Object) null))
      return;
    this.mainInfoPanel.SetActive(false);
  }

  public void HailTarget()
  {
    if (!((Object) this.track.GetComponent<StationManager>() != (Object) null))
      return;
    this.shui.SetOn(this.track.GetComponent<StationManager>());
  }
}
