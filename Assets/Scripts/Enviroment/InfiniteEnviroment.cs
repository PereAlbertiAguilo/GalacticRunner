using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteEnviroment : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] GameObject spaceTile;
    [SerializeField] Sprite[] tileSprites;
    [SerializeField] int poolSize = 5;

    [SerializeField] int saveTile;
    [SerializeField] float lastTilePos;
    [SerializeField] float maxSpaceGap;
    [SerializeField] float tileSize = 16;

    [SerializeField] List<GameObject> spaceTilePool = new List<GameObject>();

    int lastTile;

    private void Start()
    {
        FillPool();
    }

    private void Update()
    {
        NextTile();
    }

    // Gets a tile from a gameobject pool
    void NextTile()
    {
        int activeTiles = 0;

        // Gets the amount of active tiles in the pool
        foreach (GameObject g in spaceTilePool)
        {
            if (g.activeInHierarchy)
            {
                activeTiles++;
            }
        }
        for (int i = 0; i < spaceTilePool.Count; i++)
        {
            // If the tile is to far from the player it deactivates
            if (spaceTilePool[i].activeInHierarchy && (playerPos.position.y - spaceTilePool[i].transform.position.y) >= maxSpaceGap)
            {
                spaceTilePool[i].SetActive(false);
            }
            // If there is 3 or less active tiles spawns a next tile
            if (activeTiles <= 3 && !spaceTilePool[i].activeInHierarchy)
            {
                SpawnTile(i);
            }
        }
        
    }

    int RandomTile()
    {
        return Random.Range(0, tileSprites.Length);
    }

    // Spawns a new tile with a random sprite
    void SpawnTile(int i)
    {
        int r = RandomTile();
        if(r == lastTile)
        {
            r = saveTile;
        }
        lastTile = r;

        lastTilePos += tileSize;
        spaceTilePool[i].GetComponent<SpriteRenderer>().sprite = tileSprites[r];
        spaceTilePool[i].SetActive(true);
        spaceTilePool[i].transform.position = new Vector2(0, transform.position.y + lastTilePos);
    }

    
    // Fills a gameobject pool
    void FillPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject g = Instantiate(spaceTile, transform);
            spaceTilePool.Add(g);
            g.SetActive(false);
        }
    }
}
