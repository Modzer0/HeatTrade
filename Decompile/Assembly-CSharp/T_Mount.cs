// Decompiled with JetBrains decompiler
// Type: T_Mount
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class T_Mount : MonoBehaviour, IHealth
{
  [Header("Blueprint")]
  [SerializeField]
  private MountBP bp;
  [Header("INFO")]
  public string prefabName;
  public string productName;
  public PartType partType;
  public SizeClass sizeClass;
  [Header("DATA")]
  public float health;
  public float healthMax;
  public float healthRatio;
  public float resource;
  public int resourceMax;
  public int value;
  public float mass;
  [Header("PARENT")]
  public int factionID;
  private T_Module parentModule;
  private ShipController parentShip;
  public Transform parentShipTransform;
  private Track parentShipTrack;
  [Header("POWER AND HEAT")]
  public bool isOn = true;
  public bool isDead;
  public float power;
  public float heat;
  public float currentPower;
  public float currentHeat;
  public HeatClass heatClass;
  [Header("HEAT FX")]
  [SerializeField]
  private float intensity = 3f;
  [SerializeField]
  private Color heatColor;
  private Color orange;
  [Header("DAMAGE FX")]
  public Renderer moduleRenderer;
  public Material material;

  public MountBP BP => this.bp;

  public virtual void Start()
  {
    this.healthMax = 100f;
    this.healthRatio = Mathf.Clamp01(this.health / this.healthMax);
    if (!(bool) (Object) this.moduleRenderer)
      this.moduleRenderer = this.GetComponent<Renderer>();
    if ((bool) (Object) this.moduleRenderer)
      this.material = this.moduleRenderer.material;
    if ((bool) (Object) this.material)
      this.material.SetVector("_TileScale", (Vector4) this.transform.localScale);
    this.orange = new Color(1f, 0.098f, 0.0f, 1f);
    this.parentModule = this.GetComponentInParent<T_Module>();
    this.parentShip = this.GetComponentInParent<ShipController>();
    this.parentShipTransform = this.parentShip.transform;
    this.parentShipTrack = this.parentShip.GetComponent<Track>();
    this.factionID = this.parentShipTrack.factionID;
  }

  public virtual void Update()
  {
    if ((double) this.healthRatio > 0.0)
      return;
    this.isOn = false;
  }

  public TacticalMountData ToTacticalData()
  {
    return new TacticalMountData()
    {
      bpKey = this.bp.PrefabKey,
      productName = this.productName,
      health = this.health,
      resource = this.resource
    };
  }

  public float GetHealth() => this.health;

  public bool TryDamageKinetic(float relativeMagnitude)
  {
    relativeMagnitude = Mathf.Abs(relativeMagnitude);
    this.ModifyHealth(-relativeMagnitude);
    return true;
  }

  public void TryDamagePhotonic(float tryDamage)
  {
    if ((bool) (Object) this.parentShip && this.parentShip.isDead)
      return;
    float num = 1f / 1000f;
    MonoBehaviour.print((object) $"{this.name} mount p damage: {tryDamage.ToString()}");
    this.ModifyHealth(-tryDamage * num);
  }

  public void ModifyHealth(float mod)
  {
    this.health += mod;
    if ((double) this.health > (double) this.healthMax)
      this.health = this.healthMax;
    else if ((double) this.health < 0.0)
    {
      this.health = 0.0f;
      if ((bool) (Object) this.parentModule)
        this.parentModule.ModifyHealth(mod * 0.01f);
      if (!this.isDead)
        this.SetIsDead();
    }
    this.healthRatio = (double) this.healthMax > 0.0 ? Mathf.Clamp01(this.health / this.healthMax) : 0.0f;
    if (!(bool) (Object) this.material)
      return;
    this.material.SetFloat("_HealthRatio", this.healthRatio);
  }

  public virtual void SetIsDead() => this.isDead = true;
}
