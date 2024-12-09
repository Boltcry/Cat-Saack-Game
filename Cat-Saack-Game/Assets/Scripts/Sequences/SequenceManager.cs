using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SequenceManager : MonoBehaviour
{
    public static SequenceManager Instance;

    [Tooltip("Max number of sequence players to be idle and available in scene.")]
    [Range(1,5)]
    public int maxCachedPlayers = 1;
    public SequencePlayer sequencePlayerPrefab;

    private List<SequencePlayer> availablePlayers = new List<SequencePlayer>();
    private List<SequencePlayer> activePlayers = new List<SequencePlayer>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        foreach (SequencePlayer player in GetComponentsInChildren<SequencePlayer>())
        {
            availablePlayers.Add(player);
        }
    }

    public static void StartSequence(Sequence aSequence)
    {
        SequencePlayer player = Instance.GetAvailablePlayer();
        if (player != null)
        {
            Instance.activePlayers.Add(player);
            player.StartSequence(aSequence);
        }
    }

    private SequencePlayer GetAvailablePlayer()
    {
        if (availablePlayers.Count > 0)
        {
            SequencePlayer player = availablePlayers[0];
            availablePlayers.RemoveAt(0);
            return player;
        }

        return CreateNewPlayer();
    }

    private SequencePlayer CreateNewPlayer()
    {
        if (sequencePlayerPrefab != null)
        {
            GameObject newPlayer = Instantiate(sequencePlayerPrefab.gameObject, Instance.transform);
            return newPlayer.GetComponent<SequencePlayer>();
        }
        return null;
    }

    public void ReturnPlayerToPool(SequencePlayer aPlayer)
    {
        if (availablePlayers.Count >= maxCachedPlayers)
        {
            Destroy(aPlayer);
        }
        else
        {
            availablePlayers.Add(aPlayer);
        }
        activePlayers.Remove(aPlayer);
    }

    public bool AreSequencesPlaying(SequencePlayer playerToExclude = null)
    {
        foreach (SequencePlayer player in activePlayers)
        {
            if (player != playerToExclude && player.isPlaying)
            {
                return true;
            }
        }
        return false;
    }
    
}
