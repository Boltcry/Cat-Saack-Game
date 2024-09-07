using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public TextDisplayer dialogueText;
    public TextDisplayer skinnyText;

    List<TextDisplayer> textDisplayers = new List<TextDisplayer>();
    public static bool sequenceStarted = false;

    void Awake()
    {
        Instance = this;
    }

    public static void StartDialogue() //for testing purposes
    {
        Instance.dialogueText.AddTextToQueue("Hi! This is a test of the text code from Please Stab For Me.");
        Instance.dialogueText.AddTextToQueue("(I should also test short text...)");
        Instance.dialogueText.AddTextToQueue("Test!");
        Instance.dialogueText.AddTextToQueue("This is the last text. I hope the closing of the box works properly.");

        Instance.skinnyText.AddTextToQueue("This is short text");
        Instance.skinnyText.AddTextToQueue("It runs simultaneously with the main dialogue panel.");
        Instance.skinnyText.AddTextToQueue("It could behave a little weird though...");
        Instance.skinnyText.AddTextToQueue("I want it to be a tutorial dialogue option");
        Instance.skinnyText.AddTextToQueue("I wonder if the text will close itself after it's done?");

        sequenceStarted = true; //DEBUG ONLY
        Instance.StartCoroutine(Instance.dialogueText.StartTextSequence());
        Instance.StartCoroutine(Instance.skinnyText.StartTextSequence());
    }

    public static void EndDialogue()
    {
        sequenceStarted = false; //DEBUG ONLY
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
