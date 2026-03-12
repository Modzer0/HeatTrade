// Decompiled with JetBrains decompiler
// Type: CommandTooltip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class CommandTooltip : MonoBehaviour
{
  public static CommandTooltip current;
  private bool isOn;
  private bool isOnTheLeft;
  private Vector2 targetPos;
  [SerializeField]
  private uiAnimator uia;
  [SerializeField]
  private RectTransform rectTransform;
  [SerializeField]
  private TMP_Text commandText;
  [SerializeField]
  private TMP_Text shipText;

  private void Awake() => CommandTooltip.current = this;

  private void Start() => this.rectTransform = this.uia.GetComponent<RectTransform>();

  private void Update()
  {
    if (!this.isOn)
      return;
    this.targetPos = (Vector2) Input.mousePosition;
    this.isOnTheLeft = false;
    if ((double) this.targetPos.x - (double) this.rectTransform.rect.width < 32.0)
      this.isOnTheLeft = true;
    this.targetPos = !this.isOnTheLeft ? new Vector2(this.targetPos.x - 8f, this.targetPos.y) : new Vector2((float) ((double) this.targetPos.x + (double) this.rectTransform.rect.width + 8.0), this.targetPos.y);
    this.rectTransform.position = (Vector3) this.targetPos;
  }

  public void Set(bool newIsOn, string command, string ship)
  {
    this.isOn = newIsOn;
    if (this.isOn)
      this.uia.Show();
    else
      this.uia.Hide();
    this.commandText.text = command;
    this.shipText.text = ship;
  }
}
