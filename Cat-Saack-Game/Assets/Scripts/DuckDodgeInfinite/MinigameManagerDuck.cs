using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MinigameManagerDuck : MonoBehaviour
{
    public static MinigameManagerDuck Instance;

    public MinigameUIManagerDuck uiManager;
    public EnemySpawner enemySpawner;
    public PlayerDuckDodgeInfinite player;
    public BoxCollider2D gameRoomBounds;

    [HideInInspector]
    public static DifficultyLevel currentDifficulty;
    [HideInInspector]
    public static bool gameIsRunning = false;

    // difficulty values
    [Header("Difficulty Settings")]
    public DifficultyLevel defaultDifficulty = DifficultyLevel.Easy;

    [Tooltip("Number of seconds elapsed when the game changes to Medium difficulty")]
    public float mediumDifficultyStart = 15;
    [Tooltip("Number of seconds elapsed when the game changes to Hard difficulty")]
    public float hardDifficultyStart = 40;

    private float startTime;
    [HideInInspector]
    public float elapsedTime = 0;

    void Awake()
    {
        Instance = this;
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<MinigameUIManagerDuck>();
        }
        if (enemySpawner == null)
        {
            enemySpawner = FindObjectOfType<EnemySpawner>();
        }
        if (player == null)
        {
            player = FindObjectOfType<PlayerDuckDodgeInfinite>();
        }

        StartGame();
    }

    void Update()
    {
        if (gameIsRunning)
        {
            elapsedTime = Time.time - startTime;

            // update difficulty if needed
            if (elapsedTime >= mediumDifficultyStart)
            {
                SetDifficulty(DifficultyLevel.Medium);
            }
            if (elapsedTime >= hardDifficultyStart)
            {
                SetDifficulty(DifficultyLevel.Hard);
            }

            // update UI
            // print current elapsed time
            uiManager.DisplayElapsedTime(CalculateCurrentTime());
            uiManager.DisplayHealth(player.GetHealth());
        }
    }

    // returns the current time in minutes & seconds
    public (int minutes, int seconds) CalculateCurrentTime()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        return (minutes, seconds);
    }

    void SetDifficulty(DifficultyLevel aDifficultyLevel)
    {
        currentDifficulty = aDifficultyLevel;
        if (enemySpawner != null)
        {
            enemySpawner.UpdateDifficultySettings();
        }
        Debug.Log("Updated Difficulty");
    }

    public static void StartGame()
    {
        Instance.startTime = Time.time;
        Instance.SetDifficulty(Instance.defaultDifficulty);
        gameIsRunning = true;
        Debug.Log("Started Game");
    }

    public static void GameOver()
    {
        Instance.uiManager.DisplayHealth(Instance.player.GetHealth()); // display final health
        gameIsRunning = false;
        Debug.Log("DuckDodgeInfinite: Game Over");
        // game over text + time survived
        // display the leaderboard
    }
}
