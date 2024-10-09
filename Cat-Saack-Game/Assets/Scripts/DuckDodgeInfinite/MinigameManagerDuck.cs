using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MinigameManagerDuck : MonoBehaviour
{
    public static MinigameManagerDuck Instance;
    // StartGame() event handler
    public delegate void GameStartedHandler();
    public static event GameStartedHandler OnGameStarted;

    public MinigameUIManagerDuck uiManager;
    public EnemySpawner enemySpawner;
    public PlayerDuckDodgeInfinite player;
    public BoxCollider2D gameRoomBounds;

    public AudioClip minigameMusic;

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
    }

    void Start()
    {
        if (minigameMusic != null)
        {
            AudioManager.PlayAudioClip(AudioType.AMBIENT, minigameMusic);
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

        OnGameStarted?.Invoke();
        Instance.StartCoroutine(Instance.LateStartGame());
        Debug.Log("Started Game");
    }

    IEnumerator LateStartGame()
    {
        yield return new WaitForEndOfFrame();
        if (InputManager.Instance != null)
        {
            InputManager.SwitchInputModeOverworld();
        }
    }

    public static void GameOver()
    {
        Instance.uiManager.DisplayHealth(Instance.player.GetHealth()); // display final health
        gameIsRunning = false;
        // game over text + time survived
        Instance.uiManager.DisplayGameOver();
        // display the leaderboard
    }
}
