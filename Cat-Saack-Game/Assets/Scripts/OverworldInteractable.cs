using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldInteractable : MonoBehaviour, Interactable
{
    public Sequence sequenceToRun = new Sequence();

    //Ink Dialogue file
    public TextAsset inkJSON;

    OutlineObject outlineObject;

    void Awake()
    {
        outlineObject = GetComponent<OutlineObject>();
    }

    // Plays the dialogue associated with the interactable. May change to a Cutscene later
    public void OnSelect()
    {
        SequenceManager.StartSequence(sequenceToRun);

        // if(inkJSON != null)
        // {
        //     DialogueManager.StartDialogue(inkJSON);
        // }
    }

    public void SetOutlineActive(bool aActive)
    {
        outlineObject.SetOutlineActive(aActive);
    }
}
