using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : LevelManager, SavableData
{
    public static LobbyManager Instance;
    
    // data to be saved
    private Vector3 lastPlayerPosition;
    private bool tutorialPassed;

    void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        InputManager.SwitchInputModeOverworld();
        base.StartLevel();
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
