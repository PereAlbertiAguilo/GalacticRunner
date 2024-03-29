using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] float bulletSpawnRate = .15f;
    float startRate;

    [SerializeField] GameObject bullet;

    [SerializeField] int poolSize = 50;

    [SerializeField] List<GameObject> bulletPool = new List<GameObject>();

    [SerializeField] AudioClip bulletClip;

    private void Start()
    {
        bulletSpawnRate -= (PlayerPrefs.HasKey("bulletShotSpeedSelect") ? (float)PlayerPrefs.GetInt("bulletShotSpeedSelect") + 1 : 0) / 100;

        startRate = bulletSpawnRate;

        FillPool();
    }

    private void Update()
    {
        SpawnRepeting();
    }

    void SpawnRepeting()
    {
        bulletSpawnRate -= Time.deltaTime;

        if (bulletSpawnRate <= 0.0f)
        {
            ShootBullet();

            bulletSpawnRate = startRate;
        }
    }

    // From a gameobejct pool spawns a new bullet if the gameobject isn't active and plays a sound
    void ShootBullet()
    {
        foreach (GameObject g in bulletPool) 
        {
            if (!g.activeSelf)
            {
                AudioManager.instance.AudioPlayOneShotVolume(bulletClip, .0015f, false);
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
