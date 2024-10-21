using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour, SavableData
{
    public static LobbyManager Instance;

    public AudioClip lobbyMusic;
    
    // data to be saved
    private Vector3 lastPlayerPosition;
    private bool tutorialPassed;

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

    public void SaveData()
    {
        //
    }

    public void LoadData()
    {
        //
    }
}
