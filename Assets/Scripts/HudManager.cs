using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms;

public class HudManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Transform healthBar;
    [SerializeField] GameObject healthPoint;

    PlayerController playerController;

    public int obstacleScore = 0;
    public int timeScore = 0;
    public int maxScore = 0;
    float seconds;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (PlayerPrefs.HasKey("maxScore"))
        {
            maxScore = PlayerPrefs.GetInt("maxScore");
        }
        else
        {
            maxScore = 0;
        }

        FillHealthBar();
    }

    private void Update()
    {
        UpdateScore();
    }

    void UpdateScore()
    {
        if (GameManager.instance.gameOver)
        {
            return;
        }

        seconds += Time.deltaTime;
        timeScore = Mathf.RoundToInt(seconds % 60);

        scoreText.text = "Score: " + (obstacleScore + timeScore);
    }

    void FillHealthBar()
    {
        for (int i = 0; i < playerController.health.maxHealth; i++)
        {
            Instantiate(healthPoint, healthBar);
        }
    }

    public void SaveData()
    {
        if (obstacleScore + timeScore > maxScore)
        {
            maxScore = obstacleScore + timeScore;

            PlayerPrefs.SetInt("maxScore", maxScore);
        }
    }
}
