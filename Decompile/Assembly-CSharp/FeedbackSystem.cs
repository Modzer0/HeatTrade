// Decompiled with JetBrains decompiler
// Type: FeedbackSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

#nullable disable
public class FeedbackSystem : MonoBehaviour
{
  public string webAppUrl = "https://script.google.com/macros/s/AKfycbx8ktRj563FHbw2qaGx0sOprzzq5hFTmDXfNLnvC9SwjSbkJR6U8e15gbAPIVZcNDZKtA/exec";
  private StringBuilder logLog = new StringBuilder();
  [SerializeField]
  private GameObject bugReportPanel;
  [SerializeField]
  private TMP_InputField description;
  [SerializeField]
  private TMP_Text submitText;
  [SerializeField]
  private GameObject notice;
  private int severityInt;
  private bool isOn;
  private MapInputs mi;
  private TacticalInputs ti;
  private bool isInit;
  private bool isSubmitting;
  private AudioManager am;
  private NotificationsManager notifs;
  [SerializeField]
  private uiAnimator sendingUIA;
  [SerializeField]
  private uiAnimator sentUIA;
  [SerializeField]
  private uiAnimator failedUIA;

  private void Start()
  {
    this.Init();
    this.bugReportPanel.SetActive(false);
    this.isOn = false;
    this.am = this.GetComponent<AudioManager>();
  }

  private void Init()
  {
    if (this.isInit)
      return;
    this.logLog.Clear();
    this.mi = MapInputs.current;
    this.ti = TacticalInputs.current;
    this.notifs = NotificationsManager.current;
    this.isInit = true;
  }

  public void PanelToggle()
  {
    this.Init();
    this.isOn = !this.isOn;
    if ((bool) (UnityEngine.Object) this.mi)
      this.mi.isInputOn = !this.isOn;
    else if ((bool) (UnityEngine.Object) this.ti)
      this.ti.isInputOn = !this.isOn;
    this.bugReportPanel.SetActive(this.isOn);
  }

  public void SubmitFeedback()
  {
    if (this.description.text.Length < 1 || this.isSubmitting)
    {
      this.am.PlaySFX(4);
    }
    else
    {
      this.am.PlaySFX(1);
      MonoBehaviour.print((object) "submitting feedback");
      this.StartCoroutine((IEnumerator) this.UploadFeedback());
    }
  }

  private IEnumerator UploadFeedback()
  {
    Debug.Log((object) "1. Starting Upload...");
    this.isSubmitting = true;
    yield return (object) new WaitForEndOfFrame();
    Texture2D tex = ScreenCapture.CaptureScreenshotAsTexture();
    string base64String = Convert.ToBase64String(tex.EncodeToPNG());
    UnityEngine.Object.Destroy((UnityEngine.Object) tex);
    int num = base64String.Length;
    Debug.Log((object) ("2. Screenshot converted. Length: " + num.ToString()));
    WWWForm formData = new WWWForm();
    formData.AddField("message", $"SEVERITY: {this.severityInt.ToString()}\n\n{this.description.text}");
    formData.AddField("image", base64String);
    string[] strArray = new string[12];
    strArray[0] = "OS: ";
    strArray[1] = SystemInfo.operatingSystem;
    strArray[2] = "\nGPU: ";
    strArray[3] = SystemInfo.graphicsDeviceName;
    strArray[4] = "\nRAM: ";
    num = SystemInfo.systemMemorySize;
    strArray[5] = num.ToString();
    strArray[6] = "\nCPU: ";
    strArray[7] = SystemInfo.processorType;
    strArray[8] = "\nRESOLUTION: ";
    num = Screen.width;
    strArray[9] = num.ToString();
    strArray[10] = "x";
    num = Screen.height;
    strArray[11] = num.ToString();
    string str1 = string.Concat(strArray);
    string str2 = $"\n\nGAME VERSION: {Application.version}\nSCENE: {SceneManager.GetActiveScene().name}";
    string str3 = $"\n\nGST: {SceneTransitionManager.current.gst.nla.ToString()}, {SceneTransitionManager.current.gst.isPrometheus.ToString()}, {SceneTransitionManager.current.gst.saveName}";
    formData.AddField("specs", str1 + str2 + str3);
    formData.AddField("logs", this.logLog.ToString());
    using (UnityWebRequest www = UnityWebRequest.Post(this.webAppUrl, formData))
    {
      www.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
      Debug.Log((object) ("3. Sending Request to: " + this.webAppUrl));
      this.submitText.text = "SENDING...";
      yield return (object) www.SendWebRequest();
      if (www.result != UnityWebRequest.Result.Success)
      {
        Debug.LogError((object) ("FAILED: " + www.error));
        this.TryNotif("Send failed!", "red");
        this.Finish();
        this.am.PlaySFX(4);
      }
      else
      {
        Debug.Log((object) ("4. SUCCESS! Response: " + www.downloadHandler.text));
        this.TryNotif("Feedback sent!", "green");
        this.Finish();
        this.am.PlaySFX(3);
      }
    }
  }

  private void TryNotif(string message, string color)
  {
    if ((bool) (UnityEngine.Object) this.notifs)
      this.notifs.NewNotif("[BUG REPORT] " + message, color);
    if (color == "green")
      this.StartCoroutine((IEnumerator) this.FadeUIA(this.sentUIA));
    else
      this.StartCoroutine((IEnumerator) this.FadeUIA(this.failedUIA));
  }

  private IEnumerator FadeUIA(uiAnimator uia)
  {
    uia.Show();
    yield return (object) new WaitForSeconds(3f);
    uia.Hide();
  }

  private void Finish()
  {
    this.description.text = "";
    this.submitText.text = "SUBMIT";
    this.isSubmitting = false;
  }

  private void OnEnable()
  {
    Application.logMessageReceived += new Application.LogCallback(this.HandleLog);
  }

  private void OnDisable()
  {
    Application.logMessageReceived -= new Application.LogCallback(this.HandleLog);
  }

  private void HandleLog(string logString, string stackTrace, LogType type)
  {
    string str = DateTime.Now.ToString("HH:mm:ss");
    if (type == LogType.Exception || type == LogType.Error)
    {
      this.logLog.AppendLine($"\n--- ERROR AT {str} ---");
      this.logLog.AppendLine("Message: " + logString);
      this.logLog.AppendLine("Location: " + stackTrace);
      this.logLog.AppendLine("---------------------------\n");
    }
    else
      this.logLog.AppendLine($"[{str}] [{type}] {logString}");
    int num = 2000;
    if (this.logLog.Length <= num)
      return;
    this.logLog.Remove(0, this.logLog.Length - num);
  }

  public void SetSeverity(int i) => this.severityInt = i;
}
