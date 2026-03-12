// Decompiled with JetBrains decompiler
// Type: TerminalTyper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using TMPro;
using UnityEngine;

#nullable disable
public class TerminalTyper : MonoBehaviour
{
  [TextArea(3, 10)]
  public string fullText;
  public float charDelay = 0.03f;
  public bool flickerCursor = true;
  public char cursorChar = '_';
  public AudioSource typingSound;
  [SerializeField]
  private TMP_Text tmpText;
  private string currentText = "";
  private bool typingFinished;
  private Coroutine typingRoutine;

  public event Action OnTypingFinished;

  private void Update()
  {
    if (this.typingFinished || !Input.GetKeyDown(KeyCode.Return))
      return;
    this.FinishTyping();
  }

  public void TypeCurrent() => this.StartCoroutine((IEnumerator) this.TypeText());

  public void TypeThis(string toType)
  {
    if (!this.typingFinished && this.typingRoutine != null)
      this.StopCoroutine(this.typingRoutine);
    this.fullText = toType;
    this.tmpText.text = "";
    this.typingRoutine = this.StartCoroutine((IEnumerator) this.TypeText());
  }

  public IEnumerator TypeText()
  {
    this.typingFinished = false;
    this.currentText = "";
    this.tmpText.text = "";
    string str = this.fullText;
    for (int index = 0; index < str.Length; ++index)
    {
      this.currentText += str[index].ToString();
      this.tmpText.text = this.currentText;
      if ((bool) (UnityEngine.Object) this.typingSound)
        this.typingSound.Play();
      yield return (object) new WaitForSeconds(this.charDelay);
    }
    str = (string) null;
    this.FinishTyping();
  }

  private void FinishTyping()
  {
    this.typingFinished = true;
    this.tmpText.text = this.fullText;
    Action onTypingFinished = this.OnTypingFinished;
    if (onTypingFinished != null)
      onTypingFinished();
    if (this.typingRoutine == null)
      return;
    this.StopCoroutine(this.typingRoutine);
  }

  private IEnumerator CursorBlink()
  {
    while (true)
    {
      while (!this.typingFinished || !this.flickerCursor)
        yield return (object) null;
      this.tmpText.text = this.currentText;
      yield return (object) new WaitForSeconds(0.3f);
      this.tmpText.text = this.currentText;
      yield return (object) new WaitForSeconds(0.3f);
    }
  }
}
