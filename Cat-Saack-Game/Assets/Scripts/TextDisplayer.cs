using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Handles showing text in the game. Should be attached to whatever object holds the relevant panel and text field
// Registers itself to the Dialogue Manager and can have multiple TextDisplayers running simultaneously
public class TextDisplayer : MonoBehaviour
{

    public float typeSpeed = 10;
    // the text object that will be populated by DisplayText
    public TextMeshProUGUI textField;
    public GameObject TextPanel;
    public MenuButton continueButton;


    Queue<string> textQueue = new Queue<string>();
    bool isTyping = false;
    bool shouldContinue = false;
    string currentText;

    Coroutine displayTextCoroutine;
    const float MAX_TYPE_TIME = 0.1f;


    void Awake()
    {
    }

    void Start()
    {
        SetContinueButtonVisible(false);
        gameObject.SetActive(false);
        DialogueManager.RegisterTextDisplayer(this);
    }

    public void AddTextToQueue(string aText)
    {
        if (aText != "")
        {
            textQueue.Enqueue(aText);
        }
    }

    public IEnumerator StartTextSequence()
    {
        TextPanel.SetActive(true);
        yield return StartCoroutine(StartNextText());
    }

    public IEnumerator EndTextSequence()
    {
        textField.text = "";
        TextPanel.SetActive(false);
        DialogueManager.EndDialogue();
        yield return null;
    }


    public IEnumerator StartNextText()
    {
        // if there is no more text to display
        if (textQueue.Count == 0)
        {
            yield return StartCoroutine(EndTextSequence());
            yield break;
        }

        if (!isTyping)
        {
            currentText = textQueue.Dequeue();

            // Call DisplayText() and wait for it to end
            displayTextCoroutine = StartCoroutine(DisplayText(currentText));
            yield return displayTextCoroutine;
        }
    }

    // display a single string of text character by character.
    // if the select button is pressed immediately display the rest of the line.
    // when the dialogue is finished displaying show the menu button to continue
    // perhaps have one menu button to complete the dialogue, and upon completing it set the continue button as active and switch the cursor to it manually.
    public IEnumerator DisplayText(string aText)
    {
        isTyping = true;
        int maxVisibleChars = 0;

        textField.text = aText;
        textField.maxVisibleCharacters = maxVisibleChars;

        foreach (char c in aText.ToCharArray())
        {
            if (!isTyping)
            {
                break;
            }
            maxVisibleChars++;
            textField.maxVisibleCharacters = maxVisibleChars;

            yield return new WaitForSeconds(MAX_TYPE_TIME / typeSpeed);
        }

        isTyping = false;
        yield return StartCoroutine(WaitForContinue());
        
        yield return StartNextText();
    }

    // Wait for the player to press a button that calls RecieveDialogueSelect() before continuing.
    IEnumerator WaitForContinue()
    {
        // set continue button to active
        SetContinueButtonVisible(true);
        while (!shouldContinue)
        {
            yield return null;
        }
        SetContinue(false);
        // set continue button to inactive
        SetContinueButtonVisible(false);
    }

    // Immediately display the rest of the line.
    public void FinishParagraphEarly()
    {
        textField.maxVisibleCharacters = textField.text.Length;
        isTyping = false;
        // changing isTyping should also end the typing early in the DisplayText coroutine
    }

    public bool IsTyping()
    {
        return isTyping;
    }

    // Sets shouldContinue to the provided bool.
    public void SetContinue(bool aContinue)
    {
        shouldContinue = aContinue;
    }

    // Enables or disables the Continue button's Image if it has one.
    void SetContinueButtonVisible(bool aActive)
    {
        if (continueButton != null)
        {
            Image continueButtonImage = continueButton.gameObject.GetComponent<Image>();
            if (continueButtonImage != null)
            {
                continueButtonImage.enabled = aActive;
            }
        }
    }

    public MenuButton GetContinueButton()
    {
        return continueButton;
    }

}
