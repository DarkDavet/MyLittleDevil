using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSpellProjectile : BaseProjectile
{
    [Header("Настройки траектории \"Змейка\"")]
    [SerializeField] private float noiseMagnitude = 2f; // Амплитуда виляния (ширина змейки)
    [SerializeField] private float noiseFrequency = 5f; // Скорость/частота виляния

    private Vector2 _targetPosition; // Запомненная позиция игрока
    private Vector2 _fixedDirection;  // Постоянное направление полета
    private float _noiseSeed;
    private bool _hasTarget = false;

    public override void OnObjectSpawn()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _targetPosition = playerObj.transform.position;

            _fixedDirection = (_targetPosition - (Vector2)transform.position).normalized;
            _hasTarget = true;
        }
        else
        {
            _fixedDirection = -transform.right;
            _hasTarget = false;
        }

        timer = StartCoroutine(ReturnToPoolAfterTime());
        _noiseSeed = Random.Range(0f, 1000f);
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        Vector2 perpendicular = new Vector2(-_fixedDirection.y, _fixedDirection.x);

        float noiseTime = Time.time * noiseFrequency + _noiseSeed;
        float wave = Mathf.Sin(noiseTime) * noiseMagnitude;

        rb.velocity = (_fixedDirection * speed) + (perpendicular * wave);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ResetProjectile();
        PoolManager.Instance.ReturnToPool("MageSpell", gameObject);
    }

    protected IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(timeLimit);
        ResetProjectile();
        PoolManager.Instance.ReturnToPool("MageSpell", gameObject);
    }

    private void ResetProjectile()
    {
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }
        if (rb != null) rb.velocity = Vector2.zero;
    }
}
