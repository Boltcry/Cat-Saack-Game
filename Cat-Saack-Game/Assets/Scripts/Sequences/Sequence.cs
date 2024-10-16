using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class Sequence
{
    [SerializeReference] // marks list as holding polymorphic types
    public List<SequenceStep> sequenceSteps = new List<SequenceStep>();
}

[System.Serializable]
public abstract class SequenceStep
{
    [Tooltip("Switch input mode to Menu during this step")]
    public bool disableMove = true;

    public abstract IEnumerator Execute();
}

[System.Serializable]
public class DialogueStep : SequenceStep
{
    public TextAsset inkJSON;
    public TextDisplayer textPanel;

    public override IEnumerator Execute()
    {
        yield return SequenceManager.Instance.StartCoroutine(DialogueManager.StartDialogue(inkJSON, textPanel));
    }
}

[System.Serializable]
public class TimelineStep : SequenceStep
{
    public TimelineAsset timelineToPlay;

    public override IEnumerator Execute()
    {
        if (SequenceManager.Instance.playableDirector != null && timelineToPlay != null)
        {
            SequenceManager.Instance.playableDirector.playableAsset = timelineToPlay;
            SequenceManager.Instance.playableDirector.Play();

            while (SequenceManager.Instance.playableDirector.state == PlayState.Playing)
            {
                yield return null;
            }
        }
    }
}

[System.Serializable]
public class SceneLoadStep : SequenceStep
{
    public string targetScene;

    public override IEnumerator Execute()
    {
        yield return SequenceManager.Instance.StartCoroutine(SceneLoader.GoToScene(targetScene));
    }
}

[System.Serializable]
public class WaitForButtonPressedStep : SequenceStep
{
    public MenuButton targetButton;

    public override IEnumerator Execute()
    {
        bool buttonPressed = false;

        UnityEngine.Events.UnityAction action = () => buttonPressed = true;
        targetButton.onSelect.AddListener(action);

        yield return new WaitUntil(() => buttonPressed);

        Debug.Log("Button pressed, continuing sequence");
        targetButton.onSelect.RemoveListener(action);
    }
}

[System.Serializable]
public class WaitForInteractStep : SequenceStep
{
    public Interactable targetInteractable;

    public override IEnumerator Execute()
    {
        bool interacted = false;
        targetInteractable.OnSelected += () => interacted = true;

        yield return new WaitUntil(() => interacted);

        targetInteractable.OnSelected -= () => interacted = true;

        Debug.Log("Interactable selected, continuing sequence.");
    }
}

[System.Serializable]
public class WaitForTriggerEnterStep : SequenceStep
{
    public SequenceStepTrigger targetTrigger;

    public override IEnumerator Execute()
    {
        bool triggerEntered = false;
        targetTrigger.OnTriggered += () => triggerEntered = true;

        yield return new WaitUntil(() => triggerEntered);

        targetTrigger.OnTriggered -= () => triggerEntered = true;

        Debug.Log("Trigger bounds entered, continuing sequence");
    }
}