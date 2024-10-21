using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public MenuButton defaultButton;
    public Animator panelAnimator;

    protected void Start()
    {
        if (panelAnimator ==  null)
        {
            panelAnimator = GetComponent<Animator>();
        }
        gameObject.SetActive(false);
    }

    public virtual void OpenMenuPanel()
    {
        gameObject.SetActive(true);
        if (panelAnimator != null)
        {
            // play opening animation
        }
    }

    public virtual void CloseMenuPanel()
    {
        if (panelAnimator != null)
        {
            // play closing animation
        }
        gameObject.SetActive(false);
    }

    public MenuButton GetDefaultButton()
    {
        if (defaultButton != null)
        {
            return defaultButton;
        }
        else
        {
            return GetComponentInChildren<MenuButton>();
        }
    }
}
