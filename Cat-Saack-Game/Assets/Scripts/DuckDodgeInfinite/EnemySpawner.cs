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

    void Start()
    {
        if (enemyTarget == null)
        {
            enemyTarget = FindObjectOfType<PlayerDuckDodgeInfinite>().transform;
        }
        gameRoomBounds = MinigameManagerDuck.Instance.gameRoomBounds;
        bounds = gameRoomBounds.bounds;
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
    }

    // starts the SpawnEnemies coroutine. Meant to be used with the OnGameStarted event
    void StartSpawnEnemies()
    {
        StartCoroutine(SpawnEnemies());
    }

    // Spawns enemies as long as the game is running.
    IEnumerator SpawnEnemies()
    {
        while (MinigameManagerDuck.gameIsRunning)
        {
            SpawnEnemy();
            Debug.Log("Current spawnTime: "+currentSpawnTime);
            yield return new WaitForSeconds(currentSpawnTime);

            currentSpawnTime = Mathf.Max(currentSettings.minSpawnTime, currentSpawnTime - Time.deltaTime * spawnTimeReductionRate);
        }
    }

    // Spawns a singular enemy
    void SpawnEnemy()
    {
        if (currentSettings != null && currentSettings.diffEnemies.Count > 0)
        {
            // find enemy to spawn randomly, maybe replace later with an enemy spawn chance
            Enemy enemyToSpawn = currentSettings.diffEnemies[Random.Range(0, currentSettings.diffEnemies.Count)];
            // instantiate enemy, maybe replace later with an enemy bank
            Enemy spawnedEnemy = Instantiate(enemyToSpawn, FindSpawnPosition(), Quaternion.identity);
            spawnedEnemy.SetLookTarget(enemyTarget);
            spawnedEnemy.SetDestroyDistance(destroyDistance);
        }
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
                Debug.Log("Finding spawn position for difficulty Hard");
                break;
            default:
                Debug.LogWarning("Difficulty level is currently at a level not handled");
                break;
        }
        return spawnPosition;
    }

    // May move to a separate OnGameStart class that this can inherit from later
    void OnEnable()
    {
        MinigameManagerDuck.OnGameStarted += StartSpawnEnemies;
    }

    void OnDisable()
    {
        MinigameManagerDuck.OnGameStarted -= StartSpawnEnemies;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // draw gameRoomBounds
        if (bounds != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }

        // draw destroyDistance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, destroyDistance);
    }
#endif
}
