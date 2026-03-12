// Decompiled with JetBrains decompiler
// Type: LTUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class LTUtility
{
  public static Vector3[] reverse(Vector3[] arr)
  {
    int length = arr.Length;
    int index1 = 0;
    for (int index2 = length - 1; index1 < index2; --index2)
    {
      Vector3 vector3 = arr[index1];
      arr[index1] = arr[index2];
      arr[index2] = vector3;
      ++index1;
    }
    return arr;
  }
}
