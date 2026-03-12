// Decompiled with JetBrains decompiler
// Type: LoadImg
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LoadImg : MonoBehaviour
{
  private SaveLoadSystem sls;
  private MainMenuSaveManager mmsm;
  public TMP_Text nameText;
  public TMP_Text dateText;
  [SerializeField]
  private Color outdatedBG;
  [SerializeField]
  private Color outdatedName;
  [SerializeField]
  private Color outdatedDate;

  private void Start()
  {
    this.sls = SaveLoadSystem.current;
    this.mmsm = MainMenuSaveManager.current;
  }

  public void Load()
  {
    MonoBehaviour.print((object) "loading game from load img");
    if ((bool) (Object) this.sls)
    {
      this.sls.LoadGame(this.nameText.text);
    }
    else
    {
      if (!(bool) (Object) this.mmsm)
        return;
      this.mmsm.LoadGame(this.nameText.text);
    }
  }

  public void Delete()
  {
    if ((bool) (Object) this.sls)
    {
      this.sls.DeleteSave(this.nameText.text);
    }
    else
    {
      if (!(bool) (Object) this.mmsm)
        return;
      this.mmsm.DeleteSave(this.nameText.text);
    }
  }

  public void SetOutdated()
  {
    this.GetComponent<Image>().color = this.outdatedBG;
    this.nameText.color = this.outdatedName;
    this.dateText.color = this.outdatedDate;
  }
}
