using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player: MonoBehaviour
{
    public InventoryObject inventory;
    [SerializeField] private float _heightOfFlyight;
    
    private Animator _animator;
    private Rigidbody2D _rb;
    private PlayerHealthSystem _health;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<PlayerHealthSystem>();
    }

    public void Jump()
    {
        _rb.velocity = Vector2.up * _heightOfFlyight;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
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

    private void OnApplicationQuit()
    {
        inventory.Clear();
    }
}
