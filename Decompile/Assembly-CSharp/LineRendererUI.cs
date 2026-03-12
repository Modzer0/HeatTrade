// Decompiled with JetBrains decompiler
// Type: LineRendererUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LineRendererUI : MonoBehaviour
{
  [SerializeField]
  private RectTransform m_myTransform;
  [SerializeField]
  private Image m_image;
  [SerializeField]
  private bool isOffOnStart;

  private void Start()
  {
    this.m_myTransform = this.GetComponent<RectTransform>();
    this.m_image = this.GetComponent<Image>();
    if (!this.isOffOnStart)
      this.CreateLine((Vector2) Vector3.zero, (Vector2) (Vector3.up * 1000f), Color.white);
    else
      this.CreateLine((Vector2) Vector3.zero, (Vector2) Vector3.zero, Color.white);
  }

  public void CreateLine(Vector2 point1, Vector2 point2, Color color)
  {
    this.m_image.color = color;
    this.m_myTransform.anchoredPosition = (point1 + point2) / 2f;
    Vector2 vector2 = point1 - point2;
    this.m_myTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(vector2.y, vector2.x) * 57.29578f);
    this.m_myTransform.sizeDelta = new Vector2(vector2.magnitude, 2f);
    this.m_myTransform.localScale = Vector3.one;
  }
}
