using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageCurseProjectile : BaseProjectile
{
    [Header("Настройки непредсказуемости")]
    [SerializeField] private float rotateSpeed = 50f;
    [SerializeField] private float noiseMagnitude = 5f;
    [SerializeField] private float noiseFrequency = 3f;

    [Header("Длительность накладываемой немоты")]
    [SerializeField] private float silenceDuration = 3f;

    private Transform _player;
    private float _noiseSeed;

    public override void OnObjectSpawn()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) _player = playerObj.transform;

        timer = StartCoroutine(ReturnToPoolAfterTime());
        _noiseSeed = Random.Range(0f, 1000f);
    }

    private void FixedUpdate()
    {
        if (_player == null || rb == null) return;

        Vector2 targetDirection = (_player.position - transform.position).normalized;

        float noiseTime = Time.time * noiseFrequency + _noiseSeed;
        float noise = (Mathf.PerlinNoise(noiseTime, 0f) - 0.5f) * 2f;

        Vector2 noisyDirection = Quaternion.Euler(0, 0, noise * noiseMagnitude) * targetDirection;

        float angleDiff = Vector2.SignedAngle(transform.right, noisyDirection);
        float rotationStep = Mathf.Sign(angleDiff) * Mathf.Min(Mathf.Abs(angleDiff), rotateSpeed * Time.fixedDeltaTime);

        rb.MoveRotation(rb.rotation + rotationStep);

        rb.velocity = -transform.right * speed;
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
