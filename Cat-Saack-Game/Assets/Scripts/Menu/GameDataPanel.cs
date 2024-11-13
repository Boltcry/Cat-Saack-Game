using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class GameDataPanel : MenuPanel
{
    [Header("Main UI")]
    public TextMeshProUGUI gameNameText;
    public TextMeshProUGUI gameRulesText;
    public TextMeshProUGUI gameControlsText;
    public TextMeshProUGUI timesPlayedText;
    public MenuButton startGameButton;
    // show some kind of different thing if its locked, like a greyed out panel with question marks on it

    [Header("Video Settings")]
    public RawImage rawVideoImage;
    private VideoPlayer videoPlayer;
    
    private MinigameDataSO currentGameData;

    new void Start()
    {
        base.Start();

        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.targetTexture = rawVideoImage.texture as RenderTexture;
    }

    public override void OpenMenuPanel()
    {
        base.OpenMenuPanel();
        ShowGameData();
    }

    public void ShowGameData()
    {
        // update minigame data
        currentGameData.LoadData();

        // populate text fields with new info
        gameNameText.text = currentGameData.gameName;
        gameRulesText.text = currentGameData.rules;
        gameControlsText.text = currentGameData.controls;
        timesPlayedText.text = "Times played: " + currentGameData.timesPlayedCount;
        if (startGameButton != null)
        {
            UpdateStartGameButtonData();
        }
        if (rawVideoImage != null)
        {
            UpdateVideoData();
        }
    }

    void UpdateStartGameButtonData()
    {
        startGameButton.onSelect.RemoveAllListeners();
        Sequence sequence = currentGameData.startGameSequence.sequenceToRun;
        startGameButton.onSelect.AddListener(() => SequenceManager.StartSequence(sequence));
    }

    void UpdateVideoData()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.url = currentGameData.demoClipURL;
            //videoPlayer.clip = currentGameData.demoClip;
            videoPlayer.isLooping = true;
            videoPlayer.Play();
        }
    }

    public void SetGameData(MinigameDataSO aGameData)
    {
        currentGameData = aGameData;
        if (currentGameData == null)
        {
            Debug.LogError("aGameData given is null");
        }
    }
}
