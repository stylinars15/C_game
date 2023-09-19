using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarHandler : MonoBehaviour
{
    public Slider slider;
    public Gradient colorChange;
    public Image fill; 
    
    public void SetMaxBar(int input)
    { 
        slider.maxValue = input;
        slider.value = input; 
        
        fill.color = colorChange.Evaluate(1f);

    }
    
    public void SetBar(int input)
    {
        slider.value = input;
        fill.color = colorChange.Evaluate(slider.normalizedValue);

    }
}
