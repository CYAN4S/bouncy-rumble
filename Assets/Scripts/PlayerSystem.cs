using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerSystem : NetworkBehaviour
{
    [SerializeField] private Transform hand;
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Transform playerCamera;

    [Header("Settings")] 
    [SerializeField] private float inflateSpeed = 1f;
    [SerializeField] private float chargeSpeed = 0.8f;
    [SerializeField] private float powerMultiply = 50f;

    [Header("States")] 
    [SerializeField] private bool isInflating;
    [SerializeField] private bool isCharging;
    [SerializeField] private float ballScale;
    [SerializeField] [Range(0f, 1f)] private float power;

    [Header("Observers")] 
    [SerializeField] private UnityEvent<float> ballScaleChanged;
    [SerializeField] private UnityEvent<float> powerChanged;

    private PlayerInput _playerInput;

    [SerializeField] private Ball currentBall;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        isInflating = false;
        ballScale = 0;

        Debug.Log("Player Awake!");
    }
    
    public override void OnNetworkSpawn()
    {
        Debug.Log($"{(IsServer ? "Host" : "Client")} Spawned!");

        if (IsOwner)
        {
            FollowPlayer.Singleton.SetCameraFollowing(playerCamera);
        }
        
        base.OnNetworkSpawn();
    }
    
    private void Update()
    {
        // Generate ball if the player don't have it in its own hand.
        if (!currentBall)
        {
            currentBall = Instantiate(ballPrefab, hand.position, hand.rotation);
            currentBall.Initialize(IsHost);
        }

        Inflate();

        currentBall.transform.localScale = Vector3.one * ballScale;
        currentBall.transform.position =
            hand.transform.position + hand.TransformDirection(Vector3.forward * ballScale / 2);
        currentBall.GetComponent<Rigidbody>().mass = Mathf.Pow(ballScale, 3) + 2;

        if (isCharging)
        {
            Charge();
        }
        else if (power != 0)
        {
            Shoot();
        }
    }

    private void Inflate()
    {
        if (isInflating)
        {
            ballScale = Math.Min(ballScale + Time.deltaTime * inflateSpeed, 1.5f);
            ballScaleChanged?.Invoke(ballScale);
        }
        else if (ballScale < 0.15)
        {
            ballScale = Math.Min(ballScale + Time.deltaTime * inflateSpeed, 0.15f);
            ballScaleChanged?.Invoke(ballScale);
        }
    }

    private void Charge()
    {
        power = Math.Min(power + Time.deltaTime * chargeSpeed, 1);
        powerChanged?.Invoke(power);
    }

    private void Shoot()
    {
        var forceVector = power * powerMultiply * playerCamera.forward;
        currentBall.BeThrown(forceVector);
        currentBall = null;
        
        power = 0;
        ballScale = 0;
        powerChanged?.Invoke(power);
        ballScaleChanged?.Invoke(ballScale);
    }

    public void OnInflate(InputValue value)
    {
        isInflating = value.isPressed;
    }

    public void OnShoot(InputValue value)
    {
        isCharging = value.isPressed;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var body = hit.collider.attachedRigidbody;
        
        if (body == null || body.isKinematic)
            return;

        Debug.Log(hit.point);
    }
}