using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI maxScoreText;

    public int score = 0;
    float seconds;

    private void Start()
    {
        maxScoreText.text = "MaxScore: " + PlayerPrefs.GetInt("maxScore");
    }

    private void Update()
    {
        seconds += Time.deltaTime;

        score = Mathf.RoundToInt(seconds % 60);

        scoreText.text = "Score: " + score;
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
