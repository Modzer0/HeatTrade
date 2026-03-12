// Decompiled with JetBrains decompiler
// Type: Starter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Starter : MonoBehaviour
{
  [SerializeField]
  private bool startActive = true;
  [SerializeField]
  private bool scaleOnStart;
  [SerializeField]
  private Vector3 startScale = new Vector3(1f, 1f, 1f);
  [SerializeField]
  private bool alphaOnStart;
  [SerializeField]
  private float startAlpha = 1f;

  private void Start()
  {
    this.gameObject.SetActive(this.startActive);
    if (this.scaleOnStart)
      this.transform.localScale = this.startScale;
    if (!this.alphaOnStart || !(bool) (Object) this.GetComponent<CanvasGroup>())
      return;
    this.GetComponent<CanvasGroup>().alpha = this.startAlpha;
  }
}
