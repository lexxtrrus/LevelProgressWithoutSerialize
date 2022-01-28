using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Slider progressBar;

    private void Update()
    {
        progressBar.value = Mathf.Lerp(progressBar.value, slider.value, Time.deltaTime * 10f);
    }
}
