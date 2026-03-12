// Decompiled with JetBrains decompiler
// Type: uiText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class uiText : MonoBehaviour
{
  [SerializeField]
  private TMP_Text myText;
  [SerializeField]
  private string ender = "";

  private void Start()
  {
  }

  private void Update()
  {
  }

  public void SetText(string newText) => this.myText.text = newText;

  public void SliderToText()
  {
    this.myText.text = this.GetComponent<Slider>().value.ToString() + this.ender;
  }
}
