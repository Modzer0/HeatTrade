// Decompiled with JetBrains decompiler
// Type: NavPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NavPanel : MonoBehaviour
{
  public static NavPanel current;
  [SerializeField]
  private FleetManager fleet;
  [SerializeField]
  private Navigation navigation;
  [SerializeField]
  private Orbiter orbiter;
  private Scaling scaling;
  private AudioManager am;
  [Header("Colors")]
  [SerializeField]
  private Color white;
  [SerializeField]
  private Color green;
  [SerializeField]
  private Color yellow;
  [SerializeField]
  private Color red;
  [SerializeField]
  private Color gray;
  [Header("Panels")]
  [SerializeField]
  private GameObject setupPanel;
  [SerializeField]
  private GameObject movementPanel;
  [SerializeField]
  private GameObject altitudePanel;
  [SerializeField]
  private GameObject brakePanel;
  [SerializeField]
  private GameObject currentDataPanel;
  [Header("Setup")]
  [SerializeField]
  private TMP_Text navTarget;
  [SerializeField]
  private TMP_Text orbitable;
  [Header("Movement")]
  [SerializeField]
  private Slider orbitDistSlider;
  [SerializeField]
  private TMP_Text flightDistance;
  [SerializeField]
  private TMP_InputField accGInput;
  [SerializeField]
  private TMP_InputField decGInput;
  [SerializeField]
  private Slider accSlider;
  [SerializeField]
  private Slider decSlider;
  [SerializeField]
  private TMP_Text accGText;
  [SerializeField]
  private TMP_Text topVelText;
  [SerializeField]
  private TMP_Text interceptVelText;
  [SerializeField]
  private TMP_Text interceptTimeText;
  [SerializeField]
  private TMP_Text decGText;
  [SerializeField]
  private TMP_Text accTimeText;
  [SerializeField]
  private TMP_Text driftTimeText;
  [SerializeField]
  private TMP_Text decTimeText;
  [SerializeField]
  private TMP_Text accDistText;
  [SerializeField]
  private TMP_Text driftDistText;
  [SerializeField]
  private TMP_Text decDistText;
  [SerializeField]
  private TMP_Text flightDuration;
  [SerializeField]
  private TMP_Text dVRequired;
  [SerializeField]
  private RectTransform dVBar;
  [SerializeField]
  private RectTransform dVUsedBar;
  [SerializeField]
  private GameObject startFlightButton;
  [SerializeField]
  private GameObject warningButton;
  [SerializeField]
  private TMP_Text warningText;
  [Header("Current Data")]
  [SerializeField]
  private TMP_Text nameText;
  [SerializeField]
  private TMP_Text status;
  [SerializeField]
  private uiBar dVBarCurrentData;
  [SerializeField]
  private TMP_Text dVRemaining;
  [SerializeField]
  private TMP_Text dVTotal;
  [SerializeField]
  private TMP_Text dVLimiter;
  [SerializeField]
  private TMP_Text velocity;
  [SerializeField]
  private TMP_Text acceleration;
  [SerializeField]
  private TMP_Text artificialGravity;
  [SerializeField]
  private TMP_Text maxAccel;
  [SerializeField]
  private TMP_Text accLimiter;

  private void Awake()
  {
    if ((Object) NavPanel.current != (Object) null && (Object) NavPanel.current != (Object) this)
      Object.Destroy((Object) this.gameObject);
    else
      NavPanel.current = this;
  }

  private void Start()
  {
    this.scaling = Scaling.current;
    this.am = AudioManager.current;
  }

  private void Update()
  {
    this.UpdateFlightData();
    this.UpdateCurrentData();
  }

  public void ModAccG(float mod)
  {
    mod *= 1f / 1000f;
    if (!(bool) (Object) this.navigation)
      return;
    this.fleet.UpdateFleet();
    if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
      mod *= 100f;
    else if (Input.GetKey(KeyCode.LeftShift))
      mod *= 10f;
    else if (Input.GetKey(KeyCode.LeftControl))
      mod *= 0.1f;
    if ((double) this.navigation.accG + (double) mod > (double) this.fleet.maxAccG)
      this.navigation.accG = this.fleet.maxAccG;
    else if ((double) this.navigation.accG + (double) mod < 0.0099999997764825821)
      this.navigation.accG = 0.01f;
    else
      this.navigation.accG += mod;
  }

  public void ModDecG(float mod)
  {
    if (!(bool) (Object) this.navigation)
      return;
    if (Input.GetKey(KeyCode.LeftShift))
      mod *= 10f;
    else if (Input.GetKey(KeyCode.LeftControl))
      mod *= 0.1f;
    if ((double) this.navigation.decG + (double) mod <= 0.0)
      return;
    if ((double) mod > 0.0 && (double) this.navigation.decG + (double) mod > (double) this.fleet.maxAccG)
      mod = this.fleet.maxAccG - this.navigation.decG;
    else if ((double) mod < 0.0 && (double) this.navigation.decG + (double) mod < 0.0)
      mod = -this.navigation.decG;
    this.navigation.decG += mod;
  }

  public void NewAccG()
  {
    if (!(bool) (Object) this.navigation || this.accGInput.text == "" || this.accGInput.text == "0" || this.accGInput.text == "0." || this.accGInput.text == "0.0" || this.accGInput.text == "NaN")
      return;
    string str = this.accGText.text.ToString().Trim();
    float num = float.Parse(this.accGInput.text.ToString().Trim());
    MonoBehaviour.print((object) $"{Time.time.ToString()} accG: {num.ToString()} accGString: {str}");
    if ((double) num <= 0.0)
      num = 1f;
    else if ((double) num > (double) this.fleet.fleetMaxAcceleration)
      num = this.fleet.fleetMaxAcceleration;
    this.navigation.accG = num;
  }

  public void NewDecG()
  {
    if (!(bool) (Object) this.navigation || this.decGInput.text == "" || this.decGInput.text == "0" || this.decGInput.text == "0." || this.decGInput.text == "0.0" || this.decGInput.text == "NaN")
      return;
    this.decGText.text.ToString().Trim();
    float num = float.Parse(this.decGInput.text.ToString().Trim());
    if ((double) num <= 0.0)
      num = 0.1f;
    else if ((double) num > (double) this.fleet.fleetMaxAcceleration)
      num = this.fleet.fleetMaxAcceleration;
    this.navigation.decG = num;
  }

  public void NewAccRatio(float newR)
  {
    if (!(bool) (Object) this.navigation)
      return;
    this.navigation.accRatio = this.accSlider.value * 0.01f;
  }

  public void NewDecRatio(float newR)
  {
    if (!(bool) (Object) this.navigation)
      return;
    this.navigation.decRatio = this.decSlider.value * 0.01f;
  }

  public void SetNewFleet(FleetManager newFleet)
  {
    this.fleet = newFleet;
    this.navigation = newFleet.navigation;
    this.orbiter = newFleet.orbiter;
    this.fleet.UpdateFleet();
    if (this.navigation.currentState != NavigationState.Orbiting)
      return;
    this.navigation.accG = this.fleet.maxAccG;
  }

  public void SetNavigationFlightType(int i)
  {
    if (!(bool) (Object) this.navigation)
      return;
    switch (i)
    {
      case 0:
        this.navigation.flightType = FlightType.MOVE;
        break;
      case 1:
        this.navigation.flightType = FlightType.ALTITUDE;
        break;
      case 2:
        this.navigation.flightType = FlightType.BRAKE;
        break;
    }
  }

  public void SetNavigationArrivalType(int i)
  {
    if (!(bool) (Object) this.navigation)
      return;
    switch (i)
    {
      case 0:
        this.navigation.arrivalType = ArrivalType.STOP;
        break;
      case 1:
        this.navigation.arrivalType = ArrivalType.ORBIT;
        break;
      case 2:
        this.navigation.arrivalType = ArrivalType.INTERCEPT;
        break;
    }
  }

  public void SetOrbitDistance()
  {
    if (!(bool) (Object) this.navigation)
      return;
    this.navigation.orbitDistance = this.orbitDistSlider.value;
  }

  private void UpdateFlightData()
  {
    if ((Object) this.navigation == (Object) null || (Object) this.navigation.target == (Object) null || !(bool) (Object) this.navigation.target.GetComponent<Track>())
    {
      this.setupPanel.SetActive(false);
      this.movementPanel.SetActive(false);
    }
    else
    {
      this.setupPanel.SetActive(true);
      this.movementPanel.SetActive(true);
      Transform target = this.navigation.target;
      Track component = target.GetComponent<Track>();
      this.navTarget.text = !(component.trackName != "") ? component.publicName : component.trackName;
      if ((bool) (Object) target.GetComponent<Orbiter>() && target.GetComponent<Orbiter>().isOrbitable)
      {
        this.orbitable.text = "TRUE";
        this.orbitable.color = this.green;
      }
      else
      {
        this.orbitable.text = "FALSE";
        this.orbitable.color = this.red;
      }
      if (this.navigation.flightType == FlightType.MOVE)
      {
        this.movementPanel.SetActive(true);
        this.orbitDistSlider.minValue = target.GetComponent<Orbiter>().rocheLimitRadius * this.scaling.toKm;
        this.orbitDistSlider.maxValue = target.GetComponent<Orbiter>().soiRadius * this.scaling.toKm;
        this.flightDistance.text = (this.navigation.distanceToIntercept * this.scaling.toKm).ToString("N") + " km";
        this.topVelText.text = ((float) ((double) this.navigation.topVelocity * (double) this.scaling.toKm / 86400.0)).ToString("N") + " km/s";
        this.interceptVelText.text = this.navigation.interceptSpeed.ToString("N") + " km/s";
        this.interceptTimeText.text = this.navigation.interceptTime.ToString("N") + " sec";
        this.accGInput.text = this.navigation.accG.ToString("F4");
        this.decGInput.text = this.navigation.decG.ToString("F4");
        this.flightDuration.text = (this.navigation.totalTime * 24f).ToString("F2") + " hours";
        if (this.navigation.currentState == NavigationState.Orbiting)
        {
          float dVrequired = this.navigation.dVRequired;
          float fleetDv = this.fleet.fleetDv;
          if ((double) dVrequired > (double) fleetDv)
            this.dVRequired.color = this.red;
          else
            this.dVRequired.color = this.green;
          this.dVRequired.text = $"{dVrequired.ToString("F2")}/{fleetDv.ToString("F2")} km/s";
          float x = fleetDv / this.fleet.fleetDvMax;
          if ((double) x == double.PositiveInfinity || (double) x == double.NaN)
            return;
          this.dVBar.localScale = new Vector3(x, 1f, 1f);
          float num = dVrequired / fleetDv;
          if ((double) num == double.PositiveInfinity || float.IsNaN(num))
            return;
          this.dVUsedBar.localScale = new Vector3(num, 1f, 1f);
        }
        else
        {
          float fleetDv = this.fleet.fleetDv;
          this.dVRequired.text = fleetDv.ToString("F2") + " km/s";
          float x = fleetDv / this.fleet.fleetDvMax;
          if ((double) x == double.PositiveInfinity || (double) x == double.NaN)
            return;
          this.dVBar.localScale = new Vector3(x, 1f, 1f);
          this.dVUsedBar.localScale = new Vector3(0.0f, 1f, 1f);
        }
      }
      else if (this.navigation.flightType == FlightType.ALTITUDE)
      {
        if ((bool) (Object) this.altitudePanel)
          this.altitudePanel.SetActive(true);
      }
      else if (this.navigation.flightType == FlightType.BRAKE && (bool) (Object) this.brakePanel)
        this.brakePanel.SetActive(true);
      if ((double) this.fleet.fleetDv == 0.0)
        this.SetFlightButton(false, "NO Δv!");
      else if (this.navigation.isCollisionWarning)
        this.SetFlightButton(false, "COLLISION WARNING!");
      else if (this.navigation.isMaxAccExceeded)
        this.SetFlightButton(false, "MAX ACC EXCEEDED!");
      else if (this.navigation.isInsufficientDv)
        this.SetFlightButton(false, "INSUFFICIENT Δv!");
      else if (this.navigation.currentState != NavigationState.Orbiting)
        this.startFlightButton.SetActive(false);
      else
        this.SetFlightButton(true, "");
    }
  }

  private void SetFlightButton(bool isFlightButtonOn, string warning)
  {
    this.startFlightButton.SetActive(isFlightButtonOn);
    this.warningButton.SetActive(!isFlightButtonOn);
    this.warningText.text = warning;
  }

  private void UpdateCurrentData()
  {
    if ((Object) this.navigation == (Object) null)
    {
      this.currentDataPanel.SetActive(false);
    }
    else
    {
      this.currentDataPanel.SetActive(true);
      this.nameText.text = this.navigation.GetComponent<Track>().trackName;
      NavigationState currentState = this.navigation.currentState;
      if (currentState == NavigationState.Accelerating)
      {
        this.status.text = "Accelerating";
        this.status.color = this.green;
      }
      else if (currentState == NavigationState.Drifting)
      {
        this.status.text = "Drifting";
        this.status.color = this.gray;
      }
      else if (currentState == NavigationState.Decelerating)
      {
        this.status.text = "Decelerating";
        this.status.color = this.red;
      }
      else if (currentState == NavigationState.Orbiting)
      {
        this.status.text = "Orbiting " + this.orbiter.parentOrbiter.GetComponent<Track>().publicName;
        this.status.color = this.yellow;
      }
      float num;
      if (currentState == NavigationState.Orbiting)
      {
        TMP_Text velocity = this.velocity;
        num = this.navigation.GetComponent<Orbiter>().orbitalVelocity / 1000f;
        string str = num.ToString("F1") + "km/s";
        velocity.text = str;
      }
      else
      {
        TMP_Text velocity = this.velocity;
        num = (float) ((double) this.navigation.currentVel.magnitude * (double) this.scaling.toKm / 86400.0);
        string str = num.ToString("F1") + "km/s";
        velocity.text = str;
      }
      string str1 = "(+";
      if ((double) this.navigation.currentAcc.magnitude == 0.0)
      {
        this.acceleration.color = this.white;
        this.acceleration.text = "(+0)";
      }
      else
      {
        if (this.navigation.currentState == NavigationState.Accelerating)
        {
          this.acceleration.color = this.green;
        }
        else
        {
          str1 = "(-";
          this.acceleration.color = this.red;
        }
        TMP_Text acceleration = this.acceleration;
        string str2 = str1;
        num = this.navigation.currentAcc.magnitude / 86400f;
        string str3 = num.ToString("F1");
        string str4 = $"{str2}{str3})";
        acceleration.text = str4;
      }
      if ((double) this.navigation.artificialGravity > 5.0)
        this.artificialGravity.color = this.red;
      else if ((double) this.navigation.artificialGravity > 0.0)
        this.artificialGravity.color = this.green;
      else
        this.artificialGravity.color = this.white;
      this.artificialGravity.text = this.navigation.artificialGravity.ToString("F2") + " G";
      if ((double) this.fleet.fleetDv > 0.0)
        this.dVRemaining.color = this.green;
      else
        this.dVRemaining.color = this.red;
      float fleetDv = this.fleet.fleetDv;
      float fleetDvMax = this.fleet.fleetDvMax;
      this.dVRemaining.text = $"{fleetDv.ToString("F2")}/{fleetDvMax.ToString("F2")} km/s";
      this.dVBarCurrentData.SetBarSize(fleetDv / fleetDvMax);
      this.dVLimiter.text = this.fleet.DvLimiter;
      this.maxAccel.text = this.fleet.maxAccG.ToString("F4") + " G";
      this.accLimiter.text = this.fleet.AccLimiter;
    }
  }
}
