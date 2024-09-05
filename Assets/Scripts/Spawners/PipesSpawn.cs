using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipesSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _pipe;
    [SerializeField] private float SpawnRate = 2;
    [SerializeField] private float HeightOffset = 10;
    private float Timer = 0;

    private void Start()
    {
        SpawnPipe();
    }
    private void Update()
    {
        if( Timer < SpawnRate)
        {
            Timer = Timer + Time.deltaTime;
        }
        else
        {
            SpawnPipe();
            Timer = 0;
        }
    }

    private void SpawnPipe()
    {

        float lowestPoinPipe = transform.position.y - HeightOffset;
        float highestPoinPipe = transform.position.y + HeightOffset;

        Instantiate(_pipe, new Vector3(transform.position.x, Random.Range(lowestPoinPipe,highestPoinPipe), 0), transform.rotation);
        
    }
}
