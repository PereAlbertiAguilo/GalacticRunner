using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        AdsManager.instance.bannerAds.ShowBannerAd();

        StartCoroutine(DysplayBannerWithDelay());
    }

    IEnumerator DysplayBannerWithDelay()
    {
        yield return new WaitForSeconds(1f);

        AdsManager.instance.bannerAds.ShowBannerAd();
    }

    // Starts the selected game stage
    public void Run(string stage)
    {
        AdsManager.instance.bannerAds.HideBannerAd();
        Time.timeScale = 1;
        StartCoroutine(ChangeSceneDelay(stage));
    }

    // Exits the game
    public void Exit()
    {
        Application.Quit();
        Debug.Log("Quit");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Adds a delay before changeing the scene
    IEnumerator ChangeSceneDelay(string sceneName)
    {
        FadeBlack.instance.FadeToBlack();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneName);
    }
}
