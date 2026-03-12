// Decompiled with JetBrains decompiler
// Type: SaveLoadSystem
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

#nullable enable
public class SaveLoadSystem : MonoBehaviour
{
  public static 
  #nullable disable
  SaveLoadSystem current;
  private GameLoader gl;
  private GameSaver gs;
  private TimeManager tm;
  private MapInputs mi;
  private AudioManager am;
  [SerializeField]
  private string gameName;
  private string savePath;
  [SerializeField]
  private GameObject savePanel;
  [SerializeField]
  private TMP_Text saveNameText;
  [SerializeField]
  private TMP_Text saveLocationText;
  [SerializeField]
  private Transform savesList;
  [SerializeField]
  private LoadImg LoadImgPF;
  [SerializeField]
  private GameObject saveOverwritePrompt;
  public Action OnLoadGame;
  private bool isPrometheus;
  private bool isInit;

  private void Awake()
  {
    if ((UnityEngine.Object) SaveLoadSystem.current != (UnityEngine.Object) null && (UnityEngine.Object) SaveLoadSystem.current != (UnityEngine.Object) this)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      SaveLoadSystem.current = this;
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
      this.savePath = Path.Combine(Application.persistentDataPath, "Saves");
      if (!Directory.Exists(this.savePath))
        Directory.CreateDirectory(this.savePath);
      this.saveLocationText.text = "SAVE LOCATION: " + this.savePath;
    }
  }

  private void Start() => this.Init();

  private void Init()
  {
    if (this.isInit)
      return;
    this.gl = GameLoader.current;
    this.gs = GameSaver.current;
    this.tm = TimeManager.current;
    this.mi = MapInputs.current;
    this.am = AudioManager.current;
    this.UpdateList();
    this.StartCoroutine((IEnumerator) this.DelayUpdateGameName());
    MonoBehaviour.print((object) ("SLS: gameName: " + this.gameName));
    this.isInit = true;
  }

  private IEnumerator DelayUpdateGameName()
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.UpdateGameName();
  }

  public void UpdateGameName() => this.gameName = FactionsManager.current.playerFaction.factionName;

  public void TrySaveGame()
  {
    if (this.CheckForDuplicate())
      this.saveOverwritePrompt.SetActive(true);
    else
      this.SaveGame();
  }

  private bool CheckForDuplicate()
  {
    bool flag = false;
    if (this.gameName == "")
      this.UpdateGameName();
    if (File.Exists(Path.Combine(this.savePath, $"{this.gameName} - {this.saveNameText.text}" + ".json")))
      flag = true;
    return flag;
  }

  public void SaveGame(
  #nullable enable
  string? customSaveName = null)
  {
    GameSaveData saveData = this.gs.GetSaveData();
    this.tm.timeScale = 0.0f;
    if (this.gameName == "")
      this.UpdateGameName();
    string str = $"{this.gameName} - {this.saveNameText.text}";
    if (customSaveName != null && customSaveName != "" && !string.IsNullOrEmpty(customSaveName))
      str = $"{this.gameName} - {customSaveName}";
    string path = Path.Combine(this.savePath, str + ".json");
    string json = JsonUtility.ToJson((object) saveData, true);
    if (File.Exists(path))
      File.Delete(path);
    File.WriteAllText(path, json);
    MonoBehaviour.print((object) ("Game saved to: " + path));
    this.UpdateList();
    this.savePanel.SetActive(false);
  }

  public bool LoadAutoSave()
  {
    MonoBehaviour.print((object) ("loading autosave. GameName: " + this.gameName));
    this.am.PlaySFX(1);
    if (this.gameName == "")
      this.UpdateGameName();
    return this.LoadGame(this.gameName + " - AUTOSAVE");
  }

  public bool LoadGame(
  #nullable disable
  string saveName)
  {
    this.Init();
    MonoBehaviour.print((object) $"===== loading SAVE: {saveName} =====");
    string path = Path.Combine(this.savePath, saveName + ".json");
    if (!File.Exists(path))
    {
      MonoBehaviour.print((object) ("No save file found at: " + path));
      return true;
    }
    GameSaveData newGSD = JsonUtility.FromJson<GameSaveData>(File.ReadAllText(path));
    this.savePanel.SetActive(false);
    this.mi.ClearAll();
    Action onLoadGame = this.OnLoadGame;
    if (onLoadGame != null)
      onLoadGame();
    MonoBehaviour.print((object) "calling on load game");
    return this.gl.StartNew(newGSD);
  }

  public void DeleteSave(string saveName)
  {
    this.am.PlaySFX(4);
    string path = Path.Combine(this.savePath, saveName + ".json");
    if (File.Exists(path))
    {
      File.Delete(path);
      MonoBehaviour.print((object) "Save deleted.");
    }
    this.UpdateList();
  }

  public void UpdateList()
  {
    if (!Directory.Exists(this.savePath))
      return;
    this.isPrometheus = FactionsManager.current.playerFaction.factionName == "Prometheus";
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
    foreach (FileInfo fileInfo in (IEnumerable<FileInfo>) ((IEnumerable<FileInfo>) new DirectoryInfo(this.savePath).GetFiles("*.json")).OrderByDescending<FileInfo, DateTime>((Func<FileInfo, DateTime>) (f => f.LastWriteTime)))
    {
      string withoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
      if (!withoutExtension.Contains("AUTOSAVE") && (this.isPrometheus || !withoutExtension.Contains("Prometheus")) && (!this.isPrometheus || withoutExtension.Contains("Prometheus")))
      {
        LoadImg loadImg = UnityEngine.Object.Instantiate<LoadImg>(this.LoadImgPF, this.savesList);
        loadImg.nameText.text = withoutExtension;
        string saveVersion = this.GetSaveVersion(withoutExtension);
        loadImg.dateText.text = $"{saveVersion} // {fileInfo.LastWriteTime.ToString("(yyyy-MM-dd HH-mm-ss)")}";
        if (saveVersion != Application.version)
        {
          loadImg.dateText.text = saveVersion + " >> VERSION MISMATCH. PROCEED AT YOUR OWN RISK!";
          loadImg.SetOutdated();
        }
      }
    }
  }

  private string GetSaveVersion(string saveName)
  {
    string path = Path.Combine(this.savePath, saveName + ".json");
    if (File.Exists(path))
      return JsonUtility.FromJson<GameSaveData>(File.ReadAllText(path)).version;
    MonoBehaviour.print((object) ("No save file found at: " + path));
    return "";
  }

  public void OpenFilePath()
  {
    if (!Directory.Exists(this.savePath))
      Directory.CreateDirectory(this.savePath);
    Process.Start("explorer.exe", this.savePath.Replace("/", "\\"));
    MonoBehaviour.print((object) ("savepath: " + this.savePath));
  }
}
