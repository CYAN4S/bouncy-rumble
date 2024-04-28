using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkManager : NetworkBehaviour
{
    [SerializeField] private Transform playerCameraRoot;

    public override void OnNetworkSpawn()
    {
        Debug.Log($"{(IsServer ? "Host" : "Client")} Spawned!");

        if (IsOwner)
        {
            FollowPlayer.Singleton.SetCameraFollowing(playerCameraRoot);
        }
        
        base.OnNetworkSpawn();
    }
}
