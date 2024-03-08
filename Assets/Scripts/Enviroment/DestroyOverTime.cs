using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    [SerializeField] float lifeTime;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
