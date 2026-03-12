// Decompiled with JetBrains decompiler
// Type: Subsystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Subsystem : MonoBehaviour, IClickable
{
  private void Start()
  {
  }

  private void Update()
  {
  }

  public void LeftClick() => MonoBehaviour.print((object) "LEFT CLICKED");

  public void RightClick() => MonoBehaviour.print((object) nameof (RightClick));

  public void MidClick() => MonoBehaviour.print((object) nameof (MidClick));

  public void OnEnter() => MonoBehaviour.print((object) nameof (OnEnter));

  public void OnExit() => MonoBehaviour.print((object) nameof (OnExit));

  public void ScrollUp() => MonoBehaviour.print((object) nameof (ScrollUp));

  public void ScrollDown() => MonoBehaviour.print((object) nameof (ScrollDown));
}
