using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    private DialogueReader dialogueReader;

    public DialogueTextDisplayer dialogueText;
    public DialogueSpeakerConfigSO defaultSpeakerConfig;
    public DialogueSpeakerConfigSO[] speakerConfigs;

    List<TextDisplayer> textDisplayers = new List<TextDisplayer>();
    Dictionary<string, DialogueSpeakerConfigSO> speakerConfigDict;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        dialogueReader = GetComponent<DialogueReader>();

        // initialize speakerConfig dictionary
        speakerConfigDict = new Dictionary<string, DialogueSpeakerConfigSO>();
        foreach (DialogueSpeakerConfigSO speakerConfig in speakerConfigs)
        {
            speakerConfigDict[speakerConfig.id] = speakerConfig;
        }
    }

    public static IEnumerator StartDialogue(TextAsset aInkJSON)
    {
        Instance.dialogueReader.ReadDialogueSetup(aInkJSON);
        Instance.dialogueReader.StartListeningVariables();
        ContinueDialogue();

        Instance.StartCoroutine(Instance.LateSetButton());
        yield return Instance.StartCoroutine(Instance.dialogueText.StartTextSequence());
    }

    private IEnumerator LateSetButton()
    {
        yield return new WaitForEndOfFrame();
        InputManager.SetCursorButton(Instance.dialogueText.GetContinueButton());
    }

    public static IEnumerator EndDialogue()
    {
        Instance.dialogueReader.StopListeningVariables();
        //InputManager.SetCursorButton(null);
        yield return null;
    }

    public static void ContinueDialogue()
    {
        // Add next dialogue chunk
        var (dialogueList, tagList) = Instance.dialogueReader.RetrieveNextStoryChunk();
        if (dialogueList.Count == tagList.Count)
        {
            for (int i = 0; i < dialogueList.Count; i++)
            {
                Instance.dialogueText.AddTextToQueue(dialogueList[i]);
                Instance.dialogueText.AddTagToQueue(tagList[i]);
            }
        }
        else
        {
            Debug.LogError("dialogueList and tagList are not the same length. dialogueList length: " +dialogueList.Count+ ". tagList length: "+tagList.Count);
        }

        // Check for if choices should be displayed
        if (dialogueList.Count == 0)
        {
            List<string> currentChoicesText = Instance.dialogueReader.GetChoicesText();
            if (currentChoicesText.Count > 0)
            {
                Instance.dialogueText.DisplayChoices(currentChoicesText);
            }
        }
    }

    // call this method from a unity event when Continue button is selected
    public static void RecieveDialogueSelect()
    {
        if (Instance.textDisplayers.Any(text => text.IsTyping()))
        {
            foreach (TextDisplayer text in Instance.textDisplayers)
            {
                text.FinishParagraphEarly();
            }
        }
        else
        {
            foreach (TextDisplayer text in Instance.textDisplayers)
            {
                text.SetContinue(true);
            }
        }
    }

    public static void RecieveChoiceSelect(int aChoiceIndex)
    {
        Instance.dialogueReader.RecieveChoiceSelect(aChoiceIndex);
        ContinueDialogue();
        Instance.dialogueText.SetContinue(true);
    }

    public static void RegisterTextDisplayer(TextDisplayer aTextDisplayer)
    {
        Instance.textDisplayers.Add(aTextDisplayer);
        aTextDisplayer.SetSpeakerConfig(Instance.defaultSpeakerConfig);
    }

    public static DialogueSpeakerConfigSO GetSpeakerConfig(string aID)
    {
        if (Instance.speakerConfigDict.TryGetValue(aID, out DialogueSpeakerConfigSO speakerConfig))
        {
            return speakerConfig;
        }
        Debug.LogWarning("SpeakerConfigSO with ID " +aID+ " not found.");
        return null;
    }

}
