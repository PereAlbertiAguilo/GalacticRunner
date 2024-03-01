using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    Health health;
    GameObject mask;

    [SerializeField] float lifeTime = 10f;
    [SerializeField] float currentLifeTime = 0;
    [SerializeField] int scorePoints = 10;

    HudManager hudManager;

    bool awake = false;

    [SerializeField] GameObject explosionParticle;

    private void Awake()
    {
        currentLifeTime = lifeTime;

        health = GetComponent<Health>();
        mask = transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        hudManager = FindObjectOfType<HudManager>();

        awake = true;
        currentLifeTime = lifeTime;
        health.isAlive = true;
    }

    void Timer()
    {
        currentLifeTime -= Time.deltaTime;

        if (currentLifeTime <= 0.0f)
        {
            DeactivateObject(false);
        }
    }

    private void Update()
    {
        if (!health.isAlive)
        {
            hudManager.pointsScore += scorePoints;
            hudManager.scoreText.GetComponent<Animator>().Play("ScorePop");

            DeactivateObject(true);
        }
        else
        {
            if (health.currentHealth <= (health.maxHealth * 20) / 100 && !mask.activeInHierarchy && awake)
            {
                mask.SetActive(true);
            }
        }

        Timer();
    }

    void DeactivateObject(bool particle)
    {
        if (particle)
        {
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
        }

        mask.SetActive(false);
        gameObject.SetActive(false);
        awake = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Bullet")
        {
            health.currentHealth -= 2;
        }
        else if (tag == "Player")
        {
            DeactivateObject(true);
        }
    }
}
