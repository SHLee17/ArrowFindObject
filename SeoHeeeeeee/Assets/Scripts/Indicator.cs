using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum IndicatorType
{
    Arrow,
    Deactive
}

public class Indicator : MonoBehaviour
{
    [SerializeField] private IndicatorType indicatorType;
    private Image indicatorImage;
    private TMP_Text distanceText;
    public IndicatorType Type
    {
        get
        {
            return indicatorType;
        }
    }
    public bool Active
    {
        get
        {
            return transform.gameObject.activeInHierarchy;
        }
    }
    void Awake()
    {
        indicatorImage = transform.GetComponent<Image>();
        distanceText = transform.GetComponentInChildren<TMP_Text>();
    }
    public void SetImageColor(Color color)
    {
        indicatorImage.color = color;
    }

    public void SetDistanceText(float value)
    {
        distanceText.text = value >= 0 ? Mathf.Floor(value) + " m" : "";
    }
    public void SetTextRotation(Quaternion rotation)
    {
        distanceText.rectTransform.rotation = rotation;
    }
    public void Activate(bool value)
    {
        transform.gameObject.SetActive(value);
    }
    
}
