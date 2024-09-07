using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Handles showing text in the game. Should be attached to GameManager
// need a method that registers all the new objects to the TextManager when a new scene is loaded in
//      maybe in GameManager that calls a separate method here?
public class TextManager : MonoBehaviour
{

    public static TextManager Instance;

    public float typeSpeed = 10;
    // the text object that will be populated by DisplayText
    public TextMeshProUGUI textField;
    public GameObject TextPanel;

    public static bool sequenceStarted = false;

    Queue<string> textQueue = new Queue<string>();
    bool isTyping = false;
    bool shouldContinue = false;
    string currentText;

    Coroutine displayTextCoroutine;
    const float MAX_TYPE_TIME = 0.1f;


    void Awake()
    {
        Instance = this;
    }

    public static void AddTextToQueue(string aText)
    {
        if (aText != "")
        {
            Instance.textQueue.Enqueue(aText);
        }
    }

    public static IEnumerator StartTextSequence()
    {
        sequenceStarted = true; //DEBUG ONLY
        Instance.TextPanel.SetActive(true);
        yield return Instance.StartCoroutine(Instance.StartNextText());
    }

    public IEnumerator EndTextSequence()
    {
        textField.text = "";
        TextPanel.SetActive(false);
        sequenceStarted = false; //DEBUG ONLY
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
        while (!shouldContinue)
        {
            yield return null;
        }
        shouldContinue = false;
        // set continue button to inactive
    }

    // Immediately display the rest of the line.
    void FinishParagraphEarly()
    {
        textField.maxVisibleCharacters = textField.text.Length;
        isTyping = false;
        // changing isTyping should also end the typing early in the DisplayText coroutine
    }

    // call this method from a unity event when Continue button is selected
    public static void RecieveDialogueSelect()
    {
        if (Instance.isTyping)
        {
            Instance.FinishParagraphEarly();
        }
        else
        {
            Instance.shouldContinue = true;
        }
    }

    // public static void RegisterTextField(TextMeshProUGUI aTextField)
    // {
    //     Instance.textField = aTextField;
    // }

}
