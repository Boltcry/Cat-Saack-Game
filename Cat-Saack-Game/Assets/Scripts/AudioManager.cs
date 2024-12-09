using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioType
{
    SFX,
    UI,
    AMBIENT
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource sfxAudioSource;
    public AudioSource uiAudioSource;
    public AudioSource ambientAudioSource;

    [Header("Audio Control Settings")]
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private float mutedVolume = -80f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        if (audioMixer != null)
        {

        }
    }

    public static void PlayAudioClip(AudioType aAudioType, AudioClip aClip)
    {
        switch (aAudioType)
        {
            case AudioType.SFX:
                if (Instance.sfxAudioSource != null)
                {
                    Instance.sfxAudioSource.PlayOneShot(aClip);
                }
                break;
            case AudioType.UI:
                if (Instance.uiAudioSource != null)
                {
                    Instance.uiAudioSource.PlayOneShot(aClip);
                }
                break;
            case AudioType.AMBIENT:
                if (Instance.ambientAudioSource != null)
                {
                    Instance.ambientAudioSource.loop = true;
                    Instance.ambientAudioSource.clip = aClip;
                    Instance.ambientAudioSource.Play();
                }
                break;
        }
    }

    public void MuteMaster(bool aMute)
    {
        if (audioMixer != null)
        {
            if (aMute)
            {
                audioMixer.SetFloat("MasterVolume", mutedVolume);
            }
            else
            {
                audioMixer.ClearFloat("MasterVolume");
            }
        }
    }

    public void MuteSFX(bool aMute)
    {
        if (audioMixer != null)
        {
            if (aMute)
            {
                audioMixer.SetFloat("SFXVolume", mutedVolume);
            }
            else
            {
                audioMixer.ClearFloat("SFXVolume");
            }
        }
    }

    public void MuteMusic(bool aMute)
    {
        if (audioMixer != null)
        {
            if (aMute)
            {
                audioMixer.SetFloat("MusicVolume", mutedVolume);
            }
            else
            {
                audioMixer.ClearFloat("MusicVolume");
            }
        }
    }
}
