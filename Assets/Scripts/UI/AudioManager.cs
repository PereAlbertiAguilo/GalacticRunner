using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource sfxAudioSource;

    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle sfxToggle;

    [SerializeField] GameObject musicMask;
    [SerializeField] GameObject sfxMask;

    bool music = true;
    bool sfx = true;

    int index = 0;

    private void Awake()
    {
        // Makes this script a static variable
        if(instance == null)
        {
            instance = this;
        }
    }

    // Updates the audio sources and toggles given player prefs values
    private void Start()
    {
        Time.timeScale = 1;

        if (PlayerPrefs.HasKey("music"))
        {
            music = PlayerPrefs.GetInt("music") == 1;
        }
        else
        {
            music = true;
        }
        if (PlayerPrefs.HasKey("sfx"))
        {
            sfx = PlayerPrefs.GetInt("sfx") == 1;
        }
        else
        {
            sfx = true;
        }

        musicToggle.isOn = music;
        sfxToggle.isOn = sfx;

        index++;
    }

    // Plays a sound form a unity event from UI elements
    public void AudioPlayOneShot(AudioClip ac)
    {
        sfxAudioSource.pitch = 1f;

        if (index > 0)
        {
            sfxAudioSource.PlayOneShot(ac);
        }
    }

    float RandomPitch()
    {
        return Random.Range(0f, 2f);
    }

    // Plays a sound with a given volume and a random pitch
    public void AudioPlayOneShotVolume(AudioClip ac, float volume, bool randomPich)
    {
        if(randomPich)
        {
            sfxAudioSource.pitch = RandomPitch();
        }
        else
        {
            sfxAudioSource.pitch = 1f;
        }

        sfxAudioSource.PlayOneShot(ac, volume);
    }

    // Music toggle that mutes an audiosource and saves the bool value to a player pref
    public void Music(bool active)
    {
        if (!active)
        {
            musicAudioSource.mute = true;
            musicMask.SetActive(true);
            music = false;
        }
        else
        {
            musicAudioSource.mute = false;
            musicMask.SetActive(false);
            music = true;
        }

        PlayerPrefs.SetInt("music", music ? 1 : 0);
    }

    // SFX toggle that mutes an audiosource and saves the bool value to a player pref
    public void SFX(bool active)
    {
        if (!active)
        {
            sfxAudioSource.mute = true;
            sfxMask.SetActive(true);
            sfx = false;
        }
        else
        {
            sfxAudioSource.mute = false;
            sfxMask.SetActive(false);
            sfx = true;
        }

        PlayerPrefs.SetInt("sfx", sfx ? 1 : 0);
    }

    // If the scripts gets disabled or the game closes the player prefs gets saved
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
