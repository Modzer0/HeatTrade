// Decompiled with JetBrains decompiler
// Type: ShipController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Track))]
[RequireComponent(typeof (Rigidbody))]
public class ShipController : MonoBehaviour
{
  [Header("BLUEPRINT")]
  [SerializeField]
  private bool isUseBlueprint;
  [SerializeField]
  private ShipBP bp;
  [Header("SHIP DATA")]
  public string className;
  public string classType;
  public Track track;
  public string status;
  public bool isSelectable = true;
  [Header("LISTS")]
  public List<WeaponSystem> weapons;
  public List<DockingPoint> dockingPoints;
  public List<T_Module> allModules;
  [Header("SYSTEMS")]
  public Track currentTarget;
  public Pathfinder pf;
  private PowerManager pm;
  private HeatManager hm;
  private AudioManager am;
  public float totalHealthRatio;
  public float healthDeathThresh;
  [Header("MODULES")]
  public float armorThickness;
  public T_Drive drive;
  public List<T_CrewModule> crews = new List<T_CrewModule>();
  public Transform scopeSpot;
  [SerializeField]
  private bool canDock;
  [Header("DAMAGE CONTROL")]
  private int dcTeamsFree;
  private int dcTeamsBusy;
  private int materials;
  private int materialsMax;
  [Header("DEATH")]
  [SerializeField]
  private int hardDeathChance = 10;
  [SerializeField]
  private bool isImmortal;
  public bool isTakeDamage = true;
  [SerializeField]
  private ParticleSystem deathPS;
  public bool isDead;
  private TacticalShipAudio tsa;
  private AudioSource ambienceSource;
  private AudioClip healthyAmbience;
  private AudioClip damagedAmbience;
  private AudioClip destroyedAmbience;

  public event Action<int, ShipController, bool> OnShipDeath;

  private void Awake()
  {
    this.track = this.GetComponent<Track>();
    this.pf = this.GetComponent<Pathfinder>();
    this.pm = this.GetComponent<PowerManager>();
    this.hm = this.GetComponent<HeatManager>();
    if ((UnityEngine.Object) this.GetComponentInChildren<ScopeSpot>() != (UnityEngine.Object) null)
      this.scopeSpot = this.GetComponentInChildren<ScopeSpot>().transform;
    this.armorThickness = 0.0f;
    if ((bool) (UnityEngine.Object) this.bp)
      this.armorThickness = this.bp.ArmorThickness;
    if (this.isUseBlueprint)
      this.pf.maxThrust = this.bp.CruiseAccMps;
    foreach (IInitable componentsInChild in this.transform.GetComponentsInChildren<IInitable>())
      componentsInChild.Init(this.track, this.GetComponent<Rigidbody>());
    foreach (Transform componentsInChild in this.transform.GetComponentsInChildren<Transform>())
    {
      if ((bool) (UnityEngine.Object) componentsInChild.GetComponent<T_Drive>())
        this.drive = componentsInChild.GetComponent<T_Drive>();
      if ((bool) (UnityEngine.Object) componentsInChild.GetComponent<WeaponSystem>())
        this.weapons.Add(componentsInChild.GetComponent<WeaponSystem>());
      if ((bool) (UnityEngine.Object) componentsInChild.GetComponent<DockingPoint>())
        this.dockingPoints.Add(componentsInChild.GetComponent<DockingPoint>());
      if ((bool) (UnityEngine.Object) componentsInChild.GetComponent<T_Module>())
      {
        this.allModules.Add(componentsInChild.GetComponent<T_Module>());
        componentsInChild.GetComponent<T_Module>().armorThickness = this.armorThickness;
      }
      if ((bool) (UnityEngine.Object) componentsInChild.GetComponent<Track>())
        componentsInChild.GetComponent<Track>().factionID = this.track.factionID;
      if ((bool) (UnityEngine.Object) componentsInChild.GetComponent<T_CrewModule>())
      {
        T_CrewModule component = componentsInChild.GetComponent<T_CrewModule>();
        this.crews.Add(component);
        this.dcTeamsFree += component.dcTeams;
        this.materials += component.materials;
        this.materialsMax += component.materials;
      }
    }
  }

  private void Start()
  {
    if ((bool) (UnityEngine.Object) this.pm)
      this.pm.Init(this.allModules);
    if ((bool) (UnityEngine.Object) this.hm)
      this.StartCoroutine((IEnumerator) this.LateHMInit());
    this.am = AudioManager.current;
    this.ambienceSource = this.GetComponentInChildren<AudioSource>();
    this.totalHealthRatio = 1f;
    this.healthDeathThresh = 0.2f;
    this.StartCoroutine((IEnumerator) this.CheckHealthCycle());
    this.StartCoroutine((IEnumerator) this.DCCycle());
    this.StartAmbience();
  }

  private void StartAmbience()
  {
    this.tsa = TacticalShipAudio.current;
    if (!(bool) (UnityEngine.Object) this.tsa)
      return;
    this.healthyAmbience = this.tsa.healthyAmbience;
    this.damagedAmbience = this.tsa.damagedAmbience;
    this.destroyedAmbience = this.tsa.destroyedAmbience;
  }

  private void SetAmbienceBasedOnHealth()
  {
    if (!(bool) (UnityEngine.Object) this.tsa)
      return;
    AudioClip clip1 = this.ambienceSource.clip;
    this.ambienceSource.clip = (double) this.totalHealthRatio >= 0.20000000298023224 ? ((double) this.totalHealthRatio >= 0.60000002384185791 ? this.healthyAmbience : this.damagedAmbience) : this.destroyedAmbience;
    AudioClip clip2 = this.ambienceSource.clip;
    if ((UnityEngine.Object) clip1 == (UnityEngine.Object) clip2)
      return;
    this.ambienceSource.Stop();
    this.ambienceSource.Play();
    this.ambienceSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
  }

  private IEnumerator LateHMInit()
  {
    yield return (object) new WaitForSeconds(0.5f);
    this.hm.Init(this.allModules);
  }

  private void Update()
  {
    float f = this.pf.currentThrust / this.pf.targetThrust;
    if (float.IsNaN(f) || float.IsInfinity(f))
      f = 0.0f;
    this.drive.currentHeat = this.drive.heat * f;
    this.DeadCheck();
  }

  private IEnumerator CheckHealthCycle()
  {
    yield return (object) new WaitForSeconds(1f);
    while (true)
    {
      this.CheckTotalHealth();
      this.BorderCheck();
      this.SetAmbienceBasedOnHealth();
      yield return (object) new WaitForSeconds(1f);
    }
  }

  private void CheckTotalHealth()
  {
    this.totalHealthRatio = 0.0f;
    foreach (T_Module allModule in this.allModules)
      this.totalHealthRatio += allModule.healthRatio;
    this.totalHealthRatio /= (float) this.allModules.Count;
    if ((double) this.totalHealthRatio >= (double) this.healthDeathThresh || this.isDead)
      return;
    this.SetIsDead();
  }

  private void DeadCheck()
  {
    if (this.isDead || (double) this.totalHealthRatio >= (double) this.healthDeathThresh)
      return;
    this.SetIsDead();
  }

  private void SetIsDead()
  {
    if (this.isImmortal)
      return;
    this.isDead = true;
    this.pf.tp.Clear();
    this.pf.isShowMoveLine = false;
    this.Death();
  }

  private void Death()
  {
    if (UnityEngine.Random.Range(0, 100) <= this.hardDeathChance)
      this.HardDeath();
    else
      this.SoftDeath();
  }

  private void BorderCheck()
  {
    if ((double) this.transform.position.sqrMagnitude <= 25100100.0 || this.isDead)
      return;
    this.SetIsDead();
  }

  public void NewTarget(Track newTarget)
  {
    if (this.isDead)
      return;
    this.currentTarget = newTarget;
    foreach (WeaponSystem weapon in this.weapons)
    {
      if ((bool) (UnityEngine.Object) weapon.GetComponent<MissileSystem>())
        weapon.GetComponent<MissileSystem>().NewTarget(newTarget);
    }
  }

  public void Move(Vector3 pos)
  {
    if (this.isDead)
      return;
    pos = Vector3.ClampMagnitude(pos, 4950f);
    this.pf.targetPos = pos;
    this.pf.TrySetState(PathfinderState.MOVE);
  }

  public void DockAt(DockingPoint dp)
  {
    if (this.isDead || (UnityEngine.Object) dp == (UnityEngine.Object) null || !this.canDock)
      return;
    this.pf.dockTarget = dp;
    this.pf.TrySetState(PathfinderState.DOCK);
  }

  public void ToggleRadiators()
  {
    if (this.isDead)
      return;
    this.hm.ToggleRadiators();
  }

  public Transform GetModuleRandom()
  {
    if (this.allModules == null || this.allModules.Count == 0)
      return (Transform) null;
    return 0 < this.allModules.Count ? this.allModules[UnityEngine.Random.Range(0, this.allModules.Count)].transform : this.transform;
  }

  public List<TacticalModuleData> GetTacticalModuleData()
  {
    List<TacticalModuleData> tacticalModuleData = new List<TacticalModuleData>();
    foreach (T_Module allModule in this.allModules)
      tacticalModuleData.Add(allModule.GetComponent<T_Module>().ToTacticalData());
    return tacticalModuleData;
  }

  public void ApplyDamageRandom(float damage)
  {
    this.CheckTotalHealth();
    this.DeadCheck();
    if (this.isDead && (double) this.totalHealthRatio <= 0.0099999997764825821)
      return;
    int index = UnityEngine.Random.Range(0, this.allModules.Count);
    if ((double) this.allModules[index].healthRatio <= 0.0)
      return;
    this.allModules[index].ModifyHealth(damage);
  }

  public void TryAddDCTo(T_Module module)
  {
    if ((UnityEngine.Object) module == (UnityEngine.Object) null || this.materials <= 0)
      this.am.PlaySFX(5);
    else if (module.isBeingFixed && this.dcTeamsBusy > 0)
    {
      module.isBeingFixed = false;
      this.DCTeamFreed();
    }
    else if (!module.isBeingFixed && this.dcTeamsFree > 0)
    {
      module.isBeingFixed = true;
      --this.dcTeamsFree;
      ++this.dcTeamsBusy;
      this.am.PlaySFX(4);
    }
    else
      this.am.PlaySFX(5);
  }

  private IEnumerator DCCycle()
  {
    while (true)
    {
      foreach (T_Module allModule in this.allModules)
      {
        if (allModule.isBeingFixed)
        {
          if ((double) allModule.health < 100.0 && this.materials > 0)
          {
            if ((double) allModule.health <= 0.0)
            {
              allModule.isOn = true;
              allModule.isDead = false;
            }
            allModule.ModifyHealth(10f);
            foreach (T_Mount mount in allModule.mounts)
            {
              if ((double) mount.health < 100.0)
              {
                if ((double) mount.health <= 0.0)
                {
                  mount.isOn = true;
                  allModule.isDead = false;
                }
                mount.ModifyHealth(10f);
              }
            }
            --this.materials;
          }
          else if ((double) allModule.health >= 100.0)
          {
            allModule.isBeingFixed = false;
            this.DCTeamFreed();
          }
        }
      }
      if (this.materials > 0)
        yield return (object) new WaitForSeconds(1f);
      else
        break;
    }
    foreach (T_Module allModule in this.allModules)
    {
      if (allModule.isBeingFixed)
      {
        allModule.isBeingFixed = false;
        this.DCTeamFreed();
      }
    }
  }

  public void DCTeamFreed()
  {
    ++this.dcTeamsFree;
    --this.dcTeamsBusy;
    this.am.PlaySFX(5);
  }

  public void DCTeamKilled() => --this.dcTeamsBusy;

  public int GetFreeDC() => this.dcTeamsFree;

  public float GetMaterialsRatio()
  {
    return this.materialsMax == 0 ? 0.0f : (float) this.materials / (float) this.materialsMax;
  }

  public void SoftDeath()
  {
    this.KillAllModules();
    this.OnDeath(false);
  }

  public void HardDeath()
  {
    this.KillAllModules();
    this.ExplodeAndDisintegrate();
    this.OnDeath(true);
  }

  private void OnDeath(bool isHardDeath)
  {
    Action<int, ShipController, bool> onShipDeath = this.OnShipDeath;
    if (onShipDeath == null)
      return;
    onShipDeath(this.track.factionID, this, isHardDeath);
  }

  private void KillAllModules()
  {
    foreach (T_Module allModule in this.allModules)
    {
      allModule.ModifyHealth(-allModule.health);
      if (allModule.mounts.Count > 0)
      {
        foreach (T_Mount mount in allModule.mounts)
          mount.ModifyHealth(-mount.health);
      }
    }
  }

  private void ExplodeAndDisintegrate()
  {
    ParticleSystem particleSystem = UnityEngine.Object.Instantiate<ParticleSystem>(this.deathPS, this.transform.position, this.transform.rotation);
    particleSystem.gameObject.SetActive(true);
    particleSystem.GetComponent<Projectile>().velocity = this.GetComponent<Rigidbody>().velocity;
    particleSystem.Play();
    this.gameObject.SetActive(false);
  }

  public void SetImmortality(bool isOn) => this.isImmortal = isOn;
}
