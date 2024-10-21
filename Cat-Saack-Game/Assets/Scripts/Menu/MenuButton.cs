using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

// Custom Menu Button, changes color based on state
// Uses links to find adjacent buttons in menu
public class MenuButton : MonoBehaviour
{
    public AudioClip buttonPressedSound;

    public MenuButton upLink;
    public MenuButton downLink;
    public MenuButton leftLink;
    public MenuButton rightLink;

    Image image;
    private Color normalColor;
    public Color highlightedColor;
    public Color pressedColor;
    public Color disabledColor;
    public float pressedColorDuration = 0.1f;

    TextMeshProUGUI buttonText;
    public ScriptableObject buttonData;
    public UnityEvent onSelect;

    private bool isHighlighted = false;
    private bool isDisabled = false;


    void Awake()
    {
        image = GetComponent<Image>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        normalColor = image.color;
    }

    // Invoke the custom Action set in the button's Inspector
    public void OnSelect()
    {
        Debug.Log("OnSelect in MenuButton called!");
        if (onSelect != null && !isDisabled)
        {
            StartCoroutine(SetColorForSeconds(pressedColor, pressedColorDuration));
            // play audio clip sound
            if (buttonPressedSound != null)
            {
                AudioManager.PlayAudioClip(AudioType.UI, buttonPressedSound);
            }
            onSelect.Invoke();
        }
    }

    // Gets the Linked button in the input direction
    public MenuButton GetButtonInDirection(Vector2 aMoveDirection)
    {
        float tolerance = 0.5f;

        if (Mathf.Abs(aMoveDirection.x) > Mathf.Abs(aMoveDirection.y))
        {
            if (aMoveDirection.x > tolerance && rightLink != null)
            {
                return rightLink;
            }
            else if (aMoveDirection.x < -tolerance && leftLink != null)
            {
                return leftLink;
            }
        }
        else
        {
            if (aMoveDirection.y > tolerance && upLink != null)
            {
                return upLink;
            }
            else if (aMoveDirection.y < -tolerance && downLink != null)
            {
                return downLink;
            }
        }

        return this;
    }

    // Updates button color based on state
    public void UpdateButtonColor()
    {
        if(!isDisabled)
        {
            if(isHighlighted)
            {
                SetButtonColor(highlightedColor);
                return;
            }
            SetButtonColor(normalColor);
            return;
        }
        SetButtonColor(disabledColor);
    }

    public void SetIsHighlighted(bool aActive)
    {
        isHighlighted = aActive;
        UpdateButtonColor();
    }

    // may be changed later to character-by-character
    public void SetButtonText(string aText)
    {
        buttonText.text = aText;
    }

    public void SetIsDisabled(bool aActive)
    {
        isDisabled = aActive;
        UpdateButtonColor();
    }

    void SetButtonColor(Color aColor)
    {
        image.color = aColor;
    }

    // Set button to a given color for given amount of time
    private IEnumerator SetColorForSeconds(Color aColor, float duration)
    {
        SetButtonColor(aColor);
        yield return new WaitForSeconds(duration);
        UpdateButtonColor();
    }

    public void SetButtonData(ScriptableObject aData)
    {
        buttonData = aData;
    }


    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Show button links
            Gizmos.color = Color.green;

            if (upLink != null)
            {
                Gizmos.DrawLine(transform.position, upLink.transform.position);
            }
            if (downLink != null)
            {
                Gizmos.DrawLine(transform.position, downLink.transform.position);
            }
            if (leftLink != null)
            {
                Gizmos.DrawLine(transform.position, leftLink.transform.position);
            }
            if (rightLink != null)
            {
                Gizmos.DrawLine(transform.position, rightLink.transform.position);
            }
            
        }
    #endif

}
