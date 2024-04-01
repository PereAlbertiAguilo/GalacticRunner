using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossController : MonoBehaviour
{
    [SerializeField] bool followPlayer = true;
    [SerializeField] float speed;

    [Space]

    public int scorePoints = 100;
    public GameObject textPopUp;

    [HideInInspector] public bool canTakeDamage = false;

    Transform playerPos;
    [HideInInspector] public Image healthBar;
    Animator _animator;
    [HideInInspector] public Health health;
    ObstacleSpawner obstacleSpawner;
    [HideInInspector] public HudManager hudManager;

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
        if (followPlayer)
        {
            transform.position = Vector3.LerpUnclamped(transform.position, new Vector2
                (playerPos.position.x, transform.position.y), Time.deltaTime * speed);
        }
    }

    public IEnumerator EnterScene()
    {
        yield return new WaitForSeconds(1);

        if (followPlayer) 
        { 
            _animator.applyRootMotion = true; 
        }
        canTakeDamage = true;
    }
    public IEnumerator ExitScene(bool hasIdleAnim)
    {
        if (hasIdleAnim)
        {
            _animator.SetBool("ExitScene", true);
        }
        else
        {
            _animator.Play("ExitScene");
        }
        canTakeDamage = false;

        yield return new WaitForSeconds(1);

        healthBar.enabled = false;
        obstacleSpawner.varieSpawnRateAmount = true;
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    public GameObject InstantiateObject(GameObject instance)
    {
        GameObject g = Instantiate(instance, transform.parent.position, Quaternion.identity);

        return g;
    }

    public static bool HasParameter(string paramName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        // If this gameobject collides with a bullet takes away 2 points of its life
        if ((tag == "Bullet" || tag == "Player") && canTakeDamage)
        {
            health.currentHealth -= collision.gameObject.GetComponent<BulletController>().bulletDamage;

            if (!health.isAlive)
            {
                hudManager.pointsScore += scorePoints;
                hudManager.pointsText.GetComponent<Animator>().Play("ScorePop");

                TextMeshProUGUI t = InstantiateObject(textPopUp).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                t.text = "" + scorePoints + " S";
                t.fontSize = 2;

                hudManager.pointsText.text = "Scraps " + hudManager.pointsScore;

                StartCoroutine(ExitScene(HasParameter("ExitScene", _animator) ? true : false));
            }

            healthBar.fillAmount = (float)health.currentHealth / health.maxHealth;
        }
    }
}
