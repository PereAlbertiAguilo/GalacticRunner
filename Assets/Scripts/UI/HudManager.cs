using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;

public class HudManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI pointsText;
    public Transform healthBar;
    public TextMeshProUGUI healthBarText;
    [SerializeField] GameObject healthPoint;

    [HideInInspector] public PlayerController playerController;

    public int pointsScore = 0;
    public float timeScore = 0;
    public int maxScore = 0;

    // Updates the game scores values given player prefs values
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

        if (PlayerPrefs.HasKey("maxScore" + SceneManager.GetActiveScene().buildIndex))
        {
            maxScore = PlayerPrefs.GetInt("maxScore" + SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            maxScore = 0;
        }

        pointsText.text = "Scraps " + pointsScore + " X 1";

        FillHealthBar();
    }

    private void Update()
    {
        // Updates the game scores
        UpdateScore();
    }

    // Updates the time scores and the points if the game isn't over
    void UpdateScore()
    {
        if (GameManager.instance.gameOver)
        {
            return;
        }

        timeScore += Time.deltaTime;

        DisplayTime(timeScore, scoreText, "");
    }

    // Displays the time score with minutes and seconds to a UI text
    public void DisplayTime(float timeToDisplay, TextMeshProUGUI text, string beforeText)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        text.text = string.Format(beforeText + "{0:00}:{1:00}", minutes, seconds);
    }

    // Fills the health bar depending on the player max health
    public void FillHealthBar()
    {
        healthBarText.gameObject.SetActive(playerController.health.maxHealth > 0 ? false : true);

        foreach (Transform t in healthBar)
        {
            Destroy(t);
        }

        for (int i = 0; i < playerController.health.maxHealth; i++)
        {
            Instantiate(healthPoint, healthBar);
        }
    }

    // Updates the score player prefs, the max score gets set if the current score value is bigger than the current saved max score
    public void SaveData()
    {
        if (Mathf.RoundToInt(timeScore) > maxScore)
        {
            maxScore = Mathf.RoundToInt(timeScore);
            timeScore = maxScore;

            PlayerPrefs.SetInt("maxScore" + SceneManager.GetActiveScene().buildIndex, maxScore);
        }

        PlayerPrefs.SetInt("pointsScore", pointsScore);
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
