// Decompiled with JetBrains decompiler
// Type: HeatSystemUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HeatSystemUI : MonoBehaviour
{
  private TacticalInputs ti;
  private HeatManager hm;
  private ColorManager cm;
  [Header("DATA")]
  private float heatIn;
  private float heatOut;
  [Header("PREFABS")]
  [SerializeField]
  private HeatSource heatSourcePF;
  [SerializeField]
  private uiBar radiatorPF;
  [Header("UI")]
  [SerializeField]
  private GameObject heatPanel;
  [SerializeField]
  private TMP_Text heatOutText;
  [SerializeField]
  private TMP_Text overflowText;
  [SerializeField]
  private Image overflowBG;
  private bool isBlinking;
  private int blinkTweenId = -1;
  [SerializeField]
  private Transform heatSourceList;
  [SerializeField]
  private Transform radiatorList;
  [SerializeField]
  private Toggle radiatorToggle;
  [SerializeField]
  private uiBar heatsinkBarGreen;
  [SerializeField]
  private uiBar heatsinkBarRed;
  [SerializeField]
  private GameObject radiatorsOnButton;
  [SerializeField]
  private GameObject radiatorsOffButton;

  private void Start()
  {
    this.ti = TacticalInputs.current;
    this.ti.newSelection += new Action(this.NewShipSelected);
    this.cm = ColorManager.current;
  }

  private void Update()
  {
    if ((UnityEngine.Object) this.hm == (UnityEngine.Object) null || !this.heatPanel.activeSelf)
      return;
    this.UpdateHeatsinkBar();
    this.heatOut = this.hm.heatOut;
    for (int index = 0; index < this.hm.radiators.Count; ++index)
    {
      T_Radiator radiator = this.hm.radiators[index];
      this.UpdateRadiatorBar(this.radiatorList.GetChild(index).GetComponent<uiBar>(), radiator);
    }
    this.heatOut *= -1f;
    this.UpdateSources();
    Color color1 = this.cm.green;
    if ((double) this.heatIn > (double) this.heatOut)
      color1 = this.cm.red;
    int heatIn = (int) this.heatIn;
    int heatOut = (int) this.heatOut;
    this.heatOutText.text = $"{heatIn} MW / {heatOut} MW ({heatIn - heatOut} MW)";
    this.heatOutText.color = color1;
    bool flag = (double) this.hm.currentHeat > 1.0 && (double) this.hm.capacity <= 0.0;
    string str1 = "";
    float num = Mathf.Round(this.hm.timeToOverflow);
    if ((double) num > 0.0 && (double) num != double.PositiveInfinity)
      str1 = $" (OVERFLOW IN {num.ToString()} SEC)";
    string str2 = "0 MJ";
    this.overflowText.text = (double) this.hm.currentHeat <= 1.0 ? str2 + str1 : Mathf.Round(this.hm.currentHeat).ToString() + " MJ";
    Color color2 = this.cm.green;
    if (flag)
      color2 = this.cm.red;
    this.overflowText.color = color2;
    if (!this.isBlinking & flag)
    {
      this.blinkTweenId = LeanTween.color(this.overflowBG.rectTransform, this.cm.red, 0.5f).setLoopPingPong().id;
      this.isBlinking = true;
    }
    else
    {
      if (!this.isBlinking || flag)
        return;
      if (this.blinkTweenId >= 0)
      {
        LeanTween.cancel(this.blinkTweenId);
        this.blinkTweenId = -1;
      }
      this.overflowBG.color = this.cm.darkGray;
      this.isBlinking = false;
    }
  }

  private void UpdateRadiatorBar(uiBar radiatorBar, T_Radiator radiator)
  {
    if (!radiator.isOn)
      radiatorBar.SetBarColor(this.cm.darkGreen);
    else
      radiatorBar.SetBarColor(this.cm.green);
    radiatorBar.SetBarSize(radiator.healthRatio);
  }

  private void UpdateSources()
  {
    this.heatIn = this.hm.heatIn;
    foreach (T_Module module in this.hm.modules)
    {
      HeatSource heatSource = (HeatSource) null;
      IEnumerator enumerator = (IEnumerator) this.heatSourceList.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          Transform current = (Transform) enumerator.Current;
          if (current.name == module.GetInstanceID().ToString())
            heatSource = current.GetComponent<HeatSource>();
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      if (!((UnityEngine.Object) heatSource == (UnityEngine.Object) null))
      {
        RectTransform component = heatSource.GetComponent<RectTransform>();
        if ((double) module.currentHeat <= 0.0)
        {
          component.sizeDelta = new Vector2(0.0f, 2f);
        }
        else
        {
          float f = module.currentHeat / this.heatIn;
          if ((double) f == double.NegativeInfinity || (double) f == double.PositiveInfinity || float.IsNaN(f))
            f = 0.0f;
          float x = f * 300f;
          component.sizeDelta = new Vector2(x, 2f);
        }
      }
    }
    foreach (T_Mount mount in this.hm.mounts)
    {
      if (!((UnityEngine.Object) mount == (UnityEngine.Object) null))
      {
        HeatSource heatSource = (HeatSource) null;
        IEnumerator enumerator = (IEnumerator) this.heatSourceList.GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            Transform current = (Transform) enumerator.Current;
            if (current.name == mount.GetInstanceID().ToString())
              heatSource = current.GetComponent<HeatSource>();
          }
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
        if (!((UnityEngine.Object) heatSource == (UnityEngine.Object) null))
        {
          RectTransform component = heatSource.GetComponent<RectTransform>();
          if ((double) mount.currentHeat <= 0.0)
          {
            component.sizeDelta = new Vector2(0.0f, 2f);
          }
          else
          {
            float f = mount.currentHeat / this.heatIn;
            if ((double) f == double.NegativeInfinity || (double) f == double.PositiveInfinity || float.IsNaN(f))
              f = 0.0f;
            float x = f * 300f;
            component.sizeDelta = new Vector2(x, 2f);
          }
        }
      }
    }
  }

  private void UpdateHeatsinkBar()
  {
    this.heatsinkBarGreen.SetBarSize(this.hm.heatsinkMax / this.hm.heatsinkMaxMax);
    this.heatsinkBarRed.SetBarSize(this.hm.heatsinkCurrent / this.hm.heatsinkMax);
  }

  private void NewShipSelected()
  {
    if ((UnityEngine.Object) this.ti.selectedShip == (UnityEngine.Object) null || (UnityEngine.Object) this.ti.selectedShip.GetComponent<HeatManager>() == (UnityEngine.Object) null)
    {
      this.heatPanel.SetActive(false);
    }
    else
    {
      this.heatPanel.SetActive(true);
      this.hm = this.ti.selectedShip.GetComponent<HeatManager>();
      IEnumerator enumerator1 = (IEnumerator) this.radiatorList.GetEnumerator();
      try
      {
        while (enumerator1.MoveNext())
          UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator1.Current).gameObject);
      }
      finally
      {
        if (enumerator1 is IDisposable disposable)
          disposable.Dispose();
      }
      this.UpdateHeatsinkBar();
      this.heatOut = this.hm.heatOut;
      foreach (T_Radiator radiator in this.hm.radiators)
      {
        this.heatOut += radiator.heat;
        this.UpdateRadiatorBar(UnityEngine.Object.Instantiate<uiBar>(this.radiatorPF, this.radiatorList), radiator);
      }
      this.heatOut *= -1f;
      IEnumerator enumerator2 = (IEnumerator) this.heatSourceList.GetEnumerator();
      try
      {
        while (enumerator2.MoveNext())
          UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator2.Current).gameObject);
      }
      finally
      {
        if (enumerator2 is IDisposable disposable)
          disposable.Dispose();
      }
      this.heatIn = this.hm.heatIn;
      foreach (T_Module module in this.hm.modules)
      {
        HeatSource heatSource = UnityEngine.Object.Instantiate<HeatSource>(this.heatSourcePF, this.heatSourceList);
        heatSource.nameText.text = module.productName;
        heatSource.name = module.GetInstanceID().ToString();
        heatSource.GetComponent<RectTransform>().sizeDelta = new Vector2(0.0f, 2f);
      }
      foreach (T_Mount mount in this.hm.mounts)
      {
        HeatSource heatSource = UnityEngine.Object.Instantiate<HeatSource>(this.heatSourcePF, this.heatSourceList);
        heatSource.nameText.text = mount.productName;
        heatSource.name = mount.GetInstanceID().ToString();
        heatSource.GetComponent<RectTransform>().sizeDelta = new Vector2(0.0f, 2f);
      }
      if (this.hm.isRadiatorsExtended)
      {
        this.radiatorsOnButton.SetActive(true);
        this.radiatorsOffButton.SetActive(false);
      }
      else
      {
        this.radiatorsOnButton.SetActive(false);
        this.radiatorsOffButton.SetActive(true);
      }
    }
  }

  public void ToggleRadiators(bool isOn)
  {
    if (!(bool) (UnityEngine.Object) this.hm)
      return;
    this.hm.ToggleRadiators(new bool?(isOn));
  }
}
