using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAimFollow : MonoBehaviour
{
    Transform playerPos;
    public float speed;

    private void Start()
    {
        playerPos = FindAnyObjectByType<PlayerController>().transform;
    }

    private void Update()
    {
        Vector3 direction = playerPos.position - transform.position;

        if (transform.position.y > playerPos.position.y + 2.5f)
        {
            var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90;

            transform.rotation = Quaternion.AngleAxis(Mathf.LerpAngle(transform.eulerAngles.z, angle, Time.deltaTime * 3), Vector3.forward);
        }

        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
}
