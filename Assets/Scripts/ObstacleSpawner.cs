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

    private void Start()
    {
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

    void SpawnObstscle()
    {
        GameObject obstacle = RandomObstacle();
        Vector2 spawnPos = RandomPosition();

        GameObject instance = Instantiate(obstacle, spawnPos, Quaternion.Euler(0, 0, RandomRotation()));
    }
}
