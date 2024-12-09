using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static bool canPause = true;
    private LevelManager activeLevelManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static void Pause()
    {
        if (Instance.activeLevelManager != null)
        {
            Instance.activeLevelManager.PauseLevel();
        }
        // muffle sound
        UIManager.OpenPauseMenu();
    }

    public static void Unpause()
    {
        // unmuffle sound
        // resume game
        if (Instance.activeLevelManager != null)
        {
            Instance.activeLevelManager.UnpauseLevel();
        }
    }

    public static void SetCanPause(bool aBool)
    {
        canPause = aBool;
    }

    public static void RegisterLevelManager(LevelManager aLevelManager)
    {
        Instance.activeLevelManager = aLevelManager;
    }
}
