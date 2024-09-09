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
    public TextDisplayer skinnyText;

    List<TextDisplayer> textDisplayers = new List<TextDisplayer>();
    public static bool sequenceStarted = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        dialogueReader = GetComponent<DialogueReader>();
    }

    public static void StartDialogue(TextAsset aInkJSON)
    {
        //TESTING ONLY
        Instance.skinnyText.AddTextToQueue("This is short text");
        Instance.skinnyText.AddTextToQueue("It runs simultaneously with the main dialogue panel.");
        Instance.skinnyText.AddTextToQueue("It could behave a little weird though...");
        Instance.skinnyText.AddTextToQueue("I want it to be a tutorial dialogue option");
        Instance.skinnyText.AddTextToQueue("I wonder if the text will close itself after it's done?");


        // Switch input mode to Menu & set cursor to continueButton. Later move this to cutscene manager?
        InputManager.SwitchInputModeMenu();
        InputManager.SetCursorButton(Instance.dialogueText.GetContinueButton());

        Instance.dialogueReader.ReadDialogueSetup(aInkJSON);
        AddNextDialogueChunk();

        sequenceStarted = true; //DEBUG ONLY
        Instance.StartCoroutine(Instance.dialogueText.StartTextSequence());
        Instance.StartCoroutine(Instance.skinnyText.StartTextSequence());
    }

    public static void EndDialogue()
    {
        sequenceStarted = false; //DEBUG ONLY
        InputManager.SwitchInputModeOverworld();
    }

    public static void AddNextDialogueChunk()
    {
        List<string> dialogueList = Instance.dialogueReader.RetrieveNextStoryChunk();
        foreach (string line in dialogueList)
        {
            Instance.dialogueText.AddTextToQueue(line);
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

    public static void RegisterTextDisplayer(TextDisplayer aTextDisplayer)
    {
        Instance.textDisplayers.Add(aTextDisplayer);
    }

}
