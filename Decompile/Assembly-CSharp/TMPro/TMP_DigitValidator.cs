// Decompiled with JetBrains decompiler
// Type: TMPro.TMP_DigitValidator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace TMPro;

[Serializable]
public class TMP_DigitValidator : TMP_InputValidator
{
  public override char Validate(ref string text, ref int pos, char ch)
  {
    if (ch < '0' || ch > '9')
      return char.MinValue;
    text += ch.ToString();
    ++pos;
    return ch;
  }
}
