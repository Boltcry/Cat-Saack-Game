using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTextDisplayer : TextDisplayer
{
    public Image portrait;
    public TextMeshProUGUI speakerName;

    [Header("Choice UI")]
    public MenuButton[] choiceButtons;

    public void SetPortrait(Image aPortrait)
    {
        if (aPortrait != null)
        {
            //portrait.sprite = aPortrait;
        }
    }

    public void SetSpeakerName(string aName)
    {
        if (aName != null)
        {
            speakerName.text = aName;
        }
    }
}
