using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Switches the material on the Object to give it an outline.
public class OutlineObject : MonoBehaviour
{
    /*
    public Material originalMaterial;
    public Material outlineMaterial;

    public SpriteRenderer spriteRenderer;
    public Image image;
    */
    [Tooltip("Shows when the object is within interact range")]
    public SpriteRenderer interactSprite;

    public void Start()
    {
        /*
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        */
        if (interactSprite == null)
        {
            interactSprite = GetComponentInChildren<SpriteRenderer>();
        }
        interactSprite.gameObject.SetActive(false);
    }

    // temporary solution since shader appears to be broken. shows a sprite instead.
    public void SetOutlineActive(bool aActive)
    {
        interactSprite.gameObject.SetActive(aActive);
    }

    /*
    // Shows or hides outline based on given bool.
    public void SetOutlineActive(bool aActive)
    {
        if(aActive)
        {
            if (image != null)
            {
                image.material = outlineMaterial;
            }
            if (spriteRenderer != null)
            {
                spriteRenderer.material = outlineMaterial;
                Debug.Log("Set sprite renderer outline");
            }
        }
        else if (!aActive)
        {
            if (image != null)
            {
                image.material = originalMaterial;
            }
            if (spriteRenderer != null)
            {
                spriteRenderer.material = originalMaterial;
                Debug.Log("Disabled sprite renderer outline");
            }
        }
    }
    */
}
