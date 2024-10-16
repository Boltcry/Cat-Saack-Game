using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollectibleSpawner : MonoBehaviour
{
    public List<Collectible> collectiblePrefabs;
    public Tilemap walkableTilemap;
    public Tilemap collisionTilemap;
    public float spawnInterval = 4f;
    public int maxCollectibles = 10;

    private List<Vector3Int> walkableTiles = new List<Vector3Int>();
    private int currentCollectibleCount = 0;
    private Coroutine collectibleSpawnCoroutine;

    void Start()
    {
        CacheWalkableTiles();
    }

    // spawns a collectible if able every [spawnInterval] seconds
    IEnumerator SpawnCollectibles()
    {
        while (MinigameManagerDuck.gameIsRunning)
        {
            SpawnCollectible();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // attempts to spawn a collectible at location provided by GetWalkablePosition
    void SpawnCollectible()
    {
        if (currentCollectibleCount < maxCollectibles)
        {
            Vector3 spawnPosition = GetWalkablePosition();
            if (spawnPosition != Vector3.zero)
            {
                Collectible spawnedCollectible = null;
                // try to pull collectible from bank
                /*
                GameObject grabbedObject = ItemBankManager.GrabObjectFromBank<Collectible>();
                if (grabbedObject != null)
                {
                    spawnedCollectible = grabbedObject.GetComponent<Collectible>();
                    spawnedCollectible.gameObject.SetActive(true);
                }
                */
                // instantiate collectible if bank grab not successful
                if (spawnedCollectible == null)
                {
                    Collectible collectibleToSpawn = GetCollectibleBySpawnChance();
                    spawnedCollectible = Instantiate(collectibleToSpawn, spawnPosition, Quaternion.identity);
                }
                currentCollectibleCount++;
                spawnedCollectible.transform.position = spawnPosition;
                spawnedCollectible.RegisterSpawner(this);
            }
        }
    }

    Collectible GetCollectibleBySpawnChance()
    {
        float totalChance = 0f;
        foreach (Collectible collectible in collectiblePrefabs)
        {
            totalChance += collectible.spawnChance;
        }
        float randomValue = Random.Range(0f, totalChance);

        float cumulativeChance = 0f;
        foreach (Collectible collectible in collectiblePrefabs)
        {
            cumulativeChance += collectible.spawnChance;
            if (randomValue < cumulativeChance)
            {
                return collectible;
            }
        }
        return collectiblePrefabs[0];
    }

    // finds a valid walkable area for collectibles to spawn on
    Vector3 GetWalkablePosition()
    {
        Vector3Int randomTilePosition = walkableTiles[Random.Range(0, walkableTiles.Count)];
        return walkableTilemap.CellToWorld(randomTilePosition) + walkableTilemap.tileAnchor;
    }

    // adds walkable tiles to a list walkableTiles using walkableTilemap and collisionTilemap
    void CacheWalkableTiles()
    {
        if (walkableTilemap != null)
        {
            BoundsInt bounds = walkableTilemap.cellBounds;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    // make sure tiles that overlap with collision are not included
                    Vector3Int tilePos = new Vector3Int(x, y, 0);
                    if (collisionTilemap != null)
                    {
                        if (walkableTilemap.HasTile(tilePos) && !collisionTilemap.HasTile(tilePos))
                        {
                            walkableTiles.Add(tilePos);
                        }
                    }
                    // if no collision Tilemap simply pick a spot on the walkableTilemap
                    else
                    {
                        if (walkableTilemap.HasTile(tilePos))
                        {
                            walkableTiles.Add(tilePos);                    
                        }
                    }
                }
            }
        }
    }

    public void DeregisterCollectible()
    {
        currentCollectibleCount--;
    }

    // Meant to be used with the OnGameStarted event
    void StartSpawnCollectibles()
    {
        if (collectibleSpawnCoroutine == null)
        {
            collectibleSpawnCoroutine = StartCoroutine(SpawnCollectibles());
        }
    }

    void StopSpawnCollectibles()
    {
        if (collectibleSpawnCoroutine != null)
        {
            StopCoroutine(collectibleSpawnCoroutine);
            collectibleSpawnCoroutine = null;
        }
    }

    void OnEnable()
    {
        MinigameManagerDuck.OnGameStarted += StartSpawnCollectibles;
    }

    void OnDisable()
    {
        MinigameManagerDuck.OnGameStarted -= StartSpawnCollectibles;
    }
}