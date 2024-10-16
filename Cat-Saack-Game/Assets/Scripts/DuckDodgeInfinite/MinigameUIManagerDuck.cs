using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// have this inherit from a general UI manager in the future?
public class MinigameUIManagerDuck : MonoBehaviour
{
    public GameObject titleScreen;
    public MenuButton startGameButton;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI healthText;
    public GameObject pauseScreen; // interact with main UI manager in some way?
    public GameObject gameOverScreen;
    public TextMeshProUGUI timeSurvivedText;

    void Start()
    {
        timeText.gameObject.SetActive(true);
        healthText.gameObject.SetActive(true);
        gameOverScreen.SetActive(false);
        titleScreen.SetActive(true);
        InputManager.SetCursorButton(startGameButton);
    }

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

    public void DisplayGameOver()
    {
        gameOverScreen.SetActive(true);

        InputManager.SetCursorButton(null);
        InputManager.SwitchInputModeMenu();

        (int minutes, int seconds) finalTime = MinigameManagerDuck.Instance.CalculateCurrentTime();
        if (timeSurvivedText != null)
        {
            timeSurvivedText.text = string.Format("Time survived: {0:00}:{1:00}", finalTime.minutes, finalTime.seconds);
        }
    }

    void HideTitleScreen()
    {
        // replace later with a fade
        titleScreen.SetActive(false);
    }

    // show leaderboard screen (have separate Leaderboard class?)

    void OnEnable()
    {
        MinigameManagerDuck.OnGameStarted += HideTitleScreen;
    }

    void OnDisable()
    {
        MinigameManagerDuck.OnGameStarted -= HideTitleScreen;
    }
}
