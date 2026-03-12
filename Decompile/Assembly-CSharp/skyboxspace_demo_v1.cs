// Decompiled with JetBrains decompiler
// Type: skyboxspace_demo_v1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class skyboxspace_demo_v1 : MonoBehaviour
{
  public Material[] skyBoxMaterial;
  public Vector3[] sunPosition;
  private int skyBoxLength;
  private int currentSkyBoxIndex;
  public string topText;
  private float counter;
  private int frames;
  private float fps;
  private static GUIStyle whiteStyle;
  private static GUIStyle blackStyle;
  public GameObject sun;

  private void Start()
  {
    RenderSettings.skybox = this.skyBoxMaterial[0];
    this.skyBoxLength = this.skyBoxMaterial.Length;
    this.topText = this.skyBoxMaterial[this.currentSkyBoxIndex].name;
    this.sun.transform.eulerAngles = this.sunPosition[0];
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.F))
      Screen.fullScreen = !Screen.fullScreen;
    if (Input.GetKeyDown(KeyCode.N))
    {
      ++this.currentSkyBoxIndex;
      if (this.currentSkyBoxIndex >= this.skyBoxLength)
        this.currentSkyBoxIndex = 0;
      RenderSettings.skybox = this.skyBoxMaterial[this.currentSkyBoxIndex];
      this.topText = this.skyBoxMaterial[this.currentSkyBoxIndex].name;
      this.sun.transform.eulerAngles = this.sunPosition[this.currentSkyBoxIndex];
    }
    this.counter += Time.deltaTime;
    ++this.frames;
    if ((double) this.counter < 1.0)
      return;
    this.fps = (float) this.frames / this.counter;
    this.counter = 0.0f;
    this.frames = 0;
  }

  protected virtual void OnGUI()
  {
    if ((double) this.fps > 0.0)
      skyboxspace_demo_v1.DrawText("FPS: " + this.fps.ToString("0"), TextAnchor.UpperLeft);
    if (string.IsNullOrEmpty(this.topText))
      return;
    skyboxspace_demo_v1.DrawText($"Skybox[{this.currentSkyBoxIndex.ToString()}] Name:{this.topText} (Press the [N] key for the next skybox)", TextAnchor.UpperCenter, 150);
  }

  private static void DrawText(string text, TextAnchor anchor, int offsetX = 15, int offsetY = 15)
  {
    if (string.IsNullOrEmpty(text))
      return;
    if (skyboxspace_demo_v1.whiteStyle == null || skyboxspace_demo_v1.blackStyle == null)
    {
      skyboxspace_demo_v1.whiteStyle = new GUIStyle();
      skyboxspace_demo_v1.whiteStyle.fontSize = 20;
      skyboxspace_demo_v1.whiteStyle.fontStyle = FontStyle.Bold;
      skyboxspace_demo_v1.whiteStyle.wordWrap = true;
      skyboxspace_demo_v1.whiteStyle.normal = new GUIStyleState();
      skyboxspace_demo_v1.whiteStyle.normal.textColor = Color.white;
      skyboxspace_demo_v1.blackStyle = new GUIStyle();
      skyboxspace_demo_v1.blackStyle.fontSize = 20;
      skyboxspace_demo_v1.blackStyle.fontStyle = FontStyle.Bold;
      skyboxspace_demo_v1.blackStyle.wordWrap = true;
      skyboxspace_demo_v1.blackStyle.normal = new GUIStyleState();
      skyboxspace_demo_v1.blackStyle.normal.textColor = Color.black;
    }
    skyboxspace_demo_v1.whiteStyle.alignment = anchor;
    skyboxspace_demo_v1.blackStyle.alignment = anchor;
    Rect position = new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height);
    position.xMin += (float) offsetX;
    position.xMax -= (float) offsetX;
    position.yMin += (float) offsetY;
    position.yMax -= (float) offsetY;
    ++position.x;
    GUI.Label(position, text, skyboxspace_demo_v1.blackStyle);
    position.x -= 2f;
    GUI.Label(position, text, skyboxspace_demo_v1.blackStyle);
    ++position.x;
    ++position.y;
    GUI.Label(position, text, skyboxspace_demo_v1.blackStyle);
    position.y -= 2f;
    GUI.Label(position, text, skyboxspace_demo_v1.blackStyle);
    ++position.y;
    GUI.Label(position, text, skyboxspace_demo_v1.whiteStyle);
  }
}
