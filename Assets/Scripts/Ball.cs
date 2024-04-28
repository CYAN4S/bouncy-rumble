using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    [Header("Settings")]
    [SerializeField] private float minSize;
    [SerializeField] private float maxSize;
    [SerializeField] private float lifetime;

    [Header("Appearances")]
    [SerializeField] private Material hostMaterial;
    [SerializeField] private Material clientMaterial;
    [SerializeField] private AudioClip bounceSound;
    
    
    [Header("Network")]
    public NetworkVariable<bool> ownByHost = new();

    private SphereCollider _collider;
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;

    private bool _isInHand;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void Initialize(bool isHost)
    {
        ownByHost.OnValueChanged += Vc;
        // ownByHost.Value = isHost;
    }

    private void Vc(bool _, bool value)
    {
        gameObject.layer = LayerMask.NameToLayer(value ? "Our Ball" : "Their Ball");
        GetComponent<MeshRenderer>().material = value ? hostMaterial : clientMaterial;
    }

    public void Inflate(float value)
    {
        
    }

    public void BeThrown(Vector3 forceVector)
    {
        _audioSource.Play();
        _collider.isTrigger = false;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(forceVector, ForceMode.Impulse);
        StartCoroutine(ToBeDestroyed());
    }

    private IEnumerator ToBeDestroyed()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioSource.PlayClipAtPoint(bounceSound, transform.position);
    }
}
