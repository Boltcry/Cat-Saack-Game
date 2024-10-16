using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SequenceManager : MonoBehaviour
{
    public static SequenceManager Instance;

    public PlayableDirector playableDirector;

    [HideInInspector] 
    public Sequence currentSequence;

    //bool sequenceIsRunning = false;

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

        if (playableDirector == null)
        {
            playableDirector = GetComponentInChildren<PlayableDirector>();
        }
    }

    public static void StartSequence(Sequence aSequence)
    {
        if (aSequence != null)
        {
            //Debug.Log("Starting sequence");
            Instance.currentSequence = aSequence;

            //Instance.sequenceIsRunning = true;
            Instance.StartCoroutine(Instance.RunSequence());
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
            yield return StartCoroutine(step.Execute());
        }
        EndSequence();
    }

    void EndSequence()
    {
        //sequenceIsRunning = false;
        currentSequence = null;

        // Restore player overworld control & clear cursorButton
        InputManager.SetCursorButton(null);
        InputManager.SwitchInputModeOverworld();
        
        //Debug.Log("Ending Sequence");
    }
}
