// Decompiled with JetBrains decompiler
// Type: Fader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using TMPro;
using UnityEngine;

#nullable disable
public class Fader : MonoBehaviour
{
  private Vector3 worldPosition;
  [SerializeField]
  private TMP_Text headerText;
  [SerializeField]
  private TMP_Text bodyText;
  public bool isFollowWorldPosition;
  public bool isFloat;

  private void Update()
  {
    if (this.isFollowWorldPosition)
    {
      this.transform.position = Camera.main.WorldToScreenPoint(this.worldPosition);
      this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 8f, 0.0f);
    }
    else
    {
      if (!this.isFloat)
        return;
      this.transform.position += Vector3.up * Time.deltaTime;
    }
  }

  public void Setup(Vector3 worldPosition, bool isFollowWorldPosition, string header, string body)
  {
    this.worldPosition = worldPosition;
    this.isFollowWorldPosition = isFollowWorldPosition;
    this.headerText.text = header;
    this.bodyText.text = body;
    this.GetComponent<CanvasGroup>().alpha = 0.0f;
    this.GetComponent<uiAnimator>().Show();
    this.StartCoroutine((IEnumerator) this.StartHide());
  }

  public void SetColor(Color color)
  {
    this.headerText.color = color;
    this.bodyText.color = color;
  }

  public void SetupUI(Vector3 screenPosition, string header, string body)
  {
    this.transform.position = screenPosition;
    this.headerText.text = header;
    this.bodyText.text = body;
    this.GetComponent<CanvasGroup>().alpha = 0.0f;
    this.GetComponent<uiAnimator>().Show();
    this.StartCoroutine((IEnumerator) this.StartHide());
  }

  public IEnumerator StartHide()
  {
    Fader fader = this;
    yield return (object) new WaitForSeconds(3f);
    fader.GetComponent<uiAnimator>().Hide();
    yield return (object) new WaitForSeconds(0.5f);
    Object.Destroy((Object) fader.gameObject);
  }
}
