using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossController : MonoBehaviour
{
    [SerializeField] float speed;

    [Space]

    [SerializeField] int scorePoints = 100;
    [SerializeField] GameObject textPopUp;

    bool canTakeDamage = false;

    Transform playerPos;
    Image healthBar;
    Animator _animator;
    Health health;
    ObstacleSpawner obstacleSpawner;
    HudManager hudManager;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        playerPos = FindObjectOfType<PlayerController>(true).transform;
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        hudManager = FindObjectOfType<HudManager>();

        healthBar = GameObject.FindGameObjectWithTag("BossHealthBar").GetComponent<Image>();
        healthBar.enabled = true;

        StartCoroutine(EnterScene());
    }

    private void Update()
    {
        transform.position = Vector3.LerpUnclamped(transform.position, new Vector2(playerPos.position.x, transform.position.y), Time.deltaTime * speed);
    }

    IEnumerator EnterScene()
    {
        yield return new WaitForSeconds(1);

        _animator.applyRootMotion = true;
        canTakeDamage = true;
    }
    IEnumerator ExitScene()
    {
        _animator.Play("ExitScene");
        canTakeDamage = false;

        yield return new WaitForSeconds(1);

        healthBar.enabled = false;
        obstacleSpawner.varieSpawnRateAmount = true;
        Destroy(gameObject);
    }

    GameObject InstantiateObject(GameObject instance)
    {
        GameObject g = Instantiate(instance, transform.parent.position, Quaternion.identity);

        return g;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        // If this gameobject collides with a bullet takes away 2 points of its life
        if ((tag == "Bullet" || tag == "Player") && canTakeDamage)
        {
            if (health.currentHealth == health.maxHealth)
            {
               //InstantiateObject(explosionParticle);
            }

            if (PlayerPrefs.HasKey("bulletSelect") && PlayerPrefs.GetInt("bulletSelect") > 0)
            {
                health.currentHealth -= PlayerPrefs.GetInt("bulletSelect") * 2;
            }
            else
            {
                health.currentHealth -= 1;
            }

            if (!health.isAlive)
            {
                hudManager.pointsScore += scorePoints;
                hudManager.pointsText.GetComponent<Animator>().Play("ScorePop");

                TextMeshProUGUI t = InstantiateObject(textPopUp).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                t.text = "" + scorePoints + " S";
                t.fontSize = 2;

                StartCoroutine(ExitScene());
            }

            healthBar.fillAmount = (float)health.currentHealth / health.maxHealth;

            if (Mathf.RoundToInt(health.currentHealth) == Mathf.RoundToInt((health.maxHealth * 20) / 100) ||
                Mathf.RoundToInt(health.currentHealth) == Mathf.RoundToInt((health.maxHealth * 40) / 100) ||
                Mathf.RoundToInt(health.currentHealth) == Mathf.RoundToInt((health.maxHealth * 60) / 100) ||
                Mathf.RoundToInt(health.currentHealth) == Mathf.RoundToInt((health.maxHealth * 80) / 100))
            {
                //InstantiateObject(explosionParticle);
            }

        }
    }
}
