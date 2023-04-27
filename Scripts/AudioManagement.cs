using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManagement : MonoBehaviour
{
    public AudioClip[] levelMusic; // array of music for each level

    private static AudioManagement instance;

    public static bool musicMuted = false;

    private AudioSource audioSource;

    private MuteMusicButton muteButton;

    private float musicPlaybackPosition = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();

            muteButton = FindObjectOfType<MuteMusicButton>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int buildIndex = scene.buildIndex;

        if (buildIndex >= 0 && buildIndex < levelMusic.Length)
        {
            audioSource.clip = levelMusic[buildIndex];

            audioSource.time = musicPlaybackPosition;

            audioSource.Play();
        }

        // mute the music if necessary
        if (musicMuted)
        {
            audioSource.Pause();
        }

        if (muteButton != null)
        {
            muteButton.SetMusicMuted(musicMuted);
        }
    }

    public void MuteMusic()
    {
        musicMuted = true;
        AudioManagement[] audioManagers = FindObjectsOfType<AudioManagement>();
        foreach (AudioManagement audioManager in audioManagers)
        {
            audioManager.audioSource.Pause();
            if (audioManager.muteButton != null)
            {
                audioManager.muteButton.SetMusicMuted(true);
            }
        }
    }

    public void UnmuteMusic()
    {
        musicMuted = false;
        AudioManagement[] audioManagers = FindObjectsOfType<AudioManagement>();
        foreach (AudioManagement audioManager in audioManagers)
        {
            audioManager.audioSource.UnPause();
            if (audioManager.muteButton != null)
            {
                audioManager.muteButton.SetMusicMuted(false);
            }
        }
    }

    private void OnApplicationQuit()
    {
        // save the playback position of the music clip when the application is quit
        musicPlaybackPosition = audioSource.time;
    }
}