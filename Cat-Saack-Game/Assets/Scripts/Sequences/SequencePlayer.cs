using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Video;

public class SequencePlayer : MonoBehaviour
{
    [HideInInspector]
    public bool isPlaying = false;

    [HideInInspector] 
    public Sequence currentSequence;
    [HideInInspector]
    public PlayableDirector playableDirector;
    [HideInInspector]
    public VideoPlayer videoPlayer;

    void Awake()
    {
        if (playableDirector == null)
        {
            playableDirector = GetComponent<PlayableDirector>();
        }
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }
    }

    public void StartSequence(Sequence aSequence)
    {
        if (aSequence != null)
        {
            //Debug.Log("Starting sequence");
            currentSequence = aSequence;
            isPlaying = true;
            StartCoroutine(RunSequence());
        }
    }

    public IEnumerator RunSequence()
    {
        foreach (SequenceStep step in currentSequence.sequenceSteps)
        {
            yield return new WaitForEndOfFrame();
            //Debug.Log("Starting new step");
            // switch input mode based on step's disableMove flag
            if (step.disableMove)
            {
                InputManager.SwitchInputModeMenu();
                InputManager.SetCursorButton(null);
            }
            else
            {
                InputManager.SwitchInputModeOverworld();
            }
            yield return StartCoroutine(step.Execute(this));

            if (step.restoreMove)
            {
                InputManager.SwitchInputModeOverworld();
                InputManager.SetCursorButton(null);
            }
        }
        EndSequence();
    }

    void EndSequence()
    {
        currentSequence = null;
        isPlaying = false;
        
        // Restore player overworld control & clear cursorButton
        //InputManager.SetCursorButton(null);
        //InputManager.SwitchInputModeOverworld();
        
        //Debug.Log("Ending Sequence");

        SequenceManager.Instance.ReturnPlayerToPool(this);
    }
}
