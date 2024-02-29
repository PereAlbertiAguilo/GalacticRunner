using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] obstacles;

    [SerializeField] Transform playerPos;

    [SerializeField] Vector2 spawnLimits;

    [SerializeField] float spawnRate;

    [SerializeField] int poolSize = 20;

    [SerializeField] List<GameObject> pool = new List<GameObject>();

    private void Start()
    {
        FillPool();

        InvokeRepeating("SpawnObstscle", spawnRate, spawnRate);
    }

    GameObject RandomObstacle()
    {
        int randomIndex = Random.Range(0, obstacles.Length);

        return obstacles[randomIndex];
    }

    float RandomRotation()
    {
        int randomIndex = Random.Range(1, 4);

        return 90 * randomIndex;
    }

    Vector2 RandomPosition()
    {
        float x = Random.Range(-spawnLimits.x, spawnLimits.x);
        float y = Random.Range(spawnLimits.y, spawnLimits.y * 2);

        return new Vector2(x, y + playerPos.position.y);
    }

    int RandomIndex()
    {
        return Random.Range(0, pool.Count);
    }

    void SpawnObstscle()
    {
        GameObject g = pool[RandomIndex()];

        if (!g.activeSelf)
        {
            GameObject obstacle = RandomObstacle();
            Vector2 spawnPos = RandomPosition();

            g.transform.position = spawnPos;
            g.transform.rotation = Quaternion.Euler(0, 0, RandomRotation());
            g.SetActive(true);
            g.transform.parent = null;
        }
    }

    void FillPool()
    {
        foreach (GameObject o in obstacles)
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject g = Instantiate(o);
                pool.Add(g);
                g.SetActive(false);
            }
        }
    }
}
