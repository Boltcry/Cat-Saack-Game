using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldInteractable : MonoBehaviour, Interactable
{
    //Ink Dialogue file
    public TextAsset inkJSON;

    OutlineObject outlineObject;

    void Awake()
    {
        outlineObject = GetComponent<OutlineObject>();
    }


    public void OnSelect()
    {
        if(inkJSON != null)
        {
            DialogueManager.StartDialogue(inkJSON);
        }
    }

    public void SetOutlineActive(bool aActive)
    {
        outlineObject.SetOutlineActive(aActive);
    }
}
