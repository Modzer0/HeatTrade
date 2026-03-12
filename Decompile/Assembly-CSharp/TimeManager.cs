// Decompiled with JetBrains decompiler
// Type: TimeManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

#nullable disable
public class TimeManager : MonoBehaviour
{
  public static TimeManager current;
  public float timeScale = 1f;
  public bool isGettingInput = true;
  public int mins;
  public int hours;
  public int days;
  public int months;
  public int years;
  private int totalDays = 1;
  private float addSecs;
  private int addMins;
  private int addHours;
  [SerializeField]
  private TMP_Text clock;
  [SerializeField]
  private TMP_Text timeScaleText;
  public Action NewDay;
  public Action NewHour;

  public int TotalDays => this.totalDays;

  private void Awake()
  {
    if ((UnityEngine.Object) TimeManager.current != (UnityEngine.Object) null && (UnityEngine.Object) TimeManager.current != (UnityEngine.Object) this)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    else
      TimeManager.current = this;
  }

  private void Start() => Time.timeScale = 1f;

  private void Update()
  {
    if (this.isGettingInput)
    {
      if (Input.GetKeyDown(KeyCode.F5))
        this.timeScale = 1f;
      else if (Input.GetKeyDown(KeyCode.F4))
        this.timeScale = 0.1f;
      else if (Input.GetKeyDown(KeyCode.F3))
        this.timeScale = 0.01f;
      else if (Input.GetKeyDown(KeyCode.F2))
        this.timeScale = 1f / 1000f;
      else if (Input.GetKeyDown(KeyCode.F1))
        this.timeScale = 0.0f;
    }
    this.UpdateValues();
    this.UpdateClock();
  }

  public void SetTotalDays(int newTotalDays) => this.totalDays = newTotalDays;

  private void UpdateValues()
  {
    this.addSecs += this.timeScale * Time.deltaTime * 86400f;
    if ((double) this.addSecs > 60.0)
    {
      this.addMins = Mathf.FloorToInt(this.addSecs / 60f);
      this.addSecs -= (float) this.addMins * 60f;
    }
    if ((double) this.addMins > 60.0)
    {
      this.addHours = Mathf.FloorToInt((float) this.addMins / 60f);
      this.addMins = Mathf.FloorToInt((float) this.addMins % 60f);
    }
    this.mins += this.addMins;
    this.addMins = 0;
    if (this.mins >= 60)
    {
      ++this.hours;
      this.mins -= 60;
      Action newHour = this.NewHour;
      if (newHour != null)
        newHour();
    }
    this.hours += this.addHours;
    this.addHours = 0;
    if (this.hours >= 24)
    {
      ++this.days;
      this.hours -= 24;
      ++this.totalDays;
      Action newDay = this.NewDay;
      if (newDay != null)
        newDay();
    }
    if (this.days > 30)
    {
      ++this.months;
      this.days -= 30;
    }
    if (this.months <= 12)
      return;
    ++this.years;
    this.months -= 12;
  }

  private void UpdateClock()
  {
    string str1 = "Paused";
    if ((double) this.timeScale == 1.0)
      str1 = "+1 Day";
    else if ((double) this.timeScale == 0.10000000149011612)
      str1 = "+2.4 Hrs";
    else if ((double) this.timeScale == 0.0099999997764825821)
      str1 = "+14.4 Min";
    else if ((double) this.timeScale == 1.0 / 1000.0)
      str1 = "+1.44 Min";
    this.timeScaleText.text = str1;
    string str2 = this.hours.ToString();
    if (this.hours < 10)
      str2 = "0" + str2;
    string str3 = this.mins.ToString();
    if (this.mins < 10)
      str3 = "0" + str3;
    this.clock.text = $"{str2}{str3}H / D{this.days.ToString()} / M{this.months.ToString()} / {this.years.ToString()}";
  }

  public string GetDate()
  {
    string str1 = this.hours.ToString();
    if (this.hours < 10)
      str1 = "0" + str1;
    string str2 = this.mins.ToString();
    if (this.mins < 10)
      str2 = "0" + str2;
    return $"{str1}{str2}.{this.days.ToString()}.{this.months.ToString()}.{this.years.ToString()}";
  }
}
