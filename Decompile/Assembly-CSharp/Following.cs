// Decompiled with JetBrains decompiler
// Type: Following
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Following : MonoBehaviour
{
  public Transform planet;
  public Transform followArrow;
  public Transform dude1;
  public Transform dude2;
  public Transform dude3;
  public Transform dude4;
  public Transform dude5;
  public Transform dude1Title;
  public Transform dude2Title;
  public Transform dude3Title;
  public Transform dude4Title;
  public Transform dude5Title;
  private Color dude1ColorVelocity;
  private Vector3 velocityPos;
  private float fromY;
  private float velocityY;
  private Vector3 fromVec3;
  private Vector3 velocityVec3;
  private Color fromColor;
  private Color velocityColor;

  private void Start()
  {
    this.followArrow.gameObject.LeanDelayedCall(3f, new Action(this.moveArrow)).setOnStart(new Action(this.moveArrow)).setRepeat(-1);
    LeanTween.followDamp(this.dude1, this.followArrow, LeanProp.localY, 1.1f);
    LeanTween.followSpring(this.dude2, this.followArrow, LeanProp.localY, 1.1f);
    LeanTween.followBounceOut(this.dude3, this.followArrow, LeanProp.localY, 1.1f);
    LeanTween.followSpring(this.dude4, this.followArrow, LeanProp.localY, 1.1f, friction: 1.5f, accelRate: 0.8f);
    LeanTween.followLinear(this.dude5, this.followArrow, LeanProp.localY, 50f);
    LeanTween.followDamp(this.dude1, this.followArrow, LeanProp.color, 1.1f);
    LeanTween.followSpring(this.dude2, this.followArrow, LeanProp.color, 1.1f);
    LeanTween.followBounceOut(this.dude3, this.followArrow, LeanProp.color, 1.1f);
    LeanTween.followSpring(this.dude4, this.followArrow, LeanProp.color, 1.1f, friction: 1.5f, accelRate: 0.8f);
    LeanTween.followLinear(this.dude5, this.followArrow, LeanProp.color, 0.5f);
    LeanTween.followDamp(this.dude1, this.followArrow, LeanProp.scale, 1.1f);
    LeanTween.followSpring(this.dude2, this.followArrow, LeanProp.scale, 1.1f);
    LeanTween.followBounceOut(this.dude3, this.followArrow, LeanProp.scale, 1.1f);
    LeanTween.followSpring(this.dude4, this.followArrow, LeanProp.scale, 1.1f, friction: 1.5f, accelRate: 0.8f);
    LeanTween.followLinear(this.dude5, this.followArrow, LeanProp.scale, 5f);
    Vector3 offset = new Vector3(0.0f, -20f, -18f);
    LeanTween.followDamp(this.dude1Title, this.dude1, LeanProp.localPosition, 0.6f).setOffset(offset);
    LeanTween.followSpring(this.dude2Title, this.dude2, LeanProp.localPosition, 0.6f).setOffset(offset);
    LeanTween.followBounceOut(this.dude3Title, this.dude3, LeanProp.localPosition, 0.6f).setOffset(offset);
    LeanTween.followSpring(this.dude4Title, this.dude4, LeanProp.localPosition, 0.6f, friction: 1.5f, accelRate: 0.8f).setOffset(offset);
    LeanTween.followLinear(this.dude5Title, this.dude5, LeanProp.localPosition, 30f).setOffset(offset);
    Vector3 point = Camera.main.transform.InverseTransformPoint(this.planet.transform.position);
    LeanTween.rotateAround(Camera.main.gameObject, Vector3.left, 360f, 300f).setPoint(point).setRepeat(-1);
  }

  private void Update()
  {
    this.fromY = LeanSmooth.spring(this.fromY, this.followArrow.localPosition.y, ref this.velocityY, 1.1f);
    this.fromVec3 = LeanSmooth.spring(this.fromVec3, this.dude5Title.localPosition, ref this.velocityVec3, 1.1f);
    this.fromColor = LeanSmooth.spring(this.fromColor, this.dude1.GetComponent<Renderer>().material.color, ref this.velocityColor, 1.1f);
    Debug.Log((object) $"Smoothed y:{this.fromY.ToString()} vec3:{this.fromVec3.ToString()} color:{this.fromColor.ToString()}");
  }

  private void moveArrow()
  {
    LeanTween.moveLocalY(this.followArrow.gameObject, UnityEngine.Random.Range(-100f, 100f), 0.0f);
    LeanTween.color(this.followArrow.gameObject, new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value), 0.0f);
    this.followArrow.localScale = Vector3.one * UnityEngine.Random.Range(5f, 10f);
  }
}
