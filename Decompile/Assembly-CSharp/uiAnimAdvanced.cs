// Decompiled with JetBrains decompiler
// Type: uiAnimAdvanced
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class uiAnimAdvanced : 
  MonoBehaviour,
  IPointerEnterHandler,
  IEventSystemHandler,
  IPointerExitHandler
{
  [Header("Colors")]
  public Color colorA = Color.white;
  public Color colorB = Color.red;
  [Header("Settings")]
  public float speed = 1f;
  private Image targetImage;
  [SerializeField]
  private uiAnimator textUIA;

  private void Start() => this.targetImage = this.GetComponent<Image>();

  private void Update()
  {
    if (!((Object) this.targetImage != (Object) null))
      return;
    this.targetImage.color = Color.Lerp(this.colorA, this.colorB, Mathf.PingPong(Time.time * this.speed, 1f));
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    if (!(bool) (Object) this.textUIA)
      return;
    this.textUIA.Show();
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (!(bool) (Object) this.textUIA)
      return;
    this.textUIA.Hide();
  }
}
