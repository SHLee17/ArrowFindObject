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
        // 우리의 screenPosition의 원점은 화면의 왼쪽 하단 모서리입니다.
        // 하지만 우리는 스크린 센터에 대한 화살표의 화면 위치와 회전을 얻어야 합니다.
        screenPosition -= screenCentre;

        // 대상이 카메라 뒤에 있으면 화면(WorldToScreenPoint)의 투영이 반전됩니다,
        // 그러니까 그냥 뒤집기만 하면 됩니다.
        if (screenPosition.z < 0)
        {
            screenPosition *= -1;
        }

        // x축(화면 하단)과 0(화면 왼쪽 하단)에서 시작하여 화면 위치에서 끝나는 벡터 사이의 각도.
        angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
        // 0에서 시작하여 화면 위치에서 끝나는 선의 기울기입니다.
        float slope = Mathf.Tan(angle);

        // T두 점의 선의 형태는 (y2 - y1) = m (x2 - x1) + c입니다,
        // s시작점(x1, y1)은 화면 하단 왼쪽(0, 0)입니다,
        // e끝점(x2, y2)은 화면 경계 중 하나입니다,
        // mm은 기울기입니다
        // ccisy 가로채기는 0이 될 것입니다. 라인이 오리진을 통과하고 있기 때문입니다.
        // F최종 방정식은 y = mx입니다.
        if (screenPosition.x > 0)
        {
            // x 화면 위치를 최대 x 경계로 유지하고
            // y = mx를 사용하여 화면 위치를 찾습니다.
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
