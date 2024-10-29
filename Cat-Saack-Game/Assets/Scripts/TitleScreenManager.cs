using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : LevelManager
{
    public static TitleScreenManager Instance;

    public AudioClip titleMusic;
    public MenuButton startGameButton;

    void Awake()
    {
        Instance = this;
    }

    public void StartTitleScreen()
    {
        //InputManager.SwitchInputModeMenu();
        if (startGameButton != null)
        {
            InputManager.SetCursorButton(startGameButton);
        }

        if (titleMusic != null)
        {
            AudioManager.PlayAudioClip(AudioType.AMBIENT, titleMusic);
        }
    }

}
