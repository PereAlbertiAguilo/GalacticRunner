using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float forwardSpeed = 10f;

    [SerializeField] bool randomSpeedOffset = true;

    private void OnEnable()
    {
        if (randomSpeedOffset)
        {
            float randomVar = Random.Range(0, .5f);

            forwardSpeed += randomVar;
        }
    }

    // Moves the holder of this script down with a given speed
    private void Update()
    {
        transform.Translate(Vector2.down * forwardSpeed * Time.deltaTime, Space.World);
    }
}
