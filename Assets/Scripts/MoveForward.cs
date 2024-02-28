using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float forwardSpeed = 10f;

    private void Update()
    {
        transform.Translate(Vector2.up * forwardSpeed * Time.deltaTime, Space.World);
    }
}
