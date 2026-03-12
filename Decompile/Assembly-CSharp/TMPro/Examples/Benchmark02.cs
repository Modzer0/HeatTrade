// Decompiled with JetBrains decompiler
// Type: TMPro.Examples.Benchmark02
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace TMPro.Examples;

public class Benchmark02 : MonoBehaviour
{
  public int SpawnType;
  public int NumberOfNPC = 12;
  public bool IsTextObjectScaleStatic;
  private TextMeshProFloatingText floatingText_Script;

  private void Start()
  {
    for (int index = 0; index < this.NumberOfNPC; ++index)
    {
      if (this.SpawnType == 0)
      {
        GameObject gameObject = new GameObject();
        gameObject.transform.position = new Vector3(Random.Range(-95f, 95f), 0.25f, Random.Range(-95f, 95f));
        TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
        textMeshPro.autoSizeTextContainer = true;
        textMeshPro.rectTransform.pivot = new Vector2(0.5f, 0.0f);
        textMeshPro.alignment = TextAlignmentOptions.Bottom;
        textMeshPro.fontSize = 96f;
        textMeshPro.enableKerning = false;
        textMeshPro.color = (Color) new Color32(byte.MaxValue, byte.MaxValue, (byte) 0, byte.MaxValue);
        textMeshPro.text = "!";
        textMeshPro.isTextObjectScaleStatic = this.IsTextObjectScaleStatic;
        this.floatingText_Script = gameObject.AddComponent<TextMeshProFloatingText>();
        this.floatingText_Script.SpawnType = 0;
        this.floatingText_Script.IsTextObjectScaleStatic = this.IsTextObjectScaleStatic;
      }
      else if (this.SpawnType == 1)
      {
        GameObject gameObject = new GameObject();
        gameObject.transform.position = new Vector3(Random.Range(-95f, 95f), 0.25f, Random.Range(-95f, 95f));
        TextMesh textMesh = gameObject.AddComponent<TextMesh>();
        textMesh.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
        textMesh.GetComponent<Renderer>().sharedMaterial = textMesh.font.material;
        textMesh.anchor = TextAnchor.LowerCenter;
        textMesh.fontSize = 96 /*0x60*/;
        textMesh.color = (Color) new Color32(byte.MaxValue, byte.MaxValue, (byte) 0, byte.MaxValue);
        textMesh.text = "!";
        this.floatingText_Script = gameObject.AddComponent<TextMeshProFloatingText>();
        this.floatingText_Script.SpawnType = 1;
      }
      else if (this.SpawnType == 2)
      {
        GameObject gameObject = new GameObject();
        gameObject.AddComponent<Canvas>().worldCamera = Camera.main;
        gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        gameObject.transform.position = new Vector3(Random.Range(-95f, 95f), 5f, Random.Range(-95f, 95f));
        TextMeshProUGUI textMeshProUgui = new GameObject().AddComponent<TextMeshProUGUI>();
        textMeshProUgui.rectTransform.SetParent(gameObject.transform, false);
        textMeshProUgui.color = (Color) new Color32(byte.MaxValue, byte.MaxValue, (byte) 0, byte.MaxValue);
        textMeshProUgui.alignment = TextAlignmentOptions.Bottom;
        textMeshProUgui.fontSize = 96f;
        textMeshProUgui.text = "!";
        this.floatingText_Script = gameObject.AddComponent<TextMeshProFloatingText>();
        this.floatingText_Script.SpawnType = 0;
      }
    }
  }
}
