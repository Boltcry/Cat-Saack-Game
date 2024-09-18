using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTextDisplayer : TextDisplayer
{
    public PortraitManager portrait;
    public TextMeshProUGUI speakerName;

    [Header("Choice UI")]
    public MenuButton[] choiceButtons;

    Queue<List<string[]>> tagQueue = new Queue<List<string[]>>();
    bool isWaitingChoices = false;
    Animator animator;

    new void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        foreach (MenuButton button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    void SetSpeakerName(string aName)
    {
        if (aName != null)
        {
            speakerName.text = aName;
        }
    }

    void SetLayout(string aLayoutType)
    {
        if (aLayoutType.ToLower() == "left")
        {
            animator.SetBool("isLeftLayout", true);
        }
        else if (aLayoutType.ToLower() == "right")
        {
            animator.SetBool("isLeftLayout", false);
        }
    }

    public void SetIsWaitingChoices(bool aActive)
    {
        isWaitingChoices = aActive;
    }

    public void AddTagToQueue(List<string[]> aTagLine)
    {
        tagQueue.Enqueue(aTagLine);
    }

    public void DisplayChoices(List<string> aChoiceListText)
    {
        // check if UI can support the number of choices coming in
        if (aChoiceListText.Count > choiceButtons.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: "+ aChoiceListText.Count);
            // remove the last element until the length is the same as the # of UI buttons
            while (aChoiceListText.Count > choiceButtons.Length)
            {
                aChoiceListText.RemoveAt(aChoiceListText.Count - 1); 
            }
        }

        // enable and initialize choices up to the amount of choices for the lines of dialogue
        int index = 0;
        foreach(string choice in aChoiceListText)
        {
            choiceButtons[index].gameObject.SetActive(true);
            choiceButtons[index].SetButtonText(choice);
            index++;
        }

        // Go through remaining choices and hide them
        for (int i = index; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].gameObject.SetActive(false);
        }

        // Set cursor button to highlight the first choice
        InputManager.SetCursorButton(choiceButtons[0]);

        SetIsWaitingChoices(true);
    }

    //wait for recieveDialogueSelect but also display choices if needed
    protected override IEnumerator WaitForContinue()
    {
        // handle choices
        // if no more text to display
        if (textQueue.Count == 0)
        {
            if (!isWaitingChoices)
            {
                SetContinueButtonVisible(true);
            }
            DialogueManager.ContinueDialogue();
        }
        // handle normal dialogue continue
        else
        {
            // set continue button to active
            SetContinueButtonVisible(true);
        }

        while (!shouldContinue)
        {
            yield return null;
        }
        SetContinue(false);
        SetIsWaitingChoices(false);
        // set continue button to inactive
        SetContinueButtonVisible(false);
        // reset cursor button
        InputManager.SetCursorButton(continueButton);

        // hide all choice buttons
        foreach (MenuButton choice in choiceButtons)
        {
            choice.gameObject.SetActive(false);
        }
    }

    public override IEnumerator EndTextSequence()
    {
        textField.text = "";
        DialogueManager.EndDialogue();
        TextPanel.SetActive(false);
        yield return null;
    }

    public override IEnumerator DisplayText(string aText)
    {
        HandleTags(tagQueue.Dequeue());
        yield return StartCoroutine(base.DisplayText(aText));
    }

    // handles tags for a single line of story.
    public void HandleTags(List<string[]> currentTags)
    {
        if (DialogueTag.TagLineIsEmpty(currentTags))
        {
            Debug.Log("Tag line is empty.");
            return;
        }

        foreach (string [] tag in currentTags)
        {
            string tagKey = tag[0];
            string tagValue = tag[1];

            //handle tag
            switch(tagKey)
            {
                case DialogueTag.SPEAKER_TAG:
                    //Debug.Log("speaker=" + tagValue);
                    SetSpeakerName(tagValue);
                    break;
                case DialogueTag.PORTRAIT_TAG:
                    // split portrait tag
                    //Debug.Log("portrait=" + tagValue);
                    string [] splitTag = tagValue.Split('_');
                    string character = splitTag[0];
                    string expression = splitTag[1];
                    if (portrait != null)
                    {
                        portrait.SetPortrait(character, expression);
                    }
                    break;
                case DialogueTag.LAYOUT_TAG:
                    Debug.Log("layout=" + tagValue);
                    SetLayout(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: "+ tagKey + ":" + tagValue);
                    break;
            }
        }
    }
}
