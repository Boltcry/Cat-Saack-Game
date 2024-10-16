using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;

    public AudioClip lobbyMusic;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InputManager.SwitchInputModeOverworld();

        if (lobbyMusic != null)
        {
            AudioManager.PlayAudioClip(AudioType.AMBIENT, lobbyMusic);
        }
    }
}
