using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms;

public class HudManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI pointsText;
    public Transform healthBar;
    [SerializeField] GameObject healthPoint;

    PlayerController playerController;

    public int pointsScore = 0;
    public int timeScore = 0;
    public int maxScore = 0;
    float seconds;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (PlayerPrefs.HasKey("pointsScore"))
        {
            pointsScore = PlayerPrefs.GetInt("pointsScore");
        }
        else
        {
            pointsScore = 0;
        }

        if (PlayerPrefs.HasKey("maxScore"))
        {
            maxScore = PlayerPrefs.GetInt("maxScore");
        }
        else
        {
            maxScore = 0;
        }

        scoreText.text = "Time " + timeScore;
        scoreText.text = "Scraps " + pointsScore;

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

        scoreText.text = "Time " + timeScore;
        pointsText.text = "Scraps " + pointsScore;
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
        if (timeScore > maxScore)
        {
            maxScore = timeScore;

            PlayerPrefs.SetInt("maxScore", maxScore);
        }

        PlayerPrefs.SetInt("pointsScore", pointsScore);
    }
}
