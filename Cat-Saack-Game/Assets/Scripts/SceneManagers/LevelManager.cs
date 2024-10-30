using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Sequence sequenceOnAwake = new Sequence();
    public MenuPanel pauseMenu;
    public AudioClip ambientMusic;

    protected virtual void Start()
    {
        if (sequenceOnAwake != null)
        {
            SequenceManager.StartSequence(sequenceOnAwake);
        }
    }

    // executes when the level is enabled, to be called as the 'start' of a scene
    // EX: after the title logo video finishes playing
    public virtual void StartLevel()
    {
        if (pauseMenu != null)
        {
            UIManager.SetPauseMenu(pauseMenu);
        }

        if (ambientMusic != null)
        {
            AudioManager.PlayAudioClip(AudioType.AMBIENT, ambientMusic);
        }
    }
}
