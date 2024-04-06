using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    const float SPAWN_RATE_AUGMENT_SPEED = 0.01f;

    [SerializeField] ObstacleScriptableObject[] obstacleScriptableObject;

    [Space]

    [SerializeField] float initialSpawnRate = 2.0f;
    public float maxSpawnRate = 0.5f;
    public float spawnRateAugment = 0;
    public bool varieSpawnRateAmount = true;

    [Space]

    [SerializeField] bool randomRotation = true;

    List<GameObject> obstaclePool = new List<GameObject>();
    float currentSpawnRate;

    [HideInInspector] public bool canSpawn = true;

    private void Start()
    {
        initialSpawnRate += maxSpawnRate;

        spawnRateAugment = initialSpawnRate;

        FillPool();
    }

    private void Update()
    {
        if (canSpawn)
        {
            SpawnRepeting();
        }

        // Makes a value go down slowly over time 
        if (spawnRateAugment > maxSpawnRate && varieSpawnRateAmount)
        {
            spawnRateAugment -= Time.deltaTime * SPAWN_RATE_AUGMENT_SPEED;
        }
        if (spawnRateAugment <= maxSpawnRate)
        {
            StartCoroutine(ResetSpawnRate());
        }
    }

    IEnumerator ResetSpawnRate()
    {
        yield return new WaitForSeconds(1);

        spawnRateAugment = initialSpawnRate;
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

    // Returns a random value between 0 and 360
    float RandomRotation()
    {
        int randomIndex = Random.Range(1, 360);

        return randomRotation ? randomIndex : 0;
    }

    // Returns a random vector 2 within some parameters
    Vector2 RandomPosition()
    {
        float x = Random.Range(-ScreenBorderLimit.X() + .5f, ScreenBorderLimit.X() - .5f);
        float y = Random.Range(ScreenBorderLimit.Y() + 25, ScreenBorderLimit.Y() + 50);

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
        foreach (GameObject a in obstaclePool)
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
                obstaclePool.Add(g);
                g.SetActive(false);
            }
        }
    }
}
