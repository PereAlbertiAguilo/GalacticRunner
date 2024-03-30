using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    [SerializeField] Sprite[] playerSprites;
    [SerializeField] AudioClip explosionClip;
    [HideInInspector] public SpriteRenderer _playerSpriteRenderer;

    [HideInInspector] public Animator _animator;
    public Animator _explosionAnimator;
    SpriteRenderer _spriteRenderer;

    [HideInInspector] public Health health;

    [SerializeField] GameObject[] bulletShooter;

    public bool canTakeDamage = true;

    HudManager hudManager;

    private void Awake()
    {
        health = GetComponent<Health>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();

        // Updates the player health
        ShieldsUpdate();
    }

    private void Start()
    {
        hudManager = FindObjectOfType<HudManager>();

        // Updates the player sprite
        SpriteUpdate();

        // Adds a delay when enetering the scene
        StartCoroutine(EnterScene());

        // Instantiates a bullet shooter depending on a stored player pref
        Instantiate(bulletShooter[PlayerPrefs.HasKey("spaceSelect") ? PlayerPrefs.GetInt("spaceSelect") : 0], transform);

        // Upadtes the player move speed
        speed += PlayerPrefs.HasKey("engineSelect") ? PlayerPrefs.GetInt("engineSelect") + 1 : 0;
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
            health.maxHealth = (PlayerPrefs.GetInt("shieldSelect") + 1) * 4;
            health.currentHealth = health.maxHealth;
        }
    }

    // Sets the border that the player can move to the current screen resolution
    void ScreenBorderLimit()
    {
        Vector2 screenTop = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1));
        Vector2 screenRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 0.5f));

        float upLimit = transform.parent.InverseTransformPoint(screenTop).y - .5f;
        float rightLimit = transform.parent.InverseTransformPoint(screenRight).x - .5f;

        if (Mathf.Abs(transform.localPosition.y) >= upLimit)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y > 0 ? upLimit : -upLimit);
        }
        if (Mathf.Abs(transform.localPosition.x) >= rightLimit)
        {
            transform.localPosition = new Vector2(transform.localPosition.x > 0 ? rightLimit : -rightLimit, transform.localPosition.y);
        }
    }

    void PlayerInput()
    {
        if(EventSystem.current.currentSelectedGameObject == null)
        {
            // Moves the player if the FINGER touches the screen
            if (Input.touchCount > 0)
            {
                foreach (Touch t in Input.touches)
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

            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    // Takes and gives a health point to the player
    void HealthUpdate(bool take)
    {
        if (take)
        {
            health.currentHealth--;

            foreach (Transform t in hudManager.healthBar)
            {
                Image i = t.GetComponent<Image>();

                if (t.gameObject.activeInHierarchy)
                {
                    if(i.fillAmount > .25f)
                    {
                        i.fillAmount -= .25f;
                    }
                    else
                    {
                        t.gameObject.SetActive(false);
                    }
                    break;
                }
            }
        }
        else
        {
            health.currentHealth++;

            foreach (Transform t in hudManager.healthBar)
            {
                Image i = t.GetComponent<Image>();

                if (!t.gameObject.activeInHierarchy)
                {
                    if (i.fillAmount < 1)
                    {
                        i.fillAmount += .25f;
                    }
                    else
                    {
                        t.gameObject.SetActive(true);
                        i.fillAmount = .25f;
                    }
                    break;
                }
            }
        }
    }

    IEnumerator ImmunityFrames()
    {
        canTakeDamage = false;
        _spriteRenderer.color = new Color(0, 0, 0, 0);
        yield return new WaitForSeconds(.20f);
        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(.20f);
        _spriteRenderer.color = new Color(0, 0, 0, 0);
        yield return new WaitForSeconds(.20f);
        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(.20f);
        _spriteRenderer.color = new Color(0, 0, 0, 0);
        yield return new WaitForSeconds(.20f);
        _spriteRenderer.color = Color.white;
        canTakeDamage = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player collides with an obstacle takes 1 health point
        if (collision.gameObject.CompareTag("Obstacle") && canTakeDamage)
        {
            StartCoroutine(ImmunityFrames());
            HealthUpdate(true);
            _explosionAnimator.Play("SpaceCraft_Explosion");
            AudioManager.instance.AudioPlayOneShotVolume(explosionClip, .2f, false);

            if (health.currentHealth <= health.deathPoint + 1)
            {
                hudManager.healthBarText.gameObject.SetActive(true);
            }
        }

        if (collision.gameObject.CompareTag("FinalObstacle"))
        {
            AudioManager.instance.AudioPlayOneShotVolume(explosionClip, .2f, false);
            GameManager.instance.GameOver();
        }
    }
}
