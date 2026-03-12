// Decompiled with JetBrains decompiler
// Type: MainMenuSaveManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

#nullable disable
public class MainMenuSaveManager : MonoBehaviour
{
  public static MainMenuSaveManager current;
  [SerializeField]
  private MainMenu mm;
  private SceneTransitionManager stm;
  private AudioManager am;
  private string savePath;
  [Header("UI")]
  [SerializeField]
  private TMP_Text saveLocationText;
  [SerializeField]
  private Transform savesList;
  [SerializeField]
  private LoadImg LoadImgPF;
  [SerializeField]
  private GameObject continueButton;
  [SerializeField]
  private TMP_Text continueText;
  private int savesCount;

  private void Awake()
  {
    MainMenuSaveManager.current = this;
    this.savePath = Path.Combine(Application.persistentDataPath, "Saves");
    if (!Directory.Exists(this.savePath))
      Directory.CreateDirectory(this.savePath);
    this.saveLocationText.text = "SAVE LOCATION: " + this.savePath;
  }

  private void Start()
  {
    this.am = AudioManager.current;
    this.stm = SceneTransitionManager.current;
    this.UpdateList();
  }

  public void Continue()
  {
    if (this.savesCount <= 0)
      return;
    this.LoadGame(this.savesList.GetChild(0).GetComponent<LoadImg>().nameText.text);
  }

  public void UpdateList()
  {
    if (!Directory.Exists(this.savePath))
      return;
    this.savesCount = 0;
    IEnumerator enumerator = (IEnumerator) this.savesList.GetEnumerator();
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
    IOrderedEnumerable<FileInfo> orderedEnumerable = ((IEnumerable<FileInfo>) new DirectoryInfo(this.savePath).GetFiles("*.json")).OrderByDescending<FileInfo, DateTime>((Func<FileInfo, DateTime>) (f => f.LastWriteTime));
    bool flag = false;
    foreach (FileInfo fileInfo in (IEnumerable<FileInfo>) orderedEnumerable)
    {
      string withoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
      if (!withoutExtension.Contains("AUTOSAVE"))
      {
        LoadImg loadImg = UnityEngine.Object.Instantiate<LoadImg>(this.LoadImgPF, this.savesList);
        loadImg.nameText.text = withoutExtension;
        string saveVersion = this.GetSaveVersion(withoutExtension);
        loadImg.dateText.text = $"{saveVersion} // {fileInfo.LastWriteTime.ToString("(yyyy-MM-dd HH-mm-ss)")}";
        if (saveVersion != Application.version)
        {
          loadImg.dateText.text = saveVersion + " >> VERSION MISMATCH. PROCEED AT YOUR OWN RISK!";
          loadImg.SetOutdated();
          if (this.savesCount == 0)
            flag = true;
        }
        ++this.savesCount;
      }
    }
    if (this.savesCount > 0 && !flag)
    {
      this.continueButton.SetActive(true);
      this.continueText.text = this.savesList.GetChild(0).GetComponent<LoadImg>().nameText.text;
    }
    else
      this.continueButton.SetActive(false);
  }

  private string GetSaveVersion(string saveName)
  {
    string path = Path.Combine(this.savePath, saveName + ".json");
    if (File.Exists(path))
      return JsonUtility.FromJson<GameSaveData>(File.ReadAllText(path)).version;
    MonoBehaviour.print((object) ("No save file found at: " + path));
    return "";
  }

  public void LoadGame(string saveName)
  {
    this.am.PlaySFX(1);
    MonoBehaviour.print((object) ("LOADING SAVED GAME: " + saveName));
    SceneTransitionManager current = SceneTransitionManager.current;
    current.gst = !saveName.Contains("Prometheus") ? current.loadCustomGST : current.loadPrometheusGST;
    current.gst.saveName = saveName;
    this.mm.NewGameLoad();
  }

  public void DeleteSave(string saveName)
  {
    this.am.PlaySFX(3);
    string path = Path.Combine(this.savePath, saveName + ".json");
    if (File.Exists(path))
    {
      File.Delete(path);
      MonoBehaviour.print((object) "Save deleted.");
    }
    this.UpdateList();
  }

  public void OpenFilePath()
  {
    if (!Directory.Exists(this.savePath))
      Directory.CreateDirectory(this.savePath);
    Process.Start("explorer.exe", this.savePath.Replace("/", "\\"));
    MonoBehaviour.print((object) ("savepath: " + this.savePath));
  }
}
