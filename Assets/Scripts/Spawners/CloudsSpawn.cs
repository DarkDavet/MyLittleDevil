using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] _clouds;
    [SerializeField] private float SpawnRate = 3;
    [SerializeField] private float HeightOffset = 10;
    private float Timer = 0;

    private void Start()
    {
        SpawnCloud();
    }
    private void Update()
    {
        if (Timer < SpawnRate)
        {
            Timer = Timer + Time.deltaTime;
        }
        else
        {
            SpawnCloud();
            Timer = 0;
        }
    }

    private void SpawnCloud()
    {

        float lowestPoinPipe = transform.position.y - HeightOffset;
        float highestPoinPipe = transform.position.y + HeightOffset;

        GameObject cloud = Instantiate(_clouds[Random.Range(0, _clouds.Length)], new Vector3(transform.position.x, Random.Range(lowestPoinPipe, highestPoinPipe), 0), transform.rotation);
        float randomScale = Random.Range(0.5f, 1.5f);
        cloud.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }
}
