// Decompiled with JetBrains decompiler
// Type: NotificationsManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class NotificationsManager : MonoBehaviour
{
  public static NotificationsManager current;
  private AudioManager am;
  [Header("UI")]
  [SerializeField]
  private Transform notifsList;
  [SerializeField]
  private GameObject notifPrefab;
  [SerializeField]
  private uiAnimator permanentNotifsUIA;
  [SerializeField]
  private Transform permanentNotifsList;
  [Header("COLORS")]
  [SerializeField]
  private List<ColorTheme> colorThemes;

  private void Awake() => NotificationsManager.current = this;

  private void Start() => this.am = AudioManager.current;

  public void TogglePermaNotifs() => this.permanentNotifsUIA.Toggle();

  public void ClearNotifs()
  {
    IEnumerator enumerator = (IEnumerator) this.permanentNotifsList.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
        UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator.Current).gameObject);
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
  }

  public void NewNotif(string message, string color)
  {
    ColorTheme colorTheme = this.colorThemes.Find((Predicate<ColorTheme>) (ct => ct.name == color)) ?? this.colorThemes[0];
    this.SpawnNotif(message, colorTheme);
  }

  public void NewNotif(string message) => this.SpawnNotif(message, this.colorThemes[0]);

  private void SpawnNotif(string message, ColorTheme colorTheme)
  {
    UnityEngine.Object.Instantiate<GameObject>(this.notifPrefab, this.notifsList).GetComponent<Notification>().Setup(message, colorTheme, true);
    UnityEngine.Object.Instantiate<GameObject>(this.notifPrefab, this.permanentNotifsList).GetComponent<Notification>().Setup(message, colorTheme, false);
    this.am.PlaySFX(7);
  }
}
