// Decompiled with JetBrains decompiler
// Type: Carousel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Carousel : MonoBehaviour
{
  [SerializeField]
  private List<GameObject> items = new List<GameObject>();
  private int index;
  [SerializeField]
  private bool loop = true;
  [SerializeField]
  private GameObject button;

  private void Start()
  {
    foreach (GameObject gameObject in this.items)
      gameObject.SetActive(false);
    this.items[this.index].SetActive(true);
  }

  public void Next()
  {
    if (!this.loop)
    {
      if (this.index == this.items.Count - 2)
        this.button.SetActive(false);
      else if (this.index == this.items.Count - 1)
        return;
    }
    this.items[this.index].SetActive(false);
    this.index = (this.index + 1) % this.items.Count;
    this.items[this.index].SetActive(true);
  }

  public void Previous()
  {
    this.items[this.index].SetActive(false);
    this.index = (this.index - 1 + this.items.Count) % this.items.Count;
    this.items[this.index].SetActive(true);
  }
}
