using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
    }
}
