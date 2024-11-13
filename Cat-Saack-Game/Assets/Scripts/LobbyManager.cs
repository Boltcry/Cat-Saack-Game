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
        GameManager.RegisterLevelManager(this);
        InputManager.SwitchInputModeOverworld();
        base.StartLevel();

        // start tutorial sequence
        if (!tutorialPassed && sequenceOnAwake != null)
        {
            SequenceManager.StartSequence(sequenceOnAwake);
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
