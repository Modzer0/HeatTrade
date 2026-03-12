// Decompiled with JetBrains decompiler
// Type: SquadronDataUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SquadronDataUI : MonoBehaviour
{
  private Squadron currentSquadron;
  [Header("UI")]
  [SerializeField]
  private TMP_Text targetText;
  [SerializeField]
  private TMP_Text commandText;
  [SerializeField]
  private TMP_Text dispersionText;
  [SerializeField]
  private Slider dispersionSlider;
  [SerializeField]
  private Transform orderDelayImg;

  private void Start()
  {
  }

  private void Update() => this.UpdateSquadronData();

  public void SetSquadron(Squadron newSquadron)
  {
    this.currentSquadron = newSquadron;
    this.dispersionSlider.value = (float) (this.currentSquadron.dispersion / 100);
    this.UpdateSquadronData();
  }

  private void UpdateSquadronData()
  {
    this.targetText.text = !(bool) (Object) this.currentSquadron.targetTrack ? "NULL" : this.currentSquadron.targetTrack.trackName;
    this.commandText.text = this.currentSquadron.command == SquadronCommand.NULL ? "NULL" : this.currentSquadron.command.ToString();
    this.dispersionText.text = this.dispersionSlider.value.ToString("F0") + "km";
    if (this.currentSquadron.isTakingNewOrders)
    {
      this.orderDelayImg.gameObject.SetActive(true);
      this.orderDelayImg.Rotate(-Vector3.forward, Time.deltaTime * 360f);
    }
    else
    {
      this.orderDelayImg.gameObject.SetActive(false);
      this.orderDelayImg.rotation = Quaternion.identity;
    }
  }

  public void SetDispersion()
  {
    if (!(bool) (Object) this.currentSquadron)
      return;
    this.currentSquadron.SetDispersion((int) this.dispersionSlider.value * 100);
  }
}
