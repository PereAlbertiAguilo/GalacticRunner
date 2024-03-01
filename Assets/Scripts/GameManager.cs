using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    PlayerController playerController;
    HudManager hudManager;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI maxScoreText;

    [SerializeField] GameObject gameOverPanel;

    [HideInInspector] public bool gameOver = false;

    private void Awake()
    {
        // Makes this script a static variable
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        hudManager = FindObjectOfType<HudManager>();

        // Updates the gameover score texts
        UpdateScore();
    }

    // Updates the gameover score texts
    void UpdateScore()
    {
        hudManager.DisplayTime(hudManager.timeScore, scoreText);
        hudManager.DisplayTime(hudManager.maxScore, maxScoreText);
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
        playerController.sideSpeed = 0;

        // Stops the shooting behaviour
        foreach (BulletShooter b in FindObjectsOfType<BulletShooter>())
        {
            b.CancelInvoke();
        }

        // Opens the gameover panel with some delay
        StartCoroutine(GameoverPanel());
    }

    // Opens the gameover panel with some delay
    IEnumerator GameoverPanel()
    {
        yield return new WaitForSeconds(2);

        gameOverPanel.SetActive(true);
    }
}
