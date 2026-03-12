// Decompiled with JetBrains decompiler
// Type: SceneTransitionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#nullable disable
public class SceneTransitionManager : MonoBehaviour
{
  private MapInputs mi;
  private FactionsManager fm;
  private SaveLoadSystem sls;
  private EncounterPanelManager epm;
  private CameraManager cm;
  private TrackDisplayer td;
  [SerializeField]
  private int sunDistance = 149600000;
  public List<BodyInfo> bodies;
  public List<TacticalGroupData> groups;
  public TacticalGroupData winningGroup;
  [SerializeField]
  private LoadingScreen loadingScreen;
  private List<TacticalGroupData> currentGroups;
  private Vector3 currentEncounterPos;
  private AsyncOperation currentAsyncOp;
  private string strategicSceneName;
  public bool isPlayerVictory;
  public bool isBountyHunt;
  public int bountyRewardCredits;
  [Header("Game Start Type")]
  public GameStartType gst;
  public GameStartType newPrometheusGST;
  public GameStartType newCustomGST;
  public GameStartType loadPrometheusGST;
  public GameStartType loadCustomGST;
  public GameStartType autosavePrometheusGST;
  public GameStartType autosaveCustomGST;
  public GameStartType skirmishGST;
  public SkirmishMapName skirmishMapName;

  public static SceneTransitionManager current { get; private set; }

  private void Awake()
  {
    if ((UnityEngine.Object) SceneTransitionManager.current != (UnityEngine.Object) null && (UnityEngine.Object) SceneTransitionManager.current != (UnityEngine.Object) this)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      SceneTransitionManager.current = this;
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
    }
  }

  private void Start()
  {
    this.GetDependencies();
    SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    if (scene.name != "SCENE - Tactical")
      this.GetDependencies();
    if (scene.name == "SCENE - Prometheus" || scene.name == "SCENE - Strategic")
    {
      MonoBehaviour.print((object) ("STM ON SCENE LOADED: " + scene.name));
      this.strategicSceneName = SceneManager.GetActiveScene().name;
      MonoBehaviour.print((object) $"GST: {this.gst.nla}, {this.gst.isPrometheus}, {this.gst.saveName}");
      if (this.gst == null)
        Debug.LogError((object) "NO GAME START TYPE SET!");
      else if (this.gst.nla == GameStartType.NLA.New)
      {
        NewGameManager.current.SetupNewGame(this.gst.isPrometheus);
      }
      else
      {
        if (this.gst.nla != GameStartType.NLA.Load)
          return;
        NewGameManager.current.SetupLoadGame(this.gst.isPrometheus, this.gst.saveName);
      }
    }
    else if (scene.name == "SCENE - Main Menu")
    {
      this.gst = (GameStartType) null;
      this.skirmishMapName = SkirmishMapName.Earth;
    }
    else
    {
      if (!(scene.name == "SCENE - Tactical"))
        return;
      MonoBehaviour.print((object) ("STM: loading tactical scene. GST.nla: " + this.gst.nla.ToString()));
      if (this.gst.nla == GameStartType.NLA.Skirmish)
        return;
      if (this.gst.isPrometheus)
        this.gst = this.autosavePrometheusGST;
      else
        this.gst = this.autosaveCustomGST;
    }
  }

  private void GetDependencies()
  {
    this.mi = MapInputs.current;
    this.fm = FactionsManager.current;
    this.sls = SaveLoadSystem.current;
    this.epm = EncounterPanelManager.current;
    this.cm = CameraManager.current;
    this.GetLoadingScreen();
    this.td = TrackDisplayer.current;
    this.bodies = new List<BodyInfo>();
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Body"))
      this.bodies.Add(new BodyInfo(gameObject.transform));
  }

  private void SetBountyReward(TacticalGroupData bountyFleetTGD)
  {
    this.bountyRewardCredits = 0;
    foreach (TacticalShipData tacticalShipData in bountyFleetTGD.objects)
      this.bountyRewardCredits += tacticalShipData.bp.Value;
  }

  public void SetupEngagement(List<TacticalGroupData> newGroups, Vector3 encounterPos)
  {
    MonoBehaviour.print((object) "===== ENCOUNTER! =====");
    encounterPos.y = 0.0f;
    MonoBehaviour.print((object) $"groups: {newGroups.Count.ToString()} encounterPos: {encounterPos.ToString()}");
    if ((bool) (UnityEngine.Object) this.epm)
      this.epm.PromptOn(newGroups, encounterPos);
    Vector3 zero = Vector3.zero;
    this.isBountyHunt = false;
    foreach (TacticalGroupData newGroup in newGroups)
    {
      zero += newGroup.initPos;
      if (newGroup.factionID == 99)
      {
        this.isBountyHunt = true;
        this.SetBountyReward(newGroup);
      }
    }
    Vector3 vector3 = zero / (float) newGroups.Count;
    foreach (TacticalGroupData newGroup in newGroups)
    {
      newGroup.direction = (newGroup.initPos - vector3).normalized;
      newGroup.direction.y += UnityEngine.Random.Range(-0.2f, 0.21f);
      newGroup.direction = newGroup.direction.normalized;
      newGroup.spawnPos = newGroup.direction * 4900f;
    }
    this.currentGroups = newGroups;
    this.currentEncounterPos = encounterPos;
  }

  public void NewEngagement()
  {
    MonoBehaviour.print((object) "New engagement!");
    if ((bool) (UnityEngine.Object) this.epm)
      this.epm.PromptOff();
    if ((bool) (UnityEngine.Object) this.cm)
      this.cm.Zoom(1000);
    if ((bool) (UnityEngine.Object) this.mi)
      this.mi.isInputOn = false;
    this.ClearData();
    this.groups = this.currentGroups;
    this.StartEncounter(this.currentEncounterPos);
  }

  private void StartEncounter(Vector3 encounterPos)
  {
    MonoBehaviour.print((object) "start encounter");
    this.loadingScreen.Show();
    if ((bool) (UnityEngine.Object) this.sls)
      this.sls.SaveGame("AUTOSAVE");
    this.StartCoroutine((IEnumerator) this.StartEncounterDelayed(encounterPos));
  }

  private IEnumerator StartEncounterDelayed(Vector3 encounterPos)
  {
    yield return (object) new WaitForSeconds(1f);
    foreach (BodyInfo body in this.bodies)
    {
      body.distance = Vector3.Distance(encounterPos, body.transform.position) * 1000f;
      body.direction = (encounterPos - body.transform.position).normalized;
    }
    this.LoadScene("SCENE - Tactical");
  }

  public void ClearData() => this.groups.Clear();

  public void EndCombat()
  {
    MonoBehaviour.print((object) "===== END COMBAT");
    if (this.gst.nla == GameStartType.NLA.Skirmish)
    {
      this.LoadScene("SCENE - Main Menu");
    }
    else
    {
      this.LoadScene(this.strategicSceneName);
      this.StartCoroutine((IEnumerator) this.LoadStrategicSceneRoutine());
    }
  }

  private void GetLoadingScreen() => this.loadingScreen = UnityEngine.Object.FindObjectOfType<LoadingScreen>();

  private void LoadScene(string sceneName)
  {
    MonoBehaviour.print((object) ("loading scene: " + sceneName));
    this.StartCoroutine((IEnumerator) this.LoadSceneAsync(sceneName));
  }

  private IEnumerator LoadSceneAsync(string sceneName)
  {
    MonoBehaviour.print((object) ("loading scene async: " + sceneName));
    this.currentAsyncOp = SceneManager.LoadSceneAsync(sceneName);
    this.currentAsyncOp.allowSceneActivation = false;
    while (!this.currentAsyncOp.isDone)
    {
      float progress = Mathf.Clamp01(this.currentAsyncOp.progress / 0.9f);
      if ((UnityEngine.Object) this.loadingScreen == (UnityEngine.Object) null)
        this.GetLoadingScreen();
      if ((UnityEngine.Object) this.loadingScreen != (UnityEngine.Object) null)
        this.loadingScreen.UpdateProgress(progress);
      if ((double) this.currentAsyncOp.progress >= 0.89999997615814209)
        this.currentAsyncOp.allowSceneActivation = true;
      yield return (object) null;
    }
  }

  private IEnumerator LoadStrategicSceneRoutine()
  {
    MonoBehaviour.print((object) "load strategic scene routine");
    while (!this.currentAsyncOp.isDone)
      yield return (object) null;
    if ((UnityEngine.Object) this.sls == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) "SaveLoadSystem not found, waiting for it...");
      float timeout = 5f;
      while ((UnityEngine.Object) this.sls == (UnityEngine.Object) null && (double) timeout > 0.0)
      {
        this.sls = SaveLoadSystem.current;
        timeout -= Time.deltaTime;
        yield return (object) null;
      }
    }
    bool isAutosaveLoadDone = false;
    if ((UnityEngine.Object) this.sls != (UnityEngine.Object) null)
    {
      this.sls.UpdateGameName();
      isAutosaveLoadDone = this.sls.LoadAutoSave();
      NewGameManager.current.SetupAutosave(this.gst.isPrometheus, this.gst.saveName);
    }
    while (!isAutosaveLoadDone)
      yield return (object) null;
    if (this.winningGroup.factionID == 1)
    {
      this.isPlayerVictory = true;
      if (this.isBountyHunt)
        this.fm.ModPlayerCredits("BOUNTY RECEIVED", this.bountyRewardCredits);
    }
    else
      this.isPlayerVictory = false;
    foreach (TacticalGroupData group in this.groups)
    {
      Track currentTrack = (Track) null;
      foreach (Track allTrack in this.td.allTracks)
      {
        if (allTrack.id == group.trackID)
          currentTrack = allTrack;
      }
      if ((bool) (UnityEngine.Object) currentTrack)
      {
        if (group.trackID == this.winningGroup.trackID)
        {
          if (this.winningGroup.objects.Count == 0)
          {
            if ((UnityEngine.Object) currentTrack != (UnityEngine.Object) null)
              UnityEngine.Object.Destroy((UnityEngine.Object) currentTrack.gameObject);
          }
          else
          {
            currentTrack.GetComponent<FleetManager>().DestroyOldShips();
            yield return (object) null;
            currentTrack.GetComponent<FleetManager>().PopulateFleet(this.winningGroup.objects);
          }
        }
        else
        {
          if (currentTrack.transform.childCount > 0)
          {
            IEnumerator enumerator = (IEnumerator) currentTrack.transform.GetEnumerator();
            try
            {
              while (enumerator.MoveNext())
              {
                Transform current = (Transform) enumerator.Current;
                if ((bool) (UnityEngine.Object) current.GetComponent<FleetManager>())
                {
                  current.parent = (Transform) null;
                  if ((bool) (UnityEngine.Object) current.GetComponent<Target>())
                    current.GetComponent<Target>().enabled = true;
                }
              }
            }
            finally
            {
              if (enumerator is IDisposable disposable)
                disposable.Dispose();
            }
          }
          UnityEngine.Object.Destroy((UnityEngine.Object) currentTrack.gameObject);
        }
        currentTrack = (Track) null;
      }
    }
    this.ClearData();
  }
}
