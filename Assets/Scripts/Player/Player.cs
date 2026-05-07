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
        // Вращение: используем unscaledDeltaTime только если игрок "вне времени"
        float dt = TimeManager.Instance.IgnoreTimeScale ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime;
        _currentRotation = Mathf.LerpAngle(_currentRotation, GetTargetRotation(), _rotationSpeed * dt);
        _rb.rotation = _currentRotation;

        // Компенсация физики ТОЛЬКО для режима "ExceptPlayer"
        if (TimeManager.Instance.IgnoreTimeScale && TimeManager.Instance.IsSlowedDown)
        {
            float multiplier = 1f / Time.timeScale;
            float gravityComp = (multiplier * multiplier) - 1f;

            _rb.AddForce(Physics2D.gravity * _rb.gravityScale * gravityComp, ForceMode2D.Force);
        }
    }

    public void Jump()
    {

        float boost = 1f;
        // Прыжок быстрый ТОЛЬКО в режиме исключения игрока
        if (TimeManager.Instance.IgnoreTimeScale && TimeManager.Instance.IsSlowedDown)
        {
            boost = 1f / Time.timeScale;
        }
        _rb.velocity = Vector2.up * _heightOfFlyight * boost;
    }

    private float GetTargetRotation()
    {
        if (_rb.velocity.y > 0.1f) return _upwardRotation;
        if (_rb.velocity.y < -0.1f) return _downwardRotation;
        return 0f;
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
        return Mathf.Abs(_rb.velocity.x * TimeManager.Instance.PlayerDeltaTime);
    }
}
