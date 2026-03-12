// Decompiled with JetBrains decompiler
// Type: Manager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Manager : MonoBehaviour
{
  private static Manager instance;
  public Transform[] Waypoints1;

  public static Manager GetInstance() => Manager.instance;

  private void Awake() => Manager.instance = this;

  private void Start()
  {
  }

  private void Update()
  {
  }
}
