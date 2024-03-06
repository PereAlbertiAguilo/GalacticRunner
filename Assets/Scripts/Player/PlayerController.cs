using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    [SerializeField] Vector2 screenBorderLimit;

    [SerializeField] Sprite[] playerSprites;
    [SerializeField] AudioClip explosionClip;
    [HideInInspector] public SpriteRenderer _playerSpriteRenderer;

    [HideInInspector] public Animator _animator;
    public Animator _explosionAnimator;

    [HideInInspector] public Health health;

    [SerializeField] GameObject[] bulletShooter;

    HudManager hudManager;

    private void Awake()
    {
        health = GetComponent<Health>();
        _animator = GetComponent<Animator>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        hudManager = FindObjectOfType<HudManager>();

        // Updates the player sprite
        SpriteUpdate();

        // Updates the player health
        ShieldsUpdate();

        // Adds a delay when enetering the scene
        StartCoroutine(EnterScene());

        // Instantiates a bullet shooter depending on a stored player pref
        Instantiate(bulletShooter[PlayerPrefs.HasKey("spaceSelect") ? PlayerPrefs.GetInt("spaceSelect") : 0], transform);
    }

    private void Update()
    {
        // Updates the player input
        PlayerInput();

        // Updates the screen border
        ScreenBorderLimit();

        // Activates the gameover state if the player dies and plays a sound
        if (!health.isAlive && !GameManager.instance.gameOver)
        {
            AudioManager.instance.AudioPlayOneShotVolume(explosionClip, .5f, false);
            GameManager.instance.GameOver();
        }
    }

    IEnumerator EnterScene()
    {
        yield return new WaitForSeconds(1);

        _animator.applyRootMotion = true;
    }

    // The sprite of the player updates depending on a player pref int
    void SpriteUpdate()
    {
        if (PlayerPrefs.HasKey("spaceSelect"))
        {
            _playerSpriteRenderer.sprite = playerSprites[PlayerPrefs.GetInt("spaceSelect")];
        }
    }

    // The health of the player updates depending on a player pref int
    void ShieldsUpdate()
    {
        if (PlayerPrefs.HasKey("shieldSelect") && PlayerPrefs.GetInt("shieldSelect") >= 0)
        {
            health.maxHealth = PlayerPrefs.GetInt("shieldSelect") + 1;
            health.currentHealth = health.maxHealth;
            hudManager.FillHealthBar();
        }
    }

    // Sets the border that the player can move to with a certain limit
    void ScreenBorderLimit()
    {
        if(Mathf.Abs(transform.position.x) >= screenBorderLimit.x)
        {
            transform.localPosition = new Vector2(transform.localPosition.x > 0 ? screenBorderLimit.x : -screenBorderLimit.x, transform.localPosition.y);
        }
        
    }

    void PlayerInput()
    {
        // Moves the player if the FINGER touches the screen
        if (Input.touchCount > 0)
        {
            foreach(Touch t in Input.touches)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(t.position);

                transform.position = Vector3.LerpUnclamped(transform.position, touchPos, Time.deltaTime * speed);
            }
        }
        else
        {
            // Moves the player if the MOUSE touches the screen
            if (Input.GetMouseButton(0)) 
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                transform.position = Vector3.LerpUnclamped(transform.position, mousePos, Time.deltaTime * speed);
            }
        }
    }

    // Takes and gives a health point to the player
    void TakeHealth(bool take)
    {
        if (take)
        {
            health.currentHealth--;

            foreach (Transform t in hudManager.healthBar)
            {
                if (t.gameObject.activeInHierarchy)
                {
                    t.gameObject.SetActive(false);
                    break;
                }
            }
        }
        else
        {
            health.currentHealth++;

            foreach (Transform t in hudManager.healthBar)
            {
                if (!t.gameObject.activeInHierarchy)
                {
                    t.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player collides with an obstacle takes 1 health point
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeHealth(true);
        }

        if (collision.gameObject.CompareTag("FinalObstacle"))
        {
            AudioManager.instance.AudioPlayOneShotVolume(explosionClip, .5f, false);
            GameManager.instance.GameOver();
        }
    }
}
