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
            // switch input mode to menu, do the WaitTillEndOfFrame thing
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
            yield return StartCoroutine(step.Execute());
        }
        EndSequence();
    }

    void EndSequence()
    {
        //sequenceIsRunning = false;
        currentSequence = null;
        // reset input modes
        Debug.Log("Ending Sequence");
    }
}
