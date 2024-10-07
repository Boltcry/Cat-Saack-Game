using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManagerDuck : MonoBehaviour
{
    public static MinigameManagerDuck Instance;

    public PlayerDuckDodgeInfinite player;

    private float startTime;

    void Awake()
    {
        Instance = this;
        if (player == null)
        {
            player = FindObjectOfType<PlayerDuckDodgeInfinite>();
        }

        startTime = Time.time;
    }

    void Update()
    {
        // prompt minigame UI manager to print time returned by CalculateCurrentTime()

        // update health UI
    }

    public (int minutes, int seconds) CalculateCurrentTime()
    {
        float elapsedTime = Time.time - startTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        return (minutes, seconds);
    }

    public static void GameOver()
    {
        // stop the game
        // game over text + time survived
        // display the leaderboard
    }
}
