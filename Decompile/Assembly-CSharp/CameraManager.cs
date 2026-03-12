// Decompiled with JetBrains decompiler
// Type: CameraManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class CameraManager : MonoBehaviour
{
  public static CameraManager current;
  private Rigidbody camRB;
  private AudioManager am;
  private MapInputs mi;
  private TrackDisplayer td;
  private TimeManager tm;
  private UIManager uim;
  public Camera cam;
  public CamInfo target;
  public CamInfo tempTarget;
  [SerializeField]
  private Transform miniCam;
  [SerializeField]
  private Vector2 currentZoomRange;
  [SerializeField]
  private float moveMagnitude;
  public int targetZoom;
  public int zoomSens = 30;
  private Vector3 targetDir;

  private void Awake()
  {
    if ((Object) CameraManager.current != (Object) null && (Object) CameraManager.current != (Object) this)
      Object.Destroy((Object) this.gameObject);
    else
      CameraManager.current = this;
  }

  private void Start()
  {
    this.tm = TimeManager.current;
    this.uim = UIManager.current;
    this.camRB = this.cam.GetComponent<Rigidbody>();
    this.am = AudioManager.current;
    this.mi = MapInputs.current;
    this.td = TrackDisplayer.current;
    this.targetZoom = (int) this.cam.orthographicSize;
  }

  private void LateUpdate() => this.UpdateCamPos();

  private void UpdateCamPos()
  {
    if ((Object) this.tempTarget != (Object) null)
    {
      this.uim.SetInfo(this.tempTarget);
      this.uim.UpdateInfo(this.tempTarget);
    }
    else if ((Object) this.target != (Object) null)
      this.uim.UpdateInfo(this.target);
    if ((Object) this.mi.selectedFleet != (Object) null)
    {
      this.miniCam.position = this.mi.selectedFleet.transform.position;
      this.miniCam.gameObject.SetActive(true);
    }
    else
      this.miniCam.gameObject.SetActive(false);
    this.cam.orthographicSize = Mathf.Lerp(this.cam.orthographicSize, (float) this.targetZoom, 5f * Time.deltaTime);
    Vector3 vector3 = new Vector3(90f, 0.0f, 0.0f);
    this.cam.transform.rotation = Quaternion.Euler(vector3.x, vector3.y, 0.0f);
    float num = 1600f;
    Vector3 position = this.cam.transform.position;
    if ((double) position.sqrMagnitude > (double) num * (double) num)
    {
      this.cam.transform.position = Vector3.zero + position.normalized * num;
      this.cam.transform.position = new Vector3(this.cam.transform.position.x, 100f, this.cam.transform.position.z);
    }
    this.camRB.AddForce(this.targetDir * this.cam.orthographicSize * this.moveMagnitude, ForceMode.VelocityChange);
  }

  public void ClearTarget() => this.target = (CamInfo) null;

  public void CenterCam(Transform target)
  {
    if ((Object) target == (Object) this.cam.transform.parent)
    {
      this.cam.transform.SetParent((Transform) null);
    }
    else
    {
      this.cam.transform.SetParent(target);
      this.StartCoroutine((IEnumerator) this.CenterCamSmooth(target.position, 0.1f));
    }
    this.am.PlaySFX(4);
  }

  private IEnumerator CenterCamSmooth(Vector3 position, float duration)
  {
    if ((Object) this.cam == (Object) null)
      this.cam = Camera.main;
    Vector3 startPos = this.cam.transform.position;
    Vector3 endPos = new Vector3(position.x, this.cam.transform.position.y, position.z);
    float t = 0.0f;
    while ((double) t < (double) duration)
    {
      if ((Object) this.cam == (Object) null)
        this.cam = Camera.main;
      t += Time.deltaTime;
      this.cam.transform.position = Vector3.Lerp(startPos, endPos, t / duration);
      yield return (object) null;
    }
    this.cam.transform.position = endPos;
  }

  public void CenterCamAt(Vector3 pos)
  {
    MonoBehaviour.print((object) ("center cam at: " + pos.ToString()));
    this.StartCoroutine((IEnumerator) this.CenterCamSmooth(pos, 0.5f));
  }

  public void Zoom(int dir)
  {
    float orthographicSize = this.cam.orthographicSize;
    this.targetZoom = dir > 0 ? (int) Mathf.Max(10f, orthographicSize - (float) (this.zoomSens * dir) * (orthographicSize / 25f)) : (int) Mathf.Min(1500f, orthographicSize - (float) (this.zoomSens * dir) * (orthographicSize / 25f));
    this.am.PlayZoom(dir);
  }

  public void Move(Vector3 dir) => this.targetDir = dir;
}
