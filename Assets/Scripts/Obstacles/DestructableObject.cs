using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    public GameObject explosionEffectPrefab;
    [SerializeField] private GameObject _objectToDestroy;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Explode();
            Destroy(_objectToDestroy);
        }
    }

    private void Explode()
    {
        GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(explosionEffect, 3f); 
    }
}
