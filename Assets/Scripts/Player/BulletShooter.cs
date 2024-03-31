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

    [SerializeField] bool isPlayerBullet = true;

    private void Start()
    {
        if (isPlayerBullet)
        {
            bulletSpawnRate -= (PlayerPrefs.HasKey("BulletShotSpeedSelect") ? (float)PlayerPrefs.GetInt("BulletShotSpeedSelect") + 1 : 0) / 100;
        }

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

        if (bulletSpawnRate <= 0.0f && gameObject.activeInHierarchy)
        {
            ShootBullet();

            bulletSpawnRate = startRate;
        }
    }

    // From a gameobejct pool spawns a new bullet if the gameobject isn't active and plays a sound
    void ShootBullet()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if(bullet == null)
            {
                bulletPool.Remove(bullet);
            }
        }

        foreach (GameObject g in bulletPool) 
        {
            if (!g.activeSelf)
            {
                AudioManager.instance.AudioPlayOneShotVolume(bulletClip, .0015f, false);
                g.SetActive(true);

                if (!isPlayerBullet)
                {
                    g.transform.parent = null;
                }

                break;
            }
        }
    }

    // Fills a gameobject pool
    void FillPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject g = Instantiate(bullet, transform.position, Quaternion.identity);
            g.transform.parent = transform;
            bulletPool.Add(g);
            g.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject g in bulletPool)
        {
            if (!isPlayerBullet && g != null && g.activeInHierarchy)
            {
                g.transform.parent = null;
            }
        }
    }
}
