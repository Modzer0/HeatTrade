// Decompiled with JetBrains decompiler
// Type: Weapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Weapon : MonoBehaviour
{
  public string weaponName;
  public string position;
  public string status;
  public string targetName;
  private Target target;
  public int ammoCount;
  public int maxRange;
  private Turret turret;
  [SerializeField]
  private int cycleTime;
  [SerializeField]
  private int cycleTimeLeft;
  [SerializeField]
  private ParticleSystem pShooter;
  [Header("AUDIO")]
  [SerializeField]
  private bool isAudioLoop;
  private AudioSource audioSource;
  private float lastFireTime;

  private void Start()
  {
    if ((bool) (Object) this.GetComponent<Turret>())
      this.turret = this.GetComponent<Turret>();
    this.audioSource = this.GetComponent<AudioSource>();
  }

  private void Update()
  {
    if (this.status == "Firing" && !this.IsFacingTarget())
    {
      this.status = "Turning";
    }
    else
    {
      if (!(this.status == "Turning") || !this.IsFacingTarget())
        return;
      this.status = "Firing";
    }
  }

  private bool IsFacingTarget()
  {
    int layerMask = 1 << LayerMask.NameToLayer("Trackable") | 1 << LayerMask.NameToLayer("Obstacle");
    Debug.DrawRay(this.pShooter.transform.position, this.pShooter.transform.forward * (float) this.maxRange, Color.red, 0.1f);
    RaycastHit hitInfo;
    if (Physics.Raycast(this.pShooter.transform.position, this.pShooter.transform.forward, out hitInfo, (float) this.maxRange, layerMask))
    {
      if (hitInfo.collider.gameObject.GetComponent<ITrackable>() != null && hitInfo.collider.gameObject.GetComponent<ITrackable>().GetName() == this.target.GetComponent<ITrackable>().GetName())
        return true;
      if (this.isAudioLoop)
        this.audioSource.Stop();
    }
    return false;
  }

  private void FixedUpdate()
  {
    if (this.status == "Firing")
    {
      this.FireProjectile();
    }
    else
    {
      if (this.audioSource.isPlaying && this.isAudioLoop)
        this.audioSource.Stop();
      if (this.status == "Cycling")
      {
        --this.cycleTimeLeft;
        if (this.cycleTimeLeft <= 0)
          this.status = "Firing";
      }
    }
    if (!(bool) (Object) this.target)
      return;
    int num = (bool) (Object) this.pShooter ? 1 : 0;
  }

  private void FireProjectile()
  {
    if (this.ammoCount > 0)
    {
      this.pShooter.Play();
      --this.ammoCount;
    }
    if (this.cycleTime > 0)
    {
      this.status = "Cycling";
      this.cycleTimeLeft = this.cycleTime;
    }
    if ((!this.isAudioLoop || this.audioSource.isPlaying) && this.isAudioLoop)
      return;
    this.audioSource.pitch = Random.Range(0.75f, 1.25f);
    this.audioSource.Play();
  }

  public void SetTarget(Target newTarget)
  {
    this.target = newTarget;
    if ((Object) newTarget == (Object) null)
    {
      this.targetName = "None";
      this.status = "Idle";
      if (!(bool) (Object) this.turret)
        return;
      this.turret.SetTarget((Transform) null);
    }
    else
    {
      this.targetName = this.target.targetName;
      this.status = "Turning";
      if (!(bool) (Object) this.turret)
        return;
      this.turret.SetTarget(newTarget.transform);
    }
  }

  public void CeaseFire()
  {
    this.SetTarget((Target) null);
    this.status = "Idle";
  }
}
