using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public MenuButton defaultButton;
    [Header("Animation")]
    public Animator panelAnimator;
    public string closeIdleStateName = "phone_close_idle";

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
            panelAnimator.SetBool("shouldClose", false);
        }
    }

    public virtual void CloseMenuPanel()
    {
        if (panelAnimator != null)
        {
            StartCoroutine(CloseAfterAnimation());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator CloseAfterAnimation()
    {
        panelAnimator.SetBool("shouldClose", true);

        // wait for animation to start and finish
        yield return new WaitUntil(() => 
            panelAnimator.GetCurrentAnimatorStateInfo(0).IsName(closeIdleStateName));

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
