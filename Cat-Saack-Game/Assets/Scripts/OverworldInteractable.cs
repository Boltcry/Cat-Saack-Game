using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OverworldInteractable : MonoBehaviour, Interactable
{
    public SequenceDataSO sequenceDataToRun;
    public Sequence sequenceToRun;
    
    public event Action OnSelected;

    OutlineObject outlineObject;

    void Awake()
    {
        outlineObject = GetComponent<OutlineObject>();
        if (outlineObject == null)
        {
            outlineObject = GetComponentInChildren<OutlineObject>();
        }
        if (sequenceDataToRun != null && sequenceDataToRun.sequenceToRun != null)
        {
            sequenceToRun = sequenceDataToRun.sequenceToRun;
        }
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

    public void SetSequenceData(SequenceDataSO aSequenceData)
    {
        if (aSequenceData != null && aSequenceData.sequenceToRun != null)
        {
            sequenceToRun = aSequenceData.sequenceToRun;
        }
    }

    /*
    public void SetPosition(float x, float y)
    {
        Vector3 newPosition = new Vector3(x, y, 0);
        transform.position = newPosition;
    }
    */
}
