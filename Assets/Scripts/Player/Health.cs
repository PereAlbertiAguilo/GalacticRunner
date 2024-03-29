using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 5;

    public int currentHealth;

    public bool isAlive = true;

    public int deathPoint = 0;

    // When the gameobject sets active resets the current health to its max health
    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // If the current health reaches a death points dies
        if(currentHealth <= deathPoint)
        {
            isAlive = false;
        }
    }
}
