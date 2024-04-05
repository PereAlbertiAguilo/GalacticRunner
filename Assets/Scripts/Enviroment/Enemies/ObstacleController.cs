using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] int scorePoints = 10;
    [SerializeField] int priority = 0;

    [Space]

    [SerializeField] AudioClip destroyClip;
    [SerializeField] GameObject explosionParticle;
    [SerializeField] GameObject textPopUp;

    int particleIndex = 0;

    HudManager hudManager;
    PlayerController playerController;
    Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void Start()
    {
        hudManager = FindObjectOfType<HudManager>();
        playerController = FindObjectOfType<PlayerController>();

        scorePoints *= PlayerPrefs.HasKey("ScrapsAmountSelect") ? (PlayerPrefs.GetInt("ScrapsAmountSelect") + 2) : 1;
    }

    private void OnEnable()
    {
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
            if (transform.position.y + GetComponent<SpriteRenderer>().bounds.size.y < -ScreenBorderLimit.Y())
            {
                DeactivateObject(false, false);
            }
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
        gameObject.SetActive(false);
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

            health.currentHealth -= collision.gameObject.GetComponent<BulletController>().bulletDamage;

            particleIndex++;

            if ((float)particleIndex % 6 == 0)
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
                    DeactivateObject(transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y <= ScreenBorderLimit.Y(), false);
                }
                if (priority == oc.priority)
                {
                    if (transform.position.y > oc.transform.position.y)
                    {
                        DeactivateObject(transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y <= ScreenBorderLimit.Y(), false);
                    }
                }
            }
        }
    }
}
