using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FightAreaType { Eliminate, Survive }
public class FightArea : MonoBehaviour
{
    public FightAreaType areaType;
    [SerializeField] private string areaID;
    [SerializeField] private List<GameObject> enemiesInArea;
    [SerializeField] private float surviveTime = 20f;

    private bool _isActivated = false;
    private float _timer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isActivated && other.CompareTag("Player"))
        {
            ActivateArea();
        }
    }

    private void ActivateArea()
    {
        _isActivated = true;
        _timer = surviveTime; 

        foreach (var enemy in enemiesInArea)
        {
            if (enemy != null) enemy.SetActive(true);
        }

        this.RequestState<FightGameState>();
    }

    private void Update()
    {
        if (!_isActivated) return;

        if (areaType == FightAreaType.Eliminate)
        {
            enemiesInArea.RemoveAll(enemy => enemy == null);

            if (enemiesInArea.Count == 0)
            {
                FinishArea();
            }
        }
        else if (areaType == FightAreaType.Survive)
        {
            _timer -= Time.deltaTime;


            if (_timer <= 0)
            {
                FinishArea();
            }
        }
    }

    private void FinishArea()
    {
        _isActivated = false;

        if (areaType == FightAreaType.Survive)
        {
            foreach (var enemy in enemiesInArea)
            {
                if (enemy != null) Destroy(enemy);
            }
        }

        Debug.Log($"Area {areaID} Cleared!");
        this.RequestState<RunGameState>();

        gameObject.SetActive(false);
    }
}
