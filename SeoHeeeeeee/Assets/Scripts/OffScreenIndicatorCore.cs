using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreenIndicatorCore : MonoBehaviour
{
    public static Vector3 GetScreenPosition(Camera mainCamera, Vector3 targetPosition)
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
        return screenPosition;
    }

    public static bool IsTargetVisible(Vector3 screenPosition)
    {
        bool isTargetVisible = screenPosition.z > 0 &&
            screenPosition.x > 0 &&
            screenPosition.x < Screen.width &&
            screenPosition.y > 0 &&
            screenPosition.y < Screen.height;
        return isTargetVisible;
    }
    
    public static void GetArrowIndicatorPositionAndAngle(ref Vector3 screenPosition, ref float angle, Vector3 screenCentre, Vector3 screenBounds)
    {
        // Our screenPosition's origin is screen's bottom-left corner.
        // But we have to get the arrow's screenPosition and rotation with respect to screenCentre.
        screenPosition -= screenCentre;

        // When the targets are behind the camera their projections on the screen (WorldToScreenPoint) are inverted,
        // so just invert them.
        if (screenPosition.z < 0)
        {
            screenPosition *= -1;
        }

        // Angle between the x-axis (bottom of screen) and a vector starting at zero(bottom-left corner of screen) and terminating at screenPosition.
        angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
        // Slope of the line starting from zero and terminating at screenPosition.
        float slope = Mathf.Tan(angle);

        // T두 점의 선의 형태는 (y2 - y1) = m (x2 - x1) + c입니다,
        // s시작점(x1, y1)은 화면 하단 왼쪽(0, 0)입니다,
        // e끝점(x2, y2)은 화면 경계 중 하나입니다,
        // mm은 기울기입니다
        // ccisy 가로채기는 0이 될 것입니다. 라인이 오리진을 통과하고 있기 때문입니다.
        // F최종 방정식은 y = mx입니다.
        if (screenPosition.x > 0)
        {
            // Keep the x screen position to the maximum x bounds and
            // find the y screen position using y = mx.
            screenPosition = new Vector3(screenBounds.x, screenBounds.x * slope, 0);
        }
        else
        {
            screenPosition = new Vector3(-screenBounds.x, -screenBounds.x * slope, 0);
        }
        // Incase the y ScreenPosition exceeds the y screenBounds 
        if (screenPosition.y > screenBounds.y)
        {
            // Keep the y screen position to the maximum y bounds and
            // find the x screen position using x = y/m.
            screenPosition = new Vector3(screenBounds.y / slope, screenBounds.y, 0);
        }
        else if (screenPosition.y < -screenBounds.y)
        {
            screenPosition = new Vector3(-screenBounds.y / slope, -screenBounds.y, 0);
        }
        // Bring the ScreenPosition back to its original reference.
        screenPosition += screenCentre;
    }
}
