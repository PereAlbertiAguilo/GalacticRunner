using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float sideSpeed = 10f;
    [SerializeField] float screenBorderLimit = 0f;

    [SerializeField] Sprite[] playerSprites;

    SpriteRenderer _playerSpriteRenderer;

    [HideInInspector] public Animator _animator;
    public Animator _explosionAnimator;

    [HideInInspector] public GameObject explosionParticle;

    private float horizontalInput;

    [HideInInspector] public Health health;

    HudManager hudManager;

    private void Awake()
    {
        health = GetComponent<Health>();
        _animator = GetComponent<Animator>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        explosionParticle = transform.GetChild(3).gameObject;
    }

    private void Start()
    {
        SpriteUpdate();

        hudManager = FindObjectOfType<HudManager>();

        StartCoroutine(EnterScene());
    }

    private void Update()
    {
        PlayerInput();

        ScreenBorderLimit();

        if (!health.isAlive && !GameManager.instance.gameOver)
        {
            GameManager.instance.GameOver();
        }
    }

    IEnumerator EnterScene()
    {
        yield return new WaitForSeconds(1);

        inputValue = 0;
        _animator.applyRootMotion = true;
    }

    void SpriteUpdate()
    {
        if (PlayerPrefs.HasKey("spaceSelect"))
        {
            _playerSpriteRenderer.sprite = playerSprites[PlayerPrefs.GetInt("spaceSelect")];
        }
    }

    void ScreenBorderLimit()
    {
        if(Mathf.Abs(transform.position.x) >= screenBorderLimit)
        {
            transform.position = new Vector2(transform.position.x > 0 ? screenBorderLimit : -screenBorderLimit, transform.position.y);
        }
    }

    bool right = false;

    void PlayerInput()
    {
        if (Input.touchCount > 0)
        {
            foreach(Touch t in Input.touches)
            {
                if (t.position.x > Screen.width / 2)
                {
                    MovePlayer(0, 1);
                    right = true;
                }
                else
                {
                    MovePlayer(-1, 0);
                    right = false;
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0)) 
            {
                if (Input.mousePosition.x > Screen.width / 2)
                {
                    MovePlayer(0, 1);
                    right = true;
                }
                else
                {
                    MovePlayer(-1, 0);
                    right = false;
                }
            }
            else
            {
                if (!right)
                {
                    inputValue += 7 * Time.deltaTime;
                    inputValue = Mathf.Clamp(inputValue, -1, 0);
                }
                else
                {
                    inputValue -= 7 * Time.deltaTime;
                    inputValue = Mathf.Clamp(inputValue, 0, 1);
                }
            }
        }

        transform.Translate(Vector2.right * inputValue * sideSpeed * Time.deltaTime);

        //horizontalInput = Input.GetAxis("Horizontal");
        //transform.Translate(Vector2.right * horizontalInput * sideSpeed * Time.deltaTime);
    }

    float inputValue = 0;

    void MovePlayer(int min, int max)
    {
        if (min >= 0)
        {
            inputValue += 4 * Time.deltaTime;
        }
        else
        {
            inputValue -= 4 * Time.deltaTime;
        }

        inputValue = Mathf.Clamp(inputValue, min, max);
    }

    void TakeHealth(bool take)
    {
        if (take)
        {
            foreach (Transform t in hudManager.healthBar.transform)
            {
                if (t.gameObject.activeInHierarchy)
                {
                    t.gameObject.SetActive(false);
                    health.currentHealth--;
                    break;
                }
            }
        }
        else
        {
            foreach (Transform t in hudManager.healthBar.transform)
            {
                if (!t.gameObject.activeInHierarchy)
                {
                    t.gameObject.SetActive(true);
                    health.currentHealth++;
                    break;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeHealth(true);
        }
    }
}
