// Decompiled with JetBrains decompiler
// Type: RuntimeCone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (MeshFilter), typeof (MeshRenderer))]
[ExecuteInEditMode]
public class RuntimeCone : MonoBehaviour
{
  [Min(3f)]
  public int segments = 24;
  public float height = 2f;
  public float topRadius = 1f;
  public float bottomRadius = 1f;
  private MeshFilter mf;
  private MeshCollider mc;

  private void Start()
  {
    this.mf = this.mf ?? this.GetComponent<MeshFilter>();
    this.mc = this.mc ?? this.GetComponent<MeshCollider>();
    this.Generate();
    this.SetupCollider();
  }

  private void Update()
  {
    if (Application.isPlaying)
      return;
    this.Generate();
  }

  private void SetupCollider()
  {
    if (!(bool) (Object) this.mc)
      return;
    this.mc.sharedMesh = this.mf.sharedMesh;
  }

  public void Generate()
  {
    this.mf = this.mf ?? this.GetComponent<MeshFilter>();
    int num1 = Mathf.Max(3, this.segments);
    float y = this.height * 0.5f;
    int num2 = (num1 + 1) * 2;
    int num3 = num1 + 1 + 1;
    int num4 = num1 + 1 + 1;
    int num5 = num3;
    int length = num2 + num5 + num4;
    Vector3[] vector3Array1 = new Vector3[length];
    Vector3[] vector3Array2 = new Vector3[length];
    Vector2[] vector2Array = new Vector2[length];
    int index1 = 0;
    for (int index2 = 0; index2 <= num1; ++index2)
    {
      double f = (double) index2 / (double) num1 * 3.1415927410125732 * 2.0;
      float x = Mathf.Cos((float) f);
      float z = Mathf.Sin((float) f);
      vector3Array1[index1] = new Vector3(x * this.topRadius, y, z * this.topRadius);
      vector3Array2[index1] = new Vector3(x, 0.0f, z).normalized;
      vector2Array[index1] = new Vector2((float) index2 / (float) num1, 1f);
      int index3 = index1 + 1;
      vector3Array1[index3] = new Vector3(x * this.bottomRadius, -y, z * this.bottomRadius);
      vector3Array2[index3] = new Vector3(x, 0.0f, z).normalized;
      vector2Array[index3] = new Vector2((float) index2 / (float) num1, 0.0f);
      index1 = index3 + 1;
    }
    int num6 = index1;
    int index4 = num6 + 1;
    int index5 = num6;
    vector3Array1[index5] = new Vector3(0.0f, y, 0.0f);
    vector3Array2[index5] = Vector3.up;
    vector2Array[index5] = new Vector2(0.5f, 0.5f);
    int num7 = index4;
    for (int index6 = 0; index6 <= num1; ++index6)
    {
      double f = (double) index6 / (double) num1 * 3.1415927410125732 * 2.0;
      float num8 = Mathf.Cos((float) f);
      float num9 = Mathf.Sin((float) f);
      vector3Array1[index4] = new Vector3(num8 * this.topRadius, y, num9 * this.topRadius);
      vector3Array2[index4] = Vector3.up;
      vector2Array[index4] = (double) this.topRadius > 0.0 ? new Vector2((float) (((double) num8 / (double) this.topRadius + 1.0) * 0.5), (float) (((double) num9 / (double) this.topRadius + 1.0) * 0.5)) : new Vector2(0.5f, 0.5f);
      ++index4;
    }
    int num10 = index4;
    int index7 = num10 + 1;
    int index8 = num10;
    vector3Array1[index8] = new Vector3(0.0f, -y, 0.0f);
    vector3Array2[index8] = Vector3.down;
    vector2Array[index8] = new Vector2(0.5f, 0.5f);
    int num11 = index7;
    for (int index9 = 0; index9 <= num1; ++index9)
    {
      double f = (double) index9 / (double) num1 * 3.1415927410125732 * 2.0;
      float num12 = Mathf.Cos((float) f);
      float num13 = Mathf.Sin((float) f);
      vector3Array1[index7] = new Vector3(num12 * this.bottomRadius, -y, num13 * this.bottomRadius);
      vector3Array2[index7] = Vector3.down;
      vector2Array[index7] = (double) this.bottomRadius > 0.0 ? new Vector2((float) (((double) num12 / (double) this.bottomRadius + 1.0) * 0.5), (float) (((double) num13 / (double) this.bottomRadius + 1.0) * 0.5)) : new Vector2(0.5f, 0.5f);
      ++index7;
    }
    int[] numArray1 = new int[num1 * 12];
    int num14 = 0;
    for (int index10 = 0; index10 < num1; ++index10)
    {
      int num15 = index10 * 2;
      int[] numArray2 = numArray1;
      int index11 = num14;
      int num16 = index11 + 1;
      int num17 = num15;
      numArray2[index11] = num17;
      int[] numArray3 = numArray1;
      int index12 = num16;
      int num18 = index12 + 1;
      int num19 = num15 + 2;
      numArray3[index12] = num19;
      int[] numArray4 = numArray1;
      int index13 = num18;
      int num20 = index13 + 1;
      int num21 = num15 + 1;
      numArray4[index13] = num21;
      int[] numArray5 = numArray1;
      int index14 = num20;
      int num22 = index14 + 1;
      int num23 = num15 + 1;
      numArray5[index14] = num23;
      int[] numArray6 = numArray1;
      int index15 = num22;
      int num24 = index15 + 1;
      int num25 = num15 + 2;
      numArray6[index15] = num25;
      int[] numArray7 = numArray1;
      int index16 = num24;
      num14 = index16 + 1;
      int num26 = num15 + 3;
      numArray7[index16] = num26;
    }
    for (int index17 = 0; index17 < num1; ++index17)
    {
      int index18 = index5;
      int index19 = num7 + index17;
      int index20 = num7 + index17 + 1;
      if ((double) Vector3.Dot(Vector3.Cross(vector3Array1[index19] - vector3Array1[index18], vector3Array1[index20] - vector3Array1[index18]), Vector3.up) < 0.0)
      {
        int num27 = index19;
        index19 = index20;
        index20 = num27;
      }
      int[] numArray8 = numArray1;
      int index21 = num14;
      int num28 = index21 + 1;
      int num29 = index18;
      numArray8[index21] = num29;
      int[] numArray9 = numArray1;
      int index22 = num28;
      int num30 = index22 + 1;
      int num31 = index19;
      numArray9[index22] = num31;
      int[] numArray10 = numArray1;
      int index23 = num30;
      num14 = index23 + 1;
      int num32 = index20;
      numArray10[index23] = num32;
    }
    for (int index24 = 0; index24 < num1; ++index24)
    {
      int index25 = index8;
      int index26 = num11 + index24;
      int index27 = num11 + index24 + 1;
      if ((double) Vector3.Dot(Vector3.Cross(vector3Array1[index26] - vector3Array1[index25], vector3Array1[index27] - vector3Array1[index25]), Vector3.down) < 0.0)
      {
        int num33 = index26;
        index26 = index27;
        index27 = num33;
      }
      int[] numArray11 = numArray1;
      int index28 = num14;
      int num34 = index28 + 1;
      int num35 = index25;
      numArray11[index28] = num35;
      int[] numArray12 = numArray1;
      int index29 = num34;
      int num36 = index29 + 1;
      int num37 = index26;
      numArray12[index29] = num37;
      int[] numArray13 = numArray1;
      int index30 = num36;
      num14 = index30 + 1;
      int num38 = index27;
      numArray13[index30] = num38;
    }
    Mesh mesh = new Mesh();
    mesh.name = "ProceduralCylinderFixed";
    mesh.vertices = vector3Array1;
    mesh.normals = vector3Array2;
    mesh.uv = vector2Array;
    mesh.triangles = numArray1;
    mesh.RecalculateBounds();
    if (!(bool) (Object) this.mf)
      return;
    if (!Application.isPlaying)
      this.mf.sharedMesh = mesh;
    else
      this.mf.mesh = mesh;
  }
}
