// Decompiled with JetBrains decompiler
// Type: BootstrapManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class BootstrapManager : MonoBehaviour
{
  public void Scene1() => SceneManager.LoadScene("Example Scene");

  public void Scene2() => SceneManager.LoadScene("Example1 Scene");
}
