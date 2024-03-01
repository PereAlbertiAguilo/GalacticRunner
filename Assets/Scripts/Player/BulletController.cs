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
        SpriteUpdate();
    }

    private void OnEnable()
    {
        parent = transform.parent;

        Invoke(nameof(ReturnToStartPos), bulletLifeTime);
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.Translate(Vector2.up * bulletSpeed * Time.deltaTime);
        }
    }

    void SpriteUpdate()
    {
        if (PlayerPrefs.HasKey("bulletSelect"))
        {
            _bulletSpriteRenderer.sprite = bulletSprites[PlayerPrefs.GetInt("bulletSelect")];
        }
    }

    void ReturnToStartPos()
    {
        transform.parent = parent;
        gameObject.SetActive(false);
        transform.localPosition = Vector2.zero;

        this.CancelInvoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Obtsacle")
        {
        }

        ReturnToStartPos();
    }
}