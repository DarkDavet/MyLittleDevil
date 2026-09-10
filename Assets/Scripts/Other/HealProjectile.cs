using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HealProjectile : BaseProjectile
{
    [SerializeField] private string projectileTag;
    public float rotateSpeed = 500f;

    private Transform _target;
    private LayerMask _targetLayer;
    private int _healAmount;

    public void SetTarget(Transform target)
    {
        _target = target;

        if (_target != null)
        {
            Vector2 direction = (Vector2)_target.position - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    public void SetHealAmount(int amount) => _healAmount = amount;
    // Этот метод вызывается из HealStation.HealShoot перед выстрелом
    public void SetTargetLayer(LayerMask layerMask) => _targetLayer = layerMask;

    public override void OnObjectSpawn()
    {
        timer = StartCoroutine(ReturnToPoolAfterTime());
    }

    void Update()
    {
        if (_target == null)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            return;
        }

        // Логика поворота к цели
        Vector2 direction = (Vector2)_target.position - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Плавный поворот
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. ЛОГИКА ЛЕЧЕНИЯ: Слой объекта входит в маску разрешенных для ХИЛА
        if ((_targetLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            if (collision.TryGetComponent<IHealable>(out var health))
            {
                // Лечим, только если цель ранена и жива
                if (health.CurrentHealth > 0 && health.CurrentHealth < health.MaxHealth)
                {
                    health.Heal(_healAmount);
                }

                // Снаряд коснулся валидной цели (союзника) — возвращаем в пул в любом случае,
                // чтобы он не преследовал объект с полным здоровьем.
                ReturnToPool();
            }
        }
        // 2. ЛОГИКА УРОНА: Слой НЕ совпадает (значит, это противоположная команда/враг)
        else
        {
            // Замените IDamageable и TakeDamage на ваши компоненты урона, если они называются иначе
            if (collision.TryGetComponent<IDamagable>(out var damageable))
            {
                damageable.TakeDamage(_healAmount);

                // Снаряд нанес урон врагу, возвращаем его в пул
                ReturnToPool();
            }
            // Если это объект без компонента урона (например, декорация), снаряд летит дальше
        }
    }



    private void ReturnToPool()
    {
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null; 
        }
        PoolManager.Instance.ReturnToPool(projectileTag, gameObject);
    }

    protected IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(timeLimit);
        ReturnToPool(); 
    }
}
