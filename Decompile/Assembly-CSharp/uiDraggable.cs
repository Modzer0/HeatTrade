// Decompiled with JetBrains decompiler
// Type: uiDraggable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class uiDraggable : 
  MonoBehaviour,
  IBeginDragHandler,
  IEventSystemHandler,
  IDragHandler,
  IEndDragHandler
{
  private RectTransform rectTransform;
  [SerializeField]
  private Canvas canvas;
  [SerializeField]
  private CanvasGroup canvasGroup;
  [SerializeField]
  private float dragOpacity;
  [Header("GRID")]
  [SerializeField]
  private bool snapToGrid;
  public float gridSize = 128f;

  private void Start() => this.rectTransform = this.GetComponent<RectTransform>();

  public void OnBeginDrag(PointerEventData eventData)
  {
    if (!((Object) this.canvasGroup != (Object) null))
      return;
    this.canvasGroup.alpha = this.dragOpacity;
    this.canvasGroup.blocksRaycasts = false;
  }

  public void OnDrag(PointerEventData eventData)
  {
    this.rectTransform.anchoredPosition += eventData.delta / this.canvas.scaleFactor;
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    if ((Object) this.canvasGroup != (Object) null)
    {
      this.canvasGroup.alpha = 1f;
      this.canvasGroup.blocksRaycasts = true;
    }
    if (!this.snapToGrid && (double) this.gridSize != 0.0)
      return;
    Vector2 anchoredPosition = this.rectTransform.anchoredPosition;
    this.rectTransform.anchoredPosition = new Vector2(Mathf.Round(anchoredPosition.x / this.gridSize) * this.gridSize, Mathf.Round(anchoredPosition.y / this.gridSize) * this.gridSize);
  }
}
