using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 5f;

    public float currentHealth;

    public bool isAlive = true;

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if(currentHealth <= 0)
        {
            isAlive = false;
        }
    }
}
