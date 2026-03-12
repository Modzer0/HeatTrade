// Decompiled with JetBrains decompiler
// Type: ExhaustPlume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ExhaustPlume : MonoBehaviour
{
  private ExhaustPlumes ep;
  private int factionID;
  private int damage = 5000;
  public bool isFriendlyFire;

  private void Start()
  {
    this.factionID = this.transform.root.GetComponent<Track>().factionID;
    this.ep = ExhaustPlumes.current;
    if (!(bool) (Object) this.ep)
      return;
    this.ep.Add(this);
  }

  private void OnTriggerStay(Collider other)
  {
    if (other.GetComponent<IHealth>() == null || !this.isFriendlyFire && other.transform.root.GetComponent<Track>().factionID == this.factionID)
      return;
    other.GetComponent<IHealth>().TryDamagePhotonic((float) this.damage);
  }
}
