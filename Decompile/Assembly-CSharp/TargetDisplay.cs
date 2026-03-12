// Decompiled with JetBrains decompiler
// Type: TargetDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class TargetDisplay : MonoBehaviour
{
  [SerializeField]
  private TacticalInputs ti;
  private Transform mainCam;
  [SerializeField]
  private Camera scopeCam;
  private Transform tgtCamTransform;
  [SerializeField]
  private Transform targetPanel;
  [SerializeField]
  private TMP_Text trackText;
  [SerializeField]
  private TMP_Text distText;
  [SerializeField]
  private TMP_Text typeText;
  [SerializeField]
  private uiBar healthBar;
  [SerializeField]
  private GameObject redBG;
  [SerializeField]
  private float minDistance = 10f;
  [SerializeField]
  private float maxDistance = 1000f;
  [SerializeField]
  private float minFOV = 1f;
  [SerializeField]
  private float maxFOV = 130f;
  [SerializeField]
  private float k = 0.005f;
  private Target target;
  private Track track;

  private void Start()
  {
    this.ti = TacticalInputs.current;
    this.tgtCamTransform = this.scopeCam.transform;
    this.mainCam = Camera.main.transform;
    this.minDistance = 0.5f;
  }

  private void Update()
  {
    this.target = (Target) null;
    this.track = (Track) null;
    bool flag = false;
    if ((Object) this.ti.tempTarget != (Object) null)
      this.track = this.ti.tempTarget;
    else if ((Object) this.ti.selectedShip != (Object) null && (Object) this.ti.selectedShip.currentTarget != (Object) null)
    {
      this.track = this.ti.selectedShip.currentTarget;
      flag = true;
    }
    else if ((Object) this.ti.selectedSquadron != (Object) null && (Object) this.ti.selectedSquadron.targetTrack != (Object) null)
    {
      this.track = this.ti.selectedSquadron.targetTrack;
      flag = true;
    }
    else if ((Object) this.ti.target != (Object) null)
      this.track = this.ti.target;
    if ((Object) this.track != (Object) null && (Object) this.track.GetComponent<Target>() != (Object) null)
    {
      if (!this.targetPanel.gameObject.activeSelf)
        this.targetPanel.gameObject.SetActive(true);
      this.target = this.track.GetComponent<Target>();
      if ((bool) (Object) this.healthBar && (bool) (Object) this.track.GetComponent<ShipController>())
        this.healthBar.SetBarSize(this.track.GetComponent<ShipController>().totalHealthRatio);
      this.tgtCamTransform.LookAt(this.track.transform.position);
      if ((Object) this.redBG != (Object) null)
      {
        if (flag)
          this.redBG.SetActive(true);
        else
          this.redBG.SetActive(false);
      }
      this.DynamicFOV();
      this.UpdateTexts();
    }
    else
    {
      if (!this.targetPanel.gameObject.activeSelf)
        return;
      this.targetPanel.gameObject.SetActive(false);
    }
  }

  private void DynamicFOV()
  {
    this.scopeCam.fieldOfView = Mathf.Lerp(this.maxFOV, this.minFOV, 1f - Mathf.Exp((float) (-(double) this.k * ((double) Mathf.Clamp(this.target.GetDistanceFrom(this.tgtCamTransform.position), this.minDistance, this.maxDistance) - (double) this.minDistance))));
  }

  private void UpdateTexts()
  {
    this.trackText.text = this.track.trackName;
    float num = 0.01f;
    this.distText.text = (!((Object) this.ti.selectedShip != (Object) null) ? this.target.GetDistanceFrom(this.mainCam.position) * num : Vector3.Distance(this.target.transform.position, this.ti.selectedShip.transform.position) * num).ToString("N3") + " km";
    this.typeText.text = this.track.type.ToString() ?? "";
  }
}
