using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // Resumes the game
    public void Resume()
    {
        AdsManager.instance.bannerAds.HideBannerAd();
        Time.timeScale = 1;
    }

    // Pauses the game
    public void Pause()
    {
        AdsManager.instance.bannerAds.ShowBannerAd();
        Time.timeScale = 0;
    }

    // Restarts the game
    public void Restart()
    {
        foreach (BulletShooter b in FindObjectsOfType<BulletShooter>())
        {
            b.enabled = false;
        }

        AdsManager.instance.bannerAds.HideBannerAd();

        StartCoroutine(ChangeSceneDelay(SceneManager.GetActiveScene().name));
    }

    // Goes to the main menu
    public void Menu()
    {
        StartCoroutine(ChangeSceneDelay("MainMenu"));
    }

    // Adds a delay before changeing the scene
    IEnumerator ChangeSceneDelay(string sceneName)
    {
        PlayerPrefs.SetInt("gamesPlayed", PlayerPrefs.HasKey("gamesPlayed") ? PlayerPrefs.GetInt("gamesPlayed") + 1 : 2);

        FadeBlack.instance.FadeToBlack();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneName);
    }
}
