// Decompiled with JetBrains decompiler
// Type: EndScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class EndScreen : MonoBehaviour
{
  private LoadingScreen loadingScreen;

  private void Start() => this.loadingScreen = Object.FindObjectOfType<LoadingScreen>();

  public void ReturnToMainMenu() => this.LoadScene("SCENE - Main Menu");

  public void Tutorial3Finish()
  {
    SceneTransitionManager.current.gst = SceneTransitionManager.current.newPrometheusGST;
    this.LoadScene("SCENE - Prometheus");
  }

  public void LoadScene(string sceneName)
  {
    this.StartCoroutine((IEnumerator) this.LoadSceneRoutine(sceneName));
  }

  private IEnumerator LoadSceneRoutine(string sceneName)
  {
    this.loadingScreen.gameObject.SetActive(true);
    this.loadingScreen.Show();
    AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
    op.allowSceneActivation = false;
    while (!op.isDone)
    {
      double progress = (double) op.progress;
      this.loadingScreen.UpdateProgress(Mathf.Clamp01((float) (progress / 0.89999997615814209)));
      if (progress >= 0.89999997615814209)
      {
        this.loadingScreen.UpdateProgress(1f);
        yield return (object) new WaitForSeconds(2f);
        this.loadingScreen.Hide();
        op.allowSceneActivation = true;
      }
      yield return (object) null;
    }
  }
}
