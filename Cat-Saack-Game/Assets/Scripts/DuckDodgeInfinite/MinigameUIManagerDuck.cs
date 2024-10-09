using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// have this inherit from a general UI manager in the future?
public class MinigameUIManagerDuck : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI healthText;
    public GameObject pauseScreen; // interact with main UI manager in some way?
    public GameObject gameOverScreen;
    public TextMeshProUGUI timeSurvivedText;

    // set the timeText to the current elapsed time. Called by MinigameManagerDuck
    public void DisplayElapsedTime((int aMinutes, int aSeconds) aTime)
    {
        if (timeText != null)
        {
            DisplayTime(timeText, aTime);
        }
    }

    // set a time on a text element
    public void DisplayTime(TextMeshProUGUI aText, (int aMinutes, int aSeconds) aTime)
    {
        aText.text = string.Format("{0:00}:{1:00}", aTime.aMinutes, aTime.aSeconds);
    }

    // display health
    public void DisplayHealth(int aHealth)
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + aHealth;
        }
    }

    // show leaderboard screen (have separate Leaderboard class?)
}
