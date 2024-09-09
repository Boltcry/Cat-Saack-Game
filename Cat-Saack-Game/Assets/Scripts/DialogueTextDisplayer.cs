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
    }
}
