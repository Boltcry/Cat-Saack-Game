using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sequence
{
    public bool disablePlayerInput = true;
    [SerializeReference] // marks list as holding polymorphic types
    public List<SequenceStep> sequenceSteps = new List<SequenceStep>();
}

[System.Serializable]
public abstract class SequenceStep
{
    public abstract IEnumerator Execute();
}

[System.Serializable]
public class DialogueStep : SequenceStep
{
    public TextAsset inkJSON;

    public override IEnumerator Execute()
    {
        yield return SequenceManager.Instance.StartCoroutine(DialogueManager.StartDialogue(inkJSON));
    }
}

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