using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteEnviroment : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] GameObject spaceTile;
    [SerializeField] Sprite[] tileSprites;
    [SerializeField] int poolSize = 5;

    [SerializeField] float lastTilePos;
    [SerializeField] float maxSpaceGap;

    [SerializeField] List<GameObject> spaceTilePool = new List<GameObject>();

    private void Start()
    {
        FillPool();
    }

    private void Update()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        NextTile();
    }

    void NextTile()
    {
        int activeTiles = 0;

        foreach (GameObject g in spaceTilePool)
        {
            if (g.activeInHierarchy)
            {
                activeTiles++;
            }
        }
        for (int i = 0; i < spaceTilePool.Count; i++)
        {
            if (spaceTilePool[i].activeInHierarchy && (playerPos.position.y - spaceTilePool[i].transform.position.y) >= maxSpaceGap)
            {
                spaceTilePool[i].SetActive(false);
            }
            if (activeTiles <= 3 && !spaceTilePool[i].activeInHierarchy)
            {
                SpawnTile(i);
            }
        }
        
    }

    void SpawnTile(int i)
    {
        int r = Random.Range(0, tileSprites.Length);
        lastTilePos += 16;
        spaceTilePool[i].GetComponent<SpriteRenderer>().sprite = tileSprites[r];
        spaceTilePool[i].SetActive(true);
        spaceTilePool[i].transform.position = new Vector2(0, transform.position.y + lastTilePos);
    }

    

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
