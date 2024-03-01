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
        Time.timeScale = 1;
    }

    // Pauses the game
    public void Pause()
    {
        Time.timeScale = 0;
    }

    // Restarts the game
    public void Restart()
    {
        StartCoroutine(ChangeSceneDelay("Game"));
    }

    // Goes to the main menu
    public void Menu()
    {
        StartCoroutine(ChangeSceneDelay("MainMenu"));
    }

    // Adds a delay before changeing the scene
    IEnumerator ChangeSceneDelay(string sceneName)
    {
        FadeBlack.instance.FadeToBlack();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneName);
    }
}
