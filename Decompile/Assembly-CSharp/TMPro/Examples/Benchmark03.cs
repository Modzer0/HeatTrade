// Decompiled with JetBrains decompiler
// Type: TMPro.Examples.Benchmark03
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.TextCore.LowLevel;

#nullable disable
namespace TMPro.Examples;

public class Benchmark03 : MonoBehaviour
{
  public int NumberOfSamples = 100;
  public Benchmark03.BenchmarkType Benchmark;
  public Font SourceFont;

  private void Awake()
  {
  }

  private void Start()
  {
    TMP_FontAsset tmpFontAsset = (TMP_FontAsset) null;
    switch (this.Benchmark)
    {
      case Benchmark03.BenchmarkType.TMP_SDF_MOBILE:
        tmpFontAsset = TMP_FontAsset.CreateFontAsset(this.SourceFont, 90, 9, GlyphRenderMode.SDFAA, 256 /*0x0100*/, 256 /*0x0100*/);
        break;
      case Benchmark03.BenchmarkType.TMP_SDF__MOBILE_SSD:
        tmpFontAsset = TMP_FontAsset.CreateFontAsset(this.SourceFont, 90, 9, GlyphRenderMode.SDFAA, 256 /*0x0100*/, 256 /*0x0100*/);
        tmpFontAsset.material.shader = Shader.Find("TextMeshPro/Mobile/Distance Field SSD");
        break;
      case Benchmark03.BenchmarkType.TMP_SDF:
        tmpFontAsset = TMP_FontAsset.CreateFontAsset(this.SourceFont, 90, 9, GlyphRenderMode.SDFAA, 256 /*0x0100*/, 256 /*0x0100*/);
        tmpFontAsset.material.shader = Shader.Find("TextMeshPro/Distance Field");
        break;
      case Benchmark03.BenchmarkType.TMP_BITMAP_MOBILE:
        tmpFontAsset = TMP_FontAsset.CreateFontAsset(this.SourceFont, 90, 9, GlyphRenderMode.SMOOTH, 256 /*0x0100*/, 256 /*0x0100*/);
        break;
    }
    for (int index = 0; index < this.NumberOfSamples; ++index)
    {
      switch (this.Benchmark)
      {
        case Benchmark03.BenchmarkType.TMP_SDF_MOBILE:
        case Benchmark03.BenchmarkType.TMP_SDF__MOBILE_SSD:
        case Benchmark03.BenchmarkType.TMP_SDF:
        case Benchmark03.BenchmarkType.TMP_BITMAP_MOBILE:
          TextMeshPro textMeshPro = new GameObject()
          {
            transform = {
              position = new Vector3(0.0f, 1.2f, 0.0f)
            }
          }.AddComponent<TextMeshPro>();
          textMeshPro.font = tmpFontAsset;
          textMeshPro.fontSize = 128f;
          textMeshPro.text = "@";
          textMeshPro.alignment = TextAlignmentOptions.Center;
          textMeshPro.color = (Color) new Color32(byte.MaxValue, byte.MaxValue, (byte) 0, byte.MaxValue);
          if (this.Benchmark == Benchmark03.BenchmarkType.TMP_BITMAP_MOBILE)
          {
            textMeshPro.fontSize = 132f;
            break;
          }
          break;
        case Benchmark03.BenchmarkType.TEXTMESH_BITMAP:
          TextMesh textMesh = new GameObject()
          {
            transform = {
              position = new Vector3(0.0f, 1.2f, 0.0f)
            }
          }.AddComponent<TextMesh>();
          textMesh.GetComponent<Renderer>().sharedMaterial = this.SourceFont.material;
          textMesh.font = this.SourceFont;
          textMesh.anchor = TextAnchor.MiddleCenter;
          textMesh.fontSize = 130;
          textMesh.color = (Color) new Color32(byte.MaxValue, byte.MaxValue, (byte) 0, byte.MaxValue);
          textMesh.text = "@";
          break;
      }
    }
  }

  public enum BenchmarkType
  {
    TMP_SDF_MOBILE,
    TMP_SDF__MOBILE_SSD,
    TMP_SDF,
    TMP_BITMAP_MOBILE,
    TEXTMESH_BITMAP,
  }
}
