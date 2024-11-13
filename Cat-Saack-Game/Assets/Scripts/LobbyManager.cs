using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : LevelManager, SavableData
{
    public static LobbyManager Instance;
    
    public TextDisplayer defaultTextDisplayer;
    [SerializeReference]
    public List<Condition> eventConditions;

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

        if (defaultTextDisplayer != null && DialogueManager.Instance != null)
        {
            DialogueManager.SetDefaultTextDisplayer(defaultTextDisplayer);
            Debug.Log("Set defualt next displayer level manager");
        }

        LoadData();
        // start tutorial sequence
        if (!tutorialPassed && sequenceOnAwake != null)
        {
            SequenceManager.StartSequence(sequenceOnAwake);
        }

        // check conditions
        foreach (Condition condition in eventConditions)
        {
            condition.CheckConditionMet();
        }
    }

    public void SaveData()
    {
        if (GameDataManager.Instance != null)
        {
            GameDataManager.SetTutorialPassed(tutorialPassed);
        }
    }

    public void LoadData()
    {
        if (GameDataManager.Instance != null)
        {
            tutorialPassed = GameDataManager.Instance.tutorialPassed;
        }
    }

    public void SetTutorialPassed(bool aBool)
    {
        tutorialPassed = aBool;
        SaveData();
    }
}
