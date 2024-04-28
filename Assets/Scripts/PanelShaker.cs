using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelShaker : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform lookAt;
    [SerializeField] private float value;
    [SerializeField] private Vector3 delta;

    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        var result = cam.WorldToViewportPoint(lookAt.position);
        _rect.anchoredPosition = (value * (delta + result));
    }
}
