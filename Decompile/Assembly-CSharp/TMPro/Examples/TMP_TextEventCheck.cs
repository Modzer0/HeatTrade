// Decompiled with JetBrains decompiler
// Type: TMPro.Examples.TMP_TextEventCheck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace TMPro.Examples;

public class TMP_TextEventCheck : MonoBehaviour
{
  public TMP_TextEventHandler TextEventHandler;
  private TMP_Text m_TextComponent;

  private void OnEnable()
  {
    if (!((Object) this.TextEventHandler != (Object) null))
      return;
    this.m_TextComponent = this.TextEventHandler.GetComponent<TMP_Text>();
    this.TextEventHandler.onCharacterSelection.AddListener(new UnityAction<char, int>(this.OnCharacterSelection));
    this.TextEventHandler.onSpriteSelection.AddListener(new UnityAction<char, int>(this.OnSpriteSelection));
    this.TextEventHandler.onWordSelection.AddListener(new UnityAction<string, int, int>(this.OnWordSelection));
    this.TextEventHandler.onLineSelection.AddListener(new UnityAction<string, int, int>(this.OnLineSelection));
    this.TextEventHandler.onLinkSelection.AddListener(new UnityAction<string, string, int>(this.OnLinkSelection));
  }

  private void OnDisable()
  {
    if (!((Object) this.TextEventHandler != (Object) null))
      return;
    this.TextEventHandler.onCharacterSelection.RemoveListener(new UnityAction<char, int>(this.OnCharacterSelection));
    this.TextEventHandler.onSpriteSelection.RemoveListener(new UnityAction<char, int>(this.OnSpriteSelection));
    this.TextEventHandler.onWordSelection.RemoveListener(new UnityAction<string, int, int>(this.OnWordSelection));
    this.TextEventHandler.onLineSelection.RemoveListener(new UnityAction<string, int, int>(this.OnLineSelection));
    this.TextEventHandler.onLinkSelection.RemoveListener(new UnityAction<string, string, int>(this.OnLinkSelection));
  }

  private void OnCharacterSelection(char c, int index)
  {
    Debug.Log((object) $"Character [{c.ToString()}] at Index: {index.ToString()} has been selected.");
  }

  private void OnSpriteSelection(char c, int index)
  {
    Debug.Log((object) $"Sprite [{c.ToString()}] at Index: {index.ToString()} has been selected.");
  }

  private void OnWordSelection(string word, int firstCharacterIndex, int length)
  {
    Debug.Log((object) $"Word [{word}] with first character index of {firstCharacterIndex.ToString()} and length of {length.ToString()} has been selected.");
  }

  private void OnLineSelection(string lineText, int firstCharacterIndex, int length)
  {
    Debug.Log((object) $"Line [{lineText}] with first character index of {firstCharacterIndex.ToString()} and length of {length.ToString()} has been selected.");
  }

  private void OnLinkSelection(string linkID, string linkText, int linkIndex)
  {
    if ((Object) this.m_TextComponent != (Object) null)
    {
      ref TMP_LinkInfo local = ref this.m_TextComponent.textInfo.linkInfo[linkIndex];
    }
    Debug.Log((object) $"Link Index: {linkIndex.ToString()} with ID [{linkID}] and Text \"{linkText}\" has been selected.");
  }
}
