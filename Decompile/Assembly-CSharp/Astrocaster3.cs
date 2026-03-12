// Decompiled with JetBrains decompiler
// Type: Astrocaster3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Astrocaster3 : MonoBehaviour
{
  public static Astrocaster3 current;
  [SerializeField]
  private Transform messageSpawn;
  private bool isOn;
  private NotificationsManager notifs;
  [SerializeField]
  private AstrocasterSFX sfx;
  [SerializeField]
  private AllFish3 allFish;
  [Header("= ASTROBANK ====================")]
  private int exchangeRate;
  private int astrocoins;
  private FactionsManager factions;
  private TimeManager tm;
  [Header("Fishes")]
  [SerializeField]
  private bool isDiscoverAllFishesOnStart;
  private Astrofish3 baitworm;
  private List<Astrofish3> fishesCommon = new List<Astrofish3>();
  private List<Astrofish3> fishesUncommon = new List<Astrofish3>();
  private List<Astrofish3> fishesRare = new List<Astrofish3>();
  private List<Astrofish3> fishesExotic = new List<Astrofish3>();
  private List<Astrofish3> fishesLegendary = new List<Astrofish3>();
  private List<string> fishesDiscovered = new List<string>();
  [SerializeField]
  private FishButton fishButtonPF;
  [SerializeField]
  private Image currentBaitImg;
  private Astrofish3 selectedFish;
  private Astrofish3 currentBait;
  private Astrofish3 currentFish;
  private Astrofish3 caughtFish;
  [Header("NIBBLING")]
  private bool isGameOn;
  private bool isNibbling;
  [SerializeField]
  private float reactionWindow = 1f;
  [SerializeField]
  private GameObject nibbleImg;
  [SerializeField]
  private Transform unknownFish;
  [SerializeField]
  private GameObject unknownFishImg;
  [Header("= UI ====================")]
  [SerializeField]
  private uiAnimator panel;
  [SerializeField]
  private GameObject castPanel;
  [SerializeField]
  private GameObject reelPanel;
  [SerializeField]
  private Transform grid;
  [SerializeField]
  private uiButton castButton;
  [SerializeField]
  private uiButton reelButton;
  [Header("Selected Fish Info")]
  [SerializeField]
  private GameObject selectedPanel;
  [SerializeField]
  private Image selectedIcon;
  [SerializeField]
  private TMP_Text selectedName;
  [SerializeField]
  private TMP_Text selectedDescription;
  [SerializeField]
  private TMP_Text selectedPrice;
  [SerializeField]
  private Transform selectedCatchList;
  [SerializeField]
  private FishCatchUI fishCatchUIPF;
  [SerializeField]
  private GameObject selectedBuyButton;
  [Header("Caught Fish Info")]
  [SerializeField]
  private GameObject caughtPanel;
  [SerializeField]
  private Image caughtIcon;
  [SerializeField]
  private TMP_Text caughtName;
  [SerializeField]
  private TMP_Text caughtDescription;
  [SerializeField]
  private TMP_Text caughtPrice;
  [SerializeField]
  private Transform caughtCatchList;
  [SerializeField]
  private GameObject caughtBuybutton;
  [Header("Astrobank")]
  [SerializeField]
  private TMP_Text exchangeRateText;
  [SerializeField]
  private TMP_Text astrocoinsText;
  [SerializeField]
  private TMP_Text cashoutText;
  [Header("Player")]
  [SerializeField]
  private uiBar playerStaminaBar;
  [SerializeField]
  private Transform rodIcon;
  private Vector3 rodIconStartPos;
  [SerializeField]
  private uiBar lineBar;
  [SerializeField]
  private Gradient gradient;
  [Header("Fish")]
  [SerializeField]
  private uiBar fishStaminaBar;
  [SerializeField]
  private Transform fishIcon;
  private Vector3 fishIconStartPos;
  [Header("= RESOURCES AND VALUES ====================")]
  [Header("- Player ----------")]
  private float currentPlayerStamina;
  public float maxPlayerStamina = 100f;
  public float playerStaminaRegenRate = 5f;
  private bool isReeling;
  [Header("Blocking")]
  public float blockTime = 0.25f;
  public float blockStaminaCost = 10f;
  public float parryProgress = 20f;
  private int blockTweenId;
  private bool isFishing;
  private bool isPlayerBusy;
  private bool isBlocking;
  private bool isFishBusy;
  private bool isFishComboing;
  [Header("Line")]
  public float maxLineHealth = 100f;
  public float currentLineProgress;
  public float maxLineProgress = 100f;
  private float currentLineHealth;
  [Header("- Fish ----------")]
  public float maxFishStamina = 100f;
  public float fishStaminaRegenRate = 5f;
  public float fishConstantPull = -1f;
  public Vector2 fishIdleRange = new Vector2(3f, 6f);
  private float currentFishStamina;
  [Header("Fish Struggling")]
  public float struggleStaminaCost = 10f;
  public Vector2 struggleRange = new Vector2(0.5f, 2f);
  public float struggleShakeMagnitude = 15f;
  public float struggleShakeSpeed = 0.05f;
  private bool isStruggling;
  private int struggleTweenId;
  [Header("VFX")]
  [SerializeField]
  private ParticleSystem ps1;
  [SerializeField]
  private ParticleSystem ps2;
  [SerializeField]
  private ParticleSystem ps3;
  [SerializeField]
  private ParticleSystem ps4;
  [SerializeField]
  private ParticleSystem ps5;
  [SerializeField]
  private ParticleSystem parryPS;
  [Header("Attacks")]
  public AC3Attack lightReel;
  public AC3Attack heavyReel;

  private void Awake() => Astrocaster3.current = this;

  private void Start()
  {
    this.notifs = NotificationsManager.current;
    this.factions = FactionsManager.current;
    this.tm = TimeManager.current;
    this.tm.NewDay += new Action(this.NewDay);
    IEnumerator enumerator = (IEnumerator) this.grid.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
        UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator.Current).gameObject);
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    foreach (Astrofish3 fish in this.allFish.fishes)
      this.AddFishButtonToGrid(fish);
    foreach (Astrofish3 fish in this.allFish.fishes)
    {
      if (fish.fishName == "Baitworm")
      {
        this.baitworm = fish;
        this.DiscoverFish(fish);
      }
      else if (fish.rarity == FishRarity.COMMON)
        this.fishesCommon.Add(fish);
      else if (fish.rarity == FishRarity.UNCOMMON)
        this.fishesUncommon.Add(fish);
      else if (fish.rarity == FishRarity.RARE)
        this.fishesRare.Add(fish);
      else if (fish.rarity == FishRarity.EXOTIC)
        this.fishesExotic.Add(fish);
      else if (fish.rarity == FishRarity.LEGENDARY)
        this.fishesLegendary.Add(fish);
      if (this.isDiscoverAllFishesOnStart)
        this.DiscoverFish(fish);
    }
    this.NewDay();
    this.rodIconStartPos = this.rodIcon.localPosition;
    this.fishIconStartPos = this.fishIcon.localPosition;
    this.currentLineProgress = this.maxLineProgress;
  }

  private void Update()
  {
    if (!this.isOn)
      return;
    if (Input.GetKeyDown(KeyCode.Space))
    {
      this.isReeling = true;
      this.reelButton.OnClick();
    }
    if (this.isReeling)
      this.sfx.Play("click");
    if (!this.isFishing && !this.isGameOn)
    {
      if (this.isReeling)
      {
        if ((bool) (UnityEngine.Object) this.caughtFish && this.caughtFish.catches.Count > 0)
          this.UseCaughtFishAsBait();
        else if ((bool) (UnityEngine.Object) this.currentBait)
          this.Cast();
        else if ((bool) (UnityEngine.Object) this.selectedFish)
          this.UseSelectedFishAsBait();
      }
    }
    else if (this.isFishing)
    {
      this.Inputs();
      this.StaminaRegen();
      this.CheckWinLoss();
    }
    this.UpdateFishingUI();
  }

  private void LateUpdate() => this.isReeling = false;

  public void Reel()
  {
    MonoBehaviour.print((object) "reel");
    this.isReeling = true;
  }

  private void GameStart()
  {
    this.StopAnims();
    this.isGameOn = true;
    this.rodIcon.localPosition = this.rodIconStartPos;
    this.fishIcon.localPosition = this.fishIconStartPos;
    this.currentPlayerStamina = this.maxPlayerStamina;
    this.currentLineHealth = this.maxLineHealth;
    this.currentFishStamina = this.maxFishStamina;
    this.notifs.NewNotif("[ASTROCASTER] New Game!");
  }

  public void Cast()
  {
    if ((UnityEngine.Object) this.currentBait == (UnityEngine.Object) null)
      return;
    this.GameStart();
    this.reelPanel.SetActive(true);
    this.isNibbling = false;
    this.StartCoroutine((IEnumerator) this.Nibble());
    this.UpdateUI();
    this.sfx.Play("cast");
  }

  private void SetCurrentFish(Astrofish3 newFish)
  {
    this.currentFish = newFish;
    this.maxFishStamina = newFish.staminaMax;
    this.currentFishStamina = this.maxFishStamina;
    this.fishStaminaRegenRate = newFish.staminaRegenRate;
    this.fishConstantPull = newFish.constantPull;
    this.fishIdleRange = newFish.idleRange;
    this.struggleRange = newFish.struggleRange;
  }

  private void SetStartProgress()
  {
    this.currentLineProgress = 0.2f * this.maxLineProgress;
    if (this.currentFish.rarity == FishRarity.COMMON)
      this.currentLineProgress = 0.6f * this.maxLineProgress;
    else if (this.currentFish.rarity == FishRarity.UNCOMMON)
      this.currentLineProgress = 0.5f * this.maxLineProgress;
    else if (this.currentFish.rarity == FishRarity.RARE)
    {
      this.currentLineProgress = 0.4f * this.maxLineProgress;
    }
    else
    {
      if (this.currentFish.rarity != FishRarity.EXOTIC)
        return;
      this.currentLineProgress = 0.3f * this.maxLineProgress;
    }
  }

  private IEnumerator Nibble()
  {
    MonoBehaviour.print((object) "START NIBBLE MINIGAME");
    this.SetCurrentFish(this.currentBait.GetLuredFish());
    if ((UnityEngine.Object) this.currentFish == (UnityEngine.Object) null)
      yield return (object) null;
    float startTime = Time.time;
    float startNibbleTime = startTime + this.currentFish.GetNibbleTime();
    float stopNibbleTime = startNibbleTime + this.currentFish.GetReactionTime();
    this.SetStartProgress();
    this.isNibbling = false;
    this.isReeling = false;
    while ((double) Time.time < (double) stopNibbleTime)
    {
      if ((double) Time.time >= (double) startNibbleTime)
      {
        if (!this.isNibbling)
        {
          if (this.currentFish.rarity == FishRarity.COMMON || this.currentFish.rarity == FishRarity.UNCOMMON)
            this.sfx.Play("nibble");
          if (this.currentFish.rarity == FishRarity.COMMON)
            this.notifs.NewNotif("[ASTROCASTER] NIBBLE!", "yellow");
        }
        this.isNibbling = true;
        if (this.isReeling)
        {
          this.StartFishing();
          break;
        }
      }
      else if ((double) Time.time < (double) startNibbleTime && this.isReeling)
      {
        this.FishingFail("escape");
        break;
      }
      yield return (object) null;
    }
    if ((double) Time.time >= (double) startTime + (double) this.reactionWindow && !this.isFishing)
      this.FishingFail("escape");
    yield return (object) null;
  }

  private void StartFishing()
  {
    MonoBehaviour.print((object) "FISHING!");
    this.isFishing = true;
    this.isNibbling = false;
    this.StartCoroutine((IEnumerator) this.FishBehavior());
  }

  private void Inputs()
  {
    if (this.isPlayerBusy || !this.isFishing)
      return;
    if (this.isReeling)
    {
      this.StartCoroutine((IEnumerator) this.PlayerAttack(this.lightReel));
    }
    else
    {
      if (!Input.GetKeyDown(KeyCode.Q) || (double) this.currentPlayerStamina < (double) this.blockStaminaCost)
        return;
      this.currentPlayerStamina -= this.blockStaminaCost;
      this.StartCoroutine((IEnumerator) this.PlayerBlock());
    }
  }

  private void StaminaRegen()
  {
    if (!this.isFishing)
      return;
    if (!this.isPlayerBusy)
      this.currentPlayerStamina += this.playerStaminaRegenRate * Time.deltaTime;
    this.currentPlayerStamina = Mathf.Clamp(this.currentPlayerStamina, 0.0f, this.maxPlayerStamina);
    if (!this.isFishBusy)
      this.currentFishStamina += this.fishStaminaRegenRate * Time.deltaTime;
    this.currentFishStamina = Mathf.Clamp(this.currentFishStamina, 0.0f, this.maxFishStamina);
    this.currentLineProgress += this.fishConstantPull * (this.isStruggling ? 0.2f : 1f) * Time.deltaTime;
  }

  private void UpdateFishingUI()
  {
    this.currentPlayerStamina = Mathf.Clamp(this.currentPlayerStamina, 0.0f, this.maxPlayerStamina);
    this.playerStaminaBar.SetBarSize(this.currentPlayerStamina / this.maxPlayerStamina);
    this.currentFishStamina = Mathf.Clamp(this.currentFishStamina, 0.0f, this.maxFishStamina);
    this.fishStaminaBar.SetBarSize(this.currentFishStamina / this.maxFishStamina);
    this.lineBar.SetBarSize(1f - Mathf.Clamp01(this.currentLineProgress / this.maxLineProgress));
    this.lineBar.SetBarColor(this.gradient.Evaluate(this.currentLineHealth / this.maxLineHealth));
    this.nibbleImg.SetActive(this.isNibbling);
    this.unknownFishImg.SetActive(this.isFishing);
  }

  private void CheckWinLoss()
  {
    if (!this.isFishing)
      return;
    if ((double) this.currentLineHealth <= 0.0)
      this.FishingFail("lineSnap");
    else if ((double) this.currentLineProgress >= (double) this.maxLineProgress - 1.0)
    {
      this.notifs.NewNotif("[ASTROCASTER] FISH CAUGHT. Good job!", "green");
      this.CatchFish();
    }
    else
    {
      if ((double) this.currentLineProgress > 0.0)
        return;
      this.FishingFail("escape");
    }
  }

  private void CatchFish()
  {
    MonoBehaviour.print((object) "FISH CAUGHT!");
    this.caughtFish = this.currentFish;
    if (!this.fishesDiscovered.Contains(this.caughtFish.fishName))
    {
      this.DiscoverFish(this.caughtFish);
      this.ShowCaughtFishInfo(this.caughtFish);
    }
    FishRarity rarity = this.caughtFish.rarity;
    MonoBehaviour.print((object) ("rarity: " + rarity.ToString()));
    switch (rarity)
    {
      case FishRarity.COMMON:
        MonoBehaviour.print((object) "common");
        this.sfx.Play("win1");
        this.ps1.Play();
        break;
      case FishRarity.UNCOMMON:
        this.sfx.Play("win2");
        this.ps2.Play();
        break;
      case FishRarity.RARE:
        this.sfx.Play("win3");
        this.ps3.Play();
        break;
      case FishRarity.EXOTIC:
        this.sfx.Play("win4");
        this.ps4.Play();
        break;
      case FishRarity.LEGENDARY:
        this.sfx.Play("win5");
        this.ps5.Play();
        break;
    }
    this.GameEnd();
  }

  private void FishingFail(string reason)
  {
    MonoBehaviour.print((object) ("fail: " + reason));
    switch (reason)
    {
      case "lineSnap":
        this.notifs.NewNotif("[ASTROCASTER] LINE SNAPPED!", "red");
        break;
      case "escape":
        this.notifs.NewNotif("[ASTROCASTER] FISH ESCAPED!", "red");
        break;
    }
    this.GameEnd();
    this.sfx.Play("lose");
  }

  private void GameEnd()
  {
    this.StopAnims();
    this.currentPlayerStamina = this.maxPlayerStamina;
    this.currentLineHealth = this.maxLineHealth;
    this.currentLineProgress = this.maxLineProgress;
    this.currentFishStamina = this.maxFishStamina;
    this.isFishing = false;
    this.isPlayerBusy = false;
    this.isBlocking = false;
    this.isFishBusy = false;
    this.isFishComboing = false;
    this.isNibbling = false;
    this.isGameOn = false;
    this.currentBait = (Astrofish3) null;
    this.currentFish = (Astrofish3) null;
    this.selectedFish = (Astrofish3) null;
    this.reelPanel.SetActive(false);
    this.currentBaitImg.gameObject.SetActive(false);
    this.UpdateUI();
  }

  private void StopAnims()
  {
    this.StopAllCoroutines();
    LeanTween.cancel(this.blockTweenId);
    LeanTween.cancel(this.struggleTweenId);
  }

  private IEnumerator PlayerAttack(AC3Attack attack)
  {
    if ((double) this.currentPlayerStamina >= (double) attack.staminaCost)
    {
      this.isPlayerBusy = true;
      this.currentPlayerStamina -= attack.staminaCost;
      LeanTween.moveLocal(this.rodIcon.gameObject, this.rodIconStartPos + Vector3.down * attack.pullDist, attack.pullTime).setEase(LeanTweenType.easeOutQuart);
      yield return (object) new WaitForSeconds(attack.pullTime);
      yield return (object) new WaitForSeconds(attack.waitTime);
      LeanTween.moveLocal(this.rodIcon.gameObject, this.rodIconStartPos, attack.pushTime).setEase(LeanTweenType.linear);
      yield return (object) new WaitForSeconds(attack.pushTime);
      this.currentLineProgress += attack.progressChange * (this.isStruggling ? 0.2f : 1f);
      this.currentLineProgress = Mathf.Clamp(this.currentLineProgress, 0.0f, this.maxLineProgress);
      this.isPlayerBusy = false;
    }
  }

  private IEnumerator PlayerBlock()
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.isPlayerBusy = true;
    this.isBlocking = true;
    this.blockTweenId = LeanTween.moveLocalX(this.rodIcon.gameObject, this.rodIconStartPos.x - this.struggleShakeMagnitude, this.struggleShakeSpeed).setEase(LeanTweenType.linear).setLoopPingPong().uniqueId;
    yield return (object) new WaitForSeconds(this.blockTime);
    LeanTween.cancel(this.blockTweenId);
    this.rodIcon.localPosition = this.rodIconStartPos;
    this.isPlayerBusy = false;
    this.isBlocking = false;
  }

  private IEnumerator FishBehavior()
  {
    Astrocaster3 astrocaster3 = this;
    while (astrocaster3.isFishing)
    {
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(astrocaster3.fishIdleRange.x, astrocaster3.fishIdleRange.y));
      if ((double) astrocaster3.currentFishStamina <= 0.0)
        yield return (object) new WaitForSeconds(1f);
      else if (!astrocaster3.isFishBusy && !astrocaster3.isFishComboing)
      {
        int move = astrocaster3.currentFish.GetMove();
        if (move == 1 && (double) astrocaster3.currentFishStamina >= (double) astrocaster3.struggleStaminaCost)
          astrocaster3.StartCoroutine((IEnumerator) astrocaster3.FishStruggle());
        else if (move == 2 && (double) astrocaster3.currentFishStamina >= (double) astrocaster3.currentFish.lightPull.staminaCost)
          astrocaster3.StartCoroutine((IEnumerator) astrocaster3.FishAttack(astrocaster3.currentFish.lightPull));
        else if (move == 3 && (double) astrocaster3.currentFishStamina >= (double) astrocaster3.currentFish.heavyPull.staminaCost)
        {
          astrocaster3.StartCoroutine((IEnumerator) astrocaster3.FishAttack(astrocaster3.currentFish.heavyPull));
        }
        else
        {
          float num = 0.0f;
          foreach (AC3Attack ac3Attack in astrocaster3.currentFish.combo)
            num += ac3Attack.staminaCost;
          if ((double) astrocaster3.currentFishStamina >= (double) num)
            astrocaster3.StartCoroutine((IEnumerator) astrocaster3.FishCombo(astrocaster3.currentFish.combo));
        }
      }
    }
  }

  private IEnumerator FishCombo(List<AC3Attack> combo)
  {
    Astrocaster3 astrocaster3 = this;
    astrocaster3.isFishComboing = true;
    foreach (AC3Attack attack in combo)
    {
      astrocaster3.StartCoroutine((IEnumerator) astrocaster3.FishAttack(attack));
      yield return (object) new WaitForSeconds(attack.pullTime + attack.waitTime + attack.pushTime);
    }
    astrocaster3.isFishComboing = false;
  }

  private IEnumerator FishAttack(AC3Attack attack)
  {
    this.sfx.Play("nibble");
    this.isFishBusy = true;
    this.currentFishStamina -= attack.staminaCost;
    LeanTween.moveLocal(this.fishIcon.gameObject, this.fishIconStartPos + Vector3.up * attack.pullDist, attack.pullTime).setEase(LeanTweenType.easeOutQuart);
    yield return (object) new WaitForSeconds(attack.pullTime);
    yield return (object) new WaitForSeconds(attack.waitTime);
    bool isParried = false;
    float attackStartTime = Time.time;
    LeanTween.moveLocal(this.fishIcon.gameObject, this.fishIconStartPos, attack.pushTime).setEase(LeanTweenType.linear).setOnComplete((Action) (() =>
    {
      if (isParried)
        return;
      Debug.Log((object) "Push animation finished naturally.");
    }));
    while ((double) Time.time < (double) attackStartTime + (double) attack.pushTime)
    {
      if (Input.GetKeyDown(KeyCode.Q) && !this.isPlayerBusy && (double) this.currentPlayerStamina > 1.0)
      {
        Debug.Log((object) "Parry Successful!");
        this.currentLineProgress -= attack.progressChange * 1.5f;
        this.currentLineProgress = Mathf.Clamp(this.currentLineProgress, 0.0f, this.maxLineProgress);
        this.notifs.NewNotif("[ASTROCASTER] PARRIED!", "blue");
        isParried = true;
        this.currentPlayerStamina += this.blockStaminaCost * 2f;
        LeanTween.cancel(this.fishIcon.gameObject);
        this.fishIcon.localPosition = this.fishIconStartPos;
        this.sfx.Play("coin");
        this.parryPS.Play();
        break;
      }
      yield return (object) null;
    }
    if (!isParried)
    {
      float seconds = attackStartTime + attack.pushTime - Time.time;
      if ((double) seconds > 0.0)
        yield return (object) new WaitForSeconds(seconds);
    }
    if (!isParried)
    {
      if (this.isBlocking)
      {
        Debug.Log((object) "Blocked Fish Attack!");
        this.notifs.NewNotif("[ASTROCASTER] BLOCKED!", "green");
        this.currentLineHealth -= attack.lineDamage * 0.2f;
        this.currentLineProgress += attack.progressChange * 0.2f;
        this.sfx.Play("win1");
      }
      else
      {
        this.currentLineHealth -= attack.lineDamage;
        this.currentLineProgress += attack.progressChange;
        this.sfx.Play("lose");
      }
    }
    this.currentLineProgress = Mathf.Clamp(this.currentLineProgress, 0.0f, this.maxLineProgress);
    this.isFishBusy = false;
  }

  private IEnumerator FishStruggle()
  {
    this.isFishBusy = true;
    this.isStruggling = true;
    this.currentFishStamina -= this.struggleStaminaCost;
    this.struggleTweenId = LeanTween.moveLocalX(this.fishIcon.gameObject, this.fishIconStartPos.x + this.struggleShakeMagnitude, this.struggleShakeSpeed).setEase(LeanTweenType.linear).setLoopPingPong().uniqueId;
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(this.struggleRange.x, this.struggleRange.y));
    LeanTween.cancel(this.struggleTweenId);
    this.fishIcon.localPosition = this.fishIconStartPos;
    this.isStruggling = false;
    this.isFishBusy = false;
  }

  private void NewDay()
  {
    this.exchangeRate = UnityEngine.Random.Range(10, 101);
    this.UpdateUI();
  }

  public void BuyCoins(int amount)
  {
    if (this.factions.playerFaction.credits >= amount * this.exchangeRate)
    {
      this.factions.ModPlayerCredits("[ASTROCASTER] Buying astrocoins", -amount * this.exchangeRate);
      this.astrocoins += amount;
    }
    else
      this.notifs.NewNotif("[ASTROCASTER] Insufficient funds!", "orange");
    this.UpdateUI();
  }

  public void CashOut()
  {
    int credits = this.factions.playerFaction.credits;
    if (this.astrocoins > 0)
    {
      this.factions.ModPlayerCredits("[ASTROCASTER] CASHING OUT!", this.astrocoins * this.exchangeRate);
      this.astrocoins = 0;
    }
    this.UpdateUI();
    this.SetOff();
  }

  private void AddFishButtonToGrid(Astrofish3 fish)
  {
    UnityEngine.Object.Instantiate<FishButton>(this.fishButtonPF, this.grid).Setup(fish);
  }

  private void DiscoverFish(Astrofish3 newFish)
  {
    if (this.fishesDiscovered.Contains(newFish.fishName))
      return;
    this.fishesDiscovered.Add(newFish.fishName);
    IEnumerator enumerator = (IEnumerator) this.grid.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if ((bool) (UnityEngine.Object) current.GetComponent<FishButton>().fish && current.GetComponent<FishButton>().fish.fishName == newFish.fishName)
          current.GetComponent<FishButton>().Uncover();
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
  }

  public void Toggle()
  {
    this.isOn = !this.isOn;
    if (this.isOn)
      this.SetOn();
    else
      this.SetOff();
  }

  private void SetOn()
  {
    this.panel.Show();
    this.sfx.Play("start");
  }

  public void SetOff()
  {
    this.isOn = false;
    this.panel.Hide();
    this.sfx.Play("exit");
  }

  public void SelectFish(Astrofish3 fish)
  {
    this.selectedFish = fish;
    this.selectedIcon.sprite = fish.icon;
    this.selectedName.text = fish.fishName;
    this.selectedDescription.text = fish.description;
    this.selectedPrice.text = (fish.basePrice * 2).ToString("#,0") + " AC";
    if (this.selectedFish.catches.Count == 0)
      this.selectedBuyButton.SetActive(false);
    else
      this.selectedBuyButton.SetActive(true);
    IEnumerator enumerator = (IEnumerator) this.selectedCatchList.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
        UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator.Current).gameObject);
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    MonoBehaviour.print((object) "showing catches");
    foreach (CatchChance3 catchChance3 in this.selectedFish.catches)
    {
      if (this.fishesDiscovered.Contains(catchChance3.fish.fishName))
      {
        MonoBehaviour.print((object) "discovered!");
        UnityEngine.Object.Instantiate<FishCatchUI>(this.fishCatchUIPF, this.selectedCatchList).Setup(catchChance3.fish.icon, catchChance3.fish.fishName, catchChance3.chance);
      }
      else
      {
        MonoBehaviour.print((object) "not discovered!");
        UnityEngine.Object.Instantiate<FishCatchUI>(this.fishCatchUIPF, this.selectedCatchList).Setup((Sprite) null, "Unknown", 0);
      }
    }
    this.UpdateUI();
  }

  private void ShowCaughtFishInfo(Astrofish3 fish)
  {
    this.caughtPanel.SetActive(true);
    this.caughtIcon.sprite = fish.icon;
    this.caughtName.text = fish.fishName;
    this.caughtDescription.text = fish.description;
    this.caughtPrice.text = fish.basePrice.ToString("#,0") + " AC";
    if (fish.catches.Count == 0)
      this.caughtBuybutton.SetActive(false);
    else
      this.caughtBuybutton.SetActive(true);
    IEnumerator enumerator = (IEnumerator) this.caughtCatchList.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
        UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator.Current).gameObject);
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    foreach (CatchChance3 catchChance3 in fish.catches)
    {
      if (this.fishesDiscovered.Contains(catchChance3.fish.fishName))
        UnityEngine.Object.Instantiate<FishCatchUI>(this.fishCatchUIPF, this.caughtCatchList).Setup(catchChance3.fish.icon, catchChance3.fish.fishName, catchChance3.chance);
      else
        UnityEngine.Object.Instantiate<FishCatchUI>(this.fishCatchUIPF, this.caughtCatchList).Setup((Sprite) null, "Unknown", 0);
    }
  }

  private void UpdateUI()
  {
    if ((bool) (UnityEngine.Object) this.selectedFish)
      this.selectedPanel.SetActive(true);
    else
      this.selectedPanel.SetActive(false);
    this.exchangeRateText.text = $"1 AC = {this.exchangeRate.ToString("#,0")}cr";
    this.astrocoinsText.text = this.astrocoins.ToString("#,0");
    this.cashoutText.text = $"CASH OUT: {(this.astrocoins * this.exchangeRate).ToString("#,0")}cr";
    if ((bool) (UnityEngine.Object) this.currentBait)
    {
      this.currentBaitImg.gameObject.SetActive(true);
      this.currentBaitImg.sprite = this.currentBait.icon;
      this.castPanel.SetActive(!this.isFishing);
      this.selectedPanel.SetActive(false);
      this.caughtPanel.SetActive(false);
    }
    else
    {
      this.currentBaitImg.gameObject.SetActive(false);
      this.castPanel.SetActive(false);
    }
    if ((bool) (UnityEngine.Object) this.caughtFish)
      this.ShowCaughtFishInfo(this.caughtFish);
    else
      this.caughtPanel.SetActive(false);
  }

  public void UseSelectedFishAsBait()
  {
    int num = this.selectedFish.basePrice * 2;
    if (num > this.astrocoins || (bool) (UnityEngine.Object) this.caughtFish || (bool) (UnityEngine.Object) this.currentBait)
    {
      this.sfx.Play("lose");
    }
    else
    {
      this.currentBait = this.selectedFish;
      this.astrocoins -= num;
      this.UpdateUI();
    }
  }

  public void UseCaughtFishAsBait()
  {
    if (this.caughtFish.rarity == FishRarity.LEGENDARY)
    {
      this.sfx.Play("lose");
    }
    else
    {
      this.currentBait = this.caughtFish;
      this.caughtFish = (Astrofish3) null;
      this.SelectFish(this.currentBait);
      this.UpdateUI();
    }
  }

  public void SellCaughtFish()
  {
    this.astrocoins += this.caughtFish.basePrice;
    this.caughtFish = (Astrofish3) null;
    this.UpdateUI();
  }
}
