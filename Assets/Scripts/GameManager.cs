using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    PlayerController playerController;
    HudManager hudManager;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI maxScoreText;
    [SerializeField] TextMeshProUGUI scrapsText;

    [SerializeField] GameObject gameOverPanel;

    [HideInInspector] public bool gameOver = false;

    public int gamesPlayed = 1;

    TextMeshProUGUI gamOverText;

    private void Awake()
    {
        // Makes this script a static variable
        if (instance == null)
        {
            instance = this;
        }

        gamesPlayed = PlayerPrefs.HasKey("gamesPlayed") ? PlayerPrefs.GetInt("gamesPlayed") : 1;

        if (gamesPlayed % 3 == 0)
        {
            AdsManager.instance.interstitialAds.LoadInterstatialAd();
        }
    }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        hudManager = FindObjectOfType<HudManager>();

        gamOverText = gameOverPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();

        // Updates the gameover score texts
        UpdateScore();
    }

    // When the player kills the 3rd boss the stage ender will bew activated
    public IEnumerator StageCleared()
    {
        yield return new WaitForSeconds(8);

        gameOver = true;

        gamOverText.text = "Stage\nCleared";
        gamOverText.color = Color.green;

        UpdateScore();
        StopPlayer();
        StopSpawnRepeating();

        PlayerPrefs.SetInt("stagesEnded", SceneManager.GetActiveScene().buildIndex);

        yield return new WaitForSeconds(1);

        playerController._animator.applyRootMotion = false;
        playerController._animator.Play("ExitScene");

        yield return new WaitForSeconds(1);

        StartCoroutine(GameoverPanel());
    }


    // Updates the gameover score texts
    void UpdateScore()
    {
        hudManager.DisplayTime(hudManager.timeScore, scoreText, "Current time: ");
        hudManager.DisplayTime(hudManager.maxScore, maxScoreText, "Max time: ");
        scrapsText.text = "Scraps: " + hudManager.pointsScore;
    }

    void StopSpawnRepeating()
    {
        // Stops the shooting and obstacle spawning behaviours
        foreach (BulletShooter b in FindObjectsOfType<BulletShooter>())
        {
            b.enabled = false;
        }
        foreach (ObjectSpawner o in FindObjectsOfType<ObjectSpawner>())
        {
            o.enabled = false;
        }
    }

    void StopPlayer()
    {
        //playerController.transform.parent.GetComponent<MoveForward>().forwardSpeed = 0;
        playerController.speed = 0;
        playerController.GetComponent<CapsuleCollider2D>().enabled = false;
    }

    // Gameover state
    public void GameOver()
    {
        gameOver = true;

        // Updates the gameover score texts
        UpdateScore();

        // Stops the player controller moving behaviours and plays an explosion animation
        StopPlayer();
        playerController._explosionAnimator.Play("SpaceCraft_Explosion");
        playerController._playerSpriteRenderer.enabled = false;
        playerController.transform.GetChild(0).gameObject.SetActive(false);

        StopSpawnRepeating();

        // Opens the gameover panel with some delay
        StartCoroutine(GameoverPanel());
    }

    // Opens the gameover panel with some delay
    IEnumerator GameoverPanel()
    {
        yield return new WaitForSeconds(1.5f);

        AdsManager.instance.bannerAds.ShowBannerAd();

        if (gamesPlayed % 3 == 0 && gameOver)
        {
            AdsManager.instance.bannerAds.HideBannerAd();
            AdsManager.instance.interstitialAds.ShowInterstitialAd();
        }

        gameOverPanel.SetActive(true);
    }
}
