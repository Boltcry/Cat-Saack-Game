using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Events;

// holds SequenceSteps
[System.Serializable]
public class Sequence
{
    [SerializeReference] // marks list as holding polymorphic types
    public List<SequenceStep> sequenceSteps = new List<SequenceStep>();
}

// General template for a SequenceStep
[System.Serializable]
public abstract class SequenceStep
{
    [Tooltip("Switch input mode to Menu during this step")]
    public bool disableMove = true;
    [Tooltip("Reset input mode to Overworld after this step")]
    public bool restoreMove = true;

    public abstract IEnumerator Execute(SequencePlayer aSequencePlayer);
}

// plays inkJSON dialogue
[System.Serializable]
public class DialogueStep : SequenceStep
{
    public TextAsset inkJSON;
    public TextDisplayer textPanel;

    public override IEnumerator Execute(SequencePlayer aSequencePlayer)
    {
        yield return SequenceManager.Instance.StartCoroutine(DialogueManager.StartDialogue(inkJSON, textPanel));
    }
}

// plays a playable timeline asset
[System.Serializable]
public class TimelineStep : SequenceStep
{
    public TimelineAsset timelineToPlay;

    public override IEnumerator Execute(SequencePlayer aSequencePlayer)
    {
        if (aSequencePlayer.playableDirector != null && timelineToPlay != null)
        {
            aSequencePlayer.playableDirector.playableAsset = timelineToPlay;
            aSequencePlayer.playableDirector.Play();

            while (aSequencePlayer.playableDirector.state == PlayState.Playing)
            {
                yield return null;
            }
        }
    }
}

// loads a scene
[System.Serializable]
public class SceneLoadStep : SequenceStep
{
    public string targetScene;

    public override IEnumerator Execute(SequencePlayer aSequencePlayer)
    {
        yield return SequenceManager.Instance.StartCoroutine(SceneLoader.GoToScene(targetScene));
    }
}

// waits for a menu button to be pressed
[System.Serializable]
public class WaitForButtonPressedStep : SequenceStep
{
    public MenuButton targetButton;

    public override IEnumerator Execute(SequencePlayer aSequencePlayer)
    {
        bool buttonPressed = false;

        UnityEngine.Events.UnityAction action = () => buttonPressed = true;
        targetButton.onSelect.AddListener(action);

        yield return new WaitUntil(() => buttonPressed);

        Debug.Log("Button pressed, continuing sequence");
        targetButton.onSelect.RemoveListener(action);
    }
}

// waits for an interactable to be interacted with
[System.Serializable]
public class WaitForInteractStep : SequenceStep
{
    public Interactable targetInteractable;

    public override IEnumerator Execute(SequencePlayer aSequencePlayer)
    {
        bool interacted = false;
        targetInteractable.OnSelected += () => interacted = true;

        yield return new WaitUntil(() => interacted);

        targetInteractable.OnSelected -= () => interacted = true;

        Debug.Log("Interactable selected, continuing sequence.");
    }
}

// waits for a trigger with the SequenceStepTrigger script to be entered
[System.Serializable]
public class WaitForTriggerEnterStep : SequenceStep
{
    public SequenceStepTrigger targetTrigger;

    public override IEnumerator Execute(SequencePlayer aSequencePlayer)
    {
        bool triggerEntered = false;
        targetTrigger.OnTriggered += () => triggerEntered = true;

        yield return new WaitUntil(() => triggerEntered);

        targetTrigger.OnTriggered -= () => triggerEntered = true;

        Debug.Log("Trigger bounds entered, continuing sequence");
    }
}

// wait for all other sequences playing to end
[System.Serializable]
public class WaitForSequenceEndStep : SequenceStep
{
    public Sequence sequenceToWaitFor;

    public override IEnumerator Execute(SequencePlayer aSequencePlayer)
    {
        while (SequenceManager.Instance.AreSequencesPlaying(aSequencePlayer))
        {
            yield return null;
        }
    }
}

// invokes an event
[System.Serializable]
public class InvokeEventStep : SequenceStep
{
    public UnityEvent eventToInvoke;

    public override IEnumerator Execute(SequencePlayer aSequencePlayer)
    {
        if (eventToInvoke != null)
        {
            eventToInvoke.Invoke();
        }
        yield return null;
    }
}