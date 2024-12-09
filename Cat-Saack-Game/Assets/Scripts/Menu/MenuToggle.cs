using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuToggle : MenuButton
{
    [Header("Menu Toggle Settings")]
    [SerializeField]
    private GameObject toggleSprite;

    [SerializeField]
    private UnityEvent onToggleOn;
    [SerializeField]
    private UnityEvent onToggleOff;

    [SerializeField]
    private bool toggledOn = false;

    void Start()
    {
        RefreshToggleSprite();
    }

    override public void OnSelect()
    {
        Debug.Log("OnSelect in MenuToggle called");
        base.OnSelect();
        ToggleStatus();
        if (toggledOn && onToggleOn != null)
        {
            onToggleOn.Invoke();
        }
        if (!toggledOn && onToggleOff != null)
        {
            onToggleOff.Invoke();
        }
    }

    void ToggleStatus()
    {
        toggledOn = !toggledOn;
        RefreshToggleSprite();
    }

    void RefreshToggleSprite()
    {
        if (toggleSprite != null)
        {
            toggleSprite.SetActive(toggledOn);
        }
    }
}
