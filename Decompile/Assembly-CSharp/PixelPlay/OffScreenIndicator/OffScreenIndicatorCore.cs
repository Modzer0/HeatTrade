// Decompiled with JetBrains decompiler
// Type: PixelPlay.OffScreenIndicator.OffScreenIndicatorCore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 16756D39-DAA9-460B-A67B-12FE3A9D5968
// Assembly location: D:\SteamLibrary\steamapps\common\Spacefleet - Heat Death\Spacefleet - Heat Death_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace PixelPlay.OffScreenIndicator;

public class OffScreenIndicatorCore
{
  public static int width;
  public static int height;
  public static bool isWHSet;

  public static Vector3 GetScreenPosition(Camera mainCamera, Vector3 targetPosition)
  {
    return mainCamera.WorldToScreenPoint(targetPosition);
  }

  public static void SetWH()
  {
    OffScreenIndicatorCore.width = Screen.width;
    OffScreenIndicatorCore.height = Screen.height;
    OffScreenIndicatorCore.isWHSet = true;
  }

  public static bool IsTargetVisibleStrategic(Vector3 pos, Vector3 bounds, Vector3 center) => true;

  public static bool IsTargetVisibleTactical(Vector3 pos, Vector3 bounds, Vector3 center)
  {
    if (!OffScreenIndicatorCore.isWHSet)
      OffScreenIndicatorCore.SetWH();
    return (double) pos.z > 0.0 && (double) pos.x > 0.0 && (double) pos.x < (double) OffScreenIndicatorCore.width && (double) pos.y > 0.0 && (double) pos.y < (double) OffScreenIndicatorCore.height;
  }

  public static void GetArrowIndicatorPositionAndAngle(
    ref Vector3 screenPosition,
    ref float angle,
    Vector3 screenCentre,
    Vector3 screenBounds)
  {
    screenPosition -= screenCentre;
    if ((double) screenPosition.z < 0.0)
      screenPosition *= -1f;
    angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
    float num1 = Mathf.Tan(angle);
    Vector3 vector3_1 = (double) screenPosition.x <= 0.0 ? new Vector3(-screenBounds.x, -screenBounds.x * num1, 0.0f) : new Vector3(screenBounds.x, screenBounds.x * num1, 0.0f);
    if ((double) Mathf.Abs(vector3_1.y) > (double) screenBounds.y)
    {
      vector3_1 = new Vector3(screenBounds.y / num1, screenBounds.y, 0.0f);
      if ((double) screenPosition.y < 0.0)
        vector3_1 = new Vector3(-screenBounds.y / num1, -screenBounds.y, 0.0f);
    }
    Vector3 vector3_2 = vector3_1 + screenCentre;
    float num2 = (float) OffScreenIndicatorCore.width * 0.25f;
    float num3 = (float) OffScreenIndicatorCore.width * 0.75f;
    float num4 = (float) OffScreenIndicatorCore.height * 0.4f;
    float num5 = (float) OffScreenIndicatorCore.height * 0.6f;
    bool flag = false;
    float x = vector3_2.x;
    float y = vector3_2.y;
    if ((double) vector3_2.x < (double) num2)
    {
      if ((double) vector3_2.y < (double) num4 || (double) vector3_2.y > (double) num5)
      {
        x = num2;
        y = x * (vector3_2.y / vector3_2.x);
        flag = true;
      }
    }
    else if ((double) vector3_2.x > (double) num3 && ((double) vector3_2.y < (double) num4 || (double) vector3_2.y > (double) num5))
    {
      x = num3;
      y = x * (vector3_2.y / vector3_2.x);
      flag = true;
    }
    if (flag)
    {
      if ((double) y < (double) num4)
      {
        y = num4;
        x = y * (vector3_2.x / vector3_2.y);
      }
      else if ((double) y > (double) num5)
      {
        y = num5;
        x = y * (vector3_2.x / vector3_2.y);
      }
    }
    screenPosition = !flag ? vector3_1 : new Vector3(x, y, 0.0f) - screenCentre;
    screenPosition += screenCentre;
  }

  public static void GetArrowIndicatorPositionAndAngleTactical(
    ref Vector3 screenPosition,
    ref float angle,
    Vector3 screenCentre,
    Vector3 screenBounds)
  {
    screenPosition -= screenCentre;
    if ((double) screenPosition.z < 0.0)
      screenPosition *= -1f;
    angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
    float num = Mathf.Tan(angle);
    screenPosition = (double) screenPosition.x <= 0.0 ? new Vector3(-screenBounds.x, -screenBounds.x * num, 0.0f) : new Vector3(screenBounds.x, screenBounds.x * num, 0.0f);
    if ((double) screenPosition.y > (double) screenBounds.y)
      screenPosition = new Vector3(screenBounds.y / num, screenBounds.y, 0.0f);
    else if ((double) screenPosition.y < -(double) screenBounds.y)
      screenPosition = new Vector3(-screenBounds.y / num, -screenBounds.y, 0.0f);
    screenPosition += screenCentre;
  }
}
