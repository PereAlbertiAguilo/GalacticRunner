using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] ObstacleScriptableObject[] obstacleScriptableObject;

    //[SerializeField] GameObject[] obstacles;

    [SerializeField] Transform playerPos;

    [SerializeField] Vector2 spawnLimits;

    [SerializeField] float initialSpawnRate;
    [SerializeField] float maxSpawnRate;

    [SerializeField] bool randomRotation = true;

    //[SerializeField] int poolSize = 20;

    [SerializeField] List<GameObject> pool = new List<GameObject>();

    float currentSpawnRate;

    private void Start()
    {
        FillPool();
    }

    private void Update()
    {
        // Makes a value go down slowly over time 
        if (initialSpawnRate > maxSpawnRate)
        {
            initialSpawnRate -= Time.deltaTime * .005f;

            initialSpawnRate = Mathf.Clamp(initialSpawnRate, maxSpawnRate, initialSpawnRate);
        }

        SpawnRepeting();
    }

    // Spawns obstacles depending on a timer that goes down depending on the initial spawn rate
    void SpawnRepeting()
    {
        currentSpawnRate -= Time.deltaTime;

        if (currentSpawnRate <= 0.0f)
        {
            SpawnObstscle();

            currentSpawnRate = initialSpawnRate;
        }
    }

    // Returns a random value between 0 and 360
    float RandomRotation()
    {
        int randomIndex = Random.Range(1, 360);

        return randomRotation ? randomIndex : 0;
    }

    // Returns a random vector 2 within some parameters
    Vector2 RandomPosition()
    {
        float x = Random.Range(-spawnLimits.x, spawnLimits.x);
        float y = Random.Range(spawnLimits.y, spawnLimits.y * 2);

        return new Vector2(x, y + playerPos.position.y);
    }

    // Returns a random integer bettween 0 and a list lenght
    int RandomIndex(List<GameObject> l)
    {
        return Random.Range(0, l.Count);
    }

    Vector2 lastPos = Vector2.zero;

    // Spawns obstacles from a gameobject pool
    void SpawnObstscle()
    {
        Vector2 spawnPos = RandomPosition();

        List<GameObject> unactivePool = new List<GameObject>();

        // Gets all te unactive gamobjects of the pool
        foreach (GameObject a in pool)
        {
            if (!a.activeInHierarchy)
            {
                unactivePool.Add(a);
            }
        }

        // Spawns a random obstacle if the gameobject isn't active with a random position and rotation
        if (unactivePool.Count > 0)
        {
            GameObject g = unactivePool[RandomIndex(unactivePool)];

            // If the last spawned obtsale pos is near within a certain range to the next spawning obtsacle choses a different random position
            if (Mathf.Abs(spawnPos.x) < Mathf.Abs(lastPos.x) + 1.5f && Mathf.Abs(spawnPos.y) < Mathf.Abs(lastPos.y) + 1.5f)
            {
                spawnPos = RandomPosition();
            }

            lastPos = spawnPos;

            g.transform.position = spawnPos;
            g.transform.rotation = Quaternion.Euler(0, 0, RandomRotation());
            g.SetActive(true);
            g.transform.parent = null;
        }
        

        // Clears the unactive pool
        unactivePool.Clear();
    }

    // Fills a gameobject pool
    void FillPool()
    {
        foreach (ObstacleScriptableObject o in obstacleScriptableObject)
        {
            for (int i = 0; i < o.poolSize; i++)
            {
                GameObject g = Instantiate(o.obstacle);
                pool.Add(g);
                g.SetActive(false);
            }
        }
    }
}
