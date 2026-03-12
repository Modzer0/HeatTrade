// Decompiled with JetBrains decompiler
// Type: Indicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class Indicator : 
  MonoBehaviour,
  IPointerEnterHandler,
  IEventSystemHandler,
  IPointerClickHandler,
  IPointerExitHandler
{
  private TacticalInputs ti;
  private PlayerFleetUI pfui;
  public IndicatorType indicatorType;
  public Image indicatorImage;
  [SerializeField]
  private TMP_Text nameText;
  [SerializeField]
  private TMP_Text distText;
  public Target target;
  public Track track;
  private bool isTactical;
  public Color green;
  public Color yellow;
  public Color red;

  public bool Active => this.transform.gameObject.activeInHierarchy;

  public IndicatorType Type => this.indicatorType;

  private void Awake() => this.indicatorImage = this.transform.GetComponent<Image>();

  private void Start()
  {
    this.ti = TacticalInputs.current;
    this.isTactical = true;
    this.pfui = PlayerFleetUI.current;
    if (!((Object) this.ti == (Object) null))
      return;
    this.isTactical = false;
  }

  public void SetSourceImg(Sprite sprite) => this.indicatorImage.sprite = sprite;

  public void SetImageColor(Color color) => this.indicatorImage.color = color;

  public void SetNameText(string newText)
  {
    this.nameText.gameObject.SetActive(true);
    this.nameText.text = newText;
  }

  public void SetDistText(float newDist)
  {
    if (this.target.isOwned)
      this.distText.gameObject.SetActive(false);
    else
      this.distText.gameObject.SetActive(true);
    if ((double) newDist == 0.0)
    {
      this.distText.text = "";
      this.distText.gameObject.SetActive(false);
    }
    else
    {
      this.distText.gameObject.SetActive(true);
      if (this.isTactical)
      {
        newDist /= 1000f;
        this.distText.text = (double) newDist <= 1.0 ? newDist.ToString("F3") + "km" : newDist.ToString("F2") + "km";
        if ((double) newDist >= 5000.0)
          this.distText.color = Color.gray;
        else if ((double) newDist >= 1000.0)
          this.distText.color = Color.white;
        else if ((double) newDist >= 500.0)
          this.distText.color = this.green;
        else if ((double) newDist >= 100.0)
          this.distText.color = this.yellow;
        else
          this.distText.color = this.red;
      }
      else
      {
        this.distText.text = newDist.ToString("F2") + "Kkm";
        if ((double) newDist >= 50.0)
          this.distText.color = Color.gray;
        else if ((double) newDist >= 10.0)
          this.distText.color = Color.white;
        else if ((double) newDist >= 5.0)
          this.distText.color = this.green;
        else if ((double) newDist >= 1.0)
          this.distText.color = this.yellow;
        else
          this.distText.color = this.red;
      }
    }
  }

  public void SetColor(Color color, bool isDead)
  {
    if (isDead)
      color = new Color(color.r, color.g, color.b, 0.5f);
    this.nameText.color = color;
    this.indicatorImage.color = color;
  }

  public void SetTarget(Target target)
  {
    if ((Object) this.target == (Object) target)
      return;
    this.target = target;
    this.track = target.GetComponent<Track>();
  }

  public void SetTextRotation(Quaternion rotation)
  {
  }

  public void Activate(bool value) => this.transform.gameObject.SetActive(value);

  public void PrintName()
  {
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    if (this.isTactical)
    {
      if ((Object) this.target == (Object) null)
        return;
      this.ti.tempTarget = this.track;
    }
    else
    {
      if (!(bool) (Object) this.target || this.target.GetComponent<IClickable>() == null)
        return;
      this.target.GetComponent<IClickable>().OnEnter();
    }
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    if (!this.isTactical || (Object) this.target == (Object) null || !(bool) (Object) this.target.GetComponent<Track>())
      return;
    if (this.target.GetComponent<Track>().iff == IFF.HOSTILE)
    {
      this.ti.SetTarget(this.track, false);
    }
    else
    {
      if (this.target.GetComponent<Track>().iff != IFF.OWNED)
        return;
      if ((bool) (Object) this.target.GetComponent<ShipController>())
      {
        this.ti.SelectNew(this.target.GetComponent<ShipController>());
      }
      else
      {
        if (!(bool) (Object) this.target.GetComponent<Squadron>())
          return;
        this.ti.SelectNew(this.target.GetComponent<Squadron>());
      }
    }
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (this.isTactical)
    {
      if ((Object) this.target == (Object) null)
        return;
      this.ti.tempTarget = (Track) null;
    }
    else
    {
      if (!(bool) (Object) this.target || this.target.GetComponent<IClickable>() == null)
        return;
      this.target.GetComponent<IClickable>().OnExit();
    }
  }
}
