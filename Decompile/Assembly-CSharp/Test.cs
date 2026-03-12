// Decompiled with JetBrains decompiler
// Type: Test
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Test : MonoBehaviour
{
  private Pathfinder pf;
  [SerializeField]
  private Transform target;
  [SerializeField]
  private bool isShip;
  [SerializeField]
  private bool isFaceTarget;
  [SerializeField]
  private bool isMissileBay;
  [Space(10f)]
  [Header("HEALTH")]
  [SerializeField]
  private ParticleSystem explosionPS;
  private MissileSystem ms;
  [SerializeField]
  private pdcSystem pdcsys;

  private void Start()
  {
    this.pf = this.GetComponent<Pathfinder>();
    if (!this.isMissileBay)
      return;
    this.ms = this.GetComponent<MissileSystem>();
    this.ms.NewTarget(this.target.GetComponent<Track>());
    this.GetComponent<MissileGuidance>().SetTarget(this.target);
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Alpha9))
    {
      this.pf.targetPos = this.target.position;
      this.pf.TrySetState(PathfinderState.MOVE);
    }
    if (Input.GetKeyDown(KeyCode.Alpha7))
    {
      this.pf.isRCSOnly = false;
      this.pf.targetPos = this.target.position;
      this.pf.TrySetState(PathfinderState.MOVE);
    }
    if (!this.isShip || !this.isFaceTarget)
      return;
    this.FaceTarget();
  }

  private void FaceTarget()
  {
    this.pf.targetPos = this.target.position;
    this.pf.TrySetState(PathfinderState.FACE);
  }
}
