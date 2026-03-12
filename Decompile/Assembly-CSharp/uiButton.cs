// Decompiled with JetBrains decompiler
// Type: uiButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class uiButton : MonoBehaviour
{
  [SerializeField]
  private Vector3 onClickPos;
  [SerializeField]
  private Vector3 noClickPos;
  private bool isClick;

  private void Update()
  {
    if (this.isClick)
      this.transform.localPosition = this.onClickPos;
    else
      this.transform.localPosition = this.noClickPos;
    this.isClick = false;
  }

  public void OnClick() => this.isClick = true;
}
