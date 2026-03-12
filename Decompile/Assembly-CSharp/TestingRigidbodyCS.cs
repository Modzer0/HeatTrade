// Decompiled with JetBrains decompiler
// Type: TestingRigidbodyCS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TestingRigidbodyCS : MonoBehaviour
{
  private GameObject ball1;

  private void Start()
  {
    this.ball1 = GameObject.Find("Sphere1");
    LeanTween.rotateAround(this.ball1, Vector3.forward, -90f, 1f);
    LeanTween.move(this.ball1, new Vector3(2f, 0.0f, 7f), 1f).setDelay(1f).setRepeat(-1);
  }

  private void Update()
  {
  }
}
