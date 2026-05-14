using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageCurseProjectile : BaseProjectile
{
    [Header("Настройки траектории \"Змейка\"")]
    [SerializeField] private float noiseMagnitude = 2f; // Ширина змейки (амплитуда)
    [SerializeField] private float noiseFrequency = 5f; // Частота/скорость виляния

    [Header("Длительность накладываемой немоты")]
    [SerializeField] private float silenceDuration = 3f;

    private Vector2 _fixedDirection; // Зафиксированное направление полета
    private float _noiseSeed;

    public override void OnObjectSpawn()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Vector2 targetPosition = playerObj.transform.position;
            _fixedDirection = (targetPosition - (Vector2)transform.position).normalized;
        }
        else
        {
            _fixedDirection = -transform.right;
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
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerInputHandler inputHandler = collision.gameObject.GetComponent<PlayerInputHandler>();

            if (inputHandler != null)
            {
                inputHandler.DisableShootingForTime(silenceDuration);
            }
        }

        ResetProjectile();
        PoolManager.Instance.ReturnToPool("MageCurse", gameObject);
    }

    protected IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(timeLimit);
        ResetProjectile();
        PoolManager.Instance.ReturnToPool("MageCurse", gameObject);
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
