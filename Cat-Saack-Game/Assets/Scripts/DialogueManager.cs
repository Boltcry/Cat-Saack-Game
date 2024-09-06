using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public static void StartDialogue() //for testing purposes
    {
        TextManager.AddTextToQueue("Hi! This is a test of the text code from Please Stab For Me.");
        TextManager.AddTextToQueue("(I should also test short text...)");
        TextManager.AddTextToQueue("Test!");
        TextManager.AddTextToQueue("This is the last text. I hope the closing of the box works properly.");

        Instance.StartCoroutine(TextManager.StartTextSequence());
    }

}
