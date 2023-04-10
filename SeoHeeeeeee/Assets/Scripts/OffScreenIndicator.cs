using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[DefaultExecutionOrder(-1)]
public class OffScreenIndicator : MonoBehaviour
{

    float screenBoundOffset = 0.9f;

    Camera mainCamera;
    Vector3 screenCentre;
    Vector3 screenBounds;

    [SerializeField]
    List<Target> targetList = new List<Target>();
    Color alpha = new Color(0, 0, 0, 0);

    public static Action<Target, bool> TargetStateChanged;

    private void Awake()
    {
        mainCamera = Camera.main;
        screenCentre = new Vector3(Screen.width, Screen.height, 0) / 2;
        screenBounds = screenCentre * screenBoundOffset;
        TargetStateChanged += HandleTargetStateChanged;
    }

    private void LateUpdate()
    {
        DrawIndicators();
    }

    void DrawIndicators()
    {
        foreach (Target target in targetList)
        {
            Vector3 screenPosition = OffScreenIndicatorCore.GetScreenPosition(mainCamera, target.transform.position);
            bool isTargetVisible = OffScreenIndicatorCore.IsTargetVisible(screenPosition);
            float distanceFromCamera = target.NeedDistanceText ? target.GetDistanceFromCamera(mainCamera.transform.position) : float.MinValue;// Gets the target distance from the camera.
            Indicator indicator = null;

            if (target.NeedBoxIndicator && isTargetVisible)
            {
                screenPosition.z = 0;
                indicator = GetIndicator(ref target.indicator, IndicatorType.Deactive); // Gets the box indicator from the pool.
                indicator.SetImageColor(alpha);// Sets the image color of the indicator.

            }
            else if (target.NeedArrowIndicator && !isTargetVisible)
            {
                float angle = float.MinValue;
                OffScreenIndicatorCore.GetArrowIndicatorPositionAndAngle(ref screenPosition, ref angle, screenCentre, screenBounds);
                indicator = GetIndicator(ref target.indicator, IndicatorType.Arrow); // Gets the arrow indicator from the pool.
                indicator.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg); // Sets the rotation for the arrow indicator.
                indicator.SetImageColor(target.TargetColor);// Sets the image color of the indicator.
            }
            if (indicator)
            {
                
                indicator.SetDistanceText(distanceFromCamera); //Set the distance text for the indicator.
                indicator.transform.position = screenPosition; //Sets the position of the indicator on the screen.
                indicator.SetTextRotation(Quaternion.identity); // Sets the rotation of the distance text of the indicator.
            }
        }
    }

    void HandleTargetStateChanged(Target target, bool active)
    {
        if (active)
            targetList.Add(target);
        else
        {
            if(target.indicator != null)
            target.indicator.Activate(false);
            target.indicator = null;
            targetList.Remove(target);
        }
    }

    private Indicator GetIndicator(ref Indicator indicator, IndicatorType type)
    {
        if (indicator != null)
        {
            if (indicator.Type != type)
            {
                indicator.Activate(false);
                indicator = type == IndicatorType.Deactive ? DeactiveObjectPool.current.GetPooledObject() : ArrowObjectPool.current.GetPooledObject();
                indicator.Activate(true); // Sets the indicator as active.
            }
        }
        else
        {
            indicator = type == IndicatorType.Deactive ? DeactiveObjectPool.current.GetPooledObject() : ArrowObjectPool.current.GetPooledObject();
            indicator.Activate(true); // Sets the indicator as active.
        }
        return indicator;
    }
}
