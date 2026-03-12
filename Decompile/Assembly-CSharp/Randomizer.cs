// Decompiled with JetBrains decompiler
// Type: Randomizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Text;
using TMPro;
using UnityEngine;

#nullable disable
public class Randomizer : MonoBehaviour
{
  [SerializeField]
  private string originalEncodedMessage;
  [Header("UI Components")]
  public TMP_Text targetText;

  private void Start() => this.Randomize();

  public void Randomize() => this.DisplayRandomizedMessage(this.originalEncodedMessage);

  public void DisplayRandomizedMessage(string encodedMessage)
  {
    StringBuilder stringBuilder = new StringBuilder();
    foreach (char c in encodedMessage)
    {
      if (c == '0')
        stringBuilder.Append('0');
      else if (char.IsDigit(c))
      {
        int num = Random.Range(1, 10);
        stringBuilder.Append(num.ToString());
      }
      else
        stringBuilder.Append(c);
    }
    if ((Object) this.targetText != (Object) null)
      this.targetText.text = stringBuilder.ToString();
    else
      Debug.LogWarning((object) "Target Text (TMP_Text) is not assigned!");
  }
}
