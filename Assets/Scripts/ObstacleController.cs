using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    Health health;
    GameObject mask;
    [SerializeField] float lifeTime = 10f;

    private void Awake()
    {
        health = GetComponent<Health>();
        mask = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

    private void Update()
    {
        if (!health.isAlive)
        {
            DestroyObject();
        }

        if(health.currentHealth <= (health.maxHealth * 20) / 100 && !mask.activeInHierarchy)
        {
            mask.SetActive(true);
        }
    }
    void DestroyObject()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Bullet")
        {
            health.currentHealth--;
        }
        else if(tag == "Player")
        {
            DestroyObject();
        }

    }
}
