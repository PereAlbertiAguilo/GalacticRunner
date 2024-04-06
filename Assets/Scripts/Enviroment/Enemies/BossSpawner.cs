using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] bosses;
    [SerializeField] Transform playerPos;
    public int bossIndex = 0;

    ObjectSpawner _obstacleSpawner;

    void Start()
    {
        _obstacleSpawner = GetComponent<ObjectSpawner>();
    }

    void Update()
    {
        if (_obstacleSpawner.spawnRateAugment <= _obstacleSpawner.maxSpawnRate)
        {
            if (bosses.Length > 0 && _obstacleSpawner.varieSpawnRateAmount)
            {
                if (bossIndex >= bosses.Length)
                {
                    _obstacleSpawner.canSpawn = false;
                    StartCoroutine(GameManager.instance.StageCleared());
                    return;
                }

                Instantiate(bosses[bossIndex], playerPos.transform);
                bossIndex++;
                _obstacleSpawner.varieSpawnRateAmount = false;
            }
        }
    }
}
