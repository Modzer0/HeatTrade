// Decompiled with JetBrains decompiler
// Type: OrbitRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (LineRenderer))]
public class OrbitRenderer : MonoBehaviour
{
  public Orbiter orbiter;
  [SerializeField]
  private Transform orbitCenter;
  private Vector3 orbitCenterPos;
  [SerializeField]
  private float orbitRadius;
  public int segments = 100;
  public float lineWidth = 1f;
  private LineRenderer lineRenderer;
  public bool drawOrbitInUpdate;
  private Camera cam;
  [SerializeField]
  private bool is3dOrbit;
  [SerializeField]
  private int orbitPlane;

  private void Start()
  {
    if ((bool) (UnityEngine.Object) this.GetComponent<Orbiter>())
      this.orbiter = this.GetComponent<Orbiter>();
    if ((bool) (UnityEngine.Object) CameraManager.current)
      this.cam = CameraManager.current.cam;
    this.lineRenderer = this.GetComponent<LineRenderer>();
    this.lineRenderer.widthMultiplier = this.lineWidth;
    this.lineRenderer.positionCount = this.segments + 1;
    this.StartCoroutine((IEnumerator) this.DelayedStart());
  }

  private IEnumerator DelayedStart()
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.DrawOrbit();
  }

  private void Update()
  {
    this.UpdateWidth();
    if (!(bool) (UnityEngine.Object) this.orbiter)
      return;
    if (this.orbiter.isOrbiting && !this.lineRenderer.enabled)
      this.lineRenderer.enabled = true;
    else if (!this.orbiter.isOrbiting && this.lineRenderer.enabled)
      this.lineRenderer.enabled = false;
    if (!(this.orbitCenterPos != this.orbiter.transform.position))
      return;
    this.DrawOrbit();
  }

  public void SetColor(Color newColor)
  {
    if (!(bool) (UnityEngine.Object) this.lineRenderer)
      this.lineRenderer = this.GetComponent<LineRenderer>();
    if (!(bool) (UnityEngine.Object) this.lineRenderer)
      return;
    newColor.a = 0.0196f;
    this.lineRenderer.startColor = newColor;
    this.lineRenderer.endColor = newColor;
  }

  private void UpdateWidth()
  {
    if ((UnityEngine.Object) this.cam == (UnityEngine.Object) null)
      return;
    this.lineRenderer.widthMultiplier = this.cam.orthographicSize / 200f;
  }

  private void DrawOrbit()
  {
    if ((UnityEngine.Object) this.orbiter != (UnityEngine.Object) null)
      this.orbitRadius = this.orbiter.orbitRadius;
    float num = 360f / (float) this.segments;
    Vector3[] positions = new Vector3[this.segments + 1];
    for (int index = 0; index < this.segments; ++index)
    {
      float f = (float) ((double) index * (double) num * (Math.PI / 180.0));
      float x = 0.0f;
      float y = 0.0f;
      float z = 0.0f;
      if (!this.is3dOrbit)
      {
        x = this.orbitRadius * Mathf.Cos(f);
        z = this.orbitRadius * Mathf.Sin(f);
      }
      else if (this.orbitPlane == 0)
      {
        y = this.orbitRadius * Mathf.Cos(f);
        z = this.orbitRadius * Mathf.Sin(f);
      }
      else if (this.orbitPlane == 1)
      {
        x = this.orbitRadius * Mathf.Cos(f);
        z = this.orbitRadius * Mathf.Sin(f);
      }
      else
      {
        x = this.orbitRadius * Mathf.Cos(f);
        y = this.orbitRadius * Mathf.Sin(f);
      }
      positions[index] = !((UnityEngine.Object) this.orbiter != (UnityEngine.Object) null) ? (!((UnityEngine.Object) this.orbitCenter != (UnityEngine.Object) null) ? new Vector3(x, 0.0f, z) : new Vector3(x, y, z) + this.orbitCenter.position) : new Vector3(x, 0.0f, z) + this.orbiter.parent.position;
    }
    positions[this.segments] = positions[0];
    this.lineRenderer.SetPositions(positions);
    if (!(bool) (UnityEngine.Object) this.orbiter)
      return;
    this.orbitCenterPos = this.orbiter.transform.position;
  }
}
