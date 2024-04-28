using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SciGauge : MonoBehaviour
{
    private Image _image;

    [SerializeField] [Range(0f, 1f)] private float asMax = 1;
    [SerializeField] private float value = 0;
    [SerializeField] private float maxValue = 1;
    [SerializeField] [Range(0f, 1f)] private float fakeValue = 1;

    public float lerp = 1;

    private void Awake()
    {
        _image = GetComponent<Image>();
        fakeValue = value;
    }

    public void SetValue(float newValue)
    {
        value = newValue;
    }

    private void Update()
    {
        fakeValue = Mathf.Lerp(value, fakeValue, lerp);
        if (Mathf.Abs(fakeValue - value) <= 0.001)
        {
            fakeValue = value;
        }
        _image.fillAmount = fakeValue / maxValue * asMax;
    }
}
