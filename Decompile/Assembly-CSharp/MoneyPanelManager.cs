// Decompiled with JetBrains decompiler
// Type: MoneyPanelManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class MoneyPanelManager : MonoBehaviour
{
  private FactionsManager fm;
  private AudioManager am;
  [SerializeField]
  private TMP_Text moneyText;
  private float currentCredits;
  private float targetCredits;

  private void Start()
  {
    this.fm = FactionsManager.current;
    this.am = AudioManager.current;
  }

  private void Update()
  {
    this.targetCredits = (float) this.fm.playerFaction.credits;
    if ((double) this.currentCredits != (double) this.targetCredits)
    {
      this.currentCredits = Mathf.Lerp(this.currentCredits, this.targetCredits, 0.1f);
      if ((double) Mathf.Abs(this.currentCredits - this.targetCredits) < 2.0)
        this.currentCredits = this.targetCredits;
      this.moneyText.text = (double) this.currentCredits <= 1000000000.0 ? this.currentCredits.ToString("#,0") : 1E+09f.ToString("#,0");
      this.am.PlayMoney((double) this.currentCredits > (double) this.targetCredits);
    }
    else
      this.am.StopMoney();
  }
}
