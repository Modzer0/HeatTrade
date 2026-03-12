// Decompiled with JetBrains decompiler
// Type: Sensors
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Sensors : MonoBehaviour
{
  public static Sensors current;
  public List<GameObject> tracks = new List<GameObject>();
  public List<GameObject> tracksThisFrame = new List<GameObject>();
  public List<GameObject> untracksThisFrame = new List<GameObject>();
  private float lastClearTime;
  private float clearFreq = 0.1f;
  [SerializeField]
  private Color colorFriendly;
  [SerializeField]
  private Color colorNeutral;
  [SerializeField]
  private Color colorUnknown;
  [SerializeField]
  private Color colorHostile;
  [SerializeField]
  private GameObject uiTrackPF;
  [SerializeField]
  private GameObject uiTracks;
  [SerializeField]
  private Transform playerShip;
  public Transform selected;
  [SerializeField]
  private GameObject uiSelectedPanel;
  [SerializeField]
  private TMP_Text uiSelectedName;
  [SerializeField]
  private TMP_Text uiSelectedIFF;
  [SerializeField]
  private TMP_Text uiSelectedDist;
  [SerializeField]
  private TMP_Text uiSelectedRelVel;

  private void Awake() => Sensors.current = this;

  private void Start()
  {
  }

  private void Update()
  {
    if ((double) Time.time - (double) this.lastClearTime >= (double) this.clearFreq)
    {
      this.tracksThisFrame.Clear();
      this.untracksThisFrame.Clear();
      this.UpdateUiDists();
      this.lastClearTime = Time.time;
    }
    foreach (GameObject track in this.tracksThisFrame)
    {
      if (!this.tracks.Contains(track))
        this.AddTrack(track);
    }
    foreach (GameObject untrack in this.untracksThisFrame)
    {
      if (this.tracks.Contains(untrack))
        this.RemoveTrack(untrack);
    }
    this.UpdateSelectedUI();
  }

  private Color GetIFFColor(int iff)
  {
    Color iffColor = this.colorUnknown;
    switch (iff)
    {
      case 0:
        iffColor = this.colorFriendly;
        break;
      case 1:
        iffColor = this.colorNeutral;
        break;
      case 3:
        iffColor = this.colorHostile;
        break;
    }
    iffColor.a = 0.59f;
    return iffColor;
  }

  private string GetIFFText(int iff)
  {
    string iffText = "Unknown";
    switch (iff)
    {
      case 0:
        iffText = "Friendly";
        break;
      case 1:
        iffText = "Neutral";
        break;
      case 3:
        iffText = "Hostile";
        break;
    }
    return iffText;
  }

  private void UpdateUiDists()
  {
    IEnumerator enumerator = (IEnumerator) this.uiTracks.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        foreach (GameObject track in this.tracks)
        {
          if (track.GetComponent<Target>().targetName == current.name)
            break;
        }
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
  }

  private void RefreshUiTracks()
  {
    IEnumerator enumerator = (IEnumerator) this.uiTracks.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        if ((UnityEngine.Object) current.GetComponent<Button>() != (UnityEngine.Object) null)
          UnityEngine.Object.Destroy((UnityEngine.Object) current.gameObject);
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    foreach (GameObject track in this.tracks)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.uiTrackPF, this.uiTracks.transform);
      Target component = track.GetComponent<Target>();
      gameObject.name = component.targetName;
      gameObject.transform.GetChild(0).GetComponent<TMP_Text>().color = this.GetIFFColor(track.GetComponent<ITrackable>().GetIFF());
      gameObject.GetComponent<TrackButton>().target = component;
    }
    SFX.current.Play(1);
  }

  public void SetSelected(Transform newSelected)
  {
    if ((UnityEngine.Object) newSelected.GetComponent<Target>() == (UnityEngine.Object) null)
      return;
    this.selected = newSelected;
    SFX.current.Play(0);
  }

  private void UpdateSelectedUI()
  {
    if (!(bool) (UnityEngine.Object) this.selected)
      return;
    if (!this.uiSelectedPanel.activeSelf)
      this.uiSelectedPanel.SetActive(true);
    this.uiSelectedName.text = this.selected.GetComponent<ITrackable>().GetName() ?? "";
    this.uiSelectedIFF.text = this.GetIFFText(this.selected.GetComponent<ITrackable>().GetIFF()) ?? "";
    this.uiSelectedDist.text = Mathf.Floor(Vector3.Distance(this.selected.position, this.playerShip.position) * 100f).ToString() + "m";
    if ((UnityEngine.Object) this.selected.GetComponent<Rigidbody>() != (UnityEngine.Object) null)
      this.uiSelectedRelVel.text = Mathf.Floor((float) (((double) this.playerShip.GetComponent<Rigidbody>().velocity.magnitude - (double) this.selected.GetComponent<Rigidbody>().velocity.magnitude) * 100.0)).ToString() + " m/s";
    else
      this.uiSelectedRelVel.text = "No rigidbody";
  }

  public void AddTrack(GameObject track)
  {
    track.AddComponent<Target>();
    track.GetComponent<Target>().targetName = track.GetComponent<ITrackable>().GetName();
    track.GetComponent<ITrackable>().GetIFF();
    this.tracks.Add(track);
    this.RefreshUiTracks();
  }

  public void RemoveTrack(GameObject untrack)
  {
    MonoBehaviour.print((object) ("UNTRACKING: " + untrack.name));
    UnityEngine.Object.Destroy((UnityEngine.Object) untrack.GetComponent<Target>());
    this.tracks.Remove(untrack);
    this.RefreshUiTracks();
  }
}
