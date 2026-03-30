using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shooting: MonoBehaviour
{
    [SerializeField] protected Player _player;
    [SerializeField] protected AspectManager aspect;
    [SerializeField] protected GameObject _projectilePrefab;
    [SerializeField] protected Transform _projectileSpawnPoint;
    [SerializeField] protected float _fireRate = 1f;
    protected Animator animator;
    protected PoolManager pool;

    protected float nextTimeToShoot = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        pool = PoolManager.Instance;
    }

    public abstract void Shoot();
}
