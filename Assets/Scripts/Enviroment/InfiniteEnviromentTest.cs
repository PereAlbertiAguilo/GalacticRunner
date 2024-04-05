using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteEnviromentTest : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] GameObject tile;
    [SerializeField] Sprite[] tileSprites;
    [SerializeField] int poolSize = 5;

    [SerializeField] int saveTile;
    GameObject lastTile;
    float tileSize;

    List<GameObject> tilePool = new List<GameObject>();

    int lastTileIndex;
    bool firstTile = false;

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
        if(!firstTile)
        {
            firstTile = true;

            float borderBottom = -ScreenBorderLimit.Y() + tileSize / 2;

            SpawnTile(0, borderBottom);
        }
        else
        {
            for (int i = 0; i < tilePool.Count; i++)
            {
                if (!tilePool[i].activeInHierarchy)
                {
                    SpawnTile(i, lastTile.transform.position.y + tileSize);
                }
            }
        }
    }

    int RandomTile()
    {
        return Random.Range(0, tileSprites.Length);
    }

    // Spawns a new tile with a random sprite
    void SpawnTile(int i, float nextPos)
    {
        int r = RandomTile();
        if(r == lastTileIndex)
        {
            r = saveTile;
        }
        lastTileIndex = r;

        tilePool[i].GetComponent<SpriteRenderer>().sprite = tileSprites[r];
        tilePool[i].transform.localPosition = transform.InverseTransformPoint(new Vector2(0, nextPos));
        tilePool[i].SetActive(true);
        lastTile = tilePool[i];
    }


    // Fills a gameobject pool
    void FillPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject g = Instantiate(tile, transform);
            tilePool.Add(g);
            g.transform.localScale = new Vector3(ScreenBorderLimit.X() * 2, ScreenBorderLimit.X() * 2);
            tileSize = g.transform.localScale.x;
            g.SetActive(false);
        }
    }
}
