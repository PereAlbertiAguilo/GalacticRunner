using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource sfxAudioSource;

    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle sfxToggle;

    bool music = true;
    bool sfx = true;

    private void Start()
    {
        if (PlayerPrefs.HasKey("music")) 
        { 
            musicAudioSource.mute = PlayerPrefs.GetInt("music") == 1 ? false : true;
            musicToggle.isOn = PlayerPrefs.GetInt("music") == 1 ? true : false;
        }
        if (PlayerPrefs.HasKey("sfx")) 
        { 
            sfxAudioSource.mute = PlayerPrefs.GetInt("sfx") == 1 ? false : true; 
            sfxToggle.isOn = PlayerPrefs.GetInt("sfx") == 1 ? true : false;
        }

        Time.timeScale = 1;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }
    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void Restart()
    {
        StartCoroutine(ChangeSceneDelay("Game"));
    }
    public void Menu()
    {
        StartCoroutine(ChangeSceneDelay("MainMenu"));
    }

    IEnumerator ChangeSceneDelay(string sceneName)
    {
        FadeBlack.instance.FadeToBlack();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneName);
    }

    public void Music()
    {
        if (!musicAudioSource.mute)
        {
            musicAudioSource.mute = true;
            music = false;
        }
        else
        {
            musicAudioSource.mute = false;
            music = true;
        }
    }
    public void SFX()
    {
        if (!sfxAudioSource.mute)
        {
            sfxAudioSource.mute = true;
            sfx = false;
        }
        else
        {
            sfxAudioSource.mute = false;
            sfx = true;
        }
    }

    void SaveData()
    {
        PlayerPrefs.SetInt("music", music ? 1 : 0);
        PlayerPrefs.SetInt("sfx", sfx ? 1 : 0);
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
