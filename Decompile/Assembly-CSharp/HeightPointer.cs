// Decompiled with JetBrains decompiler
// Type: HeightPointer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HeightPointer : MonoBehaviour
{
  public static HeightPointer current;
  private TacticalInputs ti;
  private bool isMoveOrderOn;
  [SerializeField]
  private Borderline borderline;
  [SerializeField]
  private Borderline borderlineWhite;
  private float heightPoint;
  [SerializeField]
  private List<HeightLine> heightLines;

  private void Awake()
  {
    if (!((UnityEngine.Object) HeightPointer.current == (UnityEngine.Object) null))
      return;
    HeightPointer.current = this;
  }

  private void Start()
  {
    this.ti = TacticalInputs.current;
    this.ti.toggleMoveOrder += new Action(this.ToggleMoveOrder);
    this.SetHeightPoint(0.0f);
    foreach (HeightLine heightLine in UnityEngine.Object.FindObjectsOfType<HeightLine>())
      this.heightLines.Add(heightLine);
  }

  private void ToggleMoveOrder()
  {
    this.isMoveOrderOn = !this.isMoveOrderOn;
    foreach (HeightLine heightLine in this.heightLines)
    {
      if (heightLine.onlyShowWhenMove)
        heightLine.isShow = this.isMoveOrderOn;
    }
  }

  public void SetHeightPoint(float newHeightPoint)
  {
    this.heightPoint = newHeightPoint;
    foreach (HeightLine heightLine in this.heightLines)
      heightLine.heightPoint = this.heightPoint;
    this.borderline.UpdateHeight(this.heightPoint);
    this.borderlineWhite.UpdateHeight(this.heightPoint);
  }
}
