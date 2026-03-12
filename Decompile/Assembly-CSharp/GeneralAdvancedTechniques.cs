// Decompiled with JetBrains decompiler
// Type: GeneralAdvancedTechniques
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class GeneralAdvancedTechniques : MonoBehaviour
{
  public GameObject avatarRecursive;
  public GameObject avatar2dRecursive;
  public RectTransform wingPersonPanel;
  public RectTransform textField;
  public GameObject avatarMove;
  public Transform[] movePts;
  public GameObject[] avatarSpeed;
  public GameObject[] avatarSpeed2;
  private Vector3[] circleSm = new Vector3[32 /*0x20*/]
  {
    new Vector3(16f, 0.0f, 0.0f),
    new Vector3(14.56907f, 8.009418f, 0.0f),
    new Vector3(15.96541f, 4.638379f, 0.0f),
    new Vector3(11.31371f, 11.31371f, 0.0f),
    new Vector3(11.31371f, 11.31371f, 0.0f),
    new Vector3(4.638379f, 15.96541f, 0.0f),
    new Vector3(8.009416f, 14.56908f, 0.0f),
    new Vector3(-6.993822E-07f, 16f, 0.0f),
    new Vector3(-6.993822E-07f, 16f, 0.0f),
    new Vector3(-8.009419f, 14.56907f, 0.0f),
    new Vector3(-4.63838f, 15.9654f, 0.0f),
    new Vector3(-11.31371f, 11.31371f, 0.0f),
    new Vector3(-11.31371f, 11.31371f, 0.0f),
    new Vector3(-15.9654f, 4.63838f, 0.0f),
    new Vector3(-14.56908f, 8.009415f, 0.0f),
    new Vector3(-16f, -1.398764E-06f, 0.0f),
    new Vector3(-16f, -1.398764E-06f, 0.0f),
    new Vector3(-14.56907f, -8.009418f, 0.0f),
    new Vector3(-15.9654f, -4.638382f, 0.0f),
    new Vector3(-11.31371f, -11.31371f, 0.0f),
    new Vector3(-11.31371f, -11.31371f, 0.0f),
    new Vector3(-4.638381f, -15.9654f, 0.0f),
    new Vector3(-8.009413f, -14.56908f, 0.0f),
    new Vector3(1.907981E-07f, -16f, 0.0f),
    new Vector3(1.907981E-07f, -16f, 0.0f),
    new Vector3(8.00942f, -14.56907f, 0.0f),
    new Vector3(4.638381f, -15.9654f, 0.0f),
    new Vector3(11.31371f, -11.3137f, 0.0f),
    new Vector3(11.31371f, -11.3137f, 0.0f),
    new Vector3(15.96541f, -4.638378f, 0.0f),
    new Vector3(14.56907f, -8.009418f, 0.0f),
    new Vector3(16f, 2.797529E-06f, 0.0f)
  };
  private Vector3[] circleLrg = new Vector3[32 /*0x20*/]
  {
    new Vector3(25f, 0.0f, 0.0f),
    new Vector3(22.76418f, 12.51472f, 0.0f),
    new Vector3(24.94595f, 7.247467f, 0.0f),
    new Vector3(17.67767f, 17.67767f, 0.0f),
    new Vector3(17.67767f, 17.67767f, 0.0f),
    new Vector3(7.247467f, 24.94595f, 0.0f),
    new Vector3(12.51471f, 22.76418f, 0.0f),
    new Vector3(-1.092785E-06f, 25f, 0.0f),
    new Vector3(-1.092785E-06f, 25f, 0.0f),
    new Vector3(-12.51472f, 22.76418f, 0.0f),
    new Vector3(-7.247468f, 24.94594f, 0.0f),
    new Vector3(-17.67767f, 17.67767f, 0.0f),
    new Vector3(-17.67767f, 17.67767f, 0.0f),
    new Vector3(-24.94594f, 7.247468f, 0.0f),
    new Vector3(-22.76418f, 12.51471f, 0.0f),
    new Vector3(-25f, -2.185569E-06f, 0.0f),
    new Vector3(-25f, -2.185569E-06f, 0.0f),
    new Vector3(-22.76418f, -12.51472f, 0.0f),
    new Vector3(-24.94594f, -7.247472f, 0.0f),
    new Vector3(-17.67767f, -17.67767f, 0.0f),
    new Vector3(-17.67767f, -17.67767f, 0.0f),
    new Vector3(-7.247469f, -24.94594f, 0.0f),
    new Vector3(-12.51471f, -22.76418f, 0.0f),
    new Vector3(2.98122E-07f, -25f, 0.0f),
    new Vector3(2.98122E-07f, -25f, 0.0f),
    new Vector3(12.51472f, -22.76418f, 0.0f),
    new Vector3(7.24747f, -24.94594f, 0.0f),
    new Vector3(17.67768f, -17.67766f, 0.0f),
    new Vector3(17.67768f, -17.67766f, 0.0f),
    new Vector3(24.94595f, -7.247465f, 0.0f),
    new Vector3(22.76418f, -12.51472f, 0.0f),
    new Vector3(25f, 4.371139E-06f, 0.0f)
  };

  private void Start()
  {
    LeanTween.alpha(this.avatarRecursive, 0.0f, 1f).setRecursive(true).setLoopPingPong();
    LeanTween.alpha(this.avatar2dRecursive, 0.0f, 1f).setRecursive(true).setLoopPingPong();
    LeanTween.alpha(this.wingPersonPanel, 0.0f, 1f).setRecursive(true).setLoopPingPong();
    LeanTween.value(this.avatarMove, 0.0f, (float) this.movePts.Length - 1f, 5f).setOnUpdate((Action<float>) (val =>
    {
      int index1 = (int) Mathf.Floor(val);
      int index2 = index1 < this.movePts.Length - 1 ? index1 + 1 : index1;
      float num = val - (float) index1;
      Vector3 vector3 = this.movePts[index2].position - this.movePts[index1].position;
      this.avatarMove.transform.position = this.movePts[index1].position + vector3 * num;
    })).setEase(LeanTweenType.easeInOutExpo).setLoopPingPong();
    for (int index = 0; index < this.movePts.Length; ++index)
      LeanTween.moveY(this.movePts[index].gameObject, this.movePts[index].position.y + 1.5f, 1f).setDelay((float) index * 0.2f).setLoopPingPong();
    for (int index = 0; index < this.avatarSpeed.Length; ++index)
      LeanTween.moveLocalZ(this.avatarSpeed[index], (float) (index + 1) * 5f, 1f).setSpeed(6f).setEase(LeanTweenType.easeInOutExpo).setLoopPingPong();
    for (int index = 0; index < this.avatarSpeed2.Length; ++index)
      LeanTween.moveLocal(this.avatarSpeed2[index], index == 0 ? this.circleSm : this.circleLrg, 1f).setSpeed(20f).setRepeat(-1);
  }
}
