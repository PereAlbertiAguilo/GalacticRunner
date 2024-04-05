using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpdateStats : MonoBehaviour
{
    const int HEALTH_POINTS_MINI = 12;
    const int HEALTH_POINTS_SMALL = 20;
    const int HEALTH_POINTS_MEDIUM = 35;
    const int HEALTH_POINTS_BIG = 50;
    const int HEALTH_POINTS_GIANT = 65;

    const float SPEED_MINI = 9.0f;
    const float SPEED_SMALL = 7.5f;
    const float SPEED_MEDIUM = 6.0f;
    const float SPEED_BIG = 4.5f;
    const float SPEED_GIANT = 3.0f;

    enum enemiesType { mini, small, medium, big, giant };

    [SerializeField] enemiesType enemyType;

    Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        SetStats();
    }

    void SetSpeed(float speed)
    {
        MoveForward moveForward = GetComponent<MoveForward>();
        AutoAimFollow autoAimFollow = GetComponent<AutoAimFollow>();

        if (moveForward != null) 
        {
            moveForward.forwardSpeed = speed;
        }
        else
        {
            autoAimFollow.speed = speed;
        }
    }

    void SetHealth(int healthPoints)
    {
        health.maxHealth = healthPoints * SceneManager.GetActiveScene().buildIndex;
        health.currentHealth = healthPoints * SceneManager.GetActiveScene().buildIndex;
    }

    void SetStats()
    {
        switch (enemyType)
        {
            case enemiesType.mini:
                SetHealth(HEALTH_POINTS_MINI);
                SetSpeed(SPEED_MINI);
                break;
            case enemiesType.small:
                SetHealth(HEALTH_POINTS_SMALL);
                SetSpeed(SPEED_SMALL);
                break;
            case enemiesType.medium:
                SetHealth(HEALTH_POINTS_MEDIUM);
                SetSpeed(SPEED_MEDIUM);
                break;
            case enemiesType.big:
                SetHealth(HEALTH_POINTS_BIG);
                SetSpeed(SPEED_BIG);
                break;
            case enemiesType.giant:
                SetHealth(HEALTH_POINTS_GIANT);
                SetSpeed(SPEED_GIANT);
                break;
        }
    }
}
