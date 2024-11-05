using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float destroyDistance = 50f;
    public Transform enemyTarget;
    private BoxCollider2D gameRoomBounds;
    private Bounds bounds;
    public float spawnTimeReductionRate = 600f;

    public DifficultySettings easySettings;
    public DifficultySettings mediumSettings;
    public DifficultySettings hardSettings;

    DifficultySettings currentSettings;
    float currentSpawnTime;
    float currentDifficultyTimeElapsed = 0f;
    Coroutine enemySpawnCoroutine;

    void Start()
    {
        if (enemyTarget == null)
        {
            enemyTarget = FindObjectOfType<PlayerDuckDodgeInfinite>().transform;
        }
    }

    public void UpdateDifficultySettings()
    {
        switch (MinigameManagerDuck.currentDifficulty)
        {
            case DifficultyLevel.Easy:
                currentSettings = easySettings;
                break;
            case DifficultyLevel.Medium:
                currentSettings = mediumSettings;
                break;
            case DifficultyLevel.Hard:
                currentSettings = hardSettings;
                break;
            default:
                Debug.LogWarning("Difficulty level is currently at a level not handled");
                break;
        }
        currentSpawnTime = currentSettings.startingSpawnTime;
        currentDifficultyTimeElapsed = 0f;
        StopSpawnEnemies();
        StartSpawnEnemies();
    }

    // starts the SpawnEnemies coroutine. Meant to be used with the OnGameStarted event
    public void StartSpawnEnemies()
    {
        if (enemySpawnCoroutine == null)
        {
            enemySpawnCoroutine = StartCoroutine(SpawnEnemies());
        }
    }

    public void StopSpawnEnemies()
    {
        if (enemySpawnCoroutine != null)
        {
            StopCoroutine(enemySpawnCoroutine);
            enemySpawnCoroutine = null;
        }
    }

    // Spawns enemies as long as the game is running.
    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            //Debug.Log("Current spawnTime: "+currentSpawnTime);
            yield return new WaitForSeconds(currentSpawnTime);

            currentDifficultyTimeElapsed += currentSpawnTime;
            float timeRemainingRatio = Mathf.Clamp01((currentSettings.timeToMinimum - currentDifficultyTimeElapsed) / currentSettings.timeToMinimum);

            //currentSpawnTime = Mathf.Max(currentSettings.minSpawnTime, currentSpawnTime - spawnTimeReductionRate);
            currentSpawnTime = Mathf.Lerp(currentSettings.minSpawnTime, currentSettings.startingSpawnTime, timeRemainingRatio);
        }
    }

    // Spawns a singular enemy
    void SpawnEnemy()
    {
        Enemy spawnedEnemy = null;
        // Try to pull an enemy from the item bank
        GameObject grabbedObject = ItemBankManager.GrabObjectFromBank<Enemy>();
        if (grabbedObject != null)
        {
            spawnedEnemy = GetComponent<Enemy>();
        }

        // Instantiate an enemy if grabbing from bank not successful
        if (spawnedEnemy == null)
        {
            if (currentSettings != null && currentSettings.diffEnemies.Count > 0)
            {
                // find enemy to spawn randomly, maybe replace later with an enemy spawn chance
                Enemy enemyToSpawn = currentSettings.diffEnemies[Random.Range(0, currentSettings.diffEnemies.Count)];
                spawnedEnemy = Instantiate(enemyToSpawn, FindSpawnPosition(), Quaternion.identity);
            }
        }

        spawnedEnemy.gameObject.SetActive(true);
        spawnedEnemy.SetLookTarget(enemyTarget);
        spawnedEnemy.SetDestroyDistance(destroyDistance);
    }

    // Determines spawn position based on difficulty
    Vector3 FindSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.one;

        switch(MinigameManagerDuck.currentDifficulty)
        {
            // spawn on the left or right side of the game room
            case DifficultyLevel.Easy:
                float randomOffset = currentSettings.RandomInSpawnRange();
                float spawnY = Random.Range(bounds.min.y, bounds.max.y);
                bool spawnOnLeft = Random.Range(0,2) == 0;
                float spawnX = spawnOnLeft ? bounds.min.x - randomOffset : bounds.max.x + randomOffset;
                
                spawnPosition = new Vector3(spawnX, spawnY, 0f);
                break;
            // spawn from anywhere outside of the game room
            case DifficultyLevel.Medium:
                float randomOffsetX = currentSettings.RandomInSpawnRange();
                float randomOffsetY = currentSettings.RandomInSpawnRange();

                //spawn top/bottom or left/right
                bool spawnOnVerticalEdge = Random.Range(0, 2) == 0;
                if (spawnOnVerticalEdge) // spawn top or bottom
                {
                    bool spawnOnBottom = Random.Range(0, 2) == 0;
                    spawnY = spawnOnBottom ? bounds.min.y - randomOffsetY : bounds.max.y + randomOffsetY;
                    spawnX = Random.Range(bounds.min.x, bounds.max.x);
                }
                else // spawn left or right
                {
                    spawnOnLeft = Random.Range(0, 2) == 0;
                    spawnX = spawnOnLeft ? bounds.min.x - randomOffsetX : bounds.max.x + randomOffsetX;
                    spawnY = Random.Range(bounds.min.y, bounds.max.y);
                }

                spawnPosition = new Vector3(spawnX, spawnY, 0f);
                break;
            case DifficultyLevel.Hard:
                float angle = Random.Range(0f, 360f);
                float distance = Random.Range(currentSettings.minSpawnRange, currentSettings.maxSpawnRange);
                spawnPosition = transform.position + Quaternion.Euler(0, 0, angle) * Vector3.right * distance;
                break;
            default:
                Debug.LogWarning("Difficulty level is currently at a level not handled");
                break;
        }
        return spawnPosition;
    }

    void UpdateGameRoomBounds()
    {
        gameRoomBounds = MinigameManagerDuck.Instance.gameRoomBounds;
        bounds = gameRoomBounds.bounds;
    }

    // May move to a separate OnGameStart class that this can inherit from later
    void OnEnable()
    {
        MinigameManagerDuck.OnGameStarted += UpdateGameRoomBounds;
        MinigameManagerDuck.OnGameStarted += StartSpawnEnemies;
    }

    void OnDisable()
    {
        MinigameManagerDuck.OnGameStarted -= UpdateGameRoomBounds;
        MinigameManagerDuck.OnGameStarted -= StartSpawnEnemies;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // draw destroyDistance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, destroyDistance);
    }
#endif
}
