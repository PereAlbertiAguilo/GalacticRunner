using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] Sprite[] bulletSprites;

    SpriteRenderer _bulletSpriteRenderer;

    [SerializeField] float bulletSpeed = 50f;

    [SerializeField] float bulletLifeTime = 10f;

    Transform parent;

    private void Awake()
    {
        _bulletSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        bulletSpeed += PlayerPrefs.HasKey("bulletSpeedSelect") ? (PlayerPrefs.GetInt("bulletSpeedSelect") + 1) * 10 : 0;

        SpriteUpdate();
    }

    private void OnEnable()
    {
        parent = transform.parent;

        // Invokes a function that will activate within a given time
        Invoke(nameof(ReturnToStartPos), bulletLifeTime);
    }

    private void Update()
    {
        // Moves this gameobject down if its active
        if (gameObject.activeInHierarchy)
        {
            transform.Translate(Vector2.up * bulletSpeed * Time.deltaTime);
        }
    }

    // The sprite of the bullets updates depending on a player pref int
    void SpriteUpdate()
    {
        if (PlayerPrefs.HasKey("bulletSelect"))
        {
            _bulletSpriteRenderer.sprite = bulletSprites[PlayerPrefs.GetInt("bulletSelect")];
        }
    }

    // Resets this gameobject position, active state and sets its parent
    void ReturnToStartPos()
    {
        transform.parent = parent;
        gameObject.SetActive(false);
        transform.localPosition = Vector2.zero;

        this.CancelInvoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the gameobject collides with anything this gameobject resets itself
        ReturnToStartPos();
    }
}
