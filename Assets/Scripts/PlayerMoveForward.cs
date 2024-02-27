using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveForward : MonoBehaviour
{
    [SerializeField] float forwardSpeed = 10f;

    private void Update()
    {
        transform.Translate(Vector2.up * forwardSpeed * Time.deltaTime);
    }
}
