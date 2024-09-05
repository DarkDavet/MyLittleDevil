using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] _stars;
    [SerializeField] private float SpawnRate = 3;
    [SerializeField] private float HeightOffset = 10;
    private float Timer = 0;

    private void Start()
    {
        SpawnStar();
    }
    private void Update()
    {
        if (Timer < SpawnRate)
        {
            Timer = Timer + Time.deltaTime;
        }
        else
        {
            SpawnStar();
            Timer = 0;
        }
    }

    private void SpawnStar()
    {

        float lowestPointStar = transform.position.y - HeightOffset;
        float highestPointStar = transform.position.y + HeightOffset;

        Instantiate(_stars[Random.Range(0, _stars.Length)], new Vector3(transform.position.x, Random.Range(lowestPointStar, highestPointStar), 0), transform.rotation);
       // float randomScale = Random.Range(0.5f, 1.0f);
       // cloud.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }
}
