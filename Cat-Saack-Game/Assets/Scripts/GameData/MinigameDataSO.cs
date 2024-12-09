using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "NewMinigameData", menuName = "ScriptableObjects/MinigameData/MinigameData")]
public class MinigameDataSO : ScriptableObject, SavableData
{
    [Header("General Data")]
    public string gameName;
    [TextArea]
    public string rules;
    [TextArea]
    public string controls;
    //public VideoClip demoClip;
    public string demoClipURL;
    public SequenceDataSO startGameSequence;
    
    //[HideInInspector] 
    public int timesPlayedCount = 0;
    [HideInInspector]
    public bool unlocked = false;
    // [HideInInspector]
    // public LeaderboardData leaderboard;
    // [HideInInspector]
    // public List<Mod> mods;
    // private Mod currentMod;

    public void SaveData()
    {
        //
    }

    public void LoadData()
    {
        //
    }

    public void IncrementTimesPlayed()
    {
        timesPlayedCount++;
    }
}
