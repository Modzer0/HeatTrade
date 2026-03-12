// Decompiled with JetBrains decompiler
// Type: TacticalSceneLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TacticalSceneLoader : MonoBehaviour
{
  private SceneTransitionManager stm;
  public T_StructureLibrary tsl;
  public S_StructureLibrary ssl;
  private TacticalInputs ti;
  private TacticalCamera tc;
  private CombatManager cm;
  [SerializeField]
  private BodySpawner earth;
  [SerializeField]
  private BodySpawner luna;
  [SerializeField]
  private GameObject loadingScreenObject;
  [SerializeField]
  private uiAnimator loadingScreen;
  private Transform firstShip;
  [SerializeField]
  private bool isFocusOnStart;
  [SerializeField]
  private List<GameObject> hostileSquadrons = new List<GameObject>();

  private void Start()
  {
    MonoBehaviour.print((object) "===== TSL START =====");
    this.stm = SceneTransitionManager.current;
    this.ti = TacticalInputs.current;
    this.tc = TacticalCamera.current;
    this.cm = CombatManager.current;
    this.StartCoroutine((IEnumerator) this.FadeFromBlack());
    if ((UnityEngine.Object) this.stm == (UnityEngine.Object) null)
      MonoBehaviour.print((object) "no scene transition manager found!");
    else
      this.StartSetup();
  }

  private void StartSetup()
  {
    foreach (BodyInfo body in this.stm.bodies)
    {
      MonoBehaviour.print((object) $"body: {body.bodyName} dist: {body.distance.ToString()}");
      if (body.bodyName == "PLANET - Earth")
        this.earth.SetFromBodyInfo(body);
      else if (body.bodyName == "MOON - Luna")
        this.luna.SetFromBodyInfo(body);
    }
    int num1 = 0;
    foreach (TacticalGroupData group in this.stm.groups)
    {
      if (group.factionID != 1)
      {
        num1 = group.factionID;
        break;
      }
    }
    foreach (GameObject hostileSquadron in this.hostileSquadrons)
    {
      hostileSquadron.SetActive(true);
      hostileSquadron.GetComponent<Track>().factionID = num1;
      hostileSquadron.GetComponent<Track>().iff = IFF.HOSTILE;
    }
    foreach (TacticalGroupData group in this.stm.groups)
    {
      this.cm.AddFleet(group);
      int num2 = -1;
      float num3 = 4900f;
      int count = group.objects.Count;
      Vector3 vector3 = group.direction;
      Transform transform = this.transform;
      foreach (TacticalShipData tacticalShipData in group.objects)
      {
        ++num2;
        GameObject prefab = this.tsl.GetPrefab(tacticalShipData.bp);
        double armorThickness = (double) tacticalShipData.bp.ArmorThickness;
        if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
        {
          Debug.LogError((object) ("Missing prefab for: " + tacticalShipData.bp.GetFullClassName()));
        }
        else
        {
          Vector3 position = group.spawnPos;
          if (num2 > 0)
          {
            vector3 = (vector3 + transform.right * (float) num2 * 0.01f).normalized;
            position = vector3 * num3;
          }
          Quaternion normalized = Quaternion.LookRotation(position - Vector3.zero).normalized;
          GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, position, normalized);
          if ((UnityEngine.Object) transform == (UnityEngine.Object) this.transform)
            transform = gameObject.transform;
          ShipController component1 = gameObject.GetComponent<ShipController>();
          if ((bool) (UnityEngine.Object) gameObject.GetComponent<Track>())
          {
            Track component2 = gameObject.GetComponent<Track>();
            component2.factionID = tacticalShipData.factionID;
            component2.id = tacticalShipData.trackID;
            component2.publicName = tacticalShipData.publicName;
            component2.SetTrackName();
            if (component2.factionID == 1)
            {
              this.ti.AddPlayerShip(component1);
              this.cm.AddShip(true, component1);
            }
            else
              this.cm.AddShip(false, component1);
          }
          if ((bool) (UnityEngine.Object) gameObject.GetComponent<Pathfinder>())
          {
            Pathfinder component3 = gameObject.GetComponent<Pathfinder>();
            component3.InitThrust(tacticalShipData.cruiseAcceleration);
            component3.EntryBurn();
          }
          foreach (TacticalModuleData module in tacticalShipData.modules)
          {
            IEnumerator enumerator = (IEnumerator) gameObject.transform.GetEnumerator();
            try
            {
              while (enumerator.MoveNext())
              {
                Transform current = (Transform) enumerator.Current;
                if ((bool) (UnityEngine.Object) current.GetComponent<T_Module>() && !(bool) (UnityEngine.Object) current.GetComponent<T_Module>().BP)
                  Debug.LogWarning((object) $"!!! {current.name} NO BP!");
                if ((bool) (UnityEngine.Object) current.GetComponent<T_Module>() && (bool) (UnityEngine.Object) current.GetComponent<T_Module>().BP && current.GetComponent<T_Module>().BP.PrefabKey == module.bpKey)
                {
                  T_Module component4 = current.GetComponent<T_Module>();
                  component4.health = module.health;
                  component4.armorHealth = module.armorHealth;
                  component4.resource = module.resource;
                  component4.inventoryData = module.inventoryData;
                  component4.InitBP();
                  List<T_Mount> tMountList = new List<T_Mount>();
                  foreach (TacticalMountData mount1 in module.mounts)
                  {
                    component4.InitMounts();
                    foreach (T_Mount mount2 in component4.mounts)
                    {
                      if ((bool) (UnityEngine.Object) current.GetComponent<T_Mount>() && !(bool) (UnityEngine.Object) current.GetComponent<T_Mount>().BP)
                        MonoBehaviour.print((object) (current.name + " NO BP!"));
                      if (!tMountList.Contains(mount2) && mount2.BP.PrefabKey == mount1.bpKey)
                      {
                        mount2.health = mount1.health;
                        mount2.resource = mount1.resource;
                        tMountList.Add(mount2);
                        break;
                      }
                    }
                  }
                }
              }
            }
            finally
            {
              if (enumerator is IDisposable disposable)
                disposable.Dispose();
            }
          }
          if (tacticalShipData.factionID == 1 && group.objects.IndexOf(tacticalShipData) == 0)
            this.firstShip = gameObject.transform;
        }
      }
    }
    if (!this.isFocusOnStart)
      return;
    this.ti.isInputOn = false;
  }

  private IEnumerator FadeFromBlack()
  {
    if (this.isFocusOnStart)
    {
      float delayTime = 1f;
      yield return (object) new WaitForSeconds(delayTime);
      this.tc.SetFocus(this.firstShip, 1);
      yield return (object) new WaitForSeconds(delayTime);
      this.ti.SetSelectedShipIndex(0);
      this.tc.SetFocus(this.firstShip, 2);
      this.ti.isInputOn = true;
    }
    else
      this.ti.isInputOn = true;
  }
}
