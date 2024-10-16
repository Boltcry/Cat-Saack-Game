using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OverworldInteractable : MonoBehaviour, Interactable
{
    public Sequence sequenceToRun = new Sequence();
    
    public event Action OnSelected;

    OutlineObject outlineObject;

    void Awake()
    {
        outlineObject = GetComponent<OutlineObject>();
    }

    // Plays the sequence associated with the interactable if provided
    public void OnSelect()
    {
        InputManager.SetCursorButton(null);
        if (sequenceToRun != null)
        {
            SequenceManager.StartSequence(sequenceToRun);
        }
        OnSelected?.Invoke();
    }

    public void SetOutlineActive(bool aActive)
    {
        outlineObject.SetOutlineActive(aActive);
    }
}
