using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player: MonoBehaviour
{
    public static Player instance;
    public InventoryObject inventory;
    [SerializeField] private Rigidbody2D _body;
    [SerializeField] private float _heightOfFlyight;
    private LogicScript logic;
    private bool IsAliveBird = true;
    private bool isVictory = false;
    private Animator animator;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Jump") && IsAliveBird)
        {
            _body.velocity = Vector2.up * _heightOfFlyight;
               
        }
          
        //SaveLoader();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Danger") && !isVictory)
        {
            logic.GameOver();
            IsAliveBird = false;
        }
        if (!isVictory)
        {
            if (collision.gameObject.CompareTag("Enemy") && PlayerHealthSystem.instance.currentHealth == 0)
            {
                logic.GameOver();
                IsAliveBird = false;
            }
            else if (collision.gameObject.CompareTag("Enemy") && PlayerHealthSystem.instance.currentHealth > 0)
            {
                PlayerHealthSystem.instance.TakeDamage(1);
                animator.SetTrigger("Hit");
            }
        }    
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            isVictory = true;
            logic.Victory();
            IsAliveBird = false;
        }
        if (collision.CompareTag("Danger") && !isVictory)
        {
            Debug.Log("You're dead");
            logic.GameOver();
            IsAliveBird = false;
        }
        if (collision.CompareTag("Item"))
            TakeItem(collision);
    }
    public bool CheckBirdStatus()
    {
        if (IsAliveBird)
        {
            return true;
        }
        else return false;
    }

    public void ChangeVictoryStatus()
    {
        if (!isVictory && IsAliveBird == true)
        {
            isVictory = true;
            IsAliveBird = false;
            LogicScript._instance.Victory();
        }  
    }

    public void ChangeGameOverStatus()
    {
        if (!isVictory && IsAliveBird == true)
        {
            logic.GameOver();
            IsAliveBird = false;
        }
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
        //inventory.Save();
        inventory.Clear();
    }

   /* public void SaveLoader()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            inventory.Save();
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            inventory.Load();
        }
    }*/
}
