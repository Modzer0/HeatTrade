// Decompiled with JetBrains decompiler
// Type: ShipInfoManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ShipInfoManager : MonoBehaviour
{
  public static ShipInfoManager current;
  [SerializeField]
  private Canvas canvas;
  [SerializeField]
  private ShipInfoUI shipInfoUIPF;

  private void Awake() => ShipInfoManager.current = this;

  public void NewShipInfo(S_Ship s)
  {
    ShipInfoUI shipInfoUi = Object.Instantiate<ShipInfoUI>(this.shipInfoUIPF, this.canvas.transform);
    shipInfoUi.Setup(s);
    shipInfoUi.gameObject.SetActive(true);
  }

  public void DestroyMe(ShipInfoUI shipInfoUI) => Object.Destroy((Object) shipInfoUI.gameObject);
}
