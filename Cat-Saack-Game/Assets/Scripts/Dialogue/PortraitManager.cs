using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class PortraitManager : MonoBehaviour
{
    Image portraitImage;

    UnityEngine.U2D.Animation.SpriteLibrary spriteLibrary;
    UnityEngine.U2D.Animation.SpriteLibraryAsset currentLibrary;

    void Start()
    {
        // fetch Image component
        portraitImage = GetComponent<Image>();
        if (portraitImage == null)
        {
            Debug.LogWarning("Portrait image component not found.");
        }
        spriteLibrary = GetComponent<UnityEngine.U2D.Animation.SpriteLibrary>();
    }

    public void SetPortrait(UnityEngine.U2D.Animation.SpriteLibraryAsset aSpriteLibrary, string aExpression)
    {
        //SetCharacter(aCharacter);
        currentLibrary = aSpriteLibrary;
        SetExpression(aExpression, DialogueTag.DEFAULT_LIBRARY_LABEL_NAME);
    }

    void SetExpression(string aCategory, string aLabel)
    {
        if (currentLibrary != null)
        {
            Sprite updatedSprite = currentLibrary.GetSprite(aCategory, aLabel);
            if (updatedSprite != null)
            {
                if (portraitImage != null)
                {
                    portraitImage.sprite = updatedSprite;
                }
                //Debug.Log("successfully set category to "+aCategory+ " and label to "+aLabel);
            }
        }
    }
}
