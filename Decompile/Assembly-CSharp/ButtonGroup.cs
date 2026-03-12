// Decompiled with JetBrains decompiler
// Type: ButtonGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#nullable disable
public class ButtonGroup : 
  MonoBehaviour,
  IPointerEnterHandler,
  IEventSystemHandler,
  IPointerExitHandler
{
  private TooltipSystem ts;
  private bool isInitialized;
  [SerializeField]
  private string header;
  [SerializeField]
  [TextArea]
  private string description;
  private int currentIndex;
  [SerializeField]
  private List<GameObject> buttons;
  [SerializeField]
  private UnityEvent<int> onInvokeFunction;

  private void Start() => this.Init();

  private void Init()
  {
    if (this.isInitialized)
      return;
    this.ts = TooltipSystem.current;
    this.buttons.Clear();
    IEnumerator enumerator = (IEnumerator) this.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if (!this.buttons.Contains(current.gameObject))
          this.buttons.Add(current.gameObject);
        current.gameObject.SetActive(false);
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    this.currentIndex = 0;
    this.buttons[this.currentIndex].SetActive(true);
    this.isInitialized = true;
  }

  public void Next()
  {
    if (this.buttons.Count == 0)
      return;
    this.buttons[this.currentIndex].SetActive(false);
    this.currentIndex = (this.currentIndex + 1) % this.buttons.Count;
    if (this.currentIndex == this.buttons.Count)
      this.currentIndex = 0;
    this.buttons[this.currentIndex].SetActive(true);
    this.Invoke();
  }

  public void SetIndex(int newIndex)
  {
    this.Init();
    if (this.currentIndex == newIndex)
      return;
    this.buttons[this.currentIndex].SetActive(false);
    this.currentIndex = newIndex;
    this.buttons[this.currentIndex].SetActive(true);
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    this.ts.ShowDefault(this.header, this.description);
  }

  public void OnPointerExit(PointerEventData eventData) => this.ts.Hide();

  public void Invoke() => this.onInvokeFunction.Invoke(this.currentIndex);
}
