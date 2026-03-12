// Decompiled with JetBrains decompiler
// Type: Notification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Notification : MonoBehaviour
{
  [Header("UI")]
  private uiAnimator uia;
  [SerializeField]
  private Image bg;
  [SerializeField]
  private TMP_Text text;
  private float showDuration = 10f;
  private float fadeDuration = 2f;

  private void Start() => this.uia = this.GetComponent<uiAnimator>();

  public void Setup(string message, ColorTheme colorTheme, bool isFade)
  {
    this.bg.color = colorTheme.colorBG;
    this.text.color = colorTheme.colorText;
    this.text.text = message;
    this.StartCoroutine((IEnumerator) this.SetupRoutine());
    if (!isFade)
      return;
    this.StartCoroutine((IEnumerator) this.StartFadeTimer());
  }

  private IEnumerator SetupRoutine()
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.uia.Show();
  }

  private IEnumerator StartFadeTimer()
  {
    Notification notification = this;
    yield return (object) new WaitForSeconds(notification.showDuration);
    notification.uia.Hide();
    yield return (object) new WaitForSeconds(notification.fadeDuration);
    Object.Destroy((Object) notification.gameObject);
  }
}
