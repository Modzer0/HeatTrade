// Decompiled with JetBrains decompiler
// Type: uiMover
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class uiMover : MonoBehaviour
{
  [SerializeField]
  private float moveTime = 0.5f;
  [SerializeField]
  private LeanTweenType moveEase = LeanTweenType.linear;

  public void MoveTo(Vector2 targetPos)
  {
    LeanTween.moveLocal(this.gameObject, (Vector3) targetPos, this.moveTime).setEase(this.moveEase);
  }
}
