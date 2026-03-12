// Decompiled with JetBrains decompiler
// Type: TooltipSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TooltipSystem : MonoBehaviour
{
  public static TooltipSystem current;
  public Tooltip tooltip;
  public uiAnimator tooltipAnim;

  private void Awake()
  {
    TooltipSystem.current = this;
    this.tooltipAnim = this.tooltip.GetComponent<uiAnimator>();
    this.Hide();
  }

  public void ShowDefault(string header, string content)
  {
    if (string.IsNullOrEmpty(header) || string.IsNullOrEmpty(content))
    {
      this.Hide();
    }
    else
    {
      this.tooltip.SetText(header, content);
      this.tooltipAnim.Show();
    }
  }

  public void ShowFuelBuy() => this.tooltip.SetTextFuelBuy();

  public void Hide() => this.tooltipAnim.Hide();
}
