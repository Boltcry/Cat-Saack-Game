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
        // Switch input mode to Menu & set cursor to continueButton. Later move this to cutscene manager?
        InputManager.SwitchInputModeMenu();
        InputManager.SetCursorButton(Instance.dialogueText.GetContinueButton());

        Instance.dialogueReader.ReadDialogueSetup(aInkJSON);
        ContinueDialogue();

        Instance.StartCoroutine(Instance.dialogueText.StartTextSequence());
    }

    public static void EndDialogue()
    {
        InputManager.SwitchInputModeOverworld();
    }

    public static void ContinueDialogue()
    {
        // Add next dialogue chunk
        List<string> dialogueList = Instance.dialogueReader.RetrieveNextStoryChunk();
        foreach (string line in dialogueList)
        {
            Instance.dialogueText.AddTextToQueue(line);
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
    }

}
