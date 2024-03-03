using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] float bulletSpawnRate = .5f;

    [SerializeField] GameObject bullet;

    [SerializeField] int poolSize = 50;

    [SerializeField] List<GameObject> bulletPool = new List<GameObject>();

    [SerializeField] AudioClip bulletClip;

    private void Start()
    {
        FillPool();

        // Invokes a function every few seconds
        InvokeRepeating("ShootBullet", bulletSpawnRate, bulletSpawnRate);
    }

    // From a gameobejct pool spawns a new bullet if the gameobject isn't active and plays a sound
    void ShootBullet()
    {
        foreach (GameObject g in bulletPool) 
        {
            if (!g.activeSelf)
            {
                AudioManager.instance.AudioPlayOneShotVolume(bulletClip, .0075f, false);
                g.SetActive(true);
                g.transform.parent = null;
                break;
            }
        }
    }

    // Fills a gameobject pool
    void FillPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject g = Instantiate(bullet, transform);
            g.transform.rotation = transform.rotation;
            bulletPool.Add(g);
            g.SetActive(false);
        }
    }
}
