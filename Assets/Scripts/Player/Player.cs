using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CollectibleSystem;

public class Player: MonoBehaviour, ICollectibleCollector
{
    public InventoryObject inventory;
    [SerializeField] private float _heightOfFlyight;
    
    // Rotation physics settings
    [SerializeField] private float _upwardRotation = -15f;   // Tilt backward when rising
    [SerializeField] private float _downwardRotation = 15f;  // Tilt forward when falling
    [SerializeField] private float _rotationSpeed = 10f;     // Smooth interpolation speed
    
    private Animator _animator;
    private Rigidbody2D _rb;
    private PlayerHealthSystem _health;
    private float _currentRotation = 0f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<PlayerHealthSystem>();
    }

    private void FixedUpdate()
    {
        // Determine target rotation based on vertical velocity
        float targetRotation = 0f;
        float verticalVelocity = _rb.velocity.y;
        
        if (verticalVelocity > 0.1f)
        {
            // Rising — tilt backward
            targetRotation = _upwardRotation;
        }
        else if (verticalVelocity < -0.1f)
        {
            // Falling — tilt forward
            targetRotation = _downwardRotation;
        }
        
        // Smoothly interpolate current rotation toward target
        _currentRotation = Mathf.LerpAngle(_currentRotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        
        // Apply rotation to Rigidbody2D
        _rb.rotation = _currentRotation;
    }

    public void Jump()
    {
        _rb.velocity = Vector2.up * _heightOfFlyight;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && _health != null)
        {
            _health.TakeDamage(1);
            _animator.SetTrigger("Hit");
        }
        if (collision.gameObject.CompareTag("Danger"))
        {
            this.RequestState<LoseGameState>();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Danger"))
        {
            this.RequestState<LoseGameState>();
        }
        if (collision.CompareTag("Item"))
            TakeItem(collision);
    }
   
    public void TakeItem(Collider2D other)
    {
        var item = other.GetComponent<Item>();
        if (item)
        {
            inventory.AddItem(item.item, 1);
            FindObjectOfType<AudioManager>().Play("PickItem");
            Destroy(other.gameObject);
        }
    }
    
    public void Collect(Collectible collectible)
    {
        CollectibleManager.Instance.Collect(collectible);
        FindObjectOfType<AudioManager>().Play("PickItem");
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
    }

    /// <summary>
    /// Returns the horizontal movement delta this frame (for distance tracking).
    /// </summary>
    public float GetMovementDelta()
    {
        return Mathf.Abs(_rb.velocity.x * Time.deltaTime);
    }
}
