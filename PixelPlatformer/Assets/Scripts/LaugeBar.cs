using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaugeBar : MonoBehaviour
{
    public Slider slider;
    
    public void SetMaxLauge(float lauge)
    {
        slider.maxValue = lauge;
        slider.value = lauge;
    }

    public void SetLauge(float lauge)
    {
        slider.value = lauge;
    }
}
