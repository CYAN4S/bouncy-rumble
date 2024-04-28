using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDoor : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Open1 = Animator.StringToHash("Open");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Open()
    {
        _animator.SetTrigger(Open1);
    }
    
    
}