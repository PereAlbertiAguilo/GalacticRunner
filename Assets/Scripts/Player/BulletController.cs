using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] Sprite[] bulletSprites;

    SpriteRenderer _bulletSpriteRenderer;

    [SerializeField] float bulletSpeed = 50f;
    public int bulletDamage = 1;

    [SerializeField] float bulletLifeTime = 10f;
    float startTime;

    [SerializeField] bool isPlayerBullet = true;

    [SerializeField] bool bulletFollow = false;
    [SerializeField] bool useParentRotation = false;

    Transform playerPos;
    Transform parent;
    Vector3 parentDir;
    Quaternion startRot;

    private void Awake()
    {
        _bulletSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        playerPos = FindAnyObjectByType<PlayerController>().transform;
        startTime = bulletLifeTime;

        startRot = transform.rotation;

        if (isPlayerBullet)
        {
            bulletSpeed += PlayerPrefs.HasKey("BulletSpeedSelect") ? (PlayerPrefs.GetInt("BulletSpeedSelect") + 1) * 6 : 0;

            if (PlayerPrefs.HasKey("BulletsSelect") && PlayerPrefs.GetInt("BulletsSelect") > 0)
            {
                bulletDamage += PlayerPrefs.GetInt("BulletsSelect") * 2;
            }

            SpriteUpdate();
        }
    }

    private void OnEnable()
    {
        if (transform.parent != null)
        {
            parent = transform.parent;
            parentDir = parent.up;
        }
    }

    void Timer()
    {
        if (bulletLifeTime <= 0)
        {
            bulletLifeTime = startTime;
            ReturnToStartPos();
        }
        else
        {
            bulletLifeTime -= Time.deltaTime;
        }
    }

    private void Update()
    {
        Timer();

        // Moves this gameobject and follows the player if bulletFollow is active or goes on a straight line
        if (bulletFollow)
        {
            Vector3 direction = playerPos.position - transform.position;

            if (transform.position.y > playerPos.position.y)
            {
                var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90;

                transform.rotation = Quaternion.AngleAxis(Mathf.LerpAngle(transform.eulerAngles.z, angle, Time.deltaTime * 2), Vector3.forward);
            }

            transform.Translate(Vector2.down * bulletSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate((useParentRotation ? parentDir : (isPlayerBullet ? Vector3.up : Vector3.down)) * bulletSpeed * Time.deltaTime);
        }
    }

    // The sprite of the bullets updates depending on a player pref int
    void SpriteUpdate()
    {
        if (PlayerPrefs.HasKey("BulletsSelect"))
        {
            _bulletSpriteRenderer.sprite = bulletSprites[PlayerPrefs.GetInt("BulletsSelect")];
        }
    }

    // Resets this gameobject position, active state and sets its parent
    void ReturnToStartPos()
    {
        bulletLifeTime = startTime;

        transform.parent = parent;

        transform.localPosition = Vector2.zero;
        transform.rotation = startRot;

        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the gameobject collides with anything this gameobject resets itself
        if (isPlayerBullet)
        {
            ReturnToStartPos();
        }

        string tag = collision.gameObject.tag;

        if (tag == "Player" && playerPos.GetComponent<PlayerController>().canTakeDamage)
        {
            ReturnToStartPos();
        }
    }
}
