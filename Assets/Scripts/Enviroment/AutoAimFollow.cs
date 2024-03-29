using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAimFollow : MonoBehaviour
{
    Transform playerPos;
    [SerializeField] float speed;

    bool follow = true;

    private void Start()
    {
        playerPos = FindAnyObjectByType<PlayerController>().transform;
    }

    private void Update()
    {
        Vector3 direction = playerPos.position - transform.position;

        if (transform.position.y > playerPos.position.y + 3)
        {
            if (follow)
            {
                var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90;

                transform.rotation = Quaternion.AngleAxis(Mathf.LerpAngle(transform.eulerAngles.z, angle, Time.deltaTime * 2), Vector3.forward);
            }
        }
        else if(follow == true)
        {
            follow = false;
        }

        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
}
