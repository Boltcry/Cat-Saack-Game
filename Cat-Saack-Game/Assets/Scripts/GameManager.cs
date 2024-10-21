using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
        // stop other things in the relevant game
        // muffle sound
        UIManager.OpenPauseMenu();
    }

    public static void Unpause()
    {
        // unmuffle sound
        // resume game
    }
}
