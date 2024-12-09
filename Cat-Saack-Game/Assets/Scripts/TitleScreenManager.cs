using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : LevelManager
{
    public static TitleScreenManager Instance;

    public MenuButton startGameButton;

    void Awake()
    {
        Instance = this;
    }

    public override void StartLevel()
    {
        base.StartLevel();
        
        //InputManager.SwitchInputModeMenu();
        if (startGameButton != null)
        {
            InputManager.SetCursorButton(startGameButton);
        }
    }

}
