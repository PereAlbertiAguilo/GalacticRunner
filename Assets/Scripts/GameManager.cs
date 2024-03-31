using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    PlayerController playerController;
    HudManager hudManager;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI maxScoreText;
    [SerializeField] TextMeshProUGUI scrapsText;

    [SerializeField] GameObject gameOverPanel;

    [SerializeField] GameObject finalObstacle;

    [HideInInspector] public bool gameOver = false;

    int gamesPlayed = 1;

    bool stageEnded = false;

    private void Awake()
    {
        // Makes this script a static variable
        if (instance == null)
        {
            instance = this;
        }

        gamesPlayed = PlayerPrefs.HasKey("gamesPlayed") ? PlayerPrefs.GetInt("gamesPlayed") : 1;
    }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        hudManager = FindObjectOfType<HudManager>();

        // Updates the gameover score texts
        UpdateScore();
    }

    private void Update()
    {
        // When te game time reaches 900 seconds / 15 minutes the stage ender will bew activated
        if(hudManager.timeScore >= 600 && !stageEnded)
        {
            stageEnded = true;

            finalObstacle.SetActive(true);

            foreach (ObstacleSpawner o in FindObjectsOfType<ObstacleSpawner>())
            {
                o.enabled = false;
            }

            PlayerPrefs.SetInt("stagesEnded", SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Updates the gameover score texts
    void UpdateScore()
    {
        hudManager.DisplayTime(hudManager.timeScore, scoreText, "Current time: ");
        hudManager.DisplayTime(hudManager.maxScore, maxScoreText, "Max time: ");
        scrapsText.text = "Scraps: " + hudManager.pointsScore;
    }

    // Gameover state
    public void GameOver()
    {
        gameOver = true;

        hudManager.SaveData();

        // Updates the gameover score texts
        UpdateScore();

        // Stops the player controller moving behaviours and plays an explosion animation
        playerController._explosionAnimator.Play("SpaceCraft_Explosion");
        playerController._playerSpriteRenderer.enabled = false;
        playerController.transform.parent.GetComponent<MoveForward>().forwardSpeed = 0;
        playerController.speed = 0;
        playerController.GetComponent<CapsuleCollider2D>().enabled = false;

        // Stops the shooting behaviour
        foreach (BulletShooter b in FindObjectsOfType<BulletShooter>())
        {
            b.enabled = false;
        }
        foreach (ObstacleSpawner o in FindObjectsOfType<ObstacleSpawner>())
        {
            o.enabled = false;
        }

        // Opens the gameover panel with some delay
        StartCoroutine(GameoverPanel());
    }

    // Opens the gameover panel with some delay
    IEnumerator GameoverPanel()
    {
        yield return new WaitForSeconds(2);

        AdsManager.instance.bannerAds.ShowBannerAd();

        if (gamesPlayed % 3 == 0 && gameOver)
        {
            AdsManager.instance.bannerAds.HideBannerAd();
            AdsManager.instance.interstitialAds.ShowInterstitialAd();
        }

        gameOverPanel.SetActive(true);
    }
}
