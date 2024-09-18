using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class PortraitManager : MonoBehaviour
{
    public UnityEngine.U2D.Animation.SpriteLibraryAsset[] characterLibraries;
    Image portraitImage;

    UnityEngine.U2D.Animation.SpriteLibrary spriteLibrary;
    Dictionary<string, UnityEngine.U2D.Animation.SpriteLibraryAsset> characterLibraryDict;
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

        // initialize dictionary
        characterLibraryDict = new Dictionary<string, UnityEngine.U2D.Animation.SpriteLibraryAsset>();
        foreach (UnityEngine.U2D.Animation.SpriteLibraryAsset library in characterLibraries)
        {
            characterLibraryDict[library.name] = library;
        }
    }

    public void SetPortrait(string aCharacter, string aExpression)
    {
        SetCharacter(aCharacter);
        SetExpression(aExpression, DialogueTag.DEFAULT_LIBRARY_LABEL_NAME);
    }

    void SetCharacter(string aName)
    {
        if (characterLibraryDict.TryGetValue(aName, out UnityEngine.U2D.Animation.SpriteLibraryAsset library))
        {
            currentLibrary = library;
            spriteLibrary.spriteLibraryAsset = library;
            //Debug.Log("successfully set character library to "+aName);
        }
        else
        {
            Debug.LogWarning("Character '"+aName+"' not found.");
        }
    }

    void SetExpression(string aCategory, string aLabel)
    {
        if (currentLibrary != null)
        {
            Sprite updatedSprite = currentLibrary.GetSprite(aCategory, aLabel);
            if (updatedSprite != null)
            {
                portraitImage.sprite = updatedSprite;
                //Debug.Log("successfully set category to "+aCategory+ " and label to "+aLabel);
            }
        }
    }
}
