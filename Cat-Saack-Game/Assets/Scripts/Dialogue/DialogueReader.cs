using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Ink.Runtime;

public static class DialogueTag
{
    // default name for portrait sprite library entries
    public const string DEFAULT_LIBRARY_LABEL_NAME = "Entry";

    // Character tag should be used to indicate which character will be speaking.
    // This will set the portrait sprite library and audio config
    public const string CHAR_TAG = "character";
    // Speaker tag should be used to indicate the desired name to show in the text box indicating name.
    public const string SPEAKER_TAG = "speaker";
    // portrait tags should be the name of the category in the relevant sprite library. For example if you are in speakerConfig.id "alien"'s library, use tag "#portrait:happy"
    public const string PORTRAIT_TAG = "portrait";
    public const string LAYOUT_TAG = "layout";
    public static readonly List<string[]> EMPTY_TAG_PAIR = new List<string[]> { new string[] { "", "" } };

    // checks if the given tag list has been marked as an empty line.
    public static bool TagLineIsEmpty(List<string[]> aTagList)
    {
        if (aTagList == null || aTagList.Count != EMPTY_TAG_PAIR.Count)
        {
            return false;
        }
        // compare each array
        for (int i = 0; i < aTagList.Count; i++)
        {
            if (!aTagList[i].SequenceEqual(EMPTY_TAG_PAIR[i]))
            {
                return false;
            }
        }

        return true;
    }
}

// Handles reading ink JSON files. Should be attached as a component to DialogueManager.
public class DialogueReader : MonoBehaviour
{
    [SerializeField] private TextAsset loadInkGlobalsJSON;
    [HideInInspector] public DialogueVariableObserver dialogueVariables;

    private Story currentStory;
    private List<List<string[]>> currentChunkTags = new List<List<string[]>>();

    void Awake()
    {
        dialogueVariables = new DialogueVariableObserver(loadInkGlobalsJSON);
    }

    // registers JSON file with the dialogue reader
    public void ReadDialogueSetup(TextAsset aInkJSON)
    {
        currentStory = new Story(aInkJSON.text);
    }

    // parses each line of story until it comes to a choice or the end of the ink story file.
    // Returns a List of dialogue lines (string) and a List matrix for tags for each line.
    public (List<string>, List<List<string[]>>) RetrieveNextStoryChunk()
    {
        currentChunkTags.Clear();
        List<string> dialogueList = new List<string>();
        
        while (currentStory.canContinue)
        {
            // add next dialogue line
            dialogueList.Add(currentStory.Continue());
            // parse and add current tags for the line
            currentChunkTags.Add(ParseTags(currentStory.currentTags));
        }

        return (dialogueList, currentChunkTags);
    }

    public bool CanContinue()
    {
        return currentStory.canContinue;
    }

    // gets available choices in the story.
    // if none returns an empty list
    public List<string> GetChoicesText()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        List<string> choicesText = new List<string>();
        foreach (Choice choice in currentChoices)
        {
            choicesText.Add(choice.text);
        }

        return choicesText;
    }

    // intended to be triggered by a choice Button via Unity Event.
    public void RecieveChoiceSelect(int aChoiceIndex)
    {
        currentStory.ChooseChoiceIndex(aChoiceIndex);
    }

    // parses tags for a single line of the story. returns a list of key value pairs in the form of a length 2 string[].
    public List<string[]> ParseTags(List<string> currentTags)
    {
        // if tag is empty return an empty tag
        if (currentTags.Count == 0)
        {
            // return a version of EMPTY_TAG_PAIR
            return DialogueTag.EMPTY_TAG_PAIR.ToList();
        }
        List<string[]> parsedTags = new List<string[]>();
        foreach (string tag in currentTags)
        {
            //parse tag into key:value pairs
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: "+tag);
                continue;
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();
            parsedTags.Add(new string[] { tagKey, tagValue });
        }

        return parsedTags;
    }

    public void StartListeningVariables()
    {
        dialogueVariables.StartListening(currentStory);
    }

    public void StopListeningVariables()
    {
        dialogueVariables.StopListening(currentStory);
    }

    // result must be type casted as the appropriate value
    // EX: string testVariableText = ((Ink.Runtime.StringValue) DialogueReader.GetVariableState("test_variable")).value;
    public Ink.Runtime.Object GetVariableState(string aVariableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(aVariableName, out variableValue);
        if (variableValue == null)
        {
            Debug.LogWarning("Ink variable found to be null: " +aVariableName);
        }
        return variableValue;
    }
}
