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
        Time.timeScale = 1;

        if (PlayerPrefs.HasKey("music")) 
        { 
            music = PlayerPrefs.GetInt("music") == 1 ? true : false;
        }
        else
        {
            music = true;
        }
        if (PlayerPrefs.HasKey("sfx")) 
        { 
            sfx = PlayerPrefs.GetInt("sfx") == 1 ? true : false;
        }
        else
        {
            sfx = true;
        }

        musicToggle.isOn = music;
        sfxToggle.isOn = sfx;
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

    public void Music(bool active)
    {
        if (!active)
        {
            musicAudioSource.mute = true;
            music = false;
        }
        else
        {
            musicAudioSource.mute = false;
            music = true;
        }

        PlayerPrefs.SetInt("music", music ? 1 : 0);
    }
    public void SFX(bool active)
    {
        if (!active)
        {
            sfxAudioSource.mute = true;
            sfx = false;
        }
        else
        {
            sfxAudioSource.mute = false;
            sfx = true;
        }

        PlayerPrefs.SetInt("sfx", sfx ? 1 : 0);
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
