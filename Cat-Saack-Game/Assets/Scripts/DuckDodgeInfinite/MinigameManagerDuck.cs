using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MinigameManagerDuck : LevelManager
{
    public static MinigameManagerDuck Instance;
    // StartGame() event handler
    public delegate void GameStartedHandler();
    public static event GameStartedHandler OnGameStarted;

    [Header("Minigame Settings")]
    public MinigameUIManagerDuck uiManager;
    public EnemySpawner enemySpawner;
    public CollectibleSpawner collectibleSpawner;
    public PlayerDuckDodgeInfinite player;
    public Vector3 itemStorageBank;
    // contain list of possible level layout prefabs
    public List<LevelLayout> levelLayouts = new List<LevelLayout>();

    public Sequence gameStartSequence;


    //[HideInInspector]
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
        if (collectibleSpawner == null)
        {
            collectibleSpawner = FindObjectOfType<CollectibleSpawner>();
        }
        if (player == null)
        {
            player = FindObjectOfType<PlayerDuckDodgeInfinite>();
        }
    }

    protected override void Start()
    {
        base.Start();
        base.StartLevel();

        if (InputManager.Instance != null)
        {
            InputManager.SwitchInputModeMenu();
        }
    }

    void Update()
    {
        if (gameIsRunning)
        {
            elapsedTime = Time.time - startTime;

            // update difficulty if needed
            if (currentDifficulty < DifficultyLevel.Medium && elapsedTime >= mediumDifficultyStart)
            {
                SetDifficulty(DifficultyLevel.Medium);
            }
            if (currentDifficulty < DifficultyLevel.Hard && elapsedTime >= hardDifficultyStart)
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
        Debug.Log("Updated Difficulty to "+aDifficultyLevel);
    }

    // to be called by the StartGame button in the title screen
    // Do Pre-game functions & start actual gameplay
    public static void StartGame()
    {
        //Instance.StartCoroutine(Instance.SetRandomLayout());

        if (Instance.gameStartSequence != null)
        {
            SequenceManager.StartSequence(Instance.gameStartSequence);
        }
        else
        {
            StartGameplay();
        }
    }

    // to be run after StartGame in the gameStartSequence. After the countdown happens
    public static void StartGameplay()
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

    public static void SetPlayerSpeed(float aDuration, float aSpeedMultiplier)
    {
        Instance.player.SetSpeed(aDuration, aSpeedMultiplier);
    }

    public static void SetPlayerInvincible(float aDuration)
    {
        Instance.player.SetInvincible(aDuration);
    }

    public static void AddPlayerHealth(int aHealth)
    {
        Instance.player.AddHealth(aHealth);
    }

    // grab a random layout from levelLayouts list and update relevant game info
    IEnumerator SetRandomLayout()
    {
        if (levelLayouts.Count > 0)
        {
            int randomIndex = Random.Range(0, levelLayouts.Count);
            LevelLayout layout = levelLayouts[randomIndex];

            Instantiate(layout, transform.position, Quaternion.identity);
            yield return null;

            // update level information
            gameRoomBounds = layout.GetGameRoomBounds(); // update game bounds (spawning)
            Instance.player.transform.position = layout.GetStartPosition().position; // update player start position

            // update camera bounds
            Camera camera = Camera.main;
            if (camera != null)
            {
                CameraFollow cameraFollow = camera.GetComponent<CameraFollow>();
                if (cameraFollow != null)
                {
                    cameraFollow.SetCameraBounds(layout.GetCameraBounds());
                }
                camera.transform.position = Instance.player.transform.position + new Vector3(0, 0, -10f);
            }

            // update collectible spawner tilemap info
            if (collectibleSpawner != null)
            {
                collectibleSpawner.SetTilemaps(layout.GetWalkableTilemap(), layout.GetCollisionTilemap());
            }
        }
    }
}
