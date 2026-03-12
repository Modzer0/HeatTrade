// Decompiled with JetBrains decompiler
// Type: PrometheusVideo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.Video;

#nullable disable
public class PrometheusVideo : MonoBehaviour
{
  private TimeManager tm;
  [SerializeField]
  private VideoPlayer videoPlayer;
  [SerializeField]
  private uiAnimator panel;
  [SerializeField]
  private uiAnimator text;

  private void Start()
  {
    this.tm = TimeManager.current;
    if (PlayerPrefs.GetInt("Prometheus_VideoPlayed", 0) == 0)
    {
      this.On();
    }
    else
    {
      this.HidePanel(this.videoPlayer);
      this.gameObject.SetActive(false);
    }
  }

  private void Update()
  {
    if (!Input.GetKeyDown(KeyCode.Space))
      return;
    this.HidePanel(this.videoPlayer);
  }

  private void On()
  {
    this.panel.GetComponent<CanvasGroup>().alpha = 1f;
    this.videoPlayer.loopPointReached += new VideoPlayer.EventHandler(this.HidePanel);
    this.videoPlayer.Play();
    this.text.Show();
    this.StartCoroutine((IEnumerator) this.TextFade());
    this.tm.timeScale = 0.0f;
  }

  private IEnumerator TextFade()
  {
    yield return (object) new WaitForSeconds(10f);
    this.text.Hide();
  }

  private void HidePanel(VideoPlayer vp)
  {
    this.videoPlayer.Stop();
    this.panel.Hide();
    PlayerPrefs.SetInt("Prometheus_VideoPlayed", 1);
    this.tm.timeScale = 0.01f;
    this.gameObject.SetActive(false);
  }
}
