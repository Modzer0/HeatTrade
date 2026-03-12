// Decompiled with JetBrains decompiler
// Type: Astrocaster2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Astrocaster2 : MonoBehaviour
{
  [Header("UI - Player")]
  [SerializeField]
  private uiBar playerStaminaBar;
  [SerializeField]
  private Transform rodParent;
  [SerializeField]
  private Transform rodIcon;
  [SerializeField]
  private uiBar lineBar;
  [SerializeField]
  private Gradient gradient;
  private Vector3 rodIconStartPos;
  [Header("UI - Fish")]
  [SerializeField]
  private uiBar fishStaminaBar;
  [SerializeField]
  private Transform fishParent;
  [SerializeField]
  private Transform fishIcon;
  private Vector3 fishIconStartPos;
  [Header("Resources")]
  public float maxPlayerStamina = 100f;
  public float playerStaminaRegenRate = 5f;
  public float blockStaminaCost = 10f;
  public float parryProgress = 25f;
  public float maxLineHealth = 100f;
  public float maxFishStamina = 100f;
  public float fishStaminaRegenRate = 5f;
  private float currentPlayerStamina;
  private float currentLineHealth;
  private float currentFishStamina;
  [Header("Input Settings")]
  [Tooltip("Time the Space bar must be held down to count as a Heavy Pull.")]
  public float heavyPullThreshold = 0.3f;
  private float spacePressStartTime;
  private bool spaceKeyDown;
  [Header("Game Stats")]
  [Tooltip("Total distance/progress required to catch the fish.")]
  public float maxLineProgress = 200f;
  [Tooltip("Current distance/progress, 0 = start, maxLineProgress = catch.")]
  public float currentLineProgress = 100f;
  [Tooltip("Time window in seconds for a successful Parry.")]
  public float parryWindow = 0.2f;
  [Tooltip("How much fish progress is lost when blocking.")]
  public float blockProgressReduction = 0.5f;
  private bool isFighting;
  private bool isPulling;
  private bool isDodging;
  private bool isParrying;
  private bool inPullAnimation;
  [SerializeField]
  private GameObject isFightingText;
  [SerializeField]
  private GameObject isPullingText;
  [SerializeField]
  private GameObject isDodgingText;
  [SerializeField]
  private GameObject isParryingText;
  [SerializeField]
  private GameObject inPullAnimText;
  [Header("Attack Definitions")]
  public Astrocaster2.Astrocaster2Attack lightPull;
  public Astrocaster2.Astrocaster2Attack heavyPull;
  public Astrocaster2.Astrocaster2Attack lightFishAttack;
  public Astrocaster2.Astrocaster2Attack heavyFishAttack;
  [Header("Fish Moves")]
  public float struggleDuration = 2f;
  [Tooltip("Multiplier for player pull while fish is struggling.")]
  public float strugglePullModifier = 0.2f;
  public float struggleStaminaCost = 10f;
  private bool isStruggling;
  [Tooltip("Magnitude of the fish icon shake during struggle.")]
  public float struggleShakeMagnitude = 15f;
  [Tooltip("Speed of the fish icon shake during struggle.")]
  public float struggleShakeSpeed = 0.05f;

  private void Start()
  {
    this.currentPlayerStamina = this.maxPlayerStamina;
    this.currentLineHealth = this.maxLineHealth;
    this.currentFishStamina = this.maxFishStamina;
    this.rodIconStartPos = this.rodIcon.localPosition;
    this.fishIconStartPos = this.fishIcon.localPosition;
    this.StartCoroutine((IEnumerator) this.FishingStateLoop());
    this.UpdateUI();
  }

  private void Update()
  {
    this.HandleInput();
    this.HandleStaminaRegen();
    this.UpdateUI();
    this.CheckWinLossConditions();
  }

  private IEnumerator FishingStateLoop()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Astrocaster2 astrocaster2 = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      astrocaster2.isFighting = true;
      astrocaster2.StartCoroutine((IEnumerator) astrocaster2.FishBehavior());
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(2f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void HandleInput()
  {
    if (!this.isFighting)
      return;
    if (Input.GetKeyDown(KeyCode.Space))
    {
      this.spacePressStartTime = Time.time;
      this.spaceKeyDown = true;
    }
    if (this.spaceKeyDown && (double) Time.time - (double) this.spacePressStartTime >= (double) this.heavyPullThreshold && !this.isPulling)
    {
      this.spaceKeyDown = false;
      this.StartCoroutine((IEnumerator) this.PlayerPull(this.heavyPull));
    }
    if (Input.GetKeyUp(KeyCode.Space) && (double) Time.time - (double) this.spacePressStartTime < (double) this.heavyPullThreshold)
    {
      this.spaceKeyDown = false;
      if (!this.isPulling)
        this.StartCoroutine((IEnumerator) this.PlayerPull(this.lightPull));
    }
    if (Input.GetKeyDown(KeyCode.Q))
    {
      this.isDodging = true;
      this.isParrying = false;
    }
    else if (Input.GetKey(KeyCode.Q))
    {
      if ((double) this.currentPlayerStamina <= 0.0)
        return;
      this.currentPlayerStamina -= Time.deltaTime * this.blockStaminaCost;
      this.currentLineProgress += this.blockProgressReduction * Time.deltaTime;
    }
    else
    {
      if (!Input.GetKeyUp(KeyCode.Q))
        return;
      this.isDodging = false;
    }
  }

  private void HandleStaminaRegen()
  {
    if (this.isPulling || this.spaceKeyDown || (double) this.currentPlayerStamina >= (double) this.maxPlayerStamina)
      return;
    this.currentPlayerStamina += this.playerStaminaRegenRate * Time.deltaTime;
    this.currentPlayerStamina = Mathf.Clamp(this.currentPlayerStamina, 0.0f, this.maxPlayerStamina);
  }

  private IEnumerator PlayerPull(Astrocaster2.Astrocaster2Attack attack)
  {
    if ((double) this.currentPlayerStamina >= (double) attack.staminaCost)
    {
      this.isPulling = true;
      this.currentPlayerStamina -= attack.staminaCost;
      this.currentFishStamina = Mathf.Clamp(this.currentFishStamina - attack.fishStaminaCost, 0.0f, this.maxFishStamina);
      LeanTween.moveLocal(this.rodIcon.gameObject, this.rodIconStartPos + Vector3.down * 10f, attack.pullTime).setEase(LeanTweenType.easeOutQuad);
      yield return (object) new WaitForSeconds(attack.pullTime);
      yield return (object) new WaitForSeconds(attack.waitTime);
      LeanTween.moveLocal(this.rodIcon.gameObject, this.rodIconStartPos, attack.pushTime).setEase(LeanTweenType.easeOutBack);
      yield return (object) new WaitForSeconds(attack.pushTime);
      this.currentLineProgress -= attack.progressChange * (this.isStruggling ? this.strugglePullModifier : 1f);
      this.currentLineProgress = Mathf.Clamp(this.currentLineProgress, 0.0f, this.maxLineProgress);
      this.isPulling = false;
    }
  }

  private IEnumerator FishBehavior()
  {
    Astrocaster2 astrocaster2 = this;
    while (astrocaster2.isFighting)
    {
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(1f, 3f));
      if ((double) astrocaster2.currentFishStamina <= 0.0)
      {
        astrocaster2.currentFishStamina = astrocaster2.maxFishStamina;
        yield return (object) new WaitForSeconds(1f);
      }
      else
      {
        float num = UnityEngine.Random.Range(0.0f, 1f);
        if ((double) num < 0.20000000298023224 && !astrocaster2.isStruggling && !astrocaster2.isPulling)
          astrocaster2.StartCoroutine((IEnumerator) astrocaster2.FishStruggle());
        else if ((double) num < 0.60000002384185791)
          astrocaster2.StartCoroutine((IEnumerator) astrocaster2.FishAttack(astrocaster2.lightFishAttack));
        else
          astrocaster2.StartCoroutine((IEnumerator) astrocaster2.FishAttack(astrocaster2.heavyFishAttack));
      }
    }
  }

  private IEnumerator FishAttack(Astrocaster2.Astrocaster2Attack attack)
  {
    this.inPullAnimation = true;
    LeanTween.moveLocal(this.fishIcon.gameObject, this.fishIconStartPos + Vector3.up * 10f, attack.pullTime).setEase(LeanTweenType.easeOutQuad);
    yield return (object) new WaitForSeconds(attack.pullTime);
    yield return (object) new WaitForSeconds(attack.waitTime);
    LeanTween.moveLocal(this.fishIcon.gameObject, this.fishIconStartPos, attack.pushTime).setEase(LeanTweenType.easeInSine);
    float timer = 0.0f;
    while ((double) timer < (double) attack.pushTime)
    {
      if (this.isDodging && (double) timer < (double) this.parryWindow)
        this.isParrying = true;
      timer += Time.deltaTime;
      yield return (object) null;
    }
    if ((this.isPulling || this.isParrying) && !this.isDodging)
    {
      this.currentLineProgress -= this.parryProgress;
      Debug.Log((object) "Parry Successful!");
    }
    else if (this.isDodging)
    {
      this.currentLineHealth -= attack.lineDamage * 0.5f;
      this.currentLineProgress += attack.progressChange * 0.5f;
    }
    else
    {
      this.currentLineHealth -= attack.lineDamage;
      this.currentLineProgress += attack.progressChange;
    }
    this.currentLineProgress = Mathf.Clamp(this.currentLineProgress, 0.0f, this.maxLineProgress);
    this.isParrying = false;
    this.inPullAnimation = false;
  }

  private IEnumerator FishStruggle()
  {
    this.isStruggling = true;
    this.currentFishStamina -= this.struggleStaminaCost;
    Debug.Log((object) "Fish is Struggling! Pulls slowed.");
    int tweenId = LeanTween.moveLocalX(this.fishIcon.gameObject, this.fishIconStartPos.x + this.struggleShakeMagnitude, this.struggleShakeSpeed).setEase(LeanTweenType.linear).setLoopPingPong().uniqueId;
    yield return (object) new WaitForSeconds(this.struggleDuration);
    LeanTween.cancel(tweenId);
    this.fishIcon.localPosition = this.fishIconStartPos;
    this.isStruggling = false;
  }

  private void UpdateUI()
  {
    this.playerStaminaBar.SetBarSize(this.currentPlayerStamina / this.maxPlayerStamina);
    this.fishStaminaBar.SetBarSize(this.currentFishStamina / this.maxFishStamina);
    this.lineBar.SetBarSize((float) (1.0 - (double) this.currentLineProgress / (double) this.maxLineProgress));
    this.lineBar.SetBarColor(this.gradient.Evaluate(this.currentLineHealth / this.maxLineHealth));
    this.isFightingText.SetActive(this.isFighting);
    this.isPullingText.SetActive(this.isPulling);
    this.isDodgingText.SetActive(this.isDodging);
    this.isParryingText.SetActive(this.isParrying);
    this.inPullAnimText.SetActive(this.inPullAnimation);
  }

  private void CheckWinLossConditions()
  {
    if (!this.isFighting)
      return;
    if ((double) this.currentLineHealth <= 0.0)
    {
      Debug.LogError((object) "GAME OVER: Line Snapped!");
      this.isFighting = false;
      this.StopAllCoroutines();
    }
    else if ((double) this.currentLineProgress >= (double) this.maxLineProgress)
    {
      Debug.Log((object) "SUCCESS: Fish Caught!");
      this.isFighting = false;
      this.StopAllCoroutines();
    }
    else
    {
      if ((double) this.currentLineProgress > 0.0)
        return;
      Debug.Log((object) "GAME OVER: Fish Escaped!");
      this.isFighting = false;
      this.StopAllCoroutines();
    }
  }

  [Serializable]
  public class Astrocaster2Attack
  {
    public string attackName = "Basic";
    [Tooltip("Distance/progress change (positive for player, negative for fish)")]
    public float progressChange = 25f;
    public float pullTime = 0.5f;
    public float waitTime = 0.1f;
    public float pushTime = 0.2f;
    public float staminaCost = 15f;
    public float lineDamage = 10f;
    public float fishStaminaCost = 5f;
  }
}
