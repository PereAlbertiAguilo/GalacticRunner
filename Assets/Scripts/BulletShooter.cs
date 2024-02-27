using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] float bulletSpawnRate = .5f;

    [SerializeField] GameObject bullet;

    [SerializeField] int poolSize = 50;

    [SerializeField] List<GameObject> bulletPool = new List<GameObject>();

    private void Start()
    {
        FillPool();

        InvokeRepeating("ShootBullet", bulletSpawnRate, bulletSpawnRate);
    }

    void ShootBullet()
    {
        foreach (GameObject g in bulletPool) 
        {
            if (!g.activeSelf)
            {
                g.SetActive(true);
                g.transform.parent = null;
                break;
            }
        }
    }

    void FillPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject g = Instantiate(bullet, transform);
            bulletPool.Add(g);
            g.SetActive(false);
        }
    }
}
