using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI maxScoreText;
    public Transform healthBar;
    [SerializeField] GameObject healthPoint;

    PlayerController playerController;

    public int score = 0;
    float seconds;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        maxScoreText.text = "MaxScore: " + PlayerPrefs.GetInt("maxScore");

        FillHealthBar();
    }

    private void Update()
    {
        UpdateScore();
    }

    void UpdateScore()
    {
        if (playerController.gameOver)
        {
            return;
        }

        seconds += Time.deltaTime;

        scoreText.text = "Score: " + (score + Mathf.RoundToInt(seconds % 60));
    }

    void FillHealthBar()
    {
        for (int i = 0; i < playerController.health.maxHealth; i++)
        {
            Instantiate(healthPoint, healthBar);
        }
    }

    void SaveData()
    {
        PlayerPrefs.SetInt("currentScore", score);

        if (score > PlayerPrefs.GetInt("maxScore"))
        {
            PlayerPrefs.SetInt("maxScore", score);
        }

        print("MaxScore: " + PlayerPrefs.GetInt("maxScore") + ", CurrentScore: " + PlayerPrefs.GetInt("currentScore"));
    }

    private void OnDisable()
    {
        SaveData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}
