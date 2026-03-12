// Decompiled with JetBrains decompiler
// Type: uiAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class uiAnimator : MonoBehaviour
{
  private TimeController tc;
  [SerializeField]
  private bool isOffOnStart;
  [SerializeField]
  private bool isBlockRaycastWhenOn = true;
  [Header("Fade Settings")]
  [SerializeField]
  private bool isFade = true;
  public float fadeTime = 0.3f;
  public LeanTweenType fadeEase = LeanTweenType.easeOutCubic;
  [Tooltip("Lower value = slower fade")]
  [SerializeField]
  private float fadeSpeed = 5f;
  [Header("Move Settings")]
  [SerializeField]
  private bool isMove;
  public float moveTime = 0.4f;
  public LeanTweenType moveEase = LeanTweenType.easeOutCubic;
  [Header("Scale Settings")]
  [SerializeField]
  private bool isScale;
  public float scaleTime = 0.4f;
  [SerializeField]
  private Vector3 targetScale = Vector3.one;
  public LeanTweenType scaleEase = LeanTweenType.easeOutBack;
  [Header("Others")]
  [SerializeField]
  private bool isPop;
  private CanvasGroup canvasGroup;
  private RectTransform rectTransform;
  private int fadeTweenId = -1;
  private bool isShow;
  private float timeMultiplier = 1f;

  public bool IsShow => this.isShow;

  private void Awake()
  {
    this.canvasGroup = this.GetComponent<CanvasGroup>();
    this.rectTransform = this.GetComponent<RectTransform>();
    if (!(bool) (UnityEngine.Object) this.canvasGroup)
      this.canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
    if (this.isOffOnStart)
      this.Hide();
    else
      this.isShow = true;
  }

  private void Start() => this.tc = TimeController.current;

  private void Update()
  {
    if ((bool) (UnityEngine.Object) this.tc)
    {
      this.timeMultiplier = this.tc.GetTimeMultiplier();
    }
    else
    {
      string name = SceneManager.GetActiveScene().name;
      if (name == "SCENE - Tactical" || name == "SCENE - Tutorial 1" || name == "SCENE - Tutorial 3")
      {
        this.tc = TimeController.current;
        if ((bool) (UnityEngine.Object) this.tc)
          this.timeMultiplier = this.tc.GetTimeMultiplier();
      }
    }
    if (this.isScale && this.isShow)
      this.transform.localScale = Vector3.Lerp(this.transform.localScale, this.targetScale, Time.deltaTime * this.fadeSpeed * this.timeMultiplier);
    if (this.isShow && (double) this.canvasGroup.alpha >= 0.99000000953674316 || !this.isShow && (double) this.canvasGroup.alpha <= 0.0099999997764825821)
      return;
    if (this.isFade)
    {
      this.canvasGroup.alpha = Mathf.Lerp(this.canvasGroup.alpha, this.isShow ? 1f : 0.0f, Time.deltaTime * this.fadeSpeed * this.timeMultiplier);
      if (!this.isShow && (double) this.canvasGroup.alpha <= 0.0099999997764825821)
      {
        this.canvasGroup.alpha = 0.0f;
      }
      else
      {
        if (!this.isShow || (double) this.canvasGroup.alpha < 0.99000000953674316)
          return;
        this.canvasGroup.alpha = 1f;
      }
    }
    else
    {
      if (this.isMove || !this.isScale || !this.isShow)
        return;
      this.transform.localScale = Vector3.Lerp(this.transform.localScale, this.targetScale, Time.deltaTime * this.fadeSpeed * this.timeMultiplier);
    }
  }

  public void Toggle()
  {
    if (this.isShow)
      this.Hide();
    else
      this.Show();
  }

  public void Show()
  {
    this.gameObject.SetActive(true);
    this.canvasGroup.blocksRaycasts = this.isBlockRaycastWhenOn;
    this.isShow = true;
    if (!this.isPop)
      return;
    this.Pop();
  }

  private void Pop()
  {
    LeanTween.scale(this.rectTransform, new Vector3(1.2f, 1.2f, 1f), 0.15f).setOnComplete((Action) (() => LeanTween.scale(this.rectTransform, new Vector3(1f, 1f, 1f), 0.15f)));
  }

  public void Hide()
  {
    this.isShow = false;
    if (!(bool) (UnityEngine.Object) this.canvasGroup)
      return;
    this.canvasGroup.blocksRaycasts = false;
  }

  public void Hide(float? customFadeTime = null)
  {
    this.isShow = false;
    if (!(bool) (UnityEngine.Object) this.canvasGroup)
      return;
    this.canvasGroup.blocksRaycasts = false;
  }

  public void MoveTo(Vector3 targetPos)
  {
    LeanTween.move(this.rectTransform, targetPos, this.moveTime).setEase(this.moveEase);
  }

  public void ScaleTo(Vector3 targetScale)
  {
    LeanTween.scale(this.rectTransform, targetScale, this.scaleTime).setEase(this.scaleEase);
  }

  public void SetAlpha(float alpha) => this.canvasGroup.alpha = alpha;
}
