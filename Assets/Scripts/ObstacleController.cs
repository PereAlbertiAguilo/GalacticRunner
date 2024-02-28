using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    Health health;
    GameObject mask;
    GameObject explosionParticle;

    [SerializeField] float lifeTime = 10f;
    [SerializeField] int scorePoints = 10;

    HudManager hudManager;

    private void Awake()
    {
        health = GetComponent<Health>();
        mask = transform.GetChild(0).gameObject;
        explosionParticle = transform.GetChild(1).gameObject;
    }

    private void Start()
    {
        hudManager = FindObjectOfType<HudManager>();

        Destroy(this.gameObject, lifeTime);
    }

    private void Update()
    {
        if (!health.isAlive)
        {
            DestroyObject();
            hudManager.score += scorePoints;
        }

        if(health.currentHealth <= (health.maxHealth * 20) / 100 && !mask.activeInHierarchy)
        {
            mask.SetActive(true);
        }
    }
    void DestroyObject()
    {
        explosionParticle.transform.parent = null;
        explosionParticle.SetActive(true);

        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Bullet")
        {
            health.currentHealth -= 2;
        }
        else if(tag == "Player")
        {
            DestroyObject();
        }
    }
}
