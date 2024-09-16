using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTextDisplayer : TextDisplayer
{
    public Image portrait;
    public TextMeshProUGUI speakerName;

    [Header("Choice UI")]
    public MenuButton[] choiceButtons;

    bool isWaitingChoices = false;

    new void Start()
    {
        base.Start();
        foreach (MenuButton button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void SetPortrait(Image aPortrait)
    {
        if (aPortrait != null)
        {
            //portrait.sprite = aPortrait;
        }
    }

    public void SetSpeakerName(string aName)
    {
        if (aName != null)
        {
            speakerName.text = aName;
        }
    }

    public void SetIsWaitingChoices(bool aActive)
    {
        isWaitingChoices = aActive;
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
}
