// Decompiled with JetBrains decompiler
// Type: FishButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FishButton : MonoBehaviour
{
  private Astrocaster3 astrocaster;
  private AstrocasterSFX sfx;
  public Astrofish3 fish;
  [SerializeField]
  private Image image;
  [SerializeField]
  private GameObject cover;
  private bool isUncovered;

  private void Start()
  {
    this.astrocaster = Astrocaster3.current;
    this.sfx = Object.FindObjectOfType<AstrocasterSFX>();
    if (this.isUncovered)
      return;
    this.GetComponent<Button>().interactable = false;
  }

  public void Setup(Astrofish3 newFish)
  {
    this.fish = newFish;
    this.image.sprite = this.fish.icon;
  }

  public void OnClick()
  {
    this.astrocaster.SelectFish(this.fish);
    this.sfx.Play("click");
  }

  public void Uncover()
  {
    this.GetComponent<Image>().enabled = true;
    this.GetComponent<Button>().interactable = true;
    this.isUncovered = true;
  }
}
