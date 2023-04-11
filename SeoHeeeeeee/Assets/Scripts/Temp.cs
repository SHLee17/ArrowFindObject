using UnityEngine;
using UnityEngine.UI;

public class Temp : MonoBehaviour
{
    public Transform target;
    public Image arrowImage;
    public Camera cameraToLookAt;
    public float angleThreshold = 20f;

    void Update()
    {
        if (target == null)
            return;

        Vector3 screenPos = cameraToLookAt.WorldToScreenPoint(target.position);
        bool isTargetVisible = screenPos.z > 0 && screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height;

        float angle = Vector3.Angle(target.position - cameraToLookAt.transform.position, cameraToLookAt.transform.forward);
        bool isTargetCentered = angle <= angleThreshold;

        arrowImage.enabled = isTargetVisible && !isTargetCentered;

        if (isTargetVisible && !isTargetCentered)
        {
            Vector3 cameraCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, screenPos.z);
            Vector3 arrowDirection = screenPos - cameraCenter;
            float arrowAngle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg;
            arrowImage.transform.rotation = Quaternion.Euler(new Vector3(0, 0, arrowAngle));
            arrowImage.rectTransform.anchoredPosition = new Vector2(0, 0);
        }
    }
}
