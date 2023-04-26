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
        // �츮�� screenPosition�� ������ ȭ���� ���� �ϴ� �𼭸��Դϴ�.
        // ������ �츮�� ��ũ�� ���Ϳ� ���� ȭ��ǥ�� ȭ�� ��ġ�� ȸ���� ���� �մϴ�.
        screenPosition -= screenCentre;

        // ����� ī�޶� �ڿ� ������ ȭ��(WorldToScreenPoint)�� ������ �����˴ϴ�,
        // �׷��ϱ� �׳� �����⸸ �ϸ� �˴ϴ�.
        if (screenPosition.z < 0)
        {
            screenPosition *= -1;
        }

        // x��(ȭ�� �ϴ�)�� 0(ȭ�� ���� �ϴ�)���� �����Ͽ� ȭ�� ��ġ���� ������ ���� ������ ����.
        angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
        // 0���� �����Ͽ� ȭ�� ��ġ���� ������ ���� �����Դϴ�.
        float slope = Mathf.Tan(angle);

        // T�� ���� ���� ���´� (y2 - y1) = m (x2 - x1) + c�Դϴ�,
        // s������(x1, y1)�� ȭ�� �ϴ� ����(0, 0)�Դϴ�,
        // e����(x2, y2)�� ȭ�� ��� �� �ϳ��Դϴ�,
        // mm�� �����Դϴ�
        // ccisy ����ä��� 0�� �� ���Դϴ�. ������ �������� ����ϰ� �ֱ� �����Դϴ�.
        // F���� �������� y = mx�Դϴ�.
        if (screenPosition.x > 0)
        {
            // x ȭ�� ��ġ�� �ִ� x ���� �����ϰ�
            // y = mx�� ����Ͽ� ȭ�� ��ġ�� ã���ϴ�.
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
