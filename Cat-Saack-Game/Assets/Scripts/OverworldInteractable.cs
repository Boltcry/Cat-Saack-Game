using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldInteractable : MonoBehaviour, Interactable
{
    OutlineObject outlineObject;

    void Awake()
    {
        outlineObject = GetComponent<OutlineObject>();
    }


    public void OnSelect()
    {
        //
    }

    public void SetOutlineActive(bool aActive)
    {
        outlineObject.SetOutlineActive(aActive);
    }
}
