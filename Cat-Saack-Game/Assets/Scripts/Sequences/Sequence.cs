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

/*
// temp for testing
[System.Serializable]
public class TestStep : SequenceStep
{
    public string testString;

    public override IEnumerator Execute()
    {
        Debug.Log("Running Test Step. testString value is "+testString);
        yield return new WaitForSeconds(1);
        Debug.Log("Test step done.");
    }
}
*/