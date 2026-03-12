// Decompiled with JetBrains decompiler
// Type: LeanTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class LeanTester : MonoBehaviour
{
  public float timeout = 15f;

  public void Start() => this.StartCoroutine((IEnumerator) this.timeoutCheck());

  private IEnumerator timeoutCheck()
  {
    float pauseEndTime = Time.realtimeSinceStartup + this.timeout;
    while ((double) Time.realtimeSinceStartup < (double) pauseEndTime)
      yield return (object) 0;
    if (!LeanTest.testsFinished)
    {
      Debug.Log((object) LeanTest.formatB("Tests timed out!"));
      LeanTest.overview();
    }
  }
}
