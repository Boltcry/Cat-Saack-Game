using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

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
        if (SceneInProject(targetScene))
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(targetScene);

            while (!operation.isDone)
            {
                yield return null;
            }
            Debug.Log("Loaded Scene" +targetScene);
        }
    }

    bool SceneInProject(string aSceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneNameFromPath.Equals(aSceneName))
            {
                return true; // scene found
            }
        }
        Debug.LogWarning("Scene by name " +aSceneName+ " not found.");
        return false; //scene not found
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