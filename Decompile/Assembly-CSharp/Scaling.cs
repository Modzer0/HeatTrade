// Decompiled with JetBrains decompiler
// Type: Scaling
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Scaling : MonoBehaviour
{
  public static Scaling current;
  public float toKm = 1000f;
  public float gAccDay = 73234.4f;

  private void Awake()
  {
    if ((Object) Scaling.current != (Object) null && (Object) Scaling.current != (Object) this)
      Object.Destroy((Object) this.gameObject);
    else
      Scaling.current = this;
  }

  private void Start()
  {
  }

  private void Update()
  {
  }
}
