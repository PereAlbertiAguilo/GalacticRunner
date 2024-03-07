using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    Health health;
    GameObject mask;

    [SerializeField] float lifeTime = 10f;
    [SerializeField] float currentLifeTime = 0;
    [SerializeField] int scorePoints = 10;
    [SerializeField] int priority = 0;

    [SerializeField] AudioClip destroyClip;
    [SerializeField] GameObject explosionParticle;
    [SerializeField] GameObject textPopUp;

    HudManager hudManager;

    bool awake = false;

    ParticleSystem ps;

    Transform playerPos;

    private void Awake()
    {
        health = GetComponent<Health>();
        mask = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        playerPos = FindObjectOfType<PlayerController>(true).transform.parent;
    }

    private void OnEnable()
    {
        hudManager = FindObjectOfType<HudManager>();

        awake = true;
        currentLifeTime = lifeTime;
        health.isAlive = true;
    }

    private void Update()
    {
        // If this gameobject isn't alive deactivates this gameobject
        if (!health.isAlive)
        {
            hudManager.pointsScore += scorePoints;
            hudManager.pointsText.GetComponent<Animator>().Play("ScorePop");

            DeactivateObject(true, true);
        }
        else
        {
            // If the current health of the gameobject gets to 10% activates a mask
            if (health.currentHealth <= (health.maxHealth * 20) / 100 && !mask.activeInHierarchy && awake)
            {
                mask.SetActive(true);
            }

            if(playerPos.position.y - 16 >= transform.position.y)
            {
                DeactivateObject(false, false);
            }
        }

        // Updates timer
        Timer();
    }

    // Countdown timer
    void Timer()
    {
        currentLifeTime -= Time.deltaTime;

        // If the countdown timer reaches 0 deactivates this gameobject
        if (currentLifeTime <= 0.0f)
        {
            DeactivateObject(false, false);
        }
    }

    // Deactivates this gameobject
    void DeactivateObject(bool playerDestroyed, bool collided)
    {
        // If this gameobject is destroyed by the player plays a sound and instantiates a particle
        if (playerDestroyed && !GameManager.instance.gameOver)
        {
            AudioManager.instance.AudioPlayOneShotVolume(destroyClip, 1f, true);

            InstantiateObject(explosionParticle);

            if (collided)
            {
                TextMeshProUGUI t = InstantiateObject(textPopUp).transform.GetChild(0).GetComponent<TextMeshProUGUI>();

                t.text = "" + scorePoints;
                t.fontSize = 1;
            }
        }

        mask.SetActive(false);
        gameObject.SetActive(false);
        awake = false;
    }

    GameObject InstantiateObject(GameObject instance)
    {
        GameObject g = Instantiate(instance, transform.position, Quaternion.identity);

        return g;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        // If this gameobject collides with a bullet takes away 2 points of its life
        if (tag == "Bullet")
        {
            if(health.currentHealth == health.maxHealth)
            {
                InstantiateObject(explosionParticle);
                TextMeshProUGUI t = InstantiateObject(textPopUp).transform.GetChild(0).GetComponent<TextMeshProUGUI>();

                t.text = "" + health.currentHealth;
                t.color = Color.red;
            }

            if (PlayerPrefs.HasKey("bulletDamage"))
            {
                health.currentHealth -= PlayerPrefs.GetInt("bulletDamage");
            }
            else
            {
                health.currentHealth -= 1;
            }

            if(health.currentHealth % 10 == 0)
            {
                InstantiateObject(explosionParticle);
                TextMeshProUGUI t = InstantiateObject(textPopUp).transform.GetChild(0).GetComponent<TextMeshProUGUI>();

                t.text = "" + health.currentHealth;
                t.color = Color.red;
            }

        }
        // If this gameobject collides with the player it deactivates itself and updates the health bar text
        else if (tag == "Player")
        {
            hudManager.healthBarText.gameObject.SetActive(hudManager.playerController.health.currentHealth <= 1 ? true : false);

            DeactivateObject(true, false);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Obstacle" && priority <= collision.gameObject.GetComponent<ObstacleController>().priority)
        {
            DeactivateObject(transform.position.y > playerPos.position.y + 13? false : true, false);
        }
    }
}
