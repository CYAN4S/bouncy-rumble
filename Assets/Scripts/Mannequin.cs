using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Mannequin : MonoBehaviour
{
    private Collider _collider;

    [SerializeField] private Transform lifeBar;

    [SerializeField] private float maxHealth = 500;
    [SerializeField] private float health;

    [SerializeField] private int taskCode = -1;

    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip killedSound;

    private float initialScale;
    
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        health = maxHealth;
        initialScale = lifeBar.localScale.x;
        lifeBar.localScale = new Vector3(0, lifeBar.localScale.y, lifeBar.localScale.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ball"))
            return;

        var target = collision.rigidbody;

        var scala = target.velocity.magnitude;
        var size = collision.transform.localScale.x;
        
        // var isUp = target.velocity.y >= 0;
        var damage = (scala - 2.5f) * Mathf.Pow((size + 0.5f) * 0.75f , 3) * 50;

        if (damage <= 0)
            return;

        if (damage >= health)
        {
            MannequinManager.Instance.onDamaged?.Invoke(health);
            MannequinManager.Instance.onKilled?.Invoke(taskCode);
            AudioSource.PlayClipAtPoint(killedSound, transform.position);
            Destroy(gameObject);
        }
        else
        {
            MannequinManager.Instance.onDamaged?.Invoke(damage);
            health -= damage;
            lifeBar.localScale = new Vector3(initialScale * health / maxHealth, lifeBar.localScale.y, lifeBar.localScale.z);
            AudioSource.PlayClipAtPoint(damageSound, transform.position);
        }
    }
    
    
}