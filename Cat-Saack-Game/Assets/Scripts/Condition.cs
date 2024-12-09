using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class Condition
{
    public UnityEvent eventToInvoke;
    public abstract bool CheckConditionMet();
}

// if the player has played the given minigame at least gamesPlayed times
[System.Serializable]
public class GamesPlayedCondition : Condition
{
    [Tooltip("Minigame to compare")]
    public MinigameDataSO minigameData;
    [Tooltip("Required number of games played")]
    public int gamesPlayed;

    public override bool CheckConditionMet()
    {
        if (minigameData.timesPlayedCount >= gamesPlayed)
        {
            if (eventToInvoke != null)
            {
                eventToInvoke.Invoke();
            }
            return true;
        }
        return false;
    }
}
