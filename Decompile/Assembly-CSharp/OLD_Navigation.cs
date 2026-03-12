// Decompiled with JetBrains decompiler
// Type: OLD_Navigation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class OLD_Navigation : MonoBehaviour
{
  [SerializeField]
  private Rigidbody ship;
  private float thrustMain = 98f;
  private float thrustRCS = 9.8f;
  [SerializeField]
  private ParticleSystem psMain;
  [SerializeField]
  private List<ParticleSystem> rcsFwdUp;
  [SerializeField]
  private List<ParticleSystem> rcsFwdDown;
  [SerializeField]
  private List<ParticleSystem> rcsHorUp;
  [SerializeField]
  private List<ParticleSystem> rcsHorDown;
  [SerializeField]
  private List<ParticleSystem> rcsVerUp;
  [SerializeField]
  private List<ParticleSystem> rcsVerDown;
  [SerializeField]
  private List<ParticleSystem> rcsPitUp;
  [SerializeField]
  private List<ParticleSystem> rcsPitDown;
  [SerializeField]
  private List<ParticleSystem> rcsYawUp;
  [SerializeField]
  private List<ParticleSystem> rcsYawDown;
  [SerializeField]
  private List<ParticleSystem> rcsRolUp;
  [SerializeField]
  private List<ParticleSystem> rcsRolDown;
  private int fwdPitInput;
  private int horYawInput;
  private int verRolInput;
  private int mainInput;
  [SerializeField]
  private TMP_Text uiXMove;
  [SerializeField]
  private TMP_Text uiYMove;
  [SerializeField]
  private TMP_Text uiZMove;
  [SerializeField]
  private TMP_Text uiXRot;
  [SerializeField]
  private TMP_Text uiYRot;
  [SerializeField]
  private TMP_Text uiZRot;

  private void Start()
  {
  }

  private void Update()
  {
    this.Inputs();
    this.Controls();
    this.Particles();
    this.ClearInputs();
    this.UpdateUI();
  }

  private void UpdateUI()
  {
    TMP_Text uiXmove = this.uiXMove;
    float num = Mathf.Floor(Vector3.Dot(this.ship.velocity, this.ship.transform.right) * 100f);
    string str1 = num.ToString() ?? "";
    uiXmove.text = str1;
    TMP_Text uiYmove = this.uiYMove;
    num = Mathf.Floor(Vector3.Dot(this.ship.velocity, this.ship.transform.up) * 100f);
    string str2 = num.ToString() ?? "";
    uiYmove.text = str2;
    TMP_Text uiZmove = this.uiZMove;
    num = Mathf.Floor(Vector3.Dot(this.ship.velocity, this.ship.transform.forward) * 100f);
    string str3 = num.ToString() ?? "";
    uiZmove.text = str3;
    Vector3 vector3 = this.ship.transform.InverseTransformDirection(this.ship.angularVelocity);
    TMP_Text uiXrot = this.uiXRot;
    num = Mathf.Floor(vector3.x * 1000f);
    string str4 = num.ToString() ?? "";
    uiXrot.text = str4;
    TMP_Text uiYrot = this.uiYRot;
    num = Mathf.Floor(vector3.y * 1000f);
    string str5 = num.ToString() ?? "";
    uiYrot.text = str5;
    TMP_Text uiZrot = this.uiZRot;
    num = Mathf.Floor((float) (-(double) vector3.z * 1000.0));
    string str6 = num.ToString() ?? "";
    uiZrot.text = str6;
  }

  private void Inputs()
  {
    if (CamsManager.current.isCamFree)
      return;
    this.fwdPitInput = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
    this.horYawInput = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
    this.verRolInput = (Input.GetKey(KeyCode.E) ? 1 : 0) - (Input.GetKey(KeyCode.Q) ? 1 : 0);
  }

  private void ClearInputs()
  {
    this.fwdPitInput = 0;
    this.horYawInput = 0;
    this.verRolInput = 0;
  }

  private void Controls()
  {
    if (CamsManager.current.isCamFree)
      return;
    if (Input.GetKeyDown(KeyCode.Space))
      this.psMain.Clear();
    if (Input.GetKey(KeyCode.Space))
    {
      this.ship.AddForce(this.ship.transform.forward * this.thrustMain, ForceMode.Force);
      SFX.current.PlayEngine();
      if (!this.psMain.isPlaying)
        this.psMain.Play();
    }
    else
    {
      this.psMain.Stop();
      SFX.current.StopEngine();
    }
    this.ship.drag = !Input.GetKey(KeyCode.Z) ? 0.0f : this.thrustMain / 500f;
    this.ship.angularDrag = !Input.GetKey(KeyCode.X) ? 0.0f : this.thrustRCS / 10f;
    if (Input.GetKey(KeyCode.LeftControl))
      this.ship.AddTorque(this.ship.transform.right * (float) this.fwdPitInput * this.thrustRCS + this.ship.transform.up * (float) this.horYawInput * this.thrustRCS + this.ship.transform.forward * (float) -this.verRolInput * (this.thrustRCS / 10f));
    else
      this.ship.AddForce(this.ship.transform.forward * (float) this.fwdPitInput * this.thrustRCS + this.ship.transform.right * (float) this.horYawInput * this.thrustRCS + this.ship.transform.up * (float) this.verRolInput * this.thrustRCS, ForceMode.Force);
  }

  private void Particles()
  {
    if (CamsManager.current.isCamFree)
      return;
    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
      SFX.current.PlayRCS();
    else
      SFX.current.StopRCS();
    if (Input.GetKey(KeyCode.LeftControl))
    {
      if (Input.GetKey(KeyCode.W))
      {
        foreach (ParticleSystem particleSystem in this.rcsPitUp)
          particleSystem.Play();
      }
      else if (Input.GetKeyDown(KeyCode.W))
      {
        foreach (ParticleSystem particleSystem in this.rcsPitUp)
          particleSystem.Clear();
      }
      else if (Input.GetKeyUp(KeyCode.W))
      {
        foreach (ParticleSystem particleSystem in this.rcsPitUp)
          particleSystem.Stop();
      }
      if (Input.GetKey(KeyCode.S))
      {
        foreach (ParticleSystem particleSystem in this.rcsPitDown)
          particleSystem.Play();
      }
      else if (Input.GetKeyDown(KeyCode.S))
      {
        foreach (ParticleSystem particleSystem in this.rcsPitDown)
          particleSystem.Clear();
      }
      else if (Input.GetKeyUp(KeyCode.S))
      {
        foreach (ParticleSystem particleSystem in this.rcsPitDown)
          particleSystem.Stop();
      }
      if (Input.GetKey(KeyCode.E))
      {
        foreach (ParticleSystem particleSystem in this.rcsRolUp)
          particleSystem.Play();
      }
      else if (Input.GetKeyDown(KeyCode.E))
      {
        foreach (ParticleSystem particleSystem in this.rcsRolUp)
          particleSystem.Clear();
      }
      else if (Input.GetKeyUp(KeyCode.E))
      {
        foreach (ParticleSystem particleSystem in this.rcsRolUp)
          particleSystem.Stop();
      }
      if (Input.GetKey(KeyCode.Q))
      {
        foreach (ParticleSystem particleSystem in this.rcsRolDown)
          particleSystem.Play();
      }
      else if (Input.GetKeyDown(KeyCode.Q))
      {
        foreach (ParticleSystem particleSystem in this.rcsRolDown)
          particleSystem.Clear();
      }
      else if (Input.GetKeyUp(KeyCode.Q))
      {
        foreach (ParticleSystem particleSystem in this.rcsRolDown)
          particleSystem.Stop();
      }
      if (Input.GetKey(KeyCode.D))
      {
        foreach (ParticleSystem particleSystem in this.rcsYawUp)
          particleSystem.Play();
      }
      else if (Input.GetKeyDown(KeyCode.D))
      {
        foreach (ParticleSystem particleSystem in this.rcsYawUp)
          particleSystem.Clear();
      }
      else if (Input.GetKeyUp(KeyCode.D))
      {
        foreach (ParticleSystem particleSystem in this.rcsYawUp)
          particleSystem.Stop();
      }
      if (Input.GetKey(KeyCode.A))
      {
        foreach (ParticleSystem particleSystem in this.rcsYawDown)
          particleSystem.Play();
      }
      else if (Input.GetKeyDown(KeyCode.A))
      {
        foreach (ParticleSystem particleSystem in this.rcsYawDown)
          particleSystem.Clear();
      }
      else
      {
        if (!Input.GetKeyUp(KeyCode.A))
          return;
        foreach (ParticleSystem particleSystem in this.rcsYawDown)
          particleSystem.Stop();
      }
    }
    else
    {
      if (Input.GetKey(KeyCode.W))
      {
        foreach (ParticleSystem particleSystem in this.rcsFwdUp)
          particleSystem.Play();
      }
      else if (Input.GetKeyDown(KeyCode.W))
      {
        foreach (ParticleSystem particleSystem in this.rcsFwdUp)
          particleSystem.Clear();
      }
      else if (Input.GetKeyUp(KeyCode.W))
      {
        foreach (ParticleSystem particleSystem in this.rcsFwdUp)
          particleSystem.Stop();
      }
      if (Input.GetKey(KeyCode.S))
      {
        foreach (ParticleSystem particleSystem in this.rcsFwdDown)
          particleSystem.Play();
      }
      else if (Input.GetKeyDown(KeyCode.S))
      {
        foreach (ParticleSystem particleSystem in this.rcsFwdDown)
          particleSystem.Clear();
      }
      else if (Input.GetKeyUp(KeyCode.S))
      {
        foreach (ParticleSystem particleSystem in this.rcsFwdDown)
          particleSystem.Stop();
      }
      if (Input.GetKey(KeyCode.D))
      {
        foreach (ParticleSystem particleSystem in this.rcsHorUp)
          particleSystem.Play();
      }
      else if (Input.GetKeyDown(KeyCode.D))
      {
        foreach (ParticleSystem particleSystem in this.rcsHorUp)
          particleSystem.Clear();
      }
      else if (Input.GetKeyUp(KeyCode.D))
      {
        foreach (ParticleSystem particleSystem in this.rcsHorUp)
          particleSystem.Stop();
      }
      if (Input.GetKey(KeyCode.A))
      {
        foreach (ParticleSystem particleSystem in this.rcsHorDown)
          particleSystem.Play();
      }
      else if (Input.GetKeyDown(KeyCode.A))
      {
        foreach (ParticleSystem particleSystem in this.rcsHorDown)
          particleSystem.Clear();
      }
      else if (Input.GetKeyUp(KeyCode.A))
      {
        foreach (ParticleSystem particleSystem in this.rcsHorDown)
          particleSystem.Stop();
      }
      if (Input.GetKey(KeyCode.E))
      {
        foreach (ParticleSystem particleSystem in this.rcsVerUp)
          particleSystem.Play();
      }
      else if (Input.GetKeyDown(KeyCode.E))
      {
        foreach (ParticleSystem particleSystem in this.rcsVerUp)
          particleSystem.Clear();
      }
      else if (Input.GetKeyUp(KeyCode.E))
      {
        foreach (ParticleSystem particleSystem in this.rcsVerUp)
          particleSystem.Stop();
      }
      if (Input.GetKey(KeyCode.Q))
      {
        foreach (ParticleSystem particleSystem in this.rcsVerDown)
          particleSystem.Play();
      }
      else if (Input.GetKeyDown(KeyCode.Q))
      {
        foreach (ParticleSystem particleSystem in this.rcsVerDown)
          particleSystem.Clear();
      }
      else
      {
        if (!Input.GetKeyUp(KeyCode.Q))
          return;
        foreach (ParticleSystem particleSystem in this.rcsVerDown)
          particleSystem.Stop();
      }
    }
  }
}
