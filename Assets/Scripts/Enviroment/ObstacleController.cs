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
    [SerializeField] bool canTakeDamage = true;

    [Space]

    [SerializeField] AudioClip destroyClip;
    [SerializeField] GameObject explosionParticle;
    [SerializeField] GameObject textPopUp;

    HudManager hudManager;
    PlayerController playerController;


    bool awake = false;

    Transform playerPos;

    private void Awake()
    {
        health = GetComponent<Health>();
        mask = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        playerPos = FindObjectOfType<PlayerController>(true).transform.parent;
        hudManager = FindObjectOfType<HudManager>();
        playerController = FindObjectOfType<PlayerController>();

        scorePoints *= PlayerPrefs.HasKey("ScrapsAmountSelect") ? (PlayerPrefs.GetInt("ScrapsAmountSelect") + 2) : 1;
    }

    private void OnEnable()
    {
        awake = true;
        currentLifeTime = lifeTime;
        health.isAlive = true;
    }

    private void Update()
    {
        // If this gameobject isn't alive deactivates this gameobject
        if (!health.isAlive)
        {
            DeactivateObject(true, true);
        }
        else
        {
            // If the current health of the gameobject gets to 10% activates a mask
            if (health.currentHealth <= (health.maxHealth * 20) / 100 && !mask.activeInHierarchy && awake)
            {
                mask.SetActive(true);
            }

            if(playerPos.position.y - 18 >= transform.position.y)
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
    void DeactivateObject(bool playerDestroyed, bool bulletCollided)
    {
        // If this gameobject is destroyed by the player plays a sound and instantiates a particle
        if (playerDestroyed && !GameManager.instance.gameOver)
        {
            AudioManager.instance.AudioPlayOneShotVolume(destroyClip, 1f, true);
            InstantiateObject(explosionParticle);

            if (bulletCollided)
            {
                hudManager.pointsScore += scorePoints;
                hudManager.pointsText.GetComponent<Animator>().Play("ScorePop");

                TextMeshProUGUI t = InstantiateObject(textPopUp).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                t.text = "" + scorePoints + " S";
                t.fontSize = 1;

                hudManager.pointsText.text = "Scraps " + hudManager.pointsScore;
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
            }

            if (PlayerPrefs.HasKey("BulletsSelect") && PlayerPrefs.GetInt("BulletsSelect") > 0)
            {
                health.currentHealth -= PlayerPrefs.GetInt("BulletsSelect") * 2;
            }
            else
            {
                health.currentHealth -= 1;
            }

            if(Mathf.RoundToInt(health.currentHealth) == Mathf.RoundToInt((health.maxHealth * 20) / 100) ||
                Mathf.RoundToInt(health.currentHealth) == Mathf.RoundToInt((health.maxHealth * 40) / 100) ||
                Mathf.RoundToInt(health.currentHealth) == Mathf.RoundToInt((health.maxHealth * 60) / 100) ||
                Mathf.RoundToInt(health.currentHealth) == Mathf.RoundToInt((health.maxHealth * 80) / 100))
            {
                InstantiateObject(explosionParticle);
            }

        }
        // If this gameobject collides with the player it deactivates itself and updates the health bar text
        else if (tag == "Player" && playerController.canTakeDamage)
        {
            DeactivateObject(true, false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 2);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Obstacle")
        {
            ObstacleController oc = collision.gameObject.GetComponent<ObstacleController>();

            if (oc != null)
            {
                if (priority < oc.priority)
                {
                    DeactivateObject(transform.position.y > playerPos.position.y + 14 ? false : true, false);
                }
                if (priority == oc.priority)
                {
                    if (transform.position.y > oc.transform.position.y)
                    {
                        DeactivateObject(transform.position.y > playerPos.position.y + 14 ? false : true, false);
                    }
                }
            }
        }
    }
}
