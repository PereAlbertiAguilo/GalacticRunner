using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] ObstacleScriptableObject[] obstacleScriptableObject;

    //[SerializeField] Transform playerPos;

    [Space]

    [SerializeField] float initialSpawnRate;
    [SerializeField] float maxSpawnRate;
    [SerializeField] float spawnRateAugment;
    [SerializeField] float spawnRateAugmentSpeed = 0.05f;

    [Space]

    [SerializeField] bool randomRotation = true;

    [SerializeField] List<GameObject> pool = new List<GameObject>();

    float currentSpawnRate;

    private void Start()
    {
        spawnRateAugment = initialSpawnRate;


        FillPool();
    }

    private void Update()
    {
        // Makes a value go down slowly over time 
        if (spawnRateAugment > maxSpawnRate)
        {
            spawnRateAugment -= Time.deltaTime * spawnRateAugmentSpeed;

            spawnRateAugment = Mathf.Clamp(spawnRateAugment, maxSpawnRate, spawnRateAugment);
        }
        if (spawnRateAugment <= maxSpawnRate)
        {
            spawnRateAugment = initialSpawnRate;
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

            currentSpawnRate = spawnRateAugment;
        }
    }

    Vector2 ScreenBorderLimit()
    {
        Vector2 screenRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 0.5f));
        Vector2 screenTop = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1));

        float x = screenRight.x;
        float y = screenTop.y;

        return new Vector2(x, y);
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
        float x = Random.Range(-ScreenBorderLimit().x, ScreenBorderLimit().x);
        float y = Random.Range(ScreenBorderLimit().y + 25, ScreenBorderLimit().y + 50);

        return new Vector2(x, y);
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

        // Spawns a random obstacle if the gameobject
        // isn't active with a random position and rotation
        if (unactivePool.Count > 0)
        {
            GameObject g = unactivePool[RandomIndex(unactivePool)];

            // If the last spawned obtsale pos is near within a certain range
            // to the next spawning obtsacle choses a different random position
            if (Mathf.Abs(spawnPos.x) < Mathf.Abs(lastPos.x) + 1.5f && 
                Mathf.Abs(spawnPos.y) < Mathf.Abs(lastPos.y) + 1.5f)
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
