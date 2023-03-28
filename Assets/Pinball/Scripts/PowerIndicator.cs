using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerIndicator : MonoBehaviour
{
    public RectTransform powerIndicator;
    public float minPosY = -0.4f;
    public float maxPosY = 0.4f;

    public void UpdatePowerIndicator(float power, float maxPower)
    {
        float posY = Mathf.Lerp(minPosY, maxPosY, power / maxPower);
        powerIndicator.anchoredPosition = new Vector2(powerIndicator.anchoredPosition.x, posY);
    }
}