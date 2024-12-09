using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetOverworld : OverworldInteractable
{
    [SerializeField]
    private Animator animator;

    new public void OnSelect()
    {
        base.OnSelect();
    }

    override public void SetOutlineActive(bool aActive)
    {
        base.SetOutlineActive(aActive);
        if (animator != null)
        {
            animator.SetBool("powerOn", aActive);
            //Debug.Log("called set bool in cabinet");
        }
        if (animator == null)
        {
            Debug.LogWarning("cabinet animator is null");
        }
    }
}