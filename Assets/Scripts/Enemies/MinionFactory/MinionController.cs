using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionController : MonoBehaviour
{
    [SerializeField] private Transform _minionSpawnPosition;
    [SerializeField] private Transform parentObject;
    private MinionFactory redMinionFactory;
    private MinionFactory blueMinionFactory;
    private void Start()
    {
        redMinionFactory = gameObject.AddComponent<RedMinionFactory>();
        blueMinionFactory = gameObject.AddComponent<BlueMinionFactory>();
    }

    public void SpawnRedMinion()
    {
        Minion redMinion = redMinionFactory.CreateMinion(_minionSpawnPosition.position, parentObject);
    }

    public void SpawnBlueMinion()
    {
        Minion blueMinion = blueMinionFactory.CreateMinion(_minionSpawnPosition.position, parentObject);
    }
}
