using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    public static SequenceManager Instance;

    [HideInInspector] 
    public Sequence currentSequence;

    //bool sequenceIsRunning = false;

    void Awake()
    {
        Instance = this;
    }

    public static void StartSequence(Sequence aSequence)
    {
        if (aSequence != null)
        {
            Debug.Log("Starting sequence");
            Instance.currentSequence = aSequence;

            //Instance.sequenceIsRunning = true;
            Instance.StartCoroutine(Instance.RunSequence());
        }
    }

    public IEnumerator RunSequence()
    {
        foreach (SequenceStep step in currentSequence.sequenceSteps)
        {
            Debug.Log("Starting new step");
            // switch input mode based on step's disableMove flag
            if (step.disableMove)
            {
                InputManager.SwitchInputModeMenu();
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
        InputManager.SwitchInputModeOverworld();
        InputManager.SetCursorButton(null);
        
        Debug.Log("Ending Sequence");
    }
}
