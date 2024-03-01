using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        hudManager = FindObjectOfType<HudManager>();

        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + hudManager.timeScore;
        maxScoreText.text = "Max Score: " + hudManager.maxScore;
    }

    public void GameOver()
    {
        gameOver = true;

        hudManager.SaveData();

        UpdateScore();

        playerController._explosionAnimator.Play("SpaceCraft_Explosion");
        playerController.transform.parent.GetComponent<MoveForward>().forwardSpeed = 0;
        playerController.sideSpeed = 0;

        foreach (BulletShooter b in FindObjectsOfType<BulletShooter>())
        {
            b.CancelInvoke();
        }

        StartCoroutine(GameoverPanel());
    }

    IEnumerator GameoverPanel()
    {
        yield return new WaitForSeconds(2);

        gameOverPanel.SetActive(true);
    }
}
