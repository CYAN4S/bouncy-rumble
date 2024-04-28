using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
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
    private AudioSource _audioSource;

    [SerializeField] private Ball currentBall;

    private Vector3 _startPos;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _audioSource = GetComponent<AudioSource>();

        isInflating = false;
        ballScale = 0;

        _startPos = transform.position;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void Update()
    {
        // Generate ball if the player don't have it in its own hand.
        if (!currentBall)
        {
            currentBall = Instantiate(ballPrefab, hand.position, hand.rotation);
            // currentBall.Initialize(IsHost);
            currentBall.Initialize(true);
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
        if (ballScale < 0.15)
        {
            ballScale = Math.Min(ballScale + Time.deltaTime * inflateSpeed, 0.15f);
            ballScaleChanged?.Invoke(ballScale);
            _audioSource.volume = 0.5f;
        }
        else if (isInflating)
        {
            ballScale = Math.Min(ballScale + Time.deltaTime * inflateSpeed, 1.5f);
            ballScaleChanged?.Invoke(ballScale);
            _audioSource.volume = (ballScale >= 1.5f ? 0 : 1f);
        }
        else
        {
            _audioSource.volume = 0;
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
        
        if (hit.gameObject.CompareTag("Respawn"))
        {
            SceneManager.LoadScene(1);
        }
        
        var body = hit.collider.attachedRigidbody;
        
        if (body == null || body.isKinematic)
            return;
    }

    private void OnEscape()
    {
        SceneManager.LoadScene(0);
    }
}