using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

// Handles reading ink JSON files. Should be attached as a component to DialogueManager.
public class DialogueReader : MonoBehaviour
{

    private Story currentStory;


    public void ReadDialogueSetup(TextAsset aInkJSON)
    {
        currentStory = new Story(aInkJSON.text);
    }

    public List<string> RetrieveNextStoryChunk()
    {
        List<string> dialogueList = new List<string>();
        
        while (currentStory.canContinue)
        {
            dialogueList.Add(currentStory.Continue());
        }

        return dialogueList;
    }

    public bool CanContinue()
    {
        return currentStory.canContinue;
    }


}
