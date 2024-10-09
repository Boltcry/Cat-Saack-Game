using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DifficultyLevel {Easy, Medium, Hard}

[System.Serializable]
public class DifficultySettings
{
    [Tooltip("List of types of enemies that can spawn in this level.")]
    public List<Enemy> diffEnemies;

    [Tooltip("Minimum number of seconds between each enemy spawn")]
    public float minSpawnTime = 1;
    [Tooltip("Number of seconds between each enemy spawn when starting the difficulty level")]
    public float startingSpawnTime = 3;

    public float minSpawnRange = 1f;
    public float maxSpawnRange = 5f;

    public float RandomInSpawnRange()
    {
        return Random.Range(minSpawnRange, maxSpawnRange);
    }
}
