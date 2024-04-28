using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public static FollowPlayer Singleton { get; private set; }

    private CinemachineVirtualCamera _camera;
    
    private void Awake()
    {
        Singleton = this;
        _camera = GetComponent<CinemachineVirtualCamera>();
    }

    public void SetCameraFollowing(Transform player)
    {
        _camera.Follow = player;
    }
}
