using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 50f;

    [SerializeField] float bulletLifeTime = 10f;

    [SerializeField] Transform parent;

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

    void ReturnToStartPos()
    {
        transform.parent = parent;
        gameObject.SetActive(false);
        transform.localPosition = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ReturnToStartPos();
        CancelInvoke();
    }
}
