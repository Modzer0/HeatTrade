// Decompiled with JetBrains decompiler
// Type: FaderManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FaderManager : MonoBehaviour
{
  [SerializeField]
  private Transform faderPanel;
  [SerializeField]
  private Transform transactionsPanel;
  [SerializeField]
  private Fader faderPF;
  [SerializeField]
  private Fader transactionPF;

  public void NewFader(Vector3 worldPosition, string header, string body)
  {
    Fader component = Object.Instantiate<Fader>(this.faderPF, this.faderPanel).GetComponent<Fader>();
    component.Setup(worldPosition, true, header, body);
    component.gameObject.SetActive(true);
  }

  public void NewFaderUI(Vector3 screenPosition, string header, string body, bool isFloat)
  {
    Fader component = Object.Instantiate<Fader>(this.faderPF, this.faderPanel).GetComponent<Fader>();
    component.isFloat = isFloat;
    component.SetupUI(screenPosition, header, body);
    component.gameObject.SetActive(true);
  }

  public void NewTransaction(string header, string body)
  {
    this.StartCoroutine((IEnumerator) this.SetupTransaction(Object.Instantiate<Fader>(this.transactionPF, this.transactionsPanel).GetComponent<Fader>(), header, body));
  }

  public void NewTransaction(string header, string body, Color color)
  {
    this.StartCoroutine((IEnumerator) this.SetupTransactionColor(Object.Instantiate<GameObject>(this.transactionPF.gameObject, this.transactionsPanel).GetComponent<Fader>(), header, body, color));
  }

  private IEnumerator SetupTransaction(Fader fader, string header, string body)
  {
    yield return (object) new WaitForSeconds(0.1f);
    fader.Setup(Vector3.zero, false, header, body);
    fader.gameObject.SetActive(true);
  }

  private IEnumerator SetupTransactionColor(Fader fader, string header, string body, Color color)
  {
    yield return (object) new WaitForSeconds(0.1f);
    fader.Setup(Vector3.zero, false, header, body);
    fader.SetColor(color);
    fader.gameObject.SetActive(true);
  }
}
