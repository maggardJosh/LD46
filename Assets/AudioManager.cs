using System;
using System.Collections.Generic;
using System.Linq;
using Entity.Base;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    private static AudioManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<AudioManager>();
            return _instance;
        }
    }

    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;

    private bool MuteMusic
    {
        get
        {
            return PlayerPrefs.GetInt("MuteMusic", 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("MuteMusic", value ? 1 : 0);
        }
    }

    void Awake()
    {
        _instance = this;
        musicAudioSource.volume = MuteMusic ? 0 : .3f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMusicSound();
        }
    }

    public Dictionary<AudioClip, DateTime> lastPlayed = new Dictionary<AudioClip, DateTime>();
    [SerializeField] private int MinMillisecondsBetweenSameAudioClip;

    public static void PlayOneShot(AudioClip ac)
    {
        if (AudioClipPlayedTooRecently(ac))
            return;
        Instance.lastPlayed[ac] = DateTime.Now;
        if (!Instance.sfxAudioSource.isPlaying)
            Instance.sfxAudioSource.pitch = 1.0f + Random.Range(-GameSettings.RandomSFXAmount, GameSettings.RandomSFXAmount);
        
        Instance.sfxAudioSource.PlayOneShot(ac);
    }

    private static bool AudioClipPlayedTooRecently(AudioClip ac)
    {
        if (!Instance.lastPlayed.ContainsKey(ac))
            return false;
        return Instance.lastPlayed[ac] > DateTime.Now - TimeSpan.FromMilliseconds(Instance.MinMillisecondsBetweenSameAudioClip);
    }

    public void ToggleMusicSound()
    {
        MuteMusic = !MuteMusic;
        Instance.musicAudioSource.volume = MuteMusic ? 0 : .3f;
    }

}