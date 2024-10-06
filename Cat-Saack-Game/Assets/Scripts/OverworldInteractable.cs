using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldInteractable : MonoBehaviour, Interactable
{
    public Sequence sequenceToRun = new Sequence();

    OutlineObject outlineObject;

    void Awake()
    {
        outlineObject = GetComponent<OutlineObject>();
    }

    // Plays the sequence associated with the interactable if provided
    public void OnSelect()
    {
        if (sequenceToRun != null)
        {
            SequenceManager.StartSequence(sequenceToRun);
        }
    }

    public void SetOutlineActive(bool aActive)
    {
        outlineObject.SetOutlineActive(aActive);
    }
}
