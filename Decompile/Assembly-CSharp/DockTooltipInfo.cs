// Decompiled with JetBrains decompiler
// Type: DockTooltipInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class DockTooltipInfo : 
  MonoBehaviour,
  IPointerEnterHandler,
  IEventSystemHandler,
  IPointerExitHandler
{
  private TooltipSystem ts;
  private LTDescr delayTween;
  private Structure ship;
  [SerializeField]
  private DockTooltipType type = DockTooltipType.REFUEL;
  private bool isHide;

  private void Start() => this.ts = TooltipSystem.current;

  private void Update()
  {
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (this.delayTween != null)
    {
      LeanTween.cancel(this.delayTween.uniqueId);
      this.delayTween = (LTDescr) null;
    }
    this.ts.Hide();
  }
}
