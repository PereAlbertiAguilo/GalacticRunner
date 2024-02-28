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

    bool paused = false;

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
    }

    public void Resume()
    {
        paused = false;
        Time.timeScale = 1;
    }
    public void Pause()
    {
        paused = true;
        Time.timeScale = 0;
    }
    public void Restart()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
